using FluentValidation;
using SalarioWeb.Models;

namespace SalarioWeb.Application.Validators;

public class CargoValidator : AbstractValidator<Cargo>
{
    public CargoValidator()
    {
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("O nome do cargo é obrigatório.")
            .Length(1, 100).WithMessage("O nome do cargo deve ter no máximo 100 caracteres.");

        RuleFor(c => c.Salario)
            .GreaterThan(0).WithMessage("O salário deve ser maior que zero.");
    }
}
