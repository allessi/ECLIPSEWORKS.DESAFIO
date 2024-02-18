using EW.Desafio.WebApi.Models;
using EW.Desafio.WebApi.Uteis.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EW.Desafio.WebApi.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApiContext _context;

        public UsuarioRepository(ApiContext context)
        {
            _context = context;
        }

        public async Task<Usuario> ObtenhaUsuarioPeloId(long id)
        {
            return await _context.Usuarios.FindAsync(id) ?? throw new ConceitoNaoEncontradoException("Usuário não encontrado!");
        }

        public async Task<IEnumerable<Usuario>> ObtenhaUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }
    }
}
