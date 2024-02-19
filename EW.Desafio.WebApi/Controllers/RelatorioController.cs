using EW.Desafio.WebApi.Models;
using EW.Desafio.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace EW.Desafio.WebApi.Controllers
{
    [ApiController]
    [Route("api/relatorios")]
    public class RelatorioController(
        IRelatorioService relatorioService,
        IAutorizacaoService autorizacaoService,
        ApiContext context)
        : ControllerBaseApi(context, autorizacaoService)
    {
        private readonly IRelatorioService _relatorioService = relatorioService;

        [HttpGet("gerencialMensal")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType<Relatorio>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Relatorio>> GerencialMensal()
        {
            var usuarioLogado = await ObtenhaUsuarioLogado();
            if (!_autorizacaoService.UsuarioEhGerente(usuarioLogado))
            {
                return Forbid("Usuário logado não está autorizado para acessar o relatório");
            }
            return await _relatorioService.ObtenhaTarefasConcluidasPorUsuarioUltimos30Dias();

        }
    }
}
