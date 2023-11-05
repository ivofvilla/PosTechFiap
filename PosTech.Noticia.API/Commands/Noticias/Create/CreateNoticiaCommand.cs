using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosTech.Noticia.API.Commands.Noticia
{
    public class CreateNoticiaCommand : IRequest<bool>
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Chapeu { get; set; }
        public string Autor { get; set; }

    }
}
