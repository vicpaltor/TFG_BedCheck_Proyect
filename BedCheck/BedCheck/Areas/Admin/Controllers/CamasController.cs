using AutoMapper;
using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using BedCheck.Models.DTOs;
using BedCheck.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BedCheck.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class CamasController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper _mapper; // <--- AutoMapper inyectado

        public CamasController(IContenedorTrabajo contenedorTrabajo,
            IWebHostEnvironment hostingEnvironment,
            IMapper mapper)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
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
            CamaVM camaVM = new CamaVM()
            {
                Cama = new CamaDto(), // Usamos el DTO vacío
                ListaHabitaciones = _contenedorTrabajo.Habitacion.GetListaHabitaciones()
            };
            return View(camaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Create(CamaVM camaVM)
        {
            // 1. Validar Nombre Duplicado
            var camaExistente = _contenedorTrabajo.Cama
                .GetAll(c => c.NombreCama == camaVM.Cama.NombreCama).FirstOrDefault();

            if (camaExistente != null)
            {
                ModelState.AddModelError("Cama.NombreCama", "Ya existe una cama con ese nombre.");
            }

            // 2. Validar Capacidad Habitación
            var habitacion = _contenedorTrabajo.Habitacion
                .GetFirstOrDefault(h => h.IdHabitacion == camaVM.Cama.HabitacionId);

            if (habitacion == null)
            {
                ModelState.AddModelError("HabitacionId", "La habitación seleccionada no existe.");
            }
            else
            {
                var camasAsignadas = _contenedorTrabajo.Cama
                    .GetAll(c => c.HabitacionId == habitacion.IdHabitacion).Count();

                if (camasAsignadas >= habitacion.NumCamasTotales)
                {
                    ModelState.AddModelError("Cama.HabitacionId", "Esta habitación ya ha alcanzado el número máximo de camas.");
                }
            }

            // 3. Gestión de Imágenes
            string rutaPrincipal = _hostingEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            if (archivos.Count() > 0)
            {
                // Nueva imagen
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"imagenes\camas");
                var extension = Path.GetExtension(archivos[0].FileName);

                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }

                camaVM.Cama.UrlImagen = @"\imagenes\camas\" + nombreArchivo + extension;
            }
            else
            {
                // Si no hay imagen, error (si es obligatorio)
                // Si quieres que sea opcional, quita esta línea
                if (camaVM.Cama.IdCama == 0) // Solo obligatorio al crear
                {
                    ModelState.AddModelError("Cama.UrlImagen", "Debes seleccionar una imagen");
                }
            }

            if (ModelState.IsValid)
            {
                // MAGIA: Convertimos DTO -> Entidad
                var camaEntidad = _mapper.Map<Cama>(camaVM.Cama);

                // Campos que no están en el DTO y debemos rellenar manualmente
                camaEntidad.FechaCreacion = DateTime.Now.ToString();
                camaEntidad.CamaUsada = false; // Por defecto libre
                // La propiedad 'Operacion' si es int, por defecto será 0

                _contenedorTrabajo.Cama.Add(camaEntidad);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));
            }

            // Si falla, recargamos la lista
            camaVM.ListaHabitaciones = _contenedorTrabajo.Habitacion.GetListaHabitaciones();
            return View(camaVM);
        }


        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var camaEntidad = _contenedorTrabajo.Cama.Get(id.GetValueOrDefault());
            if (camaEntidad == null) return NotFound();

            CamaVM camaVM = new CamaVM()
            {
                // MAGIA: Convertimos Entidad -> DTO para mostrar en la vista
                Cama = _mapper.Map<CamaDto>(camaEntidad),
                ListaHabitaciones = _contenedorTrabajo.Habitacion.GetListaHabitaciones()
            };

            return View(camaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Edit(CamaVM camaVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                // Obtenemos la entidad original para no perder datos (como FechaCreacion)
                // Usamos AsNoTracking para que EF Core no se líe al actualizar luego
                var camaDesdeBd = _contenedorTrabajo.Cama.GetFirstOrDefault(c => c.IdCama == camaVM.Cama.IdCama);

                if (archivos.Count() > 0)
                {
                    // Nueva imagen
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\camas");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    var rutaImagen = Path.Combine(rutaPrincipal, camaDesdeBd.UrlImagen.TrimStart('\\'));

                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    camaVM.Cama.UrlImagen = @"\imagenes\camas\" + nombreArchivo + extension;
                }
                else
                {
                    // Conservar la imagen anterior
                    camaVM.Cama.UrlImagen = camaDesdeBd.UrlImagen;
                }

                // MAGIA: Convertimos DTO actualizado -> Entidad Nueva
                var camaAActualizar = _mapper.Map<Cama>(camaVM.Cama);

                // Restauramos datos que el DTO no tiene y no queremos perder/borrar
                camaAActualizar.FechaCreacion = camaDesdeBd.FechaCreacion;
                camaAActualizar.CamaUsada = camaDesdeBd.CamaUsada;
                camaAActualizar.Operacion = camaDesdeBd.Operacion;

                // Hacemos un truco para evitar conflictos de tracking de EF Core
                // (Desvinculamos la entidad anterior leída)
               _contenedorTrabajo.Detach(camaDesdeBd);

                _contenedorTrabajo.Cama.Update(camaAActualizar);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));
            }

            camaVM.ListaHabitaciones = _contenedorTrabajo.Habitacion.GetListaHabitaciones();
            return View(camaVM);
        }



        #region Llamadas a la API

        /// <summary>
        /// Obtiene la lista completa de camas con sus habitaciones
        /// </summary>
        /// <returns>JSON con data de camas</returns>
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Cama.GetAll(includeProperties: "Habitacion") });
        }


        /// <summary>
        /// Elimina una cama por su ID
        /// </summary>
        /// <param name="id">El identificador de la cama</param>
        /// <response code="200">Si se borró correctamente</response>
        /// <response code="400">Si la cama está ocupada o no existe</response>
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Delete(int id)
        {
            var camaDesdeBd = _contenedorTrabajo.Cama.Get(id);

            if (camaDesdeBd == null)
            {
                return Json(new { success = false, message = "Error borrando la cama" });
            }

            if (camaDesdeBd.CamaUsada)
            {
                return Json(new { success = false, message = "No se puede eliminar la cama porque está en uso." });
            }

            string rutaDirectorioPrincipal = _hostingEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, camaDesdeBd.UrlImagen.TrimStart('\\'));

            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }

            _contenedorTrabajo.Cama.Remove(camaDesdeBd);
            _contenedorTrabajo.Save();

            return Json(new { success = true, message = "Cama eliminada correctamente." });
        }

        #endregion
    }
}