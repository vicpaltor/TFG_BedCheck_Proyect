using FluentValidation;
using BedCheck.Models;

namespace BedCheck.Models.Validators
{
    public class CamaValidator : AbstractValidator<Cama>
    {
        public CamaValidator()
        {
            // Validamos que el número de cama tenga valor y sea positivo
            RuleFor(c => c.IdCama)
                .NotEmpty().WithMessage("El número de cama es obligatorio.")
                .GreaterThan(0).WithMessage("El número de cama debe ser mayor a 0.");

            // Si tienes un campo Descripción u otro, puedes añadir más reglas aquí
            // Ejemplo:
            // RuleFor(c => c.Descripcion).MaximumLength(100).WithMessage("Máximo 100 caracteres.");
        }
    }
}