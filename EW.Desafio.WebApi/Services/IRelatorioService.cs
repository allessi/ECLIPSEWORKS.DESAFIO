using EW.Desafio.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EW.Desafio.WebApi.Services
{
    public interface IRelatorioService
    {
        Task<ActionResult<Relatorio>> ObtenhaTarefasConcluidasPorUsuarioUltimos30Dias();
    }
}
