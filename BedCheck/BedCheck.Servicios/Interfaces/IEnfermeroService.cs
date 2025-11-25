using BedCheck.Models.DTOs;
using System.Threading.Tasks;

namespace BedCheck.Servicios.Interfaces
{
    public interface IEnfermeroService
    {
        /// <summary>
        /// Crea un nuevo enfermero en el sistema.
        /// </summary>
        /// <param name="dto">DTO con los datos del nuevo enfermero.</param>
        /// <returns>El Id (int) del nuevo enfermero creado.</returns>
        Task<int> Crear(EnfermeroDto dto);

        // Aquí irán los demás métodos (Obtener, Actualizar, Eliminar)
    }
}