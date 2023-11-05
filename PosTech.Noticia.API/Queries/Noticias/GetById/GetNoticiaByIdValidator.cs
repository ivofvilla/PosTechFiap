using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosTech_Fase2.Application.Queries.Noticias.GetById
{
    public class GetNoticiaByIdValidator : AbstractValidator<GetNoticiaByIdQuery>
    {
        public GetNoticiaByIdValidator()
        {
            RuleFor(query => query.Id)
                .NotEmpty().WithMessage("O ID da noticia é obrigatório.");
        }
    }
}
