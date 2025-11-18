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
    internal class CamaRepository : Repository<Cama>, ICamaRepositorio
    {
        private readonly ApplicationDbContext _db;
        public CamaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetListaOperaciones()
        {
            return _db.Cama.Select(c => new SelectListItem() 
            {
                Text = c.NombreCama,
                Value = c.IdCama.ToString()
            });
        }
        public void Update(Cama cama)
        {
            var objDesdeDb = _db.Cama.FirstOrDefault(s => s.IdCama == cama.IdCama);
            if (objDesdeDb == null)
            {
                throw new Exception("No se encontró la habitación con el Id proporcionado.");
            }
            objDesdeDb.NombreCama = cama.NombreCama;
            objDesdeDb.CamaUsada = cama.CamaUsada;
            objDesdeDb.EstadoCama = cama.EstadoCama;
            objDesdeDb.Operacion = cama.Operacion;
            objDesdeDb.TipoCama = cama.TipoCama;
            objDesdeDb.FechaCreacion = cama.FechaCreacion;
            objDesdeDb.UrlImagen = cama.UrlImagen;
            objDesdeDb.HabitacionId = cama.HabitacionId;
        }
    }
}
