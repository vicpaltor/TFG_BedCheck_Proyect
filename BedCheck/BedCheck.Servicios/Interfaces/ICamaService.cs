using BedCheck.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BedCheck.Servicios.Interfaces
{
    public interface ICamaService
    {
        // Obtener datos
        Task<IEnumerable<CamaDto>> ObtenerTodas();
        Task<CamaDto> ObtenerPorId(int id);

        Task<string> Crear(CamaDto camaDto, IFormFile? imagen, string rutaRaizWeb);

        Task<bool> Actualizar(CamaDto camaDto, IFormFile? imagen, string rutaRaizWeb);

        Task<bool> Borrar(int id, string rutaRaizWeb);

        bool ExisteNombre(string nombre);
        bool HabitacionLlena(int habitacionId);

        IEnumerable<SelectListItem> ObtenerListaHabitaciones();
    }
}