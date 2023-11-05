using FluentValidation;
using MediatR;
using PosTech.Noticia.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosTech.Noticia.API.Noticias.Update
{
    public class UpdateNoticiaHandler : IRequestHandler<UpdateNoticiaCommand, bool>
    {
        private readonly INoticiaRepository _noticiaRepository;
        private readonly IValidator<UpdateNoticiaCommand> _validator;

        public UpdateNoticiaHandler(INoticiaRepository noticiaRepository, IValidator<UpdateNoticiaCommand> validator)
        {
            _noticiaRepository = noticiaRepository;
            _validator = validator;
        }

        public async Task<bool> Handle(UpdateNoticiaCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return false;
            }

            var noticia = await _noticiaRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (noticia == null)
            {
                return false;
            }

            noticia.Titulo = command.Titulo;
            noticia.Descricao = command.Descricao;
            noticia.Chapeu = command.Chapeu;
            noticia.Autor = command.Autor;
            noticia.DataAtualizacao = DateTime.UtcNow;

            await _noticiaRepository.AtualizarAsync(noticia, cancellationToken);

            return true;
        }
    }
}
