namespace EW.Desafio.WebApi.Tests.Services;

public class AutorizacaoServiceTests
{
    [Fact]
    public void ValidarAutorizacao_DeveRetornarTrueParaUsuarioComFuncaoGerente()
    {
        //Arrange
        var sut = new AutorizacaoService();
        var usuario = new Usuario() { Id = 1, Nome = "Teste", Funcao = Funcao.Gerente };

        //Act
        var result = sut.UsuarioEhGerente(usuario);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidarAutorizacao_DeveRetornarFalseParaUsuarioComFuncaoAdministrador()
    {
        //Arrange
        var sut = new AutorizacaoService();
        var usuario = new Usuario() { Id = 1, Nome = "Teste", Funcao = Funcao.AdministradorSistema };

        //Act
        var result = sut.UsuarioEhGerente(usuario);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidarAutorizacao_DeveRetornarFalseParaUsuarioComFuncaoComum()
    {
        //Arrange
        var sut = new AutorizacaoService();
        var usuario = new Usuario() { Id = 1, Nome = "Teste", Funcao = Funcao.Comum };

        //Act
        var result = sut.UsuarioEhGerente(usuario);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidarAutorizacao_DeveRetornarFalseParaUsuarioNulo()
    {
        //Arrange
        var sut = new AutorizacaoService();

        //Act
        var result = sut.UsuarioEhGerente(null);

        //Assert
        Assert.False(result);
    }
}
