
namespace PosTech.Noticia.API.Repositories
{
    public interface INoticiaRepository
    {
        Task<Models.Noticias?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Models.Noticias>?> ObterTodosAsync(CancellationToken cancellationToken = default);
        Task AdicionarAsync(Models.Noticias album, CancellationToken cancellationToken = default);
        Task AtualizarAsync(Models.Noticias album, CancellationToken cancellationToken = default);
        Task RemoverAsync(Models.Noticias album, CancellationToken cancellationToken = default);
    }
}
