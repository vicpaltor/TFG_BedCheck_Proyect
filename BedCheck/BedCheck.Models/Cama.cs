using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedCheck.Models
{
    public class Cama
    {
        [Key]
        public int IdCama { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name ="Nombre de la cama")]
        public string NombreCama { get; set; }

        [Display(Name = "¿Esta usada la cama?")]
        public bool CamaUsada { get; set; }

        [Required(ErrorMessage = "El Estado es obligatorio")]
        [Display(Name = "Estado")]
        public string EstadoCama { get; set; }
        public int Operacion { get; set; }

        [Required(ErrorMessage = "El Tipo de cama es obligario")]
        [Display(Name = "Tipo de cama")]
        public string TipoCama { get; set;}

        [Required(ErrorMessage = "El Fecha de la cama es obligario")]
        [Display(Name = "Fecha de creacion de la cama")]
        public string FechaCreacion { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string UrlImagen { get; set; }

        //1..*
        [Required(ErrorMessage = "La habitacion es obligatoria")]
        [Display(Name = "Habitacion asignada de la cama")]
        public int HabitacionId { get; set; }
        [ForeignKey("HabitacionId")]
        public Habitacion Habitacion {  get; set; }
    }
}
