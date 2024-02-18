using EW.Desafio.WebApi.Models;
using EW.Desafio.WebApi.Repositories;
using EW.Desafio.WebApi.Uteis.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EW.Desafio.WebApi.Services
{
    public class UsuarioService(IUsuarioRepository usuarioRepository) : BaseService, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

        public async Task<ActionResult<Usuario>> ObtenhaUsuarioPeloId(long id)
        {
            try
            {
                return Ok(await _usuarioRepository.ObtenhaUsuarioPeloId(id));
            }
            catch (ConceitoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return DefaultError();
            }
        }

        public async Task<ActionResult<IEnumerable<Usuario>>> ObtenhaUsuarios()
        {
            try
            {
                return Ok(await _usuarioRepository.ObtenhaUsuarios());
            }
            catch (Exception)
            {
                return DefaultError();
            }
        }
    }
}
