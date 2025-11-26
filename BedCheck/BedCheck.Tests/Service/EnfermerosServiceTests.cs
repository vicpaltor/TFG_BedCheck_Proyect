using AutoMapper;
using BedCheck.AccesoDatos.Data.Repository; // Donde está IEnfermeroRepositorio
using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models; // Para la entidad Enfermero.cs
using BedCheck.Models.DTOs; // Donde estará CrearEnfermeroDTO.cs
using BedCheck.Servicios.Implementacion; // Donde estará EnfermeroService
using BedCheck.Servicios.Interfaces; // Para IEnfermeroService (si lo usas en el setup)
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BedCheck.Tests.Service
{
    // 1. Usar 'public' en la clase de tests.
    public class EnfermerosServiceTests
    {
        // Dependencias que vamos a simular (Mock)
        private readonly Mock<IEnfermeroRepositorio> _mockEnfermeroRepository;
        private readonly Mock<IMapper> _mockMapper;

        // El servicio que vamos a probar
        private readonly EnfermeroService _enfermerosService;

        // Constructor de Setup
        public EnfermerosServiceTests()
        {
            // Inicializar Mocks
            _mockEnfermeroRepository = new Mock<IEnfermeroRepositorio>();
            _mockMapper = new Mock<IMapper>();

            // Crear la instancia del Servicio a probar, inyectando los Mocks.
            _enfermerosService = new EnfermeroService(
                _mockEnfermeroRepository.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task CrearEnfermero_NombreVacio_DebeLanzarExcepcion()
        {
            // ARRANGE (Configuración)
            // DTO inválido: Nombre vacío
            var enfermeroDtoInvalido = new EnfermeroDto { NombreEnfermero = "", RolEnfermero = "Supervisor" };

            // ACT & ASSERT (Ejecución y Verificación)
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _enfermerosService.Crear(enfermeroDtoInvalido);
            });

            // Verificamos que NUNCA se llama al repositorio (¡no debe guardarse!)
            _mockEnfermeroRepository.Verify(repo => repo.AddAsync(It.IsAny<Enfermero>()), Times.Never);
        }

        [Fact]
        public async Task CrearEnfermero_RolVacio_DebeLanzarExcepcion()
        {
            // ARRANGE 
            // DTO inválido: Rol vacío
            var enfermeroDtoInvalido = new EnfermeroDto { NombreEnfermero = "Juan", RolEnfermero = "" };

            // ACT & ASSERT 
            // Esperamos que se lance una excepción.
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _enfermerosService.Crear(enfermeroDtoInvalido);
            });

            // Verificamos que NUNCA se llama al repositorio
            _mockEnfermeroRepository.Verify(repo => repo.AddAsync(It.IsAny<Enfermero>()), Times.Never);
        }


    }
}