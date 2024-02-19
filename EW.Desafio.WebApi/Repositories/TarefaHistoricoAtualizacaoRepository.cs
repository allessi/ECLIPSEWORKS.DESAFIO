
using EW.Desafio.WebApi.Models;

namespace EW.Desafio.WebApi.Repositories
{
    public class TarefaHistoricoAtualizacaoRepository(ApiContext context) : ITarefaHistoricoAtualizacaoRepository
    {
        private readonly ApiContext _context = context;

        public async Task Salvar(TarefaHistoricoAtualizacao tarefaHistoricoAtualizacao)
        {
            _context.TarefaHistoricoAtualizacoes.Add(tarefaHistoricoAtualizacao);
            await _context.SaveChangesAsync();
        }
    }
}
