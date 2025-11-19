using System.ComponentModel.DataAnnotations;

namespace BedCheck.Models.DTOs
{
    public class CamaDto
    {
        public int IdCama { get; set; }

        [Required(ErrorMessage = "El nombre de la cama es obligatorio")]
        public string NombreCama { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        public string EstadoCama { get; set; }

        [Required(ErrorMessage = "El tipo de cama es obligatorio")]
        public string TipoCama { get; set; }

        public string UrlImagen { get; set; } // Si tienes imagen, es útil en el DTO

        public int HabitacionId { get; set; }
    }
}