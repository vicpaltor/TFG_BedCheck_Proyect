using BedCheck.Models.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectListItem

namespace BedCheck.Servicios.Interfaces
{
    public interface IOperacionService
    {
        Task<IEnumerable<OperacionDto>> ObtenerTodas();
        Task<OperacionDto> ObtenerPorId(int id);

        // Devuelve string con mensaje de error, o vacío si éxito
        Task<string> Crear(OperacionDto dto);
        Task<string> Actualizar(OperacionDto dto);
        Task<bool> Borrar(int id);

        // Métodos para llenar los desplegables
        IEnumerable<SelectListItem> ObtenerListaCamasLibres();
        IEnumerable<SelectListItem> ObtenerListaPacientes();
    }
}