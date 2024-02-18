using EW.Desafio.WebApi.Models;

namespace EW.Desafio.WebApi.Repositories
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> ObtenhaUsuarios();
        Task<Usuario> ObtenhaUsuarioPeloId(long id);
    }
}
