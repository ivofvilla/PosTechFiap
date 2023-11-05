namespace PosTech.Noticia.API.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Models.Usuarios?> ObterLogin(string email, string senha, CancellationToken cancellationToken = default);
        Task<Models.Usuarios?> Inserir(Models.Usuarios usuario, CancellationToken cancellationToken = default);

    }
}
