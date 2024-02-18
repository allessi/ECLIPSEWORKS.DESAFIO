using Microsoft.EntityFrameworkCore;

namespace EW.Desafio.WebApi.Models
{
    public class ApiContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Projeto> Projetos { get; set; }

        public DbSet<Tarefa> Tarefas { get; set; }

        public DbSet<TarefaHistoricoAtualizacao> TarefaHistoricoAtualizacoes { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Projeto>()
            //    .HasMany(p => p.Tarefas)
            //    .WithOne(t => t.Projeto)
            //    .HasForeignKey(t => t.Id);
        }


        public void SeedData()
        {
            if (!Usuarios.Any())
            {
                var usuarios = new List<Usuario>
                {
                    new() { Id = 1, Nome = "João" },
                    new () { Id = 2, Nome = "Maria" },
                    new () { Id = 3, Nome = "Pedro" },
                    new () { Id = 4, Nome = "Sebastião" },
                    new () { Id = 5, Nome = "Paula", PossuiCargoGerencia = true }
                };
                Usuarios.AddRange(usuarios);
                SaveChanges();
            }
        }
    }
}
