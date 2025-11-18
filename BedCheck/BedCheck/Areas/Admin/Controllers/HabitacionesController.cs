using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BedCheck.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class HabitacionesController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public HabitacionesController(IContenedorTrabajo contenedorTrabajo)
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Habitacion habitacion)
        //{
        //    if (ModelState.IsValid) 
        //    {
        //        //Logica para guardar en BD
        //        habitacion.CamasOcupadas = 0;
        //        habitacion.ListEnfermedadesTratamientos = new List<string>();
        //        _contenedorTrabajo.Habitacion.Add(habitacion);
        //        _contenedorTrabajo.Save();
        //        return RedirectToAction(nameof(Index));

        //    }
        //    return View(habitacion);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Habitacion habitacion)
        {
            // Verificar si ya existe una habitación con el mismo NumHabitacion
            var habitacionExistente = _contenedorTrabajo.Habitacion.GetAll(h => h.NumHabitacion == habitacion.NumHabitacion).FirstOrDefault();

            if (habitacionExistente != null)
            {
                // Si existe, agregar un error al modelo y retornar la vista con el mensaje de error
                ModelState.AddModelError("", "Ya existe esa habitacion.");
                return View(habitacion);
            }

            if (ModelState.IsValid)
            {
                habitacion.CamasOcupadas = 0;
                habitacion.ListEnfermedadesTratamientos = new List<string>();
                _contenedorTrabajo.Habitacion.Add(habitacion);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(habitacion);
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            Habitacion habitacion = new Habitacion();
            habitacion = _contenedorTrabajo.Habitacion.Get(id);
            if (habitacion == null) 
            {
                return NotFound();
            
            }
            return View(habitacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Habitacion habitacion, int id)
        {
            if (ModelState.IsValid)
            {
                //Logica para guardar en BD
                habitacion.CamasOcupadas = 0;
                habitacion.ListEnfermedadesTratamientos = new List<string>();
                habitacion.IdHabitacion = id;

                _contenedorTrabajo.Habitacion.Update(habitacion);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));

            }
            return View(habitacion);
        }

        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll() 
        {
            return Json(new { data = _contenedorTrabajo.Habitacion.GetAll() });
        
        }

        //[HttpDelete]
        //public IActionResult Delete(int id)
        //{
        //    var objFromDb = _contenedorTrabajo.Habitacion.Get(id);
        //    if (objFromDb == null) 
        //    {
        //        return Json(new { success = false, message = "Error borrando habitacion" });
        //    }

        //    _contenedorTrabajo.Habitacion.Remove(objFromDb);
        //    _contenedorTrabajo.Save();
        //    return Json(new { success = true, message = "Habitacion Borrada Correctamente" });
        //}

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            // Obtener la habitación de la base de datos
            var habitacionDesdeDb = _contenedorTrabajo.Habitacion.Get(id);

            // Verificar si la habitación existe
            if (habitacionDesdeDb == null)
            {
                return Json(new { success = false, message = "Error al intentar borrar la habitación." });
            }

            // Verificar si existen camas asignadas a la habitación
            var camasAsignadas = _contenedorTrabajo.Cama.GetAll(c => c.HabitacionId == id);

            if (camasAsignadas.Any())
            {
                // Si hay camas asignadas, no permitir la eliminación y devolver un mensaje de error
                return Json(new { success = false, message = "No se puede eliminar la habitación porque tiene camas asignadas." });
            }

            // Si no hay camas asignadas, proceder con la eliminación
            _contenedorTrabajo.Habitacion.Remove(habitacionDesdeDb);
            _contenedorTrabajo.Save();

            return Json(new { success = true, message = "Habitación eliminada correctamente." });
        }


        #endregion
    }
}
