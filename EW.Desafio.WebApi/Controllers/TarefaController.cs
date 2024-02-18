using EW.Desafio.WebApi.Enums;
using EW.Desafio.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace EW.Desafio.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly ApiContext _context;

        public TarefaController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType<Tarefa>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetTarefas()
        {
            return Ok(await _context.Tarefas.ToListAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType<Tarefa>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Tarefa>> GetTarefa(long id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);

            if (tarefa == null)
            {
                return NotFound();
            }

            return Ok(tarefa);
        }

        [HttpGet("ListagemDeTarefasPorProjeto/{projetoId}")]
        [ProducesResponseType<Tarefa>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Tarefa>>> ListagemDeTarefasPorProjeto(long projetoId)
        {
            return await _context.Tarefas.Where(x => x.ProjetoId == projetoId).ToListAsync();
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Tarefa>> PostTarefa(Tarefa tarefa)
        {
            if (tarefa.ProjetoId <= 0)
            {
                return BadRequest("O Id do projeto deve ser informado.");
            }

            var projeto = await _context.Projetos.FindAsync(tarefa.ProjetoId);
            if (projeto == null)
            {
                return NotFound("O Id do projeto informado não existe.");
            }

            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTarefa", new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}/status")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutTarefa(long id, [RequiredAttribute] Status status)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);

            if (tarefa == null)
            {
                return NotFound();
            }

            tarefa.Status = status;
            _context.Entry(tarefa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarefaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPut("{id}/descricao")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutTarefa(long id, [RequiredAttribute] string descricao)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);

            if (tarefa == null)
            {
                return NotFound();
            }

            tarefa.Descricao = descricao;
            _context.Entry(tarefa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarefaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTarefa(long id)
        {
            var produto = await _context.Tarefas.FindAsync(id);
            if (produto == null)
            {
                return NotFound("O Id da tarefa informado não existe.");
            }

            _context.Tarefas.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TarefaExists(long id)
        {
            return _context.Tarefas.Any(e => e.Id == id);
        }
    }
}
