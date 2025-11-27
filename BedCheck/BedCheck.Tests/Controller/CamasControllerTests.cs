using BedCheck.Areas.Admin.Controllers;
using BedCheck.Models.DTOs;
using BedCheck.Models.ViewModels;
using BedCheck.Servicios.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using Xunit;

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

            _controller = new CamasController(_mockService.Object,_mockWebHost.Object);

            // CONFIGURACIÓN CLAVE PARA MOCKEAR HTTPCONTEXT (ARCHIVOS)
            // -----------------------------------------------------
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            var mockFormFileCollection = new Mock<IFormFileCollection>();

            // Le decimos al mock: "Cuando te pidan archivos, devuelve una lista vacía"
            mockHttpRequest.Setup(r => r.Form.Files).Returns(mockFormFileCollection.Object);
            mockHttpContext.Setup(r => r.Request).Returns(mockHttpRequest.Object);

            // Asignamos el contexto falso al controlador
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };
            // -----------------------------------------------------
        }

        [Fact]
        public async Task Edit_Post_DebeRedirigirAIndex_CuandoActualizacionEsExitosa()
        {
            // 1. ARRANGE (Preparar)
            var camaDto = new CamaDto { IdCama = 1, NombreCama = "Cama Editada" };
            var camaVM = new CamaVM { Cama = camaDto };

            // Simulamos que el servicio dice "Todo OK" (true)
            _mockService.Setup(s => s.Actualizar(
                It.IsAny<CamaDto>(),
                It.IsAny<IFormFile>(),
                It.IsAny<string>())
            ).ReturnsAsync(true);

            // 2. ACT (Ejecutar)
            var resultado = await _controller.Edit(camaVM);

            // 3. ASSERT (Verificar)
            // Verificamos que sea una redirección
            var redireccion = Assert.IsType<RedirectToActionResult>(resultado);
            // Verificamos que vaya al Index
            Assert.Equal("Index", redireccion.ActionName);
        }

        [Fact]
        public async Task Edit_Post_DebeDevolverVistaConError_CuandoServicioFalla()
        {
            // 1. ARRANGE
            var camaDto = new CamaDto { IdCama = 1, NombreCama = "Nombre Duplicado" };
            var camaVM = new CamaVM { Cama = camaDto };

            // Simulamos que el servicio falla (false)
            _mockService.Setup(s => s.Actualizar(It.IsAny<CamaDto>(), null, It.IsAny<string>()))
                .ReturnsAsync(false);

            // Simulamos la recarga de habitaciones para que no de NullReference
            _mockService.Setup(s => s.ObtenerListaHabitaciones())
                .Returns(new List<SelectListItem>());

            // 2. ACT
            var resultado = await _controller.Edit(camaVM);

            // 3. ASSERT
            var vistaResultado = Assert.IsType<ViewResult>(resultado);

            // Verificamos que el modelo sigue siendo CamaVM
            Assert.IsType<CamaVM>(vistaResultado.Model);

            // ¡IMPORTANTE! Verificamos que se ha añadido un error al ModelState
            Assert.False(_controller.ModelState.IsValid);
            Assert.True(_controller.ModelState.ErrorCount > 0);
        }

        [Fact]
        public async Task Edit_Post_DebeRetornarNotFound_SiIdEsCero()
        {
            // 1. ARRANGE
            var camaVM = new CamaVM { Cama = new CamaDto { IdCama = 0 } }; // ID inválido

            // 2. ACT
            var resultado = await _controller.Edit(camaVM);

            // 3. ASSERT
            Assert.IsType<NotFoundResult>(resultado);
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