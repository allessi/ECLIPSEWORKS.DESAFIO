using EW.Desafio.WebApi.Models;
using EW.Desafio.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EW.Desafio.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController(IUsuarioService usuarioService) : ControllerBase
    {
        private readonly IUsuarioService _usuarioService = usuarioService;

        [HttpGet]
        [ProducesResponseType<Usuario>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _usuarioService.GetUsuarios();
        }

        [HttpGet("{id}")]
        [ProducesResponseType<Usuario>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Usuario>> GetUsuario(long id)
        {
            return await _usuarioService.GetUsuario(id);
        }
    }
}
