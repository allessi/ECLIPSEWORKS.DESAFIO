using EW.Desafio.WebApi.Models;

namespace EW.Desafio.WebApi.Services
{
    public interface IAutorizacaoService
    {
        bool UsuarioEhGerente(Usuario usuario);
    }
}
