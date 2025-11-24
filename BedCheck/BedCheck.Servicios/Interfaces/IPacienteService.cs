using BedCheck.Models.DTOs;

namespace BedCheck.Servicios.Interfaces
{
    public interface IPacienteService
    {
        Task<IEnumerable<PacienteDto>> ObtenerTodos();
        Task<PacienteDto> ObtenerPorId(int id);
        Task<bool> Crear(PacienteDto dto);
        Task<bool> Actualizar(PacienteDto dto);
        Task<bool> Borrar(int id);

        // Validación extra si quieres
        // bool ExistePaciente(string nombre); 
    }
}