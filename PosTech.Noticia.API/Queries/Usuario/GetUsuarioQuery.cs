using MediatR;
using PosTech.Noticia.API.Models;

namespace PosTech_Fase.Queries.Login
{
    public class GetUsuarioQuery : IRequest<GetUsuarioResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
