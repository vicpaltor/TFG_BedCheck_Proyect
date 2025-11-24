using AutoMapper;
using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using BedCheck.Models.DTOs;
using BedCheck.Servicios.Interfaces;

namespace BedCheck.Servicios.Implementacion
{
    public class PacienteService : IPacienteService
    {
        private readonly IContenedorTrabajo _unitOfWork;
        private readonly IMapper _mapper;

        public PacienteService(IContenedorTrabajo unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PacienteDto>> ObtenerTodos()
        {
            var lista = _unitOfWork.Paciente.GetAll();
            return _mapper.Map<IEnumerable<PacienteDto>>(lista);
        }

        public async Task<PacienteDto> ObtenerPorId(int id)
        {
            var obj = _unitOfWork.Paciente.Get(id);
            return _mapper.Map<PacienteDto>(obj);
        }

        public async Task<bool> Crear(PacienteDto dto)
        {
            // Validación simple: No repetir paciente (opcional)
            if (_unitOfWork.Paciente.GetAll(p => p.StrNombrePaciente == dto.Nombre).Any()) return false;

            var entidad = _mapper.Map<Paciente>(dto);
            _unitOfWork.Paciente.Add(entidad);
            _unitOfWork.Save();
            return true;
        }

        public async Task<bool> Actualizar(PacienteDto dto)
        {
            var entidad = _mapper.Map<Paciente>(dto);
            _unitOfWork.Paciente.Update(entidad);
            _unitOfWork.Save();
            return true;
        }

        public async Task<bool> Borrar(int id)
        {
            var entidad = _unitOfWork.Paciente.Get(id);
            if (entidad == null) return false;

            _unitOfWork.Paciente.Remove(entidad);
            _unitOfWork.Save();
            return true;
        }
    }
}