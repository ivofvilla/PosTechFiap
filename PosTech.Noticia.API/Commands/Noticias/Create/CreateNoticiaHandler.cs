using FluentValidation;
using MediatR;
using PosTech.Noticia.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosTech.Noticia.API.Commands.Noticia
{
    public class CreateNoticiaHandler : IRequestHandler<CreateNoticiaCommand, bool>
    {
        private readonly INoticiaRepository _noticiaRepository;
        private readonly IValidator<CreateNoticiaCommand> _validator;

        public CreateNoticiaHandler(INoticiaRepository noticiaRepository, IValidator<CreateNoticiaCommand> validator)
        {
            _noticiaRepository = noticiaRepository;
            _validator = validator;
        }

        public async Task<bool> Handle(CreateNoticiaCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return false;
            }

            var noticia = new Models.Noticias
            {
                Id = Guid.NewGuid(),
                Autor = command.Autor,
                Chapeu = command.Chapeu,
                DataAtualizacao = DateTime.UtcNow,
                DataPublicacao = DateTime.UtcNow,
                Descricao = command.Descricao,
                Titulo = command.Titulo,
            };

            await _noticiaRepository.AdicionarAsync(noticia, cancellationToken);

            return true;
        }
    }
}
