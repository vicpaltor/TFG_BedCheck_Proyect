using BedCheck.Models.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectListItem

namespace BedCheck.Servicios.Interfaces
{
    public interface IOperacionService
    {
        Task<IEnumerable<OperacionDto>> ObtenerTodas();
        Task<OperacionDto> ObtenerPorId(int id);
        Task<bool> Crear(OperacionDto dto);
        Task<bool> Actualizar(OperacionDto dto);
        Task<bool> Borrar(int id);

        // Métodos para llenar los desplegables en la vista
        IEnumerable<SelectListItem> ObtenerListaPacientes();
        IEnumerable<SelectListItem> ObtenerListaCamas();
    }
}