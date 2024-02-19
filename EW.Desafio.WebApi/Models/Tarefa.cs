using EW.Desafio.WebApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EW.Desafio.WebApi.Models
{
    public class Tarefa
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Projeto")]
        public required long ProjetoId { get; set; }
        [ForeignKey("Usuario")]
        public required long UsuarioId { get; set; }

        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataVencimento { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Prioridade Prioridade { get; set; }
    }
}
