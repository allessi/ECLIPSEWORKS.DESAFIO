using EW.Desafio.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EW.Desafio.WebApi.Services
{
    public interface IUsuarioService
    {
        Task<ActionResult<IEnumerable<Usuario>>> ObtenhaUsuarios();
        Task<ActionResult<Usuario>> ObtenhaUsuarioPeloId(long id);
    }
}
