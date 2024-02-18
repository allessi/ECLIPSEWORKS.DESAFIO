using EW.Desafio.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EW.Desafio.WebApi.Services
{
    public interface IProjetoService
    {
        Task<ActionResult<IEnumerable<Projeto>>> ObtenhaProjetos();
        Task<ActionResult<Projeto>> ObtenhaProjetoPeloId(long id);
        Task<ActionResult<IEnumerable<Projeto>>> ObtenhaProjetoPeloUsario(long usuarioId);
        Task<ActionResult<Projeto>> CadastrarProjeto(Projeto projeto);
    }
}
