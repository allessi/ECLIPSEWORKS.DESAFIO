using EW.Desafio.WebApi.Models;

namespace EW.Desafio.WebApi.Repositories
{
    public interface ITarefaRepository
    {
        Task<IEnumerable<Tarefa>> ObtenhaTarefas();
        Task<Tarefa> ObtenhaTarefaPeloId(long id);
        Task<IEnumerable<Tarefa>> ObtenhaTarefasPeloProjeto(long projetoId);
        Task Alterar(Tarefa tarefa);
        Task Cadastrar(Tarefa tarefa);
        Task Deletar(Tarefa tarefa);
    }
}
