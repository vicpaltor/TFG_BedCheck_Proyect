using AutoMapper;
using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using BedCheck.Models.DTOs;
using BedCheck.Servicios.Implementacion;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace BedCheck.Tests.Service
{
    public class OperacionServiceTests
    {
        private readonly Mock<IContenedorTrabajo> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly OperacionService _service;

        public OperacionServiceTests()
        {
            _mockUnitOfWork = new Mock<IContenedorTrabajo>();
            _mockMapper = new Mock<IMapper>();

            // Simulamos que el UnitOfWork tiene un repo de Operaciones
            var mockRepo = new Mock<IOperacionRepositorio>();
            _mockUnitOfWork.Setup(u => u.Operacion).Returns(mockRepo.Object);

            _service = new OperacionService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task ObtenerTodas_DebeLlamarAlRepositorio_ConRelaciones()
        {
            // Act
            await _service.ObtenerTodas();

            // Assert
            // Verificamos que llame a GetAll incluyendo "Paciente,Cama"
            // Esto es CRÍTICO para que el AutoMapper funcione luego
            _mockUnitOfWork.Verify(u => u.Operacion.GetAll(
                It.IsAny<Expression<Func<Operacion, bool>>>(),
                It.IsAny<Func<IQueryable<Operacion>, IOrderedQueryable<Operacion>>>(),
                "Paciente,Cama"), // <--- Verificamos que pida las relaciones
                Times.Once);
        }

        [Fact]
        public async Task Crear_DebeGuardarOperacion()
        {
            // Arrange
            var dto = new OperacionDto
            {
                NombreOperacion = "Apendicitis",
                PacienteId = 1,
                CamaId = 5
            };

            // Act
            var resultado = await _service.Crear(dto);

            // Assert
            Assert.True(resultado);
            _mockUnitOfWork.Verify(u => u.Operacion.Add(It.IsAny<Operacion>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
        }
    }
}