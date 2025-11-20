using BedCheck.Models.DTOs;
using BedCheck.Models.ViewModels;
using BedCheck.Servicios.Interfaces; // <--- Usamos la interfaz
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BedCheck.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class CamasController : Controller
    {

        private readonly ICamaService _camaService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CamasController(ICamaService camaService,IWebHostEnvironment hostingEnvironment)
        {
            _camaService = camaService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Create()
        {
            CamaVM camaVM = new CamaVM()
            {
                Cama = new CamaDto(),
                ListaHabitaciones = _camaService.ObtenerListaHabitaciones()
            };

            return View(camaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Create(CamaVM camaVM)
        {

            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                IFormFile? imagen = archivos.Count > 0 ? archivos[0] : null;

                string mensajeError = await _camaService.Crear(camaVM.Cama, imagen, _hostingEnvironment.WebRootPath);

                if (string.IsNullOrEmpty(mensajeError)) // Si no hay error
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Mostramos el mensaje específico que nos dio el servicio
                    ModelState.AddModelError("", mensajeError);
                }
            }

            camaVM.ListaHabitaciones = _camaService.ObtenerListaHabitaciones();

            return View(camaVM);
        }


        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var camaDto = await _camaService.ObtenerPorId(id.Value);
            if (camaDto == null) return NotFound();

            CamaVM camaVM = new CamaVM()
            {
                Cama = new CamaDto(),
                ListaHabitaciones = _camaService.ObtenerListaHabitaciones()
            };
            return View(camaVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Edit(CamaVM camaVM)
        {
            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                IFormFile? imagen = archivos.Count > 0 ? archivos[0] : null;

                // LLAMADA AL SERVICIO
                bool resultado = await _camaService.Actualizar(camaVM.Cama, imagen, _hostingEnvironment.WebRootPath);

                if (resultado) return RedirectToAction(nameof(Index));
            }

            camaVM.ListaHabitaciones = _camaService.ObtenerListaHabitaciones();

            return View(camaVM);
        }

        #region Llamadas a la API

        [HttpGet("/Admin/Camas/GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var lista = await _camaService.ObtenerTodas();
            return Json(new { data = lista });
        }


        [HttpDelete("/Admin/Camas/Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            bool borrado = await _camaService.Borrar(id, _hostingEnvironment.WebRootPath);

            if (borrado)
                return Json(new { success = true, message = "Eliminada correctamente" });
            else
                return Json(new { success = false, message = "Error al borrar (posiblemente en uso)" });
        }

        #endregion
    }
}