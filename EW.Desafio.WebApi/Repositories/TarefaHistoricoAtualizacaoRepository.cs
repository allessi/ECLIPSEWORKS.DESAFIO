
using EW.Desafio.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EW.Desafio.WebApi.Repositories
{
    public class TarefaHistoricoAtualizacaoRepository(
        ApiContext context)
        : ITarefaHistoricoAtualizacaoRepository
    {
        private readonly ApiContext _context = context;

        public async Task<IEnumerable<TarefaHistoricoAtualizacao>> ObtenhaHistoricoAtualizacaoTarefas()
        {
            return await _context.TarefaHistoricoAtualizacoes.ToListAsync();
        }

        public async Task Salvar(TarefaHistoricoAtualizacao tarefaHistoricoAtualizacao)
        {
            _context.TarefaHistoricoAtualizacoes.Add(tarefaHistoricoAtualizacao);
            await _context.SaveChangesAsync();
        }
    }
}
