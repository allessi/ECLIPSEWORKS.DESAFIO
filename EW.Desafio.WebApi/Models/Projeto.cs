using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EW.Desafio.WebApi.Models
{
    public class Projeto
    {
        [Key]
        public long Id { get; set; }
        public string Nome { get; set; } = default!;
        [ForeignKey("Usuario")]
        public required long UsuarioId { get; set; }

        public ICollection<Tarefa>? Tarefas { get; set; }
    }
}
