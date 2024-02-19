using EW.Desafio.WebApi.Models;
using EW.Desafio.WebApi.Repositories;
using EW.Desafio.WebApi.Uteis.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EW.Desafio.WebApi.Services
{
    public class TarefaHistoricoAtualizacaoService(
        ITarefaHistoricoAtualizacaoRepository tarefaHistoricoAtualizacaoRepository)
        : BaseService, ITarefaHistoricoAtualizacaoService
    {
        private readonly ITarefaHistoricoAtualizacaoRepository _tarefaHistoricoAtualizacaoRepository = tarefaHistoricoAtualizacaoRepository;

        public async Task<ActionResult<IEnumerable<TarefaHistoricoAtualizacao>>> ObtenhaTarefasConcluidas()
        {
            try
            {
                return Ok(await _tarefaHistoricoAtualizacaoRepository.ObtenhaHistoricoAtualizacaoTarefas());
            }
            catch (Exception)
            {
                return DefaultError();
            }
        }

        public async Task<IActionResult> SalvarHistorico(Tarefa? tarefaAntes, Tarefa? tarefaDepois)
        {
            try
            {
                var listaAlteracoes = new List<TarefaHistoricoAtualizacao>();

                if (tarefaAntes == null)
                {
                    listaAlteracoes.Add(TarefaHistoricoAtualizacao($"Tarefa: {tarefaDepois?.Id} cadastrada. Id do Projeto: {tarefaDepois?.ProjetoId}", tarefaAntes, tarefaDepois));
                }
                else if (tarefaDepois == null)
                {
                    listaAlteracoes.Add(TarefaHistoricoAtualizacao($"Tarefa: {tarefaAntes?.Id} excluída. Id do Projeto: {tarefaAntes?.ProjetoId}", tarefaAntes, tarefaDepois));
                }
                else
                {
                    if (tarefaAntes.UsuarioId != tarefaDepois.UsuarioId)
                    {
                        listaAlteracoes.Add(TarefaHistoricoAtualizacao($"Tarefa: {tarefaDepois.Id}. Usuário alterado - De: {tarefaAntes.UsuarioId} - Para: {tarefaDepois.UsuarioId}", tarefaAntes, tarefaDepois));
                    }
                    if (tarefaAntes.Titulo != tarefaDepois.Titulo)
                    {
                        listaAlteracoes.Add(TarefaHistoricoAtualizacao($"Tarefa: {tarefaDepois.Id}. Título alterado - De: {tarefaAntes.Titulo} - Para: {tarefaDepois.Titulo}", tarefaAntes, tarefaDepois));
                    }
                    if (tarefaAntes.Descricao != tarefaDepois.Descricao)
                    {
                        listaAlteracoes.Add(TarefaHistoricoAtualizacao($"Tarefa: {tarefaDepois.Id}. Descrição alterado - De: {tarefaAntes.Descricao} - Para: {tarefaDepois.Descricao}", tarefaAntes, tarefaDepois));
                    }
                    if (tarefaAntes.Status != tarefaDepois.Status)
                    {
                        listaAlteracoes.Add(TarefaHistoricoAtualizacao($"Tarefa: {tarefaDepois.Id}. Status alterado - De: {tarefaAntes.Status} - Para: {tarefaDepois.Status}", tarefaAntes, tarefaDepois));
                    }
                    if (tarefaAntes.DataVencimento != tarefaDepois.DataVencimento)
                    {
                        listaAlteracoes.Add(TarefaHistoricoAtualizacao($"Tarefa: {tarefaDepois.Id}. Data de vencimento alterado - De: {tarefaAntes.DataVencimento} - Para: {tarefaDepois.DataVencimento}", tarefaAntes, tarefaDepois));
                    }
                }

                foreach (var historicoAtualizacao in listaAlteracoes)
                {
                    await _tarefaHistoricoAtualizacaoRepository.Salvar(historicoAtualizacao);
                }
                return NoContent();
            }
            catch (Exception)
            {
                return DefaultError();
            }
        }

        public async Task<IActionResult> SalvarHistorico(TarefaHistoricoAtualizacao tarefaHistoricoAtualizacao)
        {
            try
            {
                await _tarefaHistoricoAtualizacaoRepository.Salvar(tarefaHistoricoAtualizacao);
                return NoContent();
            }
            catch (Exception)
            {
                return DefaultError();
            }
        }

        private TarefaHistoricoAtualizacao TarefaHistoricoAtualizacao(string alteracao, Tarefa? tarefaAntes, Tarefa? tarefaDepois)
        {
            return new TarefaHistoricoAtualizacao()
            {
                DataModificacao = DateTime.Now,
                TarefaId = tarefaAntes != null ? tarefaAntes.Id :
                        tarefaDepois != null ? tarefaDepois.Id :
                        throw new ConceitoNaoEncontradoException("Tarefa não encontrada!"),
                UsuarioId = tarefaAntes != null ? tarefaAntes.UsuarioId :
                        tarefaDepois != null ? tarefaDepois.UsuarioId :
                        throw new ConceitoNaoEncontradoException("Usuário não encontrado!"),
                Status = tarefaAntes != null ? tarefaAntes.Status :
                        tarefaDepois != null ? tarefaDepois.Status :
                        throw new ConceitoNaoEncontradoException("Status não encontrado!"),
                Alteracao = alteracao
            };
        }
    }
}
