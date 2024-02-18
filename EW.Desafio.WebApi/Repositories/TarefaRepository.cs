using EW.Desafio.WebApi.Models;
using EW.Desafio.WebApi.Uteis.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EW.Desafio.WebApi.Repositories
{
    public class TarefaRepository(ApiContext context) : ITarefaRepository
    {
        private readonly ApiContext _context = context;

        public async Task<IEnumerable<Tarefa>> ListagemDeTarefasPorProjeto(long projetoId)
        {
            return await _context.Tarefas.Where(x => x.ProjetoId == projetoId).ToListAsync();
        }

        public async Task<Tarefa> ObtenhaTarefaPeloId(long id)
        {
            return await _context.Tarefas.FindAsync(id) ?? throw new ConceitoNaoEncontradoException("Tarefa não encontrada!");
        }

        public async Task<IEnumerable<Tarefa>> ObtenhaTarefas()
        {
            return await _context.Tarefas.ToListAsync();
        }

        public async Task Cadastrar(Tarefa tarefa)
        {
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task Alterar(Tarefa tarefa)
        {
            try
            {
                _context.Entry(tarefa).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarefaExists(tarefa.Id))
                {
                    throw new ConceitoNaoEncontradoException("Tarefa não encontrada!");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task Deletar(Tarefa tarefa)
        {
            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
        }

        private bool TarefaExists(long id)
        {
            return _context.Tarefas.Any(e => e.Id == id);
        }
    }
}
