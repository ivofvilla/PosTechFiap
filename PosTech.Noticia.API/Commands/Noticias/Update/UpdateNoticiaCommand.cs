using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosTech.Noticia.API.Noticias.Update
{
    public class UpdateNoticiaCommand : IRequest<bool>
    {
        public Guid Id { get; private set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Chapeu { get; set; }
        public string Autor { get; set; }
        public void SetId(Guid id) => Id = id;
    }
}
