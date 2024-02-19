namespace EW.Desafio.WebApi.Tests.Shared
{
    public static class TestUtils
    {
        public static DbContextOptions<ApiContext> GetDbContextOptions(string databaseName)
        {
            // Configura as opções do DbContext para um banco de dados em memória
            return new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName)
                .EnableSensitiveDataLogging()
                .Options;
        }
    }
}
