using Microsoft.EntityFrameworkCore;
using PosTech.Noticia.API.Data;
using PosTech.Noticia.API.Models;

namespace PosTech.Noticia.API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DbContextClass _context;

        public UsuarioRepository(DbContextClass context)
        {
            _context = context;
        }

        public async Task<Usuarios?> Inserir(Usuarios usuario, CancellationToken cancellationToken = default)
        {
            await _context.Usuario.AddAsync(usuario);
            _context.SaveChanges();

            return usuario;
        }

        public async Task<Usuarios?> ObterLogin(string usuario, string senha, CancellationToken cancellationToken = default)
        {
            return await _context.Usuario.FirstOrDefaultAsync(w => w.Login == usuario && w.Senha == senha);
        }
    }
}
