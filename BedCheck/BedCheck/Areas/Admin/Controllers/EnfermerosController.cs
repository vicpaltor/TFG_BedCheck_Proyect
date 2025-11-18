using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BedCheck.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class EnfermerosController : Controller
    {

        private readonly IContenedorTrabajo _contenedorTrabajo;

        public EnfermerosController(IContenedorTrabajo contenedorTrabajo)
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
        public IActionResult Create(Enfermero enfermero)
        {
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.Enfermero.Add(enfermero);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));

            }
            return View(enfermero);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Enfermero enfermero = new Enfermero();
            enfermero = _contenedorTrabajo.Enfermero.Get(id);
            if (enfermero == null)
            {
                return NotFound();

            }
            return View(enfermero);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Enfermero enfermero, int id)
        {
            if (ModelState.IsValid)
            {
                //Logica para guardar en BD
                //habitacion.CamasOcupadas = 0;
                //habitacion.ListEnfermedadesTratamientos = new List<string>();

                enfermero.IdEnfermero = id;

                _contenedorTrabajo.Enfermero.Update(enfermero);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));

            }
            return View(enfermero);
        }

        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll() 
        {
            return Json(new { data = _contenedorTrabajo.Enfermero.GetAll() });
        
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _contenedorTrabajo.Enfermero.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error borrando enfermero" });
            }

            _contenedorTrabajo.Enfermero.Remove(objFromDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Enfermero Borrado Correctamente" });
        }
        #endregion

    }
}
