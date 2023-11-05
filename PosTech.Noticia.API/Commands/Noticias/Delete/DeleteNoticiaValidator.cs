using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosTech.Noticia.API.Noticias.Delete
{
    public class DeleteNoticiaValidator : AbstractValidator<DeleteNoticiaCommand>
    {
        public DeleteNoticiaValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("O ID da noticia é obrigatório.");
        }
    }
}
