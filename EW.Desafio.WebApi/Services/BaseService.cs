using Microsoft.AspNetCore.Mvc;

namespace EW.Desafio.WebApi.Services
{
    public abstract class BaseService : ControllerBase
    {

        protected ActionResult Error(int statusCode, string mensagem)
        {
            return new ObjectResult(new ResultError(statusCode, mensagem));
        }

        protected ActionResult DefaultError()
        {
            return Error(StatusCodes.Status500InternalServerError, "Erro interno, verificar com administrador");
        }

    }

    public record ResultError(int statusCode, string mensagem);
}
