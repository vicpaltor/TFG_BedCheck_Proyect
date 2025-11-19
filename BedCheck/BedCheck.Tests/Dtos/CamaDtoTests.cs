using BedCheck.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace BedCheck.Tests
{
    public class CamaDtoTests
    {
        [Fact]
        public void CamaDto_DebeSerInvalido_CuandoNombreEsNulo()
        {
            // 1. Arrange (Preparar)
            var camaDto = new CamaDto
            {
                NombreCama = null, // ERROR: Es obligatorio
                EstadoCama = "Disponible",
                TipoCama = "Estándar"
            };

            // 2. Act (Actuar - Simular validación)
            var contexto = new ValidationContext(camaDto);
            var resultados = new List<ValidationResult>();
            var esValido = Validator.TryValidateObject(camaDto, contexto, resultados, true);

            // 3. Assert (Verificar)
            Assert.False(esValido); // Esperamos que falle
            Assert.Contains(resultados, r => r.ErrorMessage.Contains("El nombre de la cama es obligatorio"));
        }

        [Fact]
        public void CamaDto_DebeSerValido_CuandoTodoEsCorrecto()
        {
            // 1. Arrange
            var camaDto = new CamaDto
            {
                NombreCama = "Cama 101",
                EstadoCama = "Disponible",
                TipoCama = "Estándar",
                HabitacionId = 1
            };

            // 2. Act
            var contexto = new ValidationContext(camaDto);
            var resultados = new List<ValidationResult>();
            var esValido = Validator.TryValidateObject(camaDto, contexto, resultados, true);

            // 3. Assert
            Assert.True(esValido); // Esperamos que sea válido
        }
    }
}