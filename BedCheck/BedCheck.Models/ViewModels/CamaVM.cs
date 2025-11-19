using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BedCheck.Models.ViewModels
{
    public class CamaVM
    {
        public BedCheck.Models.DTOs.CamaDto Cama { get; set; }
        
        [ValidateNever]
        public IEnumerable<SelectListItem> ListaHabitaciones { get; set; }
    }
}
