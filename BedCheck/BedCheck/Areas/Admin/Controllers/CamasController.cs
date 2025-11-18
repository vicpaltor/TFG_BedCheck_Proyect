using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using BedCheck.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;

namespace BedCheck.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class CamasController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CamasController(IContenedorTrabajo contenedorTrabajo,
            IWebHostEnvironment hostingEnvironment)
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
        public IActionResult Create()
        {
            CamaVM camaVM = new CamaVM()
            {
                Cama = new BedCheck.Models.Cama(),
                ListaHabitaciones = _contenedorTrabajo.Habitacion.GetListaHabitaciones()
            };
            return View(camaVM);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(CamaVM camaVM)
        //{
        //    // Obtener la habitación seleccionada para validar el número de camas
        //    var habitacion = _contenedorTrabajo.Habitacion.GetFirstOrDefault(h => h.IdHabitacion == camaVM.Cama.HabitacionId);
        //    if (habitacion == null)
        //    {
        //        ModelState.AddModelError("HabitacionId", "La habitación seleccionada no existe.");
        //    }
        //    else
        //    {
        //        // Verificar si ya se alcanzó el número máximo de camas en la habitación
        //        var camasAsignadas = _contenedorTrabajo.Cama.GetAll(c => c.HabitacionId == habitacion.IdHabitacion).Count();
        //        if (camasAsignadas >= habitacion.NumCamasTotales)
        //        {
        //            ModelState.AddModelError("Cama.HabitacionId", "Esta habitación ya ha alcanzado el número máximo de camas permitidas.");
        //            camaVM.ListaHabitaciones = _contenedorTrabajo.Habitacion.GetListaHabitaciones();
        //            return View(camaVM); // Esto detendrá el proceso y mostrará el error en la vista
        //        }

        //    }

        //    if (!(string.IsNullOrEmpty(camaVM.Cama.NombreCama) &&
        //        string.IsNullOrEmpty(camaVM.Cama.EstadoCama) &&
        //        string.IsNullOrEmpty(camaVM.Cama.TipoCama) &&
        //        string.IsNullOrEmpty(camaVM.Cama.FechaCreacion) &&
        //        string.IsNullOrEmpty(camaVM.Cama.HabitacionId.ToString())))
        //        //if (ModelState.IsValid)
        //    {
        //        string rutaPrincipal = _hostingEnvironment.WebRootPath;
        //        var archivos = HttpContext.Request.Form.Files;

        //        if (camaVM.Cama.IdCama == 0 && archivos.Count() > 0)
        //        {
        //            //Nueva cama//
        //            //Es un entero de 128 bits para usarlo como su nombre
        //            string nombreArchivo = Guid.NewGuid().ToString();
        //            var subidas = Path.Combine(rutaPrincipal, @"imagenes\camas");
        //            var extension = Path.GetExtension(archivos[0].FileName);

        //            using (var fileStremas = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
        //            {
        //                archivos[0].CopyTo(fileStremas);
        //            }
        //            //esta se encarga de guardar la ruta de la imagen en BBDD en UrlImagen
        //            camaVM.Cama.UrlImagen = @"\imagenes\camas\" + nombreArchivo + extension;
        //            camaVM.Cama.FechaCreacion = DateTime.Now.ToString();

        //            _contenedorTrabajo.Cama.Add(camaVM.Cama);
        //            _contenedorTrabajo.Save();

        //            return RedirectToAction(nameof(Index));
        //        }
        //        else 
        //        {
        //            //error personalizado
        //            ModelState.AddModelError("Cama.UrlImagen", "Debes seleccionar una imagen");
        //        }
        //    }
        //    camaVM.ListaHabitaciones = _contenedorTrabajo.Habitacion.GetListaHabitaciones();
        //        return View(camaVM);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CamaVM camaVM)
        {
            // Verificar si ya existe una cama con el mismo NombreCama
            var camaExistente = _contenedorTrabajo.Cama.GetAll(c => c.NombreCama == camaVM.Cama.NombreCama).FirstOrDefault();
            if (camaExistente != null)
            {
                // Si ya existe una cama con el mismo nombre, mostrar error y retornar a la vista
                ModelState.AddModelError("Cama.NombreCama", "Ya existe una cama con ese nombre.");
                camaVM.ListaHabitaciones = _contenedorTrabajo.Habitacion.GetListaHabitaciones();
                return View(camaVM);
            }

            // Obtener la habitación seleccionada para validar el número de camas
            var habitacion = _contenedorTrabajo.Habitacion.GetFirstOrDefault(h => h.IdHabitacion == camaVM.Cama.HabitacionId);
            if (habitacion == null)
            {
                ModelState.AddModelError("HabitacionId", "La habitación seleccionada no existe.");
            }
            else
            {
                // Verificar si ya se alcanzó el número máximo de camas en la habitación
                var camasAsignadas = _contenedorTrabajo.Cama.GetAll(c => c.HabitacionId == habitacion.IdHabitacion).Count();
                if (camasAsignadas >= habitacion.NumCamasTotales)
                {
                    ModelState.AddModelError("Cama.HabitacionId", "Esta habitación ya ha alcanzado el número máximo de camas permitidas.");
                    camaVM.ListaHabitaciones = _contenedorTrabajo.Habitacion.GetListaHabitaciones();
                    return View(camaVM);
                }
            }

            if (!(string.IsNullOrEmpty(camaVM.Cama.NombreCama) &&
                  string.IsNullOrEmpty(camaVM.Cama.EstadoCama) &&
                  string.IsNullOrEmpty(camaVM.Cama.TipoCama) &&
                  string.IsNullOrEmpty(camaVM.Cama.FechaCreacion) &&
                  string.IsNullOrEmpty(camaVM.Cama.HabitacionId.ToString())))
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                if (camaVM.Cama.IdCama == 0 && archivos.Count() > 0)
                {
                    // Nueva cama //
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\camas");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    using (var fileStremas = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStremas);
                    }

                    // Guardar la ruta de la imagen en BBDD en UrlImagen
                    camaVM.Cama.UrlImagen = @"\imagenes\camas\" + nombreArchivo + extension;
                    camaVM.Cama.FechaCreacion = DateTime.Now.ToString();

                    _contenedorTrabajo.Cama.Add(camaVM.Cama);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Error personalizado si no se selecciona una imagen
                    ModelState.AddModelError("Cama.UrlImagen", "Debes seleccionar una imagen");
                }
            }

            camaVM.ListaHabitaciones = _contenedorTrabajo.Habitacion.GetListaHabitaciones();
            return View(camaVM);
        }


        [HttpGet]
        public IActionResult Edit(int? id) 
        {
            CamaVM camaVM = new CamaVM()
            {
                Cama = new BedCheck.Models.Cama(),
                ListaHabitaciones = _contenedorTrabajo.Habitacion.GetListaHabitaciones()
            };
            if (id != null) 
            {
                camaVM.Cama = _contenedorTrabajo.Cama.Get(id.GetValueOrDefault());
            }
            return View(camaVM);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CamaVM camaVM)
        {
            if (!(string.IsNullOrEmpty(camaVM.Cama.NombreCama) &&
                string.IsNullOrEmpty(camaVM.Cama.EstadoCama) &&
                string.IsNullOrEmpty(camaVM.Cama.TipoCama) &&
                string.IsNullOrEmpty(camaVM.Cama.FechaCreacion) &&
                string.IsNullOrEmpty(camaVM.Cama.HabitacionId.ToString())))
            //if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                var camaDesdeBd = _contenedorTrabajo.Cama.Get(camaVM.Cama.IdCama);
                if (archivos.Count() > 0)
                {
                    //Nueva imagen para la cama//
                    //Es un entero de 128 bits para usarlo como su nombre
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\camas");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    var nuevoExtension = Path.GetExtension(archivos[0].FileName);

                    var rutaImagen = Path.Combine(rutaPrincipal, camaDesdeBd.UrlImagen.TrimStart('\\'));

                    //VALIDACION comprobamos si existe el fichero y si existe la eliminamos del BBDD
                    if (System.IO.File.Exists(rutaImagen)) 
                    { 
                        System.IO.File.Delete(rutaImagen);
                    }

                    //Nuevamente subimos el archivo
                    using (var fileStremas = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStremas);
                    }
                    //esta se encarga de guardar la ruta de la imagen en BBDD en UrlImagen
                    camaVM.Cama.UrlImagen = @"\imagenes\camas\" + nombreArchivo + extension;
                    camaVM.Cama.FechaCreacion = DateTime.Now.ToString();

                    _contenedorTrabajo.Cama.Update(camaVM.Cama);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Aqui seria cuando la imagen ya existe y se conserva
                    camaVM.Cama.UrlImagen = camaDesdeBd.UrlImagen;

                }

                _contenedorTrabajo.Cama.Update(camaVM.Cama);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));
            }
            camaVM.ListaHabitaciones = _contenedorTrabajo.Habitacion.GetListaHabitaciones();
            return View(camaVM);
        }
        
        #region Llamadas a la API
        
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Cama.GetAll(includeProperties: "Habitacion") });

        }

        //[HttpDelete]
        //public IActionResult Delete(int id)
        //{
        //    var camaDesdeBd = _contenedorTrabajo.Cama.Get(id);
        //    string rutaDirectorioPrincipal = _hostingEnvironment.WebRootPath;
        //    var rutaImagen = Path.Combine(rutaDirectorioPrincipal, camaDesdeBd.UrlImagen.TrimStart('\\'));

        //    //VALIDACION comprobamos si existe el fichero y si existe la eliminamos del BBDD
        //    if (System.IO.File.Exists(rutaImagen))
        //    {
        //        System.IO.File.Delete(rutaImagen);
        //    }


        //    if (camaDesdeBd == null)
        //    {
        //        return Json(new { success = false, message = "Error borrando cama" });
        //    }

        //    _contenedorTrabajo.Cama.Remove(camaDesdeBd);
        //    _contenedorTrabajo.Save();
        //    return Json(new { success = true, message = "Cama Borrada Correctamente" });
        //}

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var camaDesdeBd = _contenedorTrabajo.Cama.Get(id);

            // Verificación si la cama está en uso
            if (camaDesdeBd.CamaUsada) // Verificamos si la cama está en uso (CamaUsada == true)
            {
                return Json(new { success = false, message = "No se puede eliminar la cama porque está en uso." });
            }


            // Si la cama no está en uso, proceder con la eliminación de la imagen
            string rutaDirectorioPrincipal = _hostingEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, camaDesdeBd.UrlImagen.TrimStart('\\'));

            // Validación: comprobamos si existe el fichero y si existe lo eliminamos del sistema de archivos
            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }

            // Verificación si la cama existe en la base de datos
            if (camaDesdeBd == null)
            {
                return Json(new { success = false, message = "Error borrando la cama" });
            }

            // Eliminación de la cama en la base de datos
            _contenedorTrabajo.Cama.Remove(camaDesdeBd);
            _contenedorTrabajo.Save();

            return Json(new { success = true, message = "Cama eliminada correctamente." });
        }


        #endregion
    }
}
