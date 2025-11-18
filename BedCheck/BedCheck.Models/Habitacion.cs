using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedCheck.Models
{
    public class Habitacion
    {
        [Key]
        public int IdHabitacion { get; set; }
        [Required(ErrorMessage = "Ingrese el numero de la habitacion")]
        [Display(Name = "Numero de la habitacion")]
        [Range(1, int.MaxValue, ErrorMessage = "El numero de la habitacion debe ser mayor a 0")]
        public int? NumHabitacion { get; set; }
        [Required(ErrorMessage = "Ingrese el total de camas")]
        [Display(Name = "Numero de Camas de la habitacion")]
        [Range(0, int.MaxValue, ErrorMessage = "El total de camas debe ser mayor a 0")]
        public int? NumCamasTotales { get; set; }

        [Display(Name = "Camas ocupadas de la habitacion")]
        [Range(0, int.MaxValue, ErrorMessage = "El total de camas ocupadas debe ser un número positivo")]
        public int? CamasOcupadas { get; set; }
        public List<string>? ListEnfermedadesTratamientos { get; set; }

    }
}
