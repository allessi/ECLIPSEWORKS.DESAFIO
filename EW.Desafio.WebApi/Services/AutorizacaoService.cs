using EW.Desafio.WebApi.Enums;
using EW.Desafio.WebApi.Models;

namespace EW.Desafio.WebApi.Services;

public class AutorizacaoService : IAutorizacaoService
{
    public bool UsuarioEhGerente(Usuario usuario) => usuario != null && usuario.Funcao == Funcao.Gerente;
}
