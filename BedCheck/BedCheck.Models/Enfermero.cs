using System.ComponentModel.DataAnnotations;

namespace BedCheck.Models
{
    public class Enfermero
    {
        [Key]
        public int IdEnfermero { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre del enfermero")]
        public string NombreEnfermero { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        [Display(Name = "Rol del enfermero")]
        public string RolEnfermero { get; set; }
    }
}
