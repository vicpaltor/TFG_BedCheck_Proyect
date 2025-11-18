using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedCheck.Models
{
    public class Operacion
    {
        [Key]
        public int IdOperacion { get; set; }

        [Required(ErrorMessage = "Ingrese el nombre de la operacion")]
        [Display(Name = "Nombre de la operacion")]
        public string StrNombreOperacion { get; set; }

        [Required(ErrorMessage = "Ingrese el estado de la operacion")]
        [Display(Name = "Estado de la operacion")]
        public string StrEstadoOperacion { get; set; }

        [Required(ErrorMessage = "Ingrese la fecha de la operacion")]
        [Display(Name = "Fecha de la operacion")]
        public string StrFechaOperacion { get; set; }

        ////1..*
        [Required(ErrorMessage = "La cama es obligatoria")]
        [Display(Name = "cama asignada")]
        public int CamaId { get; set; }
        [ForeignKey("CamaId")]
        public Cama Cama { get; set; }

        ////1..*
        ///// Nueva relación con Paciente
        [Required(ErrorMessage = "El paciente es obligatorio")]
        [Display(Name = "Paciente asignado")]
        public int PacienteId { get; set; }
        [ForeignKey("PacienteId")]
        public Paciente Paciente { get; set; }
    }
}
