﻿using EW.Desafio.WebApi.Enums;
using EW.Desafio.WebApi.Models;
using EW.Desafio.WebApi.Repositories;
using EW.Desafio.WebApi.Uteis.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EW.Desafio.WebApi.Services
{
    public class TarefaService(
        ITarefaRepository tarefaRepository,
        IProjetoRepository projetoRepository,
        ITarefaHistoricoAtualizacaoService tarefaHistoricoAtualizacaoService)
        : BaseService, ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository = tarefaRepository;
        private readonly IProjetoRepository _projetoRepository = projetoRepository;
        private readonly ITarefaHistoricoAtualizacaoService _tarefaHistoricoAtualizacaoService = tarefaHistoricoAtualizacaoService;

        private const short QuantidadeMaximaTarefasProjeto = 20;

        public async Task<ActionResult<Tarefa>> CadastrarTarefa(Tarefa tarefa)
        {
            try
            {
                // valida dados do projeto
                if (tarefa.ProjetoId <= 0) return BadRequest("O Id do projeto deve ser informado.");
                var projeto = await _projetoRepository.ObtenhaProjetoPeloId(tarefa.ProjetoId);
                projeto.Tarefas = (await _tarefaRepository.ObtenhaTarefasPeloProjeto(tarefa.ProjetoId)).ToList();
                projeto.Tarefas.Add(tarefa);

                // valida se a quantidade máxima de tarefas por projeto foi estrapolada.
                _ = PassouQuantidadeMaximaDeTarefasPermitidasNoProjeto(projeto);

                // salva a tarefa.
                await _tarefaRepository.Cadastrar(tarefa);

                // salva histórico do cadastro
                await _tarefaHistoricoAtualizacaoService.SalvarHistorico(null, tarefa);

                return CreatedAtAction("GetTarefa", new { id = tarefa.Id }, tarefa);
            }
            catch (ConceitoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (QuantidadeMaximaTarefaPorProjetoException ex)
            {
                return BadRequest(ex.Message);
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
                return Ok(await _tarefaRepository.ObtenhaTarefasPeloProjeto(projetoId));
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

        public async Task<IActionResult> AlterarTarefa(long id, Tarefa dadosTarefa)
        {
            try
            {
                if (id != dadosTarefa.Id) return BadRequest("Os ids informados no parâmetro e no corpo estão inconsistentes.");

                var tarefa = await _tarefaRepository.ObtenhaTarefaPeloId(id);
                var tarefaAntiga = new Tarefa
                {
                    UsuarioId = tarefa.UsuarioId,
                    ProjetoId = tarefa.ProjetoId,
                    Id = tarefa.Id,
                    Titulo = tarefa.Titulo,
                    Descricao = tarefa.Descricao,
                    Prioridade = tarefa.Prioridade,
                    DataVencimento = tarefa.DataVencimento,
                    Status = tarefa.Status,
                    Comentarios = tarefa.Comentarios,
                };

                var tarefaNova = new Tarefa
                {
                    UsuarioId = dadosTarefa.UsuarioId,
                    ProjetoId = dadosTarefa.ProjetoId,
                    Id = dadosTarefa.Id,
                    Titulo = dadosTarefa.Titulo,
                    Descricao = dadosTarefa.Descricao,
                    Prioridade = dadosTarefa.Prioridade,
                    DataVencimento = dadosTarefa.DataVencimento,
                    Status = dadosTarefa.Status,
                    Comentarios = dadosTarefa.Comentarios
                };

                // valida se a prioridade foi alterada.
                if (tarefaAntiga.Prioridade != tarefaNova.Prioridade)
                {
                    return BadRequest("A prioridade da tarefa não pode ser alterada.");
                } // valida se o id do projeto foi alterado.
                else if (tarefaAntiga.ProjetoId != tarefaNova.ProjetoId)
                {
                    return BadRequest("O projeto da tarefa não pode ser alterado.");
                }

                tarefa.UsuarioId = dadosTarefa.UsuarioId;
                tarefa.Titulo = dadosTarefa.Titulo;
                tarefa.Descricao = dadosTarefa.Descricao;
                tarefa.DataVencimento = dadosTarefa.DataVencimento;
                tarefa.Status = dadosTarefa.Status;

                if (dadosTarefa.Comentarios != null)
                {
                    tarefa.Comentarios ??= new List<TarefaComentario>();

                    foreach (var comentarioTarefa in dadosTarefa.Comentarios)
                    {
                        if (!tarefa.Comentarios.Any(x => x.Id == comentarioTarefa.Id))
                        {
                            ((List<TarefaComentario>)tarefa.Comentarios).Add(comentarioTarefa);
                            await _tarefaRepository.CadastrarComentarios(comentarioTarefa);
                            var novoHistorico = new TarefaHistoricoAtualizacao()
                            {
                                DataModificacao = DateTime.Now,
                                Status = dadosTarefa.Status,
                                TarefaId = dadosTarefa.Id,
                                UsuarioId = dadosTarefa.UsuarioId,
                                Alteracao = $"Tarefa: {dadosTarefa.Id}. Id do Projeto: {dadosTarefa.ProjetoId}. Comentário adicionado: {comentarioTarefa.Comententario}"
                            };
                            await _tarefaHistoricoAtualizacaoService.SalvarHistorico(novoHistorico);
                        }
                    }
                }

                // altera todos os dados da tarefa.
                await _tarefaRepository.Alterar(tarefa);

                // salva histórico da alteração
                await _tarefaHistoricoAtualizacaoService.SalvarHistorico(tarefaAntiga, tarefaNova);

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

        public async Task<IActionResult> AlterarStatusTarefa(long id, Status status)
        {
            try
            {
                var tarefa = await _tarefaRepository.ObtenhaTarefaPeloId(id);
                var tarefaAntiga = new Tarefa
                {
                    UsuarioId = tarefa.UsuarioId,
                    ProjetoId = tarefa.ProjetoId,
                    Id = tarefa.Id,
                    Titulo = tarefa.Titulo,
                    Descricao = tarefa.Descricao,
                    Prioridade = tarefa.Prioridade,
                    DataVencimento = tarefa.DataVencimento,
                    Status = tarefa.Status,
                    Comentarios = tarefa.Comentarios
                };

                var tarefaNova = new Tarefa
                {
                    UsuarioId = tarefaAntiga.UsuarioId,
                    ProjetoId = tarefaAntiga.ProjetoId,
                    Id = tarefaAntiga.Id,
                    Titulo = tarefaAntiga.Titulo,
                    Descricao = tarefaAntiga.Descricao,
                    Prioridade = tarefaAntiga.Prioridade,
                    DataVencimento = tarefaAntiga.DataVencimento,
                    Status = status,
                    Comentarios = tarefaAntiga.Comentarios
                };
                tarefa.Status = status;

                // altera o status
                await _tarefaRepository.Alterar(tarefa);

                // salva histórico da alteração
                await _tarefaHistoricoAtualizacaoService.SalvarHistorico(tarefaAntiga, tarefaNova);

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
                var tarefaAntiga = new Tarefa
                {
                    UsuarioId = tarefa.UsuarioId,
                    ProjetoId = tarefa.ProjetoId,
                    Id = tarefa.Id,
                    Titulo = tarefa.Titulo,
                    Descricao = tarefa.Descricao,
                    Prioridade = tarefa.Prioridade,
                    DataVencimento = tarefa.DataVencimento,
                    Status = tarefa.Status,
                    Comentarios = tarefa.Comentarios
                };

                var tarefaNova = new Tarefa
                {
                    UsuarioId = tarefaAntiga.UsuarioId,
                    ProjetoId = tarefaAntiga.ProjetoId,
                    Id = tarefaAntiga.Id,
                    Titulo = tarefaAntiga.Titulo,
                    Descricao = descricao,
                    Prioridade = tarefaAntiga.Prioridade,
                    DataVencimento = tarefaAntiga.DataVencimento,
                    Status = tarefaAntiga.Status,
                    Comentarios = tarefaAntiga.Comentarios
                };
                tarefa.Descricao = descricao;

                // altera o status
                await _tarefaRepository.Alterar(tarefa);

                // salva histórico da alteração
                await _tarefaHistoricoAtualizacaoService.SalvarHistorico(tarefaAntiga, tarefaNova);

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

                // salva histórico da exclusão
                await _tarefaHistoricoAtualizacaoService.SalvarHistorico(tarefa, null);
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

        public Task<bool> PassouQuantidadeMaximaDeTarefasPermitidasNoProjeto(Projeto? projeto)
        {
            if (projeto?.Tarefas != null && projeto.Tarefas.Count > QuantidadeMaximaTarefasProjeto)
            {
                throw new QuantidadeMaximaTarefaPorProjetoException();
            }
            return Task.FromResult(false);
        }
    }
}
