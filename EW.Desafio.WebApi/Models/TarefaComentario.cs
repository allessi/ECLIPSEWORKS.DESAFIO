using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EW.Desafio.WebApi.Models
{
    public class TarefaComentario
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Tarefa")]
        public required long TarefaId { get; set; }
        [ForeignKey("Usuario")]
        public required long UsuarioId { get; set; }
        public required string Comententario { get; set; }
    }
}
