using FluentValidation;
using MediatR;
using PosTech.Noticia.API.Repositories;

namespace PosTech_Fase2.Application.Queries.Noticias.GetById
{
    public class GetNoticiaByIdHandle : IRequestHandler<GetNoticiaByIdQuery, PosTech.Noticia.API.Models.Noticias>
    {
        private readonly INoticiaRepository _noticiaRepository;
        private readonly IValidator<GetNoticiaByIdQuery> _validator;

        public GetNoticiaByIdHandle(INoticiaRepository noticiaRepository, IValidator<GetNoticiaByIdQuery> validator)
        {
            _noticiaRepository = noticiaRepository;
            //comentario
            _validator = validator;
        }

        public async Task<PosTech.Noticia.API.Models.Noticias> Handle(GetNoticiaByIdQuery query, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(query, cancellationToken);
            if (!validationResult.IsValid)
            {
                return null;
            }

            var album = await _noticiaRepository.ObterPorIdAsync(query.Id, cancellationToken);

            return album;
        }
    }
}
