using Microsoft.AspNetCore.Mvc.Rendering;

namespace BedCheck.Models.ViewModels
{
    public class CamaVM
    {
        // ANTES: public Cama Cama { get; set; }
        // AHORA:
        public BedCheck.Models.DTOs.CamaDto Cama { get; set; }
        public IEnumerable<SelectListItem> ListaHabitaciones { get; set; }
    }
}
