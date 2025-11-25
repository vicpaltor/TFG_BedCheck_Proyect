using AutoMapper;
using BedCheck.AccesoDatos.Data.Repository;
using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using BedCheck.Models.DTOs;
using BedCheck.Servicios.Interfaces;
using System.Threading.Tasks;

namespace BedCheck.Servicios.Implementacion
{
    public class EnfermeroService : IEnfermeroService
    {
        private readonly IEnfermeroRepositorio _enfermeroRepository;
        private readonly IMapper _mapper;

        public EnfermeroService(IEnfermeroRepositorio enfermeroRepository, IMapper mapper)
        {
            _enfermeroRepository = enfermeroRepository;
            _mapper = mapper;
        }

        public async Task<int> Crear(EnfermeroDto dto)
        {
      
            if (string.IsNullOrWhiteSpace(dto.NombreEnfermero))
            {
                throw new ArgumentException("El nombre del enfermero no puede estar vacío.", nameof(dto.NombreEnfermero));
            }

            var enfermeroEntity = _mapper.Map<Enfermero>(dto);
            await _enfermeroRepository.AddAsync(enfermeroEntity);
            return enfermeroEntity.IdEnfermero;

        }
    }
}