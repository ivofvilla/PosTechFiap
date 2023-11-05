using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosTech_Fase.Queries.Login
{
    public class GetUsuarioValidator : AbstractValidator<GetUsuarioQuery>
    {
        public GetUsuarioValidator()
        {
            RuleFor(query => query.Password)
                .NotEmpty().WithMessage("O Senha é obrigatório.");
            RuleFor(query => query.Email)
                .NotEmpty().WithMessage("O Usuário é obrigatório.");
        }
    }
}
