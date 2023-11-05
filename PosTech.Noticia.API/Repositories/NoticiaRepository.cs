using Microsoft.EntityFrameworkCore;
using PosTech.Noticia.API.Data;

namespace PosTech.Noticia.API.Repositories
{
    public class NoticiaRepository : INoticiaRepository
    {
        private readonly DbContextClass _context;

        public NoticiaRepository(DbContextClass context)
        {
            _context = context;
        }
        public async Task AdicionarAsync(Models.Noticias noticia, CancellationToken cancellationToken = default)
        {
            _context.Noticias.Add(noticia);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AtualizarAsync(Models.Noticias noticia, CancellationToken cancellationToken = default)
        {
            _context.Noticias.Update(noticia);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Models.Noticias?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Noticias.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Models.Noticias>?> ObterTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Noticias.ToListAsync();
        }

        public async Task RemoverAsync(Models.Noticias noticia, CancellationToken cancellationToken = default)
        {
            _context.Noticias.Remove(noticia);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
