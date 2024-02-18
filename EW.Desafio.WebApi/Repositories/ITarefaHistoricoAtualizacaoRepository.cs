using EW.Desafio.WebApi.Models;

namespace EW.Desafio.WebApi.Repositories
{
    public interface ITarefaHistoricoAtualizacaoRepository
    {
        Task Salvar(TarefaHistoricoAtualizacao tarefa);
    }
}
