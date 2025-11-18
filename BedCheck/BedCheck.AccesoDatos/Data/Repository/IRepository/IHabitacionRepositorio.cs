using BedCheck.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedCheck.AccesoDatos.Data.Repository.IRepository
{
    public interface IHabitacionRepositorio : IRepository<Habitacion>
    {
        void Update(Habitacion habitacion);
        IEnumerable<SelectListItem> GetListaHabitaciones();
    }
}
