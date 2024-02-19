using EW.Desafio.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EW.Desafio.WebApi.Services
{
    public interface ITarefaHistoricoAtualizacaoService
    {
        Task<IActionResult> SalvarHistorico(Tarefa? tarefaAntes, Tarefa? tarefaDepois);
        Task<ActionResult<IEnumerable<TarefaHistoricoAtualizacao>>> ObtenhaTarefasConcluidas();
    }
}
