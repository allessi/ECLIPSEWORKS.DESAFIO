using EW.Desafio.WebApi.Enums;
using EW.Desafio.WebApi.Models;
using EW.Desafio.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EW.Desafio.WebApi.Controllers
{
    public abstract class ControllerBaseApi(
        ApiContext context,
        IAutorizacaoService autorizacaoService) : ControllerBase
    {
        private readonly ApiContext _context = context;
        protected readonly IAutorizacaoService _autorizacaoService = autorizacaoService;

        //Como não foi passado como seria a forma de obter o usuário logado no desafio.
        //neste caso, criei uma verificação fictícia que poder o ponto onde o teste
        //como gerente e usuário comum pode ser feito.
        protected async Task<Usuario> ObtenhaUsuarioLogado()
        {
            if (_context == null || _context.Usuarios == null)
            {
                throw new Exception("Não foi possível obter os usuários.");
            }
            //obtenha usuario comum
            //return _context.Usuarios.FirstOrDefault() ?? throw new Exception("Não foi possível obter o usuário.");

            // obtenha o gerente
            var teste = await _context.Usuarios.Where(u => u.Funcao == Funcao.Gerente).ToListAsync();
            return teste.FirstOrDefault() ?? throw new Exception("Não foi possível obter o usuário.");
        }
    }
}
