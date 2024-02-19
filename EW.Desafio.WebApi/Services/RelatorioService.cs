using EW.Desafio.WebApi.Enums;
using EW.Desafio.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EW.Desafio.WebApi.Services
{
    public class RelatorioService(
        ITarefaHistoricoAtualizacaoService tarefaHistoricoAtualizacaoService)
        : BaseService, IRelatorioService
    {
        private readonly ITarefaHistoricoAtualizacaoService _tarefaHistoricoAtualizacaoService = tarefaHistoricoAtualizacaoService;

        public async Task<ActionResult<Relatorio>> ObtenhaTarefasConcluidasPorUsuarioUltimos30Dias()
        {
            try
            {
                var historicoAtualizacaoTarefas = (await _tarefaHistoricoAtualizacaoService.ObtenhaTarefasConcluidas()).Result;
                if (historicoAtualizacaoTarefas is not OkObjectResult objetoRetorno ||
                    objetoRetorno.Value == null)
                    return NoContent();

                var listaHistoricoAtualizacaoTarefas = (IEnumerable<TarefaHistoricoAtualizacao>)objetoRetorno.Value;

                // obtém os registros dos últimos 30 dias.
                var historicoAtualizacao30Dias = listaHistoricoAtualizacaoTarefas.Where(
                    x => x.DataModificacao >= DateTime.Now.AddDays(-30)).ToList();

                var relatorio = new Relatorio() { TotalDeRegistros = historicoAtualizacao30Dias.Count };
                var dadosRelatorio = new List<DadoRelatorio>();

                foreach (var historicoAtualizacaoTarefa in historicoAtualizacao30Dias)
                {
                    // não adiciona o usuário se ele já estiver adicionado na lista.
                    if (dadosRelatorio.Exists(x => x.UsuarioId == historicoAtualizacaoTarefa.UsuarioId))
                        continue;

                    // adiciona o usuário 
                    var dadoRelatorio =
                        new DadoRelatorio(
                            historicoAtualizacaoTarefa.Id,
                            historicoAtualizacao30Dias.Where(x =>
                                x.UsuarioId == historicoAtualizacaoTarefa.UsuarioId &&
                                x.Status == Status.Concluida).Count());
                    dadosRelatorio.Add(dadoRelatorio);
                }
                relatorio.Dados = dadosRelatorio;
                return Ok(relatorio);
            }
            catch (Exception)
            {
                return DefaultError();
            }
        }
    }
}
