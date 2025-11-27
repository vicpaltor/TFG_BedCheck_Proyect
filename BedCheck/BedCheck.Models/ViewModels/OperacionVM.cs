using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using BedCheck.Models.DTOs;

namespace BedCheck.Models.ViewModels
{
    public class OperacionVM
    {
        public OperacionDto Operacion { get; set; } // Ahora usa el DTO

        [ValidateNever]
        public IEnumerable<SelectListItem> ListaCamas { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ListaPacientes { get; set; }
    }
}
