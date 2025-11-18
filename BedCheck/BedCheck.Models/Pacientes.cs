using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedCheck.Models
{
    public class Paciente
    {
        [Key]
        public int IdPaciente { get; set; }

        [Required(ErrorMessage = "Ingrese el nombre del paciente")]
        [Display(Name = "Nombre del paciente")]
        public string StrNombrePaciente { get; set; }

        [Required(ErrorMessage = "Ingrese la edad del paciente")]
        [Display(Name = "Edad del paciente")]
        [Range(0, int.MaxValue, ErrorMessage = "El total de camas debe ser mayor a 0")]
        public int? IntEdadPaciente { get; set; }

        [Required(ErrorMessage = "Ingrese el sexo del paciente")]
        [Display(Name = "Sexo del paciente")]
        public string StrSexoPaciente { get; set; }

        [Required(ErrorMessage = "Ingrese la enfermedad del paciente")]
        [Display(Name = "Enfermedad del paciente")]
        public string ListEnfermedades { get; set; }

        [Required(ErrorMessage = "Ingrese el tratamiento del paciente")]
        [Display(Name = "Tratamiento del paciente")]
        public string ListTratamiento { get; set; }

    }
}
