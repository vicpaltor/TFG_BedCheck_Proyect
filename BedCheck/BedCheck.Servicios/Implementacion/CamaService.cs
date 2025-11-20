using AutoMapper;
using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using BedCheck.Models.DTOs;
using BedCheck.Servicios.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BedCheck.Servicios.Implementacion
{
    public class CamaService : ICamaService
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IMapper _mapper;

        public CamaService(IContenedorTrabajo contenedorTrabajo, IMapper mapper)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CamaDto>> ObtenerTodas()
        {
            var lista = _contenedorTrabajo.Cama.GetAll(includeProperties: "Habitacion");
            // Convertimos la lista de Entidades a DTOs
            return _mapper.Map<IEnumerable<CamaDto>>(lista);
        }

        public async Task<CamaDto> ObtenerPorId(int id)
        {
            var cama = _contenedorTrabajo.Cama.Get(id);
            return _mapper.Map<CamaDto>(cama);
        }

        public bool ExisteNombre(string nombre)
        {
            return _contenedorTrabajo.Cama.GetAll(c => c.NombreCama == nombre).Any();
        }

        public bool HabitacionLlena(int habitacionId)
        {
            var habitacion = _contenedorTrabajo.Habitacion.GetFirstOrDefault(h => h.IdHabitacion == habitacionId);
            if (habitacion == null) return true; // Si no existe, asumimos error/llena

            var camasAsignadas = _contenedorTrabajo.Cama.GetAll(c => c.HabitacionId == habitacionId).Count();
            return camasAsignadas >= habitacion.NumCamasTotales;
        }

        public async Task<string> Crear(CamaDto camaDto, IFormFile? imagen, string rutaRaizWeb)
        {
            if (ExisteNombre(camaDto.NombreCama))
                return "Ya existe una cama con ese nombre.";

            if (HabitacionLlena(camaDto.HabitacionId))
                return "No se puede añadir: La habitación ha alcanzado su capacidad máxima.";

            // 1. Mapear DTO a Entidad
            var camaEntidad = _mapper.Map<Cama>(camaDto);

            camaEntidad.Habitacion = null;

            // Datos por defecto
            camaEntidad.FechaCreacion = DateTime.Now.ToString();
            camaEntidad.CamaUsada = false;

            // 2. Gestión de Imagen
            if (imagen != null)
            {
                //// Guid.NewGuid() crea un código tipo "e02fd0e4-0b49-4d23..." imposible de repetir.
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaRaizWeb, @"imagenes\camas");
                var extension = Path.GetExtension(imagen.FileName);

                // Crear directorio si no existe
                if (!Directory.Exists(subidas)) Directory.CreateDirectory(subidas);

                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    await imagen.CopyToAsync(fileStreams);
                }

                camaEntidad.UrlImagen = @"\imagenes\camas\" + nombreArchivo + extension;
            }

            // 3. Guardar en BD
            try
            {
                _contenedorTrabajo.Cama.Add(camaEntidad);
                _contenedorTrabajo.Save();
                return ""; // Éxito (sin mensaje de error)
            }
            catch (Exception ex)
            {
                return $"Error interno al guardar: {ex.Message}";
            }
        }

        public async Task<bool> Actualizar(CamaDto camaDto, IFormFile? imagen, string rutaRaizWeb)
        {
            var camaDesdeBd = _contenedorTrabajo.Cama.GetFirstOrDefault(c => c.IdCama == camaDto.IdCama);
            if (camaDesdeBd == null) return false;

            // Gestión de Imagen Nueva
            if (imagen != null)
            {
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaRaizWeb, @"imagenes\camas");
                var extension = Path.GetExtension(imagen.FileName);

                // Borrar imagen antigua
                if (!string.IsNullOrEmpty(camaDesdeBd.UrlImagen))
                {
                    var rutaImagenAntigua = Path.Combine(rutaRaizWeb, camaDesdeBd.UrlImagen.TrimStart('\\'));
                    if (System.IO.File.Exists(rutaImagenAntigua)) System.IO.File.Delete(rutaImagenAntigua);
                }

                // Subir nueva
                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    await imagen.CopyToAsync(fileStreams);
                }

                camaDto.UrlImagen = @"\imagenes\camas\" + nombreArchivo + extension;
            }
            else
            {
                // Mantener la antigua
                camaDto.UrlImagen = camaDesdeBd.UrlImagen;
            }

            // Mapear cambios
            var camaAActualizar = _mapper.Map<Cama>(camaDto);

            // Restaurar datos que no cambian (o que no viajan en el DTO)
            camaAActualizar.FechaCreacion = camaDesdeBd.FechaCreacion;
            camaAActualizar.CamaUsada = camaDesdeBd.CamaUsada;
            camaAActualizar.Operacion = camaDesdeBd.Operacion;

            // Evitar conflicto de tracking
            _contenedorTrabajo.Detach(camaDesdeBd);

            _contenedorTrabajo.Cama.Update(camaAActualizar);
            _contenedorTrabajo.Save();
            return true;
        }

        public async Task<bool> Borrar(int id, string rutaRaizWeb)
        {
            var camaDesdeBd = _contenedorTrabajo.Cama.Get(id);
            if (camaDesdeBd == null) return false;
            if (camaDesdeBd.CamaUsada) return false; // Regla de negocio: No borrar si está usada

            // Borrar imagen del disco
            if (!string.IsNullOrEmpty(camaDesdeBd.UrlImagen))
            {
                var rutaImagen = Path.Combine(rutaRaizWeb, camaDesdeBd.UrlImagen.TrimStart('\\'));
                if (System.IO.File.Exists(rutaImagen)) System.IO.File.Delete(rutaImagen);
            }

            _contenedorTrabajo.Cama.Remove(camaDesdeBd);
            _contenedorTrabajo.Save();
            return true;
        }

        public IEnumerable<SelectListItem> ObtenerListaHabitaciones()
        {
            return _contenedorTrabajo.Habitacion.GetListaHabitaciones();
        }
    }
}