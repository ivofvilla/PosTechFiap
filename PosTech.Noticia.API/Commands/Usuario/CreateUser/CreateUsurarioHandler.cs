using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PosTech.Noticia.API.Models;
using PosTech.Noticia.API.Repositories;

namespace PosTech.Usuario.API.Commands
{
    public class CreateUsurarioHandler : IRequestHandler<CreateUsurarioCommand, bool>
    {
        private readonly IValidator<CreateUsurarioCommand> _validator;
        private readonly IUsuarioRepository _user;

        public CreateUsurarioHandler(IValidator<CreateUsurarioCommand> validator, IUsuarioRepository user)
        {
            _validator = validator;
            _user = user;
        }

        public async Task<bool> Handle(CreateUsurarioCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return false;
            }

            var usuaro = new Usuarios(command.Email, command.Senha, command.Nome);


            var resut = await _user.Inserir(usuaro);

            return (resut != null ? true : false);
        }
    }
}
