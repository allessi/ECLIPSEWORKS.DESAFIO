using EW.Desafio.WebApi.Enums;
using EW.Desafio.WebApi.Models;
using EW.Desafio.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace EW.Desafio.WebApi.Controllers
{
    [ApiController]
    [Route("api/tarefas")]
    public class TarefaController(ITarefaService tarefaService) : ControllerBase
    {
        private readonly ITarefaService _tarefaService = tarefaService;

        [HttpGet]
        [ProducesResponseType<Tarefa>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetTarefas()
        {
            return await _tarefaService.ObtenhaTarefas();
        }

        [HttpGet("{id}")]
        [ProducesResponseType<Tarefa>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Tarefa>> GetTarefa(long id)
        {
            return await _tarefaService.ObtenhaTarefaPeloId(id);
        }

        [HttpGet("projeto/{projetoId}")]
        [ProducesResponseType<Tarefa>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetTarefasPorProjeto(long projetoId)
        {
            return await _tarefaService.ListagemDeTarefasPorProjeto(projetoId);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Tarefa>> PostTarefa(Tarefa tarefa)
        {
            return await _tarefaService.CadastrarTarefa(tarefa);
        }

        [HttpPut("{id}/status")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutTarefaAlteracaoStatus(long id, [RequiredAttribute] Status status)
        {
            return await _tarefaService.AlterarStatusTarefa(id, status);
        }

        [HttpPut("{id}/descricao")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutTarefaAlteracaoDescricao(long id, [RequiredAttribute] string descricao)
        {
            return await _tarefaService.AlterarDescricaoTarefa(id, descricao);
        }

        [HttpDelete("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTarefa(long id)
        {
            return await _tarefaService.DeletarTarefa(id);
        }

    }
}
