using EW.Desafio.WebApi.Models;

namespace EW.Desafio.WebApi.Repositories
{
    public interface IProjetoRepository
    {
        Task<Projeto> ObtenhaProjetoPeloId(long id);
        Task<IEnumerable<Projeto>> ObtenhaProjetos();
        Task<IEnumerable<Projeto>> ObtenhaProjetoPeloUsario(long usuarioId);
        Task Cadastrar(Projeto projeto);
        Task Deletar(Projeto projeto);
    }
}
