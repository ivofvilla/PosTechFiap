using FluentValidation;
using MediatR;
using PosTech.Noticia.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosTech.Noticia.API.Noticias.Delete
{
    public class DeleteNoticiaHandle : IRequestHandler<DeleteNoticiaCommand, bool>
    {
        private readonly INoticiaRepository _noticiaRepository;
        private readonly IValidator<DeleteNoticiaCommand> _validator;

        public DeleteNoticiaHandle(INoticiaRepository noticiaRepository, IValidator<DeleteNoticiaCommand> validator)
        {
            _noticiaRepository = noticiaRepository;
            _validator = validator;
        }

        public async Task<bool> Handle(DeleteNoticiaCommand command, CancellationToken cancellationToken = default)
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

            await _noticiaRepository.RemoverAsync(noticia, cancellationToken);

            return true;
        }
    }
}
