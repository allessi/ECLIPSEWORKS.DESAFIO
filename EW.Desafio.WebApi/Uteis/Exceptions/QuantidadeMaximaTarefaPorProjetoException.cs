namespace EW.Desafio.WebApi.Uteis.Exceptions
{
    public class QuantidadeMaximaTarefaPorProjetoException : Exception
    {
        public QuantidadeMaximaTarefaPorProjetoException() : base("Somente 20 tarefas podem ser cadastradas por projeto.") { }
    }
}
