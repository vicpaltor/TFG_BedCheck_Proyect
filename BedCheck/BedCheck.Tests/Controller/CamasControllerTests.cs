using Xunit;
using Moq;
using BedCheck.Areas.Admin.Controllers;
using BedCheck.AccesoDatos.Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;

namespace BedCheck.Tests
{
    public class CamasControllerTests
    {
        // Definimos los Mocks (los objetos falsos)
        private readonly Mock<IContenedorTrabajo> _mockRepo;
        private readonly Mock<IWebHostEnvironment> _mockWebHost;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CamasController _controller;

        public CamasControllerTests()
        {
            // 1. Inicializamos los simuladores
            _mockRepo = new Mock<IContenedorTrabajo>();
            _mockWebHost = new Mock<IWebHostEnvironment>();
            _mockMapper = new Mock<IMapper>();

            // 2. Inyectamos los falsos en el controlador real
            _controller = new CamasController(
                _mockRepo.Object,
                _mockWebHost.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public void Index_DebeRetornarVista()
        {
            // Act (Ejecutar)
            var resultado = _controller.Index();

            // Assert (Comprobar)
            // Verificamos que el resultado sea de tipo ViewResult (una vista HTML)
            Assert.IsType<ViewResult>(resultado);
        }
    }
}