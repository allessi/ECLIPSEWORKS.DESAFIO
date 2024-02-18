using System.ComponentModel.DataAnnotations;

namespace EW.Desafio.WebApi.Models
{
    public class Usuario
    {
        [Key]
        public required long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool PossuiCargoGerencia { get; set; }
    }
}
