using MediatR;
using PosTech.Noticia.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosTech_Fase.Queries
{
    public class GetAllHandle : IRequestHandler<GetAllQuery, GetAllResult>
    {
        private readonly INoticiaRepository _noticiaRepository;

        public GetAllHandle(INoticiaRepository noticiaRepository)
        {
            _noticiaRepository = noticiaRepository;
        }

        public async Task<GetAllResult> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var result = await _noticiaRepository.ObterTodosAsync(cancellationToken);

            var noticias = new GetAllResult();

            noticias.Noticias = result;

            return noticias;
        }
    }
}
