using AutoMapper;
using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using BedCheck.Models.DTOs;
using BedCheck.Servicios.Interfaces;

namespace BedCheck.Servicios.Implementacion
{
    public class EnfermeroService : IEnfermeroService
    {
        private readonly IContenedorTrabajo _unitOfWork;
        private readonly IMapper _mapper;

        public EnfermeroService(IContenedorTrabajo unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EnfermeroDto>> ObtenerTodos()
        {
            var lista = _unitOfWork.Enfermero.GetAll();
            return _mapper.Map<IEnumerable<EnfermeroDto>>(lista);
        }

        public async Task<EnfermeroDto> ObtenerPorId(int id)
        {
            var obj = _unitOfWork.Enfermero.Get(id);
            return _mapper.Map<EnfermeroDto>(obj);
        }

        public async Task<int> Crear(EnfermeroDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NombreEnfermero))
                throw new ArgumentException("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.RolEnfermero))
                throw new ArgumentException("El rol es obligatorio.");

            var entidad = _mapper.Map<Enfermero>(dto);

            // ATENCIÓN: Si tu repositorio no tiene AddAsync, usa Add normal
            _unitOfWork.Enfermero.Add(entidad);
            _unitOfWork.Save();

            return entidad.IdEnfermero;
        }

        public async Task<bool> Actualizar(EnfermeroDto dto)
        {
            var entidad = _mapper.Map<Enfermero>(dto);
            _unitOfWork.Enfermero.Update(entidad);
            _unitOfWork.Save();
            return true;
        }

        public async Task<bool> Borrar(int id)
        {
            var entidad = _unitOfWork.Enfermero.Get(id);
            if (entidad == null) return false;

            _unitOfWork.Enfermero.Remove(entidad);
            _unitOfWork.Save();
            return true;
        }
    }
}