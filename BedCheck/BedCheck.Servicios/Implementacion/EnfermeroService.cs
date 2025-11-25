using AutoMapper;
using BedCheck.AccesoDatos.Data.Repository;
using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using BedCheck.Models.DTOs;
using BedCheck.Servicios.Interfaces;
using System.Threading.Tasks;

namespace BedCheck.Servicios.Implementacion
{
    // Implementamos la interfaz que definimos
    public class EnfermeroService : IEnfermeroService
    {
        // Inyección de dependencias (para interactuar con DB y mapear)
        private readonly IEnfermeroRepositorio _enfermeroRepository;
        private readonly IMapper _mapper;

        public EnfermeroService(IEnfermeroRepositorio enfermeroRepository, IMapper mapper)
        {
            _enfermeroRepository = enfermeroRepository;
            _mapper = mapper;
        }

        // Dejamos el método A IMPLEMENTAR para la fase GREEN
        public async Task<int> Crear(EnfermeroDto dto)
        {
            // Falla de compilación/ejecución si no está implementado o lanza NotImplementedException.
            // Esto garantiza que la prueba en RED ahora compile correctamente, ¡pero fallará al ejecutar!
            throw new NotImplementedException();
        }
    }
}