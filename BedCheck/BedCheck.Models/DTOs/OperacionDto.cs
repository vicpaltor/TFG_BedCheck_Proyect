using System.ComponentModel.DataAnnotations;

namespace BedCheck.Models.DTOs
{
    public class OperacionDto
    {
        public int IdOperacion { get; set; }

        [Required(ErrorMessage = "El nombre de la operación es obligatorio")]
        public string NombreOperacion { get; set; } // Mapear de StrNombreOperacion

        [Required(ErrorMessage = "El estado es obligatorio")]
        public string EstadoOperacion { get; set; } // Mapear de StrEstadoOperacion

        [Required]
        public string FechaOperacion { get; set; } // Mapear de StrFechaOperacion

        // Relaciones (IDs para guardar)
        [Required(ErrorMessage = "Debes seleccionar una cama")]
        public int CamaId { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un paciente")]
        public int PacienteId { get; set; }

        // Campos solo para VISUALIZAR en la Tabla (DataTables)
        // Estos no se guardan en BD, se rellenan con AutoMapper
        public string? NombrePaciente { get; set; }
        public string? NombreCama { get; set; }
    }
}
