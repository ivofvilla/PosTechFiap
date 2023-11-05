using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosTech.Noticia.API.Commands.Noticia
{
    public class CreateNoticiaValidator : AbstractValidator<CreateNoticiaCommand>
    {
        public CreateNoticiaValidator()
        {
            RuleFor(command => command.Titulo)
                .NotEmpty().WithMessage("O título da noticia é obrigatório.");

            RuleFor(command => command.Descricao)
                .NotEmpty().WithMessage("A Descrição da noticia é obrigatória.");

            RuleFor(command => command.Chapeu)
                .NotEmpty().WithMessage("A Chápeu da noticia é obrigatória.");

            RuleFor(command => command.Autor)
                .NotEmpty().WithMessage("A Autor da noticia é obrigatória.");
        }
    }
}
