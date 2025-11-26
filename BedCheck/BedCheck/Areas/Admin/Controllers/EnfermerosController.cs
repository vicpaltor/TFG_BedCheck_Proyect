using BedCheck.Models.DTOs;
using BedCheck.Servicios.Interfaces;
using BedCheck.Utilidades; // Para CNT
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BedCheck.Areas.Admin.Controllers
{
    [Authorize(Roles = CNT.Administrador)]
    [Area("Admin")]
    public class EnfermerosController : Controller
    {
        private readonly IEnfermeroService _servicio;

        public EnfermerosController(IEnfermeroService servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public IActionResult Index() { return View(); }

        [HttpGet]
        public IActionResult Create() { return View(); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnfermeroDto dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _servicio.Crear(dto);
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var dto = await _servicio.ObtenerPorId(id.Value);
            if (dto == null) return NotFound();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EnfermeroDto dto)
        {
            if (ModelState.IsValid)
            {
                await _servicio.Actualizar(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        #region API
        [HttpGet("/Admin/Enfermeros/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var lista = await _servicio.ObtenerTodos();
            return Json(new { data = lista });
        }

        [HttpDelete("/Admin/Enfermeros/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var borrado = await _servicio.Borrar(id);
            if (borrado) return Json(new { success = true, message = "Borrado correctamente" });
            return Json(new { success = false, message = "Error al borrar" });
        }
        #endregion
    }
}