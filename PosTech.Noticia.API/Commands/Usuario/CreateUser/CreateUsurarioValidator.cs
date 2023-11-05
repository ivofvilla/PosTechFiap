using FluentValidation;

namespace PosTech.Usuario.API.Commands
{
    public class CreateUsurarioValidator : AbstractValidator<CreateUsurarioCommand>
    {
        public CreateUsurarioValidator()
        {
            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("O Email é um campo obrigatório.");

            RuleFor(command => command.Senha)
                .NotEmpty().WithMessage("A senha é um campo obrigatória.");

            RuleFor(command => command.ConfirmacaoSenha)
                .NotEmpty().WithMessage("O campo confirmação de senha é obrigatória.");

            RuleFor(command => command.Nome)
                .NotEmpty().WithMessage("O Nome é um campo obrigatório.");
        }
    }
}
