using EW.Desafio.WebApi.Models;

namespace EW.Desafio.WebApi.Services
{
    public interface ITarefaHistoricoAtualizacaoService
    {
        Task SalvarHistorico(Tarefa? tarefaAntes, Tarefa? tarefaDepois);
    }
}
