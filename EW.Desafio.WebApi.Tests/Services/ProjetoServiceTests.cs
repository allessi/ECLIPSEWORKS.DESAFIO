using EW.Desafio.WebApi.Uteis.Exceptions;

namespace EW.Desafio.WebApi.Tests.Services;

public class ProjetoServiceTests
{
    readonly Mock<ITarefaRepository> _mockTarefaRepostiory;
    readonly Mock<IProjetoRepository> _mockProjetoRepository;
    readonly Mock<IUsuarioRepository> _mockUsuarioRepository;
    readonly Mock<ITarefaService> _mockTarefaService;
    readonly Mock<ITarefaHistoricoAtualizacaoService> _mockTarefaHistoricoAtualizacaoService;
    public ProjetoServiceTests()
    {
        _mockTarefaRepostiory = new Mock<ITarefaRepository>();
        _mockProjetoRepository = new Mock<IProjetoRepository>();
        _mockUsuarioRepository = new Mock<IUsuarioRepository>();
        _mockTarefaService = new Mock<ITarefaService>();
        _mockTarefaHistoricoAtualizacaoService = new Mock<ITarefaHistoricoAtualizacaoService>();
    }

    [Fact]
    private async void ObtenhaProjetoPeloId_DeveRetornarOkResult()
    {
        ///Arrange
        var projetoService = CriarProjetoService();
        var projeto = new Projeto() { Id = 1, Nome = "Teste1", UsuarioId = 1 };

        _mockProjetoRepository.Setup(x => x.ObtenhaProjetoPeloId(It.IsAny<long>())).Returns(Task.FromResult(projeto));

        //Act
        var actionResult = (await projetoService.ObtenhaProjetoPeloId(1)).Result;
        if (actionResult is not OkObjectResult objetoRetorno || objetoRetorno.Value == null) return;
        var projetoRetorno = (Projeto)objetoRetorno.Value;

        //Assert
        Assert.NotNull(projetoRetorno);
        Assert.Equal(projeto.Id, projetoRetorno.Id);
    }

    [Fact]
    private async void ObtenhaProjetoPeloId_DeveRetornarBadRequestResult()
    {
        ///Arrange
        var projetoService = CriarProjetoService();
        var projeto = new Projeto() { Id = 1, Nome = "Teste1", UsuarioId = 1 };

        _mockProjetoRepository.Setup(x => x.ObtenhaProjetoPeloId(It.IsAny<long>())).Returns(Task.FromResult(projeto));

        //Act
        var actionResult = (await projetoService.ObtenhaProjetoPeloId(-1)).Result;

        //Assert
        Assert.IsType<BadRequestObjectResult>(actionResult);
    }

    [Fact]
    private async void ObtenhaProjetoPeloId_ConceitoNaoEncontradoExceptionDeveRetornarNotFound()
    {
        ///Arrange
        var projetoService = CriarProjetoService();
        var projeto = new Projeto() { Id = 1, Nome = "Teste1", UsuarioId = 1 };

        _mockProjetoRepository.Setup(x => x.ObtenhaProjetoPeloId(It.IsAny<long>())).Throws(new ConceitoNaoEncontradoException("Projeto não encontrado!"));

        //Act
        var actionResult = (await projetoService.ObtenhaProjetoPeloId(1)).Result;

        //Assert
        Assert.IsType<NotFoundObjectResult>(actionResult);
    }

    [Fact]
    private async void ObtenhaProjetoPeloId_ExceptionDeveRetornarDefaultError()
    {
        ///Arrange
        var projetoService = CriarProjetoService();
        var projeto = new Projeto() { Id = 1, Nome = "Teste1", UsuarioId = 1 };

        _mockProjetoRepository.Setup(x => x.ObtenhaProjetoPeloId(It.IsAny<long>())).Throws<Exception>();

        //Act
        var actionResult = (await projetoService.ObtenhaProjetoPeloId(1)).Result;

        //Assert
        Assert.IsType<ObjectResult>(actionResult);
    }

    private ProjetoService CriarProjetoService()
    {
        return new ProjetoService(
            _mockTarefaRepostiory.Object,
            _mockProjetoRepository.Object,
            _mockUsuarioRepository.Object,
            _mockTarefaService.Object,
            _mockTarefaHistoricoAtualizacaoService.Object);
    }
}
