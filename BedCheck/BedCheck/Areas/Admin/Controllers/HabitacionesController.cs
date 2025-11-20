using BedCheck.Models.DTOs;
using BedCheck.Servicios.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BedCheck.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class HabitacionesController : Controller
    {
        private readonly IHabitacionService _servicio;

        public HabitacionesController(IHabitacionService servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(); // Esto espera que uses DataTables en la vista
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HabitacionDto dto)
        {
            if (ModelState.IsValid)
            {
                bool creado = await _servicio.Crear(dto);
                if (creado)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("NumHabitacion", "Ya existe una habitación con este número.");
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
        public async Task<IActionResult> Edit(HabitacionDto dto)
        {
            if (ModelState.IsValid)
            {
                await _servicio.Actualizar(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        // API PARA DATATABLES
        #region API
        [HttpGet("/Admin/Habitaciones/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var lista = await _servicio.ObtenerTodas();
            return Json(new { data = lista });
        }

        [HttpDelete("/Admin/Habitaciones/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool borrado = await _servicio.Borrar(id);
            if (borrado) return Json(new { success = true, message = "Borrado correctamente" });
            else return Json(new { success = false, message = "Error al borrar (quizás tiene camas)" });
        }
        #endregion
    }
}