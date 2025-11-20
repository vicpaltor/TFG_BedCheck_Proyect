using BedCheck.Models.DTOs;

namespace BedCheck.Servicios.Interfaces
{
    public interface IHabitacionService
    {
        Task<IEnumerable<HabitacionDto>> ObtenerTodas();
        Task<HabitacionDto> ObtenerPorId(int id);
        Task<bool> Crear(HabitacionDto dto);
        Task<bool> Actualizar(HabitacionDto dto);
        Task<bool> Borrar(int id);
        bool ExisteNumero(int numero);
    }
}