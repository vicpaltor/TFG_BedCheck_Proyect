using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BedCheck.Areas.Admin.Controllers
{
    [Authorize(Roles = "Enfermero")]
    [Area("Admin")]
    public class PacientesController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public PacientesController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.Paciente.Add(paciente);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));

            }
            return View(paciente);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Paciente paciente = new Paciente();
            paciente = _contenedorTrabajo.Paciente.Get(id);
            if (paciente == null)
            {
                return NotFound();

            }
            return View(paciente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Paciente paciente, int id)
        {
            if (ModelState.IsValid)
            {
                paciente.IdPaciente = id;

                _contenedorTrabajo.Paciente.Update(paciente);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));

            }
            return View(paciente);
        }

        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Paciente.GetAll() });

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _contenedorTrabajo.Paciente.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error borrando paciente" });
            }

            _contenedorTrabajo.Paciente.Remove(objFromDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Paciente Borrado Correctamente" });
        }
        #endregion
    }
}
