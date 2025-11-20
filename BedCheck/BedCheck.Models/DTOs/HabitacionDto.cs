using System.ComponentModel.DataAnnotations;

namespace BedCheck.Models.DTOs
{
    public class HabitacionDto
    {
        public int IdHabitacion { get; set; }

        [Required(ErrorMessage = "El número de habitación es obligatorio")]
        [Range(1, 9999, ErrorMessage = "Número inválido")]
        public int NumHabitacion { get; set; }

        [Required(ErrorMessage = "La capacidad es obligatoria")]
        [Range(1, 10, ErrorMessage = "La capacidad debe estar entre 1 y 10 camas")]
        public int NumCamasTotales { get; set; }

        public string? ListEnfermedadesTratamientos { get; set; }

        public int CamasOcupadas { get; set; }
    }
}