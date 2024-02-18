using EW.Desafio.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace EW.Desafio.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetoController : ControllerBase
    {
        private readonly ApiContext _context;

        public ProjetoController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType<Projeto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Projeto>>> GetProjetos()
        {
            var projetos = await _context.Projetos.ToListAsync();
            if (projetos == null)
            {
                return NotFound();
            }
            var tarefas = await _context.Tarefas.ToListAsync();
            projetos.ForEach(projeto =>
            {
                var tarefasDoProjeto = tarefas.FindAll(x => x.ProjetoId == projeto.Id);
                projeto.Tarefas = tarefasDoProjeto;
            });

            return Ok(await _context.Projetos.ToListAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType<Projeto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Projeto>> GetProjeto(long id)
        {
            var projeto = await _context.Projetos.FindAsync(id);

            if (projeto == null)
            {
                return NotFound();
            }

            projeto.Tarefas = await _context.Tarefas.Where(x => x.ProjetoId == id).ToListAsync();

            return Ok(projeto);
        }

        [HttpGet("ListagemDeProjetosPorUsuario/{usuarioId}")]
        [ProducesResponseType<Tarefa>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Projeto>>> ListagemDeProjetosPorUsuario(long usuarioId)
        {
            if (usuarioId <= 0)
            {
                return BadRequest("O Id do usuário deve ser informado.");
            }

            var projetos = await _context.Projetos.Where(x => x.UsuarioId == usuarioId).ToListAsync();

            if (projetos == null || !projetos.Any())
            {
                return NotFound();
            }

            var tarefas = await _context.Tarefas.ToListAsync();

            projetos.ForEach(projeto =>
            {
                var tarefasDoProjeto = tarefas.FindAll(x => x.ProjetoId == projeto.Id);
                projeto.Tarefas = tarefasDoProjeto;
            });

            return Ok(projetos);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Projeto>> PostProjeto(Projeto projeto)
        {
            if (projeto.UsuarioId <= 0)
            {
                return BadRequest("O Id do usuário deve ser informado.");
            }

            var usuario = await _context.Usuarios.FindAsync(projeto.UsuarioId);
            if (usuario == null)
            {
                return NotFound("O Id do usuário informado não existe.");
            }

            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjeto", new { id = projeto.Id }, projeto);
        }

    }
}
