using Xunit;
using Moq;
using BedCheck.Areas.Admin.Controllers;
using BedCheck.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace BedCheck.Tests
{
    public class CamasControllerTests
    {
        // 1. Ahora mockeamos el Servicio, no el Repositorio
        private readonly Mock<ICamaService> _mockService;
        private readonly Mock<IWebHostEnvironment> _mockWebHost;
        private readonly CamasController _controller;

        public CamasControllerTests()
        {
            // 2. Inicializamos los simuladores
            _mockService = new Mock<ICamaService>();
            _mockWebHost = new Mock<IWebHostEnvironment>();

            _controller = new CamasController(
                _mockService.Object,
                _mockWebHost.Object
            );
        }

        [Fact]
        public void Index_DebeRetornarVista()
        {
            // Act
            var resultado = _controller.Index();

            // Assert
            Assert.IsType<ViewResult>(resultado);
        }
    }
}