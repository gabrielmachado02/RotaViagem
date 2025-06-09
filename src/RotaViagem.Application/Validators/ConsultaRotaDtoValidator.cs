using FluentValidation;
using RotaViagem.Application.DTOs;

namespace RotaViagem.Application.Validators
{
    public class ConsultaRotaDtoValidator : AbstractValidator<ConsultaRotaDto>
    {
        public ConsultaRotaDtoValidator()
        {
            RuleFor(x => x.Origem)
                .NotEmpty().WithMessage("Origem é obrigatória")
                .Length(3, 3).WithMessage("Origem deve ter exatamente 3 caracteres")
                .Matches("^[A-Z]+$").WithMessage("Origem deve conter apenas letras maiúsculas");

            RuleFor(x => x.Destino)
                .NotEmpty().WithMessage("Destino é obrigatório")
                .Length(3, 3).WithMessage("Destino deve ter exatamente 3 caracteres")
                .Matches("^[A-Z]+$").WithMessage("Destino deve conter apenas letras maiúsculas");

            RuleFor(x => x)
                .Must(x => x.Origem != x.Destino)
                .WithMessage("Origem e Destino não podem ser iguais");
        }
    }
}