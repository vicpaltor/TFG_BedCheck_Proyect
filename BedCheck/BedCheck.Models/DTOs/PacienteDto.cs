using System.ComponentModel.DataAnnotations;

namespace BedCheck.Models.DTOs
{
    public class PacienteDto
    {
        public int IdPaciente { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required]
        [Range(0, 120, ErrorMessage = "Edad no válida")]
        public int Edad { get; set; }

        [Required]
        public string Sexo { get; set; }

        public string? Enfermedades { get; set; }
        public string? Tratamientos { get; set; }
    }
}