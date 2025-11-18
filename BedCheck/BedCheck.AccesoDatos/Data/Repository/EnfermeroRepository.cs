using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Data;
using BedCheck.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedCheck.AccesoDatos.Data.Repository
{
    internal class EnfermeroRepository : Repository<Enfermero>, IEnfermeroRepositorio
    {
        private readonly ApplicationDbContext _db;
        public EnfermeroRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Enfermero enfermero)
        {
            var enfermeroDesdeBb = _db.Enfermero.FirstOrDefault(s => s.IdEnfermero == enfermero.IdEnfermero);
            if (enfermeroDesdeBb == null)
            {
                throw new Exception("No se encontró el enfermero con el Id proporcionado.");
            }
            enfermeroDesdeBb.NombreEnfermero = enfermero.NombreEnfermero;
            enfermeroDesdeBb.RolEnfermero = enfermero.RolEnfermero;
        }
    }
}
