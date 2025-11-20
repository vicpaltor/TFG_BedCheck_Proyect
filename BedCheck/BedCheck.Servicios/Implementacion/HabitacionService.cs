using AutoMapper;
using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using BedCheck.Models.DTOs;
using BedCheck.Servicios.Interfaces;

namespace BedCheck.Servicios.Implementacion
{
    public class HabitacionService : IHabitacionService
    {
        private readonly IContenedorTrabajo _unitOfWork;
        private readonly IMapper _mapper;

        public HabitacionService(IContenedorTrabajo unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HabitacionDto>> ObtenerTodas()
        {
            var lista = _unitOfWork.Habitacion.GetAll();
            return _mapper.Map<IEnumerable<HabitacionDto>>(lista);
        }

        public async Task<HabitacionDto> ObtenerPorId(int id)
        {
            var obj = _unitOfWork.Habitacion.Get(id);
            return _mapper.Map<HabitacionDto>(obj);
        }

        public bool ExisteNumero(int numero)
        {
            return _unitOfWork.Habitacion.GetAll(h => h.NumHabitacion == numero).Any();
        }

        public async Task<bool> Crear(HabitacionDto dto)
        {
            if (ExisteNumero(dto.NumHabitacion)) return false;

            var entidad = _mapper.Map<Habitacion>(dto);
            _unitOfWork.Habitacion.Add(entidad);
            _unitOfWork.Save();
            return true;
        }

        public async Task<bool> Actualizar(HabitacionDto dto)
        {
            // Aquí podríamos validar si cambian el número a uno que ya existe
            var entidad = _mapper.Map<Habitacion>(dto);
            _unitOfWork.Habitacion.Update(entidad);
            _unitOfWork.Save();
            return true;
        }

        public async Task<bool> Borrar(int id)
        {
            var entidad = _unitOfWork.Habitacion.Get(id);
            if (entidad == null) return false;

            // Validación extra: ¿Tiene camas asignadas? (Opcional)
            // var camas = _unitOfWork.Cama.GetAll(c => c.HabitacionId == id);
            // if (camas.Any()) return false;

            _unitOfWork.Habitacion.Remove(entidad);
            _unitOfWork.Save();
            return true;
        }
    }
}