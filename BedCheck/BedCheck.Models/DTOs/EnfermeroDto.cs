// Dentro de BedCheck.Models/DTOs/EnfermeroDto.cs
using System.ComponentModel.DataAnnotations;

namespace BedCheck.Models.DTOs
{
    // Usamos el nombre simplificado que indicaste
    public class EnfermeroDto // Antes: CrearEnfermeroDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string NombreEnfermero { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        public string RolEnfermero { get; set; }
    }
}
