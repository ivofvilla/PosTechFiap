using MediatR;
using PosTech.Noticia.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosTech_Fase.Queries
{
    public class GetAllResult
    {
        public IEnumerable<Noticias>? Noticias { get; set; }

        public GetAllResult()
        {
            Noticias = new List<Noticias>();
        }
    }
}
