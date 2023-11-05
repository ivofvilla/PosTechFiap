using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosTech.Usuario.API.Commands
{
    public class CreateUsurarioCommand : IRequest<bool>
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public string ConfirmacaoSenha { get; set; }
        public string Nome { get; set; }
    }
}
