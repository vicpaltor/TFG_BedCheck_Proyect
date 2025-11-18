using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Data;
using BedCheck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedCheck.AccesoDatos.Data.Repository
{
    public class ContenedorTrabajo : IContenedorTrabajo
    {
        private readonly ApplicationDbContext _db;
        public ContenedorTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Habitacion = new HabitacionRepository(_db);
            Cama = new CamaRepository(_db);
            Enfermero = new EnfermeroRepository(_db);
            Operacion = new OperacionRepository(_db);
            Paciente = new PacienteRepository(_db);
            Usuario = new UsuarioRepository(_db);
        }
        public IHabitacionRepositorio Habitacion {  get; private set; }
        public ICamaRepositorio Cama { get; private set; }
        public IEnfermeroRepositorio Enfermero { get; private set; }
        public IOperacionRepositorio Operacion { get; private set; }
        public IPacienteRepositorio Paciente { get; private set; }

        public IUsuarioRepository Usuario { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
