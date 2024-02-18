using EW.Desafio.WebApi.Enums;
using EW.Desafio.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EW.Desafio.WebApi.Services
{
    public interface ITarefaService
    {
        Task<ActionResult<IEnumerable<Tarefa>>> ObtenhaTarefas();
        Task<ActionResult<Tarefa>> ObtenhaTarefaPeloId(long id);
        Task<ActionResult<IEnumerable<Tarefa>>> ListagemDeTarefasPorProjeto(long projetoId);
        Task<ActionResult<Tarefa>> CadastrarTarefa(Tarefa tarefa);
        Task<IActionResult> AlterarTarefa(long id, Tarefa tarefa);
        Task<IActionResult> AlterarStatusTarefa(long id, Status status);
        Task<IActionResult> AlterarDescricaoTarefa(long id, string descricao);
        Task<IActionResult> DeletarTarefa(long id);
    }
}
