using EW.Desafio.WebApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EW.Desafio.WebApi.Models
{
    public class TarefaHistoricoAtualizacao
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Tarefa")]
        public required long TarefaId { get; set; }
        [ForeignKey("Usuario")]
        public required long UsuarioId { get; set; }
        public required DateTime DataModificacao { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required Status Status { get; set; }
        public required string Alteracao { get; set; }
    }
}
