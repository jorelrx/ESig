using FluentValidation;
using SalarioWeb.DTOs.Pessoa;

namespace SalarioWeb.Application.Validators;

public class CreatePessoaDTOValidator : AbstractValidator<CreatePessoaDTO>
{
    public CreatePessoaDTOValidator()
    {
        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .Length(4, 100).WithMessage("O nome deve ter entre 4 e 100 caracteres.");

        RuleFor(p => p.Cidade)
            .NotEmpty().WithMessage("A cidade é obrigatória.")
            .Length(1, 50).WithMessage("A cidade deve ter no máximo 50 caracteres.");

        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("O email é obrigatório.")
            .EmailAddress().WithMessage("Formato de email inválido.");

        RuleFor(p => p.CEP)
            .NotEmpty().WithMessage("O CEP é obrigatório.")
            .Matches(@"^\d{5}-\d{3}$").WithMessage("O CEP deve estar no formato 00000-000.");

        RuleFor(p => p.Endereco)
            .NotEmpty().WithMessage("O endereço é obrigatório.")
            .Length(1, 200).WithMessage("O endereço deve ter no máximo 200 caracteres.");

        RuleFor(p => p.Pais)
            .NotEmpty().WithMessage("O país é obrigatório.")
            .Length(1, 50).WithMessage("O país deve ter no máximo 50 caracteres.");

        RuleFor(p => p.Usuario)
            .NotEmpty().WithMessage("O usuário é obrigatório.")
            .Length(4, 20).WithMessage("O usuário deve ter entre 4 e 20 caracteres.");

        RuleFor(p => p.Telefone)
            .NotEmpty().WithMessage("O telefone é obrigatório.")
            .Matches(@"^\+?\d{9,15}$").WithMessage("O formato do telefone é inválido.");

        RuleFor(p => p.Data_Nascimento)
            .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
            .LessThan(DateTime.Now).WithMessage("A data de nascimento deve ser anterior à data atual.");

        RuleFor(p => p.Cargo_ID)
            .NotEmpty().WithMessage("O cargo é obrigatório.");
    }
}
