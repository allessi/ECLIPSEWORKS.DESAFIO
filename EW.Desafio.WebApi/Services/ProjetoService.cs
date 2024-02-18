using EW.Desafio.WebApi.Models;
using EW.Desafio.WebApi.Repositories;
using EW.Desafio.WebApi.Uteis.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EW.Desafio.WebApi.Services
{
    public class ProjetoService(
        ITarefaRepository tarefaRepository,
        IProjetoRepository projetoRepository,
        IUsuarioRepository usuarioRepository)
        : BaseService, IProjetoService
    {
        private readonly ITarefaRepository _tarefaRepository = tarefaRepository;
        private readonly IProjetoRepository _projetoRepository = projetoRepository;
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

        public async Task<ActionResult<Projeto>> ObtenhaProjetoPeloId(long id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("O Id do usuário deve ser informado.");
                }
                var projeto = await _projetoRepository.ObtenhaProjetoPeloId(id);
                var tarefas = await _tarefaRepository.ObtenhaTarefas();
                projeto.Tarefas = tarefas.Where(x => x.ProjetoId == id).ToList();
                return Ok(projeto);
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

        public async Task<ActionResult<IEnumerable<Projeto>>> ObtenhaProjetoPeloUsario(long usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                {
                    return BadRequest("O Id do usuário deve ser informado.");
                }

                // valida usuário existe
                _ = await _usuarioRepository.ObtenhaUsuarioPeloId(usuarioId);

                // valida se projetos do usuário existem
                var projetos = await _projetoRepository.ObtenhaProjetoPeloUsario(usuarioId);
                if (projetos == null || !projetos.Any()) throw new ConceitoNaoEncontradoException("Projeto não encontrado!");

                var tarefas = await _tarefaRepository.ObtenhaTarefas();

                projetos.ToList().ForEach(projeto =>
                {
                    var tarefasDoProjeto = tarefas.ToList().FindAll(x => x.ProjetoId == projeto.Id);
                    projeto.Tarefas = tarefasDoProjeto;
                });

                return Ok(projetos);
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

        public async Task<ActionResult<IEnumerable<Projeto>>> ObtenhaProjetos()
        {
            try
            {
                var projetos = await _projetoRepository.ObtenhaProjetos();
                var tarefas = await _tarefaRepository.ObtenhaTarefas();
                projetos.ToList().ForEach(projeto =>
                {
                    var tarefasDoProjeto = tarefas.ToList().FindAll(x => x.ProjetoId == projeto.Id);
                    projeto.Tarefas = tarefasDoProjeto;
                });
                return Ok(projetos);
            }
            catch (Exception)
            {
                return DefaultError();
            }
        }

        public async Task<ActionResult<Projeto>> CadastrarProjeto(Projeto projeto)
        {
            try
            {
                if (projeto.UsuarioId <= 0)
                {
                    return BadRequest("O Id do usuário deve ser informado.");
                }

                // valida usuário existe
                _ = await _usuarioRepository.ObtenhaUsuarioPeloId(projeto.UsuarioId);

                // cadastra projeto
                await _projetoRepository.Cadastrar(projeto);

                return CreatedAtAction("GetProjeto", new { id = projeto.Id }, projeto);
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
    }
}
