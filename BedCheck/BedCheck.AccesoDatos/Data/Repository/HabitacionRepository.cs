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
    public class HabitacionRepository : Repository<Habitacion>, IHabitacionRepositorio
    {
        private readonly ApplicationDbContext _db;
        public HabitacionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetListaHabitaciones()
        {
            return _db.Habitacion.Select(h => new SelectListItem() 
            { 
                Text = h.NumHabitacion.ToString(),
                Value = h.IdHabitacion.ToString()
            
            });
        }
        public void Update(Habitacion habitacion)
        {
            var objDesdeDb = _db.Habitacion.FirstOrDefault(s => s.IdHabitacion == habitacion.IdHabitacion);
            if (objDesdeDb == null)
            {
                throw new Exception("No se encontró la habitación con el Id proporcionado.");
            }
            objDesdeDb.NumHabitacion = habitacion.NumHabitacion;
            objDesdeDb.CamasOcupadas = habitacion.CamasOcupadas;
            objDesdeDb.NumCamasTotales = habitacion.NumCamasTotales;
            objDesdeDb.ListEnfermedadesTratamientos = habitacion.ListEnfermedadesTratamientos;
        }
    }
}
