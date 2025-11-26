using BedCheck.Models.DTOs;

namespace BedCheck.Servicios.Interfaces
{
    public interface IEnfermeroService
    {
        // Crear
        Task<int> Crear(EnfermeroDto dto);

        // Lectura
        Task<IEnumerable<EnfermeroDto>> ObtenerTodos();
        Task<EnfermeroDto> ObtenerPorId(int id);

        // Modificar
        Task<bool> Actualizar(EnfermeroDto dto);

        // Eliminar
        Task<bool> Borrar(int id);
    }
}