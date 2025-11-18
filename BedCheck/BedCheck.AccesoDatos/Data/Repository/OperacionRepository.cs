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
    internal class OperacionRepository : Repository<Operacion>, IOperacionRepositorio
    {
        private readonly ApplicationDbContext _db;
        public OperacionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Operacion operacion)
        {
            var operacionDesdeBb = _db.Operacion.FirstOrDefault(s => s.IdOperacion == operacion.IdOperacion);
            if (operacionDesdeBb == null)
            {
                throw new Exception("No se encontró el enfermero con el Id proporcionado.");
            }
            operacionDesdeBb.StrNombreOperacion = operacion.StrNombreOperacion;
            operacionDesdeBb.StrEstadoOperacion = operacion.StrEstadoOperacion;
            operacionDesdeBb.StrFechaOperacion = operacion.StrFechaOperacion;
            operacionDesdeBb.CamaId = operacion.CamaId;
            operacionDesdeBb.PacienteId = operacion.PacienteId;
        }
    }
}
