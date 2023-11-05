using MediatR;
using PosTech.Noticia.API.Models;

namespace PosTech_Fase.Queries.Login
{
    public class GetUsuarioResult 
    {
        public string Token { get; private set; }

        public void SetToken(string token) { Token = token; }
    }
}
