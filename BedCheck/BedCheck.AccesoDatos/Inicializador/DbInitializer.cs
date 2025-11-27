using BedCheck.Data;
using BedCheck.Models;
using BedCheck.Utilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BedCheck.AccesoDatos.Data.Inicializador
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Inicializar()
        {
            // -----------------------------------------------------------
            // 1. MIGRACIONES Y CREACIÓN DE BD
            // -----------------------------------------------------------
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception) { }

            // -----------------------------------------------------------
            // 2. ROLES Y USUARIO ADMIN (Lo que ya tenías)
            // -----------------------------------------------------------
            if (!_roleManager.RoleExistsAsync(CNT.Administrador).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(CNT.Administrador)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(CNT.Cliente)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(CNT.Registrado)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@bedcheck.com",
                    Email = "admin@bedcheck.com",
                    EmailConfirmed = true,
                    Nombre = "Administrador Sistema"
                }, "Admin123*").GetAwaiter().GetResult();

                // Asignar Rol al Admin
                // 5. Asignar Rol al Admin
                ApplicationUser usuario = _userManager.FindByEmailAsync("admin@bedcheck.com").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(usuario, CNT.Administrador).GetAwaiter().GetResult();
            }

            // -----------------------------------------------------------
            // 3. DATOS DE PRUEBA (NUEVO)
            // -----------------------------------------------------------

            // Si ya hay habitaciones, asumimos que ya se cargaron los datos y salimos.
            if (_db.Habitacion.Any()) return;

            // A. CREAR HABITACIONES
            var habitacion1 = new Habitacion { NumHabitacion = 101, NumCamasTotales = 2, CamasOcupadas = 0 };
            var habitacion2 = new Habitacion { NumHabitacion = 102, NumCamasTotales = 1, CamasOcupadas = 0 };

            _db.Habitacion.AddRange(habitacion1, habitacion2);
            _db.SaveChanges(); // Guardamos para generar los IDs

            // B. CREAR CAMAS (Vinculadas a las habitaciones)
            var camas = new List<Cama>
            {
                new Cama {
                    NombreCama = "Cama 101-A",
                    EstadoCama = "Disponible",
                    TipoCama = "Estándar",
                    HabitacionId = habitacion1.IdHabitacion, // Usamos el ID generado
                    FechaCreacion = DateTime.Now.ToString(),
                    CamaUsada = false,
                    UrlImagen = "" // Dejamos sin imagen por ahora
                },
                new Cama {
                    NombreCama = "Cama 101-B",
                    EstadoCama = "Mantenimiento",
                    TipoCama = "Articulada",
                    HabitacionId = habitacion1.IdHabitacion,
                    FechaCreacion = DateTime.Now.ToString(),
                    CamaUsada = false,
                    UrlImagen = ""
                },
                new Cama {
                    NombreCama = "Cama 102-A",
                    EstadoCama = "Disponible",
                    TipoCama = "UCI",
                    HabitacionId = habitacion2.IdHabitacion,
                    FechaCreacion = DateTime.Now.ToString(),
                    CamaUsada = false,
                    UrlImagen = ""
                }
            };
            _db.Cama.AddRange(camas);

            // C. CREAR ENFERMEROS (Entidad, no usuario de Identity)
            var enfermeros = new List<Enfermero>
            {
                new Enfermero { NombreEnfermero = "Laura García", RolEnfermero = "Jefa de Planta" },
                new Enfermero { NombreEnfermero = "Pablo Ruiz", RolEnfermero = "Turno Noche" }
            };
            _db.Enfermero.AddRange(enfermeros);

            // D. CREAR PACIENTES
            var pacientes = new List<Paciente>
            {
                new Paciente {
                    StrNombrePaciente = "Juan Pérez", // Ajusta si cambiaste el nombre en el modelo
                    IntEdadPaciente = 45,
                    StrSexoPaciente = "Masculino",
                    ListEnfermedades = "Grippe",
                    ListTratamiento = "Paracetamol"
                },
                new Paciente {
                    StrNombrePaciente = "María López",
                    IntEdadPaciente = 32,
                    StrSexoPaciente = "Femenino",
                    ListEnfermedades = "Apendicitis",
                    ListTratamiento = "Cirugía"
                }
            };
            _db.Paciente.AddRange(pacientes);
            _db.SaveChanges();

            // E. CREAR OPERACIONES (Opcional, vinculando paciente y cama)
            // Solo si quieres tener operaciones de prueba al inicio
            /*
            var operacion = new Operacion {
                StrNombreOperacion = "Apendicectomía",
                StrEstadoOperacion = "Programada",
                StrFechaOperacion = DateTime.Now.AddDays(1).ToString(),
                PacienteId = pacientes[1].IdPaciente,
                CamaId = camas[2].IdCama
            };
            _db.Operacion.Add(operacion);
            */

            // GUARDADO FINAL
            _db.SaveChanges();
        }
    }
}