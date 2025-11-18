using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedCheck.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Habitacion> ListaHabitaciones { get; set; }

        public IEnumerable<Cama> ListaCamas { get; set; }

        public IEnumerable<Operacion> ListaOperaciones { get; set; }
    }
}
