using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedCheck.Models.ViewModels
{
    public class CamaVM
    {
        public Cama Cama { get; set; }
        public IEnumerable<SelectListItem> ListaHabitaciones { get; set; }
    }
}
