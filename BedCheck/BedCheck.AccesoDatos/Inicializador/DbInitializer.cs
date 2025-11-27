using BedCheck.AccesoDatos.Data.Inicializador;
using BedCheck.Data;
using BedCheck.Models;
using BedCheck.Utilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            // 1. EJECUTAR MIGRACIONES PENDIENTES
            // Esto es vital para Docker: Si la BD no existe, la crea automáticamente.
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                // Aquí podrías loguear el error si quisieras
            }

            // 2. CREAR ROLES E USUARIOS
            // Si ya existe el rol de Administrador, significa que ya inicializamos la BD, así que salimos.
            if (_roleManager.RoleExistsAsync(CNT.Administrador).GetAwaiter().GetResult()) return;

            // Crear Roles
            _roleManager.CreateAsync(new IdentityRole(CNT.Administrador)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(CNT.Registrado)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(CNT.Cliente)).GetAwaiter().GetResult(); // O Cliente, según tu CNT

            // Crear Usuario Administrador por defecto
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@bedcheck.com",
                Email = "admin@bedcheck.com",
                EmailConfirmed = true,
                Nombre = "Administrador Sistema",
            }, "Admin123*").GetAwaiter().GetResult();

            // Asignar Rol al Admin
            // 5. Asignar Rol al Admin
            ApplicationUser usuario = _userManager.FindByEmailAsync("admin@bedcheck.com").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(usuario, CNT.Administrador).GetAwaiter().GetResult();
        }
    }

}