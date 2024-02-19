namespace EW.Desafio.WebApi.Models
{
    public class Relatorio
    {
        public int TotalDeRegistros { get; set; }
        public IEnumerable<DadoRelatorio>? Dados { get; set; }
    }

    public record DadoRelatorio(long UsuarioId, int TotalTarefasConcluidas);
}
