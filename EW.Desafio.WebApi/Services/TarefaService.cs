using EW.Desafio.WebApi.Enums;
using EW.Desafio.WebApi.Models;
using EW.Desafio.WebApi.Repositories;
using EW.Desafio.WebApi.Uteis.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EW.Desafio.WebApi.Services
{
    public class TarefaService(ITarefaRepository tarefaRepository, IProjetoRepository projetoRepository)
        : BaseService, ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository = tarefaRepository;
        private readonly IProjetoRepository _projetoRepository = projetoRepository;

        public async Task<ActionResult<Tarefa>> CadastrarTarefa(Tarefa tarefa)
        {
            try
            {
                // valida dados do projeto
                if (tarefa.ProjetoId <= 0) return BadRequest("O Id do projeto deve ser informado.");
                _ = await _projetoRepository.ObtenhaProjetoPeloId(tarefa.ProjetoId);

                // salva a tarefa.
                await _tarefaRepository.Cadastrar(tarefa);

                return CreatedAtAction("GetTarefa", new { id = tarefa.Id }, tarefa);
            }
            catch (ConceitoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return DefaultError();
            }
        }

        public async Task<ActionResult<IEnumerable<Tarefa>>> ListagemDeTarefasPorProjeto(long projetoId)
        {
            try
            {
                return Ok(await _tarefaRepository.ListagemDeTarefasPorProjeto(projetoId));
            }
            catch (Exception)
            {
                return DefaultError();
            }
        }

        public async Task<ActionResult<Tarefa>> ObtenhaTarefaPeloId(long id)
        {
            try
            {
                return Ok(await _tarefaRepository.ObtenhaTarefaPeloId(id));
            }
            catch (ConceitoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return DefaultError();
            }
        }

        public async Task<ActionResult<IEnumerable<Tarefa>>> ObtenhaTarefas()
        {
            try
            {
                return Ok(await _tarefaRepository.ObtenhaTarefas());
            }
            catch (Exception)
            {
                return DefaultError();
            }
        }

        public async Task<IActionResult> AlterarStatusTarefa(long id, Status status)
        {
            try
            {
                var tarefa = await _tarefaRepository.ObtenhaTarefaPeloId(id);
                tarefa.Status = status;

                await _tarefaRepository.Alterar(tarefa);

                return NoContent();
            }
            catch (ConceitoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return DefaultError();
            }
        }

        public async Task<IActionResult> AlterarDescricaoTarefa(long id, string descricao)
        {
            try
            {
                var tarefa = await _tarefaRepository.ObtenhaTarefaPeloId(id);
                tarefa.Descricao = descricao;

                await _tarefaRepository.Alterar(tarefa);

                return NoContent();
            }
            catch (ConceitoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return DefaultError();
            }
        }

        public async Task<IActionResult> DeletarTarefa(long id)
        {
            try
            {
                var tarefa = await _tarefaRepository.ObtenhaTarefaPeloId(id);
                await _tarefaRepository.Deletar(tarefa);

                return NoContent();
            }
            catch (ConceitoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return DefaultError();
            }
        }
    }
}
