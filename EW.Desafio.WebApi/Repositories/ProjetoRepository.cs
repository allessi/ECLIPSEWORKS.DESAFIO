using EW.Desafio.WebApi.Models;
using EW.Desafio.WebApi.Uteis.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EW.Desafio.WebApi.Repositories
{
    public class ProjetoRepository(ApiContext context) : IProjetoRepository
    {
        private readonly ApiContext _context = context;

        public async Task<Projeto> ObtenhaProjetoPeloId(long id)
        {
            return await _context.Projetos.FindAsync(id) ?? throw new ConceitoNaoEncontradoException("Projeto não encontrado!");
        }

        public async Task<IEnumerable<Projeto>> ObtenhaProjetos()
        {
            return await _context.Projetos.ToListAsync();
        }

        public async Task<IEnumerable<Projeto>> ObtenhaProjetoPeloUsario(long usuarioId)
        {
            var projetos = await _context.Projetos.Where(x => x.UsuarioId == usuarioId).ToListAsync();
            return projetos;
        }

        public async Task Cadastrar(Projeto projeto)
        {
            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Projeto projeto)
        {
            _context.Projetos.Remove(projeto);
            await _context.SaveChangesAsync();
        }
    }
}
