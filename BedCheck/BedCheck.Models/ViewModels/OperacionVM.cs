using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedCheck.Models.ViewModels
{
    public class OperacionVM
    {
        public Operacion Operacion { get; set; }
        public IEnumerable<SelectListItem> ListaCamas { get; set; }

        public IEnumerable<SelectListItem> ListaPacientes { get; set; }
    }
}
