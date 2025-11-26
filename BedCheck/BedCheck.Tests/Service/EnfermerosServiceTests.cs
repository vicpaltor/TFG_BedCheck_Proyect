using AutoMapper;
using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using BedCheck.Models.DTOs;
using BedCheck.Servicios.Implementacion;
using Moq;
using Xunit;

namespace BedCheck.Tests.Service
{
    public class EnfermerosServiceTests
    {
        private readonly Mock<IContenedorTrabajo> _mockUnitOfWork; // <--- CAMBIO CLAVE
        private readonly Mock<IMapper> _mockMapper;
        private readonly EnfermeroService _service;

        public EnfermerosServiceTests()
        {
            _mockUnitOfWork = new Mock<IContenedorTrabajo>();
            _mockMapper = new Mock<IMapper>();

            // Configuramos el mock para que cuando pidan .Enfermero, devuelva un repo falso
            var mockRepo = new Mock<IEnfermeroRepositorio>();
            _mockUnitOfWork.Setup(u => u.Enfermero).Returns(mockRepo.Object);

            _service = new EnfermeroService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Crear_NombreVacio_LanzaExcepcion()
        {
            // Arrange
            var dto = new EnfermeroDto { NombreEnfermero = "", RolEnfermero = "Admin" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.Crear(dto));

            // Verificamos que NO se llamó a guardar
            _mockUnitOfWork.Verify(u => u.Save(), Times.Never);
        }

        [Fact]
        public async Task Crear_RolVacio_LanzaExcepcion()
        {
            // Arrange
            var dto = new EnfermeroDto { NombreEnfermero = "Pepe", RolEnfermero = "" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.Crear(dto));
        }
    }
}