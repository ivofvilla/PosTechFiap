using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosTech.Noticia.API.Noticias
{
    public class DeleteNoticiaCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
