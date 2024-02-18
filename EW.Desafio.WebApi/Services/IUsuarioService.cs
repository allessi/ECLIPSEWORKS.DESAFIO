using EW.Desafio.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EW.Desafio.WebApi.Services
{
    public interface IUsuarioService
    {
        Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios();
        Task<ActionResult<Usuario>> GetUsuario(long id);
    }
}
