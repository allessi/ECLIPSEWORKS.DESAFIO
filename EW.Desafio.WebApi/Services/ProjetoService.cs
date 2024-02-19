using EW.Desafio.WebApi.Enums;
using EW.Desafio.WebApi.Models;
using EW.Desafio.WebApi.Repositories;
using EW.Desafio.WebApi.Uteis.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EW.Desafio.WebApi.Services
{
    public class ProjetoService(
        ITarefaRepository tarefaRepository,
        IProjetoRepository projetoRepository,
        IUsuarioRepository usuarioRepository,
        ITarefaService tarefaService,
        ITarefaHistoricoAtualizacaoService tarefaHistoricoAtualizacaoService)
        : BaseService, IProjetoService
    {
        private readonly ITarefaRepository _tarefaRepository = tarefaRepository;
        private readonly IProjetoRepository _projetoRepository = projetoRepository;
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
        private readonly ITarefaService _tarefaService = tarefaService;
        private readonly ITarefaHistoricoAtualizacaoService _tarefaHistoricoAtualizacaoService = tarefaHistoricoAtualizacaoService;

        public async Task<ActionResult<Projeto>> ObtenhaProjetoPeloId(long id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("O Id do usuário deve ser informado.");
                }
                return Ok(await ObtenhaProjetoPorId(id));
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
                return Ok(await ObtenhaTodosProjetos());
            }
            catch (Exception)
            {
                return DefaultError();
            }
        }

        private async Task<IEnumerable<Projeto>> ObtenhaTodosProjetos()
        {
            var projetos = await _projetoRepository.ObtenhaProjetos();
            var tarefas = await _tarefaRepository.ObtenhaTarefas();
            projetos.ToList().ForEach(projeto =>
            {
                var tarefasDoProjeto = tarefas.ToList().FindAll(x => x.ProjetoId == projeto.Id);
                projeto.Tarefas = tarefasDoProjeto;
            });
            return projetos;
        }

        public async Task<ActionResult<Projeto>> CadastrarProjeto(Projeto projeto)
        {
            try
            {
                if (projeto.UsuarioId <= 0)
                {
                    return BadRequest("O Id do usuário deve ser informado.");
                }

                // valida se a quantidade máxima de tarefas por projeto foi estrapolada.
                _ = _tarefaService.PassouQuantidadeMaximaDeTarefasPermitidasNoProjeto(projeto);

                // valida usuário existe
                _ = await _usuarioRepository.ObtenhaUsuarioPeloId(projeto.UsuarioId);

                // cadastra projeto
                await _projetoRepository.Cadastrar(projeto);

                // salva atualização de cadastro de tarefas, quando houver
                await AtualizeHistoricoDeTarefas(projeto.Tarefas);

                return CreatedAtAction("GetProjeto", new { id = projeto.Id }, projeto);
            }
            catch (ConceitoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (QuantidadeMaximaTarefaPorProjetoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return DefaultError();
            }
        }

        private async Task AtualizeHistoricoDeTarefas(ICollection<Tarefa>? tarefas, bool excluindo = false)
        {
            if (tarefas == null) return;

            foreach (var tarefa in tarefas)
            {
                if (excluindo)
                {
                    await _tarefaHistoricoAtualizacaoService.SalvarHistorico(tarefa, null);
                }
                else
                {
                    await _tarefaHistoricoAtualizacaoService.SalvarHistorico(null, tarefa);
                }
            }
        }

        public async Task<IActionResult> DeletarProjeto(long id)
        {
            try
            {
                var projeto = await ObtenhaProjetoPorId(id);

                // valida se todas as tarefas associadas estão concluídas.
                if (ProjetoPossuiTarefasPendentes(projeto))
                    return BadRequest("O projeto não pode ser excluído, pois contém tarefas pendentes. Conclua ou exclua todas as tarefas primeiro.");

                // efetua a exclusão
                await _projetoRepository.Deletar(projeto);

                // salva atualização de cadastro de tarefas, quando houver
                await AtualizeHistoricoDeTarefas(projeto.Tarefas, true);

                return NoContent();
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

        private static bool ProjetoPossuiTarefasPendentes(Projeto projeto)
        {
            return projeto.Tarefas != null && !projeto.Tarefas.All(p => p.Status == Status.Concluida);
        }

        private async Task<Projeto> ObtenhaProjetoPorId(long id)
        {
            var projeto = await _projetoRepository.ObtenhaProjetoPeloId(id);
            var tarefas = await _tarefaRepository.ObtenhaTarefas();
            projeto.Tarefas = tarefas.Where(x => x.ProjetoId == id).ToList();
            return projeto;
        }
    }
}
