using FluentValidation;

namespace PosTech.Noticia.API.Noticias.Update
{
    public class UpdateNoticiaValidator : AbstractValidator<UpdateNoticiaCommand>
    {
        public UpdateNoticiaValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("O Id da noticia é obrigatório.");
        }
    }
}
