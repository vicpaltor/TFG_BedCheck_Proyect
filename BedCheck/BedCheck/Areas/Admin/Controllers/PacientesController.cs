using BedCheck.Models.DTOs;
using BedCheck.Servicios.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BedCheck.Utilidades;

namespace BedCheck.Areas.Admin.Controllers
{
    [Authorize(Roles = CNT.Administrador)]
    [Area("Admin")]
    public class PacientesController : Controller
    {
        private readonly IPacienteService _servicio;

        public PacientesController(IPacienteService servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Create(PacienteDto dto)
        {
            if (ModelState.IsValid)
            {
                bool creado = await _servicio.Crear(dto);
                if (creado) return RedirectToAction(nameof(Index));
                // Aquí podrías añadir ModelState.AddModelError si el servicio devolviera false por duplicado
            }
            return View(dto);
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var dto = await _servicio.ObtenerPorId(id.Value);
            if (dto == null) return NotFound();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Edit(PacienteDto dto)
        {
            if (ModelState.IsValid)
            {
                await _servicio.Actualizar(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        #region API DataTables
        [HttpGet("/Admin/Pacientes/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var lista = await _servicio.ObtenerTodos();
            return Json(new { data = lista });
        }

        [HttpDelete("/Admin/Pacientes/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool borrado = await _servicio.Borrar(id);
            if (borrado) return Json(new { success = true, message = "Paciente eliminado correctamente" });
            return Json(new { success = false, message = "Error al eliminar" });
        }
        #endregion
    }
}