using MediatR;
using PosTech.Noticia.API.Models;

namespace PosTech_Fase2.Application.Queries.Noticias.GetById
{
    public class GetNoticiaByIdQuery : IRequest<PosTech.Noticia.API.Models.Noticias>
    {
        public Guid Id { get; set; }
    }
}
