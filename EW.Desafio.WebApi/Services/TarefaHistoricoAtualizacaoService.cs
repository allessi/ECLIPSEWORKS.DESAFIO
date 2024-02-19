using EW.Desafio.WebApi.Models;
using EW.Desafio.WebApi.Repositories;
using EW.Desafio.WebApi.Uteis.Exceptions;

namespace EW.Desafio.WebApi.Services
{
    public class TarefaHistoricoAtualizacaoService(
        ITarefaHistoricoAtualizacaoRepository tarefaHistoricoAtualizacaoRepository)
        : ITarefaHistoricoAtualizacaoService
    {
        private readonly ITarefaHistoricoAtualizacaoRepository _tarefaHistoricoAtualizacaoRepository = tarefaHistoricoAtualizacaoRepository;

        public async Task SalvarHistorico(Tarefa? tarefaAntes, Tarefa? tarefaDepois)
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
                    if (tarefaAntes.Titulo != tarefaDepois.Titulo)
                    {
                        listaAlteracoes.Add(TarefaHistoricoAtualizacao($"Tarefa: {tarefaDepois.Id} alterada. De: {tarefaAntes.Titulo} - Para: {tarefaDepois.Titulo}", tarefaAntes, tarefaDepois));
                    }
                    if (tarefaAntes.Descricao != tarefaDepois.Descricao)
                    {
                        listaAlteracoes.Add(TarefaHistoricoAtualizacao($"Tarefa: {tarefaDepois.Id} alterada. De: {tarefaAntes.Descricao} - Para: {tarefaDepois.Descricao}", tarefaAntes, tarefaDepois));
                    }
                    if (tarefaAntes.Status != tarefaDepois.Status)
                    {
                        listaAlteracoes.Add(TarefaHistoricoAtualizacao($"Tarefa: {tarefaDepois.Id} alterada. De: {tarefaAntes.Status} - Para: {tarefaDepois.Status}", tarefaAntes, tarefaDepois));
                    }
                    if (tarefaAntes.DataVencimento != tarefaDepois.DataVencimento)
                    {
                        listaAlteracoes.Add(TarefaHistoricoAtualizacao($"Tarefa: {tarefaDepois.Id} alterada. De: {tarefaAntes.DataVencimento} - Para: {tarefaDepois.DataVencimento}", tarefaAntes, tarefaDepois));
                    }
                }

                foreach (var historicoAtualizacao in listaAlteracoes)
                {
                    await _tarefaHistoricoAtualizacaoRepository.Salvar(historicoAtualizacao);
                }
            }
            catch (Exception)
            {
                throw;
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
                UsuarioId = 1, //todo: verificar para obter o usuário que alterou a tarefa
                Alteracao = alteracao
            };
        }
    }
}
