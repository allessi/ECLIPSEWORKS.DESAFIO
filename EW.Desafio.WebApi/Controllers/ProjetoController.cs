using EW.Desafio.WebApi.Models;
using EW.Desafio.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace EW.Desafio.WebApi.Controllers
{
    [ApiController]
    [Route("api/projetos")]
    public class ProjetoController(IProjetoService projetoService) : ControllerBase
    {
        private readonly IProjetoService _projetoService = projetoService;

        [HttpGet]
        [ProducesResponseType<Projeto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Projeto>>> GetProjetos()
        {
            return await _projetoService.ObtenhaProjetos();
        }

        [HttpGet("{id}")]
        [ProducesResponseType<Projeto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Projeto>> GetProjeto(long id)
        {
            return await _projetoService.ObtenhaProjetoPeloId(id);
        }

        [HttpGet("usuario/{usuarioId}")]
        [ProducesResponseType<Tarefa>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Projeto>>> GetProjetoPorUsuario(long usuarioId)
        {
            return await _projetoService.ObtenhaProjetoPeloUsario(usuarioId);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Projeto>> PostProjeto(Projeto projeto)
        {
            return await _projetoService.CadastrarProjeto(projeto);
        }
    }
}
