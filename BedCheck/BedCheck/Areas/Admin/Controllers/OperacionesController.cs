using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using BedCheck.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace BedCheck.Areas.Admin.Controllers
{
    [Authorize(Roles = "Enfermero")]
    [Area("Admin")]
    public class OperacionesController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public OperacionesController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostingEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            OperacionVM operacionVM = new OperacionVM()
            {
                Operacion = new BedCheck.Models.Operacion()
                {
                    CamaId = id // Preseleccionar la cama con el ID
                },
                ListaCamas = _contenedorTrabajo.Cama.GetListaOperaciones(),
                ListaPacientes = _contenedorTrabajo.Paciente.GetListaOperaciones()
            };
            return View(operacionVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Operacion operacion)
        {
            //Esto es para evitar que el ModelState no haga la validacion correcta
            if (ModelState.ContainsKey("Operacion.Cama"))
            {
                ModelState.Remove("Operacion.Cama");
            }

            if (ModelState.ContainsKey("Operacion.Paciente"))
            {
                ModelState.Remove("Operacion.Paciente");
            }

            // Verificar si la cama ya tiene una operación asignada
            var operacionesAsignadas = _contenedorTrabajo.Operacion.GetAll(o => o.CamaId == operacion.CamaId).ToList();
            if (operacionesAsignadas.Any())
            {
                ModelState.AddModelError("Operacion.CamaId", "La cama ya tiene una operación asignada.");
            }

            if (ModelState.IsValid)
            {
                ////////////////////////Obtener la cama asociada a la operación
                var cama = _contenedorTrabajo.Cama.Get(operacion.CamaId);
                if (cama != null)
                {
                    // Marcar la cama como usada
                    cama.CamaUsada = true;
                    _contenedorTrabajo.Cama.Update(cama);
                }

                /////////////////////// Actualizar la lista de enfermedades y tratamientos de la habitación
                var habitacion = _contenedorTrabajo.Habitacion.Get(cama.HabitacionId);
                if (habitacion != null)
                {
                    var paciente = _contenedorTrabajo.Paciente.Get(operacion.PacienteId);
                    if (paciente != null)
                    {
                        // Combinar enfermedades y tratamientos en un solo formato
                        var enfermedadesYTratamientos = new List<string>();

                        if (!string.IsNullOrEmpty(paciente.ListEnfermedades))
                        {
                            enfermedadesYTratamientos.Add("" + paciente.ListEnfermedades);
                        }

                        if (!string.IsNullOrEmpty(paciente.ListTratamiento))
                        {
                            enfermedadesYTratamientos.Add("" + paciente.ListTratamiento);
                        }

                        habitacion.ListEnfermedadesTratamientos.AddRange(enfermedadesYTratamientos);
                        _contenedorTrabajo.Habitacion.Update(habitacion);
                    }
                }

                _contenedorTrabajo.Operacion.Add(operacion);
                _contenedorTrabajo.Save();
                // Redirigir al Index del controlador Home en el área Empleado después de editar
                return RedirectToAction("Index", "Home", new { area = "Empleado" });
                //return RedirectToAction(nameof(Index));
            }

            // Si hay errores, devolver el ViewModel con los datos actuales
            OperacionVM operacionVM = new OperacionVM()
            {
                Operacion = operacion,
                ListaCamas = _contenedorTrabajo.Cama.GetListaOperaciones(),
                ListaPacientes = _contenedorTrabajo.Paciente.GetListaOperaciones()
            };

            return View(operacionVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            OperacionVM operacionVM = new OperacionVM()
            {
                Operacion = new BedCheck.Models.Operacion(),
                ListaCamas = _contenedorTrabajo.Cama.GetListaOperaciones(),
                ListaPacientes = _contenedorTrabajo.Paciente.GetListaOperaciones()
            };

            if (id != null)
            {
                operacionVM.Operacion = _contenedorTrabajo.Operacion.Get(id.GetValueOrDefault());
            }

            return View(operacionVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(OperacionVM operacionVM)
        {
            if (!(string.IsNullOrEmpty(operacionVM.Operacion.StrNombreOperacion) &&
                  string.IsNullOrEmpty(operacionVM.Operacion.StrEstadoOperacion) &&
                  string.IsNullOrEmpty(operacionVM.Operacion.StrFechaOperacion) &&
                  string.IsNullOrEmpty(operacionVM.Operacion.CamaId.ToString()) &&
                  string.IsNullOrEmpty(operacionVM.Operacion.PacienteId.ToString())))

            {
                // Recuperar la operación sin seguimiento (no rastreada)
                var operacionDesdeBd = _contenedorTrabajo.Operacion
                        .GetAll()
                        .AsQueryable()
                        .Where(o => o.IdOperacion == operacionVM.Operacion.IdOperacion)
                        .AsNoTracking() // No rastrear esta entidad
                        .FirstOrDefault();

                // Verificar si la operación fue encontrada en la base de datos
                if (operacionDesdeBd == null)
                {
                    ModelState.AddModelError("", "La operación no fue encontrada.");
                    operacionVM.ListaCamas = _contenedorTrabajo.Cama.GetListaOperaciones();
                    operacionVM.ListaPacientes = _contenedorTrabajo.Paciente.GetListaOperaciones();
                    return View(operacionVM);
                }

                var cama = _contenedorTrabajo.Cama.Get(operacionVM.Operacion.CamaId);
                if (cama != null)
                {
                    var habitacion = _contenedorTrabajo.Habitacion.Get(cama.HabitacionId);
                    if (habitacion != null)
                    {
                        habitacion.ListEnfermedadesTratamientos.Clear(); // Limpiar la lista actual

                        var paciente = _contenedorTrabajo.Paciente.Get(operacionVM.Operacion.PacienteId);
                        if (paciente != null)
                        {
                            var enfermedadesYTratamientos = new List<string>();

                            if (!string.IsNullOrEmpty(paciente.ListEnfermedades))
                            {
                                enfermedadesYTratamientos.Add(paciente.ListEnfermedades);
                            }

                            if (!string.IsNullOrEmpty(paciente.ListTratamiento))
                            {
                                enfermedadesYTratamientos.Add(paciente.ListTratamiento);
                            }

                            habitacion.ListEnfermedadesTratamientos.AddRange(enfermedadesYTratamientos);
                            _contenedorTrabajo.Habitacion.Update(habitacion);
                        }
                    }
                }

                // Actualizar la operación en la base de datos
                try
                {
                    _contenedorTrabajo.Operacion.Update(operacionVM.Operacion);
                    _contenedorTrabajo.Save();
                }
                catch (InvalidOperationException ex)
                {
                    // Si ocurre el error de seguimiento, desattach la entidad previamente rastreada
                    _contenedorTrabajo.Operacion.DetachEntity(operacionDesdeBd);
                    _contenedorTrabajo.Operacion.Update(operacionVM.Operacion);
                    _contenedorTrabajo.Save();
                }

                // Verificar si el estado es "Disponible" después de actualizar
                if (operacionVM.Operacion.StrEstadoOperacion == "Disponible")
                {

                    // Marcar la cama como no usada
                    cama.CamaUsada = false;
                    _contenedorTrabajo.Cama.Update(cama);


                    // Limpiar la lista de enfermedades y tratamientos de la habitación
                    var habitacion = _contenedorTrabajo.Habitacion.Get(cama.HabitacionId);
                    if (habitacion != null)
                    {
                        habitacion.ListEnfermedadesTratamientos = new List<string>(); // Asignar lista vacía
                        _contenedorTrabajo.Habitacion.Update(habitacion);
                    }




                    // Eliminar la operación
                    _contenedorTrabajo.Operacion.DetachEntity(operacionDesdeBd); // Desvincular la operación antes de eliminarla
                    _contenedorTrabajo.Operacion.Remove(operacionVM.Operacion);
                    _contenedorTrabajo.Save();
                }

                // Redirigir al Index del controlador Home en el área Empleado después de editar
                return RedirectToAction("Index", "Home", new { area = "Empleado" });
            }

            // Si el modelo no es válido, mostrar de nuevo el formulario con los datos actuales
            operacionVM.ListaCamas = _contenedorTrabajo.Cama.GetListaOperaciones();
            operacionVM.ListaPacientes = _contenedorTrabajo.Paciente.GetListaOperaciones();
            return View(operacionVM);
        }

        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Operacion.GetAll(includeProperties: "Cama,Paciente") });

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _contenedorTrabajo.Operacion.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error borrando operacion" });
            }

            // Obtener la cama asociada y marcarla como no usada
            var cama = _contenedorTrabajo.Cama.Get(objFromDb.CamaId);
            if (cama != null)
            {
                cama.CamaUsada = false;
                _contenedorTrabajo.Cama.Update(cama);
            }

            var habitacion = _contenedorTrabajo.Habitacion.Get(cama.HabitacionId);
            if (habitacion != null)
            {
                habitacion.ListEnfermedadesTratamientos.Clear(); // Eliminar las enfermedades y tratamientos
                _contenedorTrabajo.Habitacion.Update(habitacion);
            }

            _contenedorTrabajo.Operacion.Remove(objFromDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Operacion Borrado Correctamente" });
        }
        #endregion

    }
}
