using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit;

namespace Marten.AspNetCore.Identity.Tests.Support
{
    public class DatabaseServerBootstrapFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
            .WithCleanUp(true)
            .WithDatabase(DatabaseName)
            .WithUsername(DatabaseUserName)
            .WithPassword(DatabaseUserPassword)
            .WithPortBinding(DatabaseHostPort, 5432)
            .WithImage("clkao/postgres-plv8")
            .WithName("MartenAspNetIdentityTestDb")
            .Build();

        private const string DatabaseName = "aspnetidentity";
        private const string DatabaseUserName = "aspnetidentity";
        private const string DatabaseUserPassword = "aspnetidentity";
        private const int DatabaseHostPort = 7435;
        
        public static readonly string ConnectionString;

        static DatabaseServerBootstrapFixture()
        {
            ConnectionString = "HOST = 127.0.0.1; " +
                               $"PORT = {DatabaseHostPort}; " +
                               $"DATABASE = '{DatabaseName}'; " +
                               $"USER ID = '{DatabaseUserName}'; " +
                               $"PASSWORD = '{DatabaseUserPassword}'; " +
                               "TIMEOUT = 15; " +
                               "POOLING = True; " +
                               "MINPOOLSIZE = 1; " +
                               "MAXPOOLSIZE = 100; " +
                               "COMMANDTIMEOUT = 20; ";
        }        
        public async Task InitializeAsync()
        {
            await _postgreSqlContainer.StartAsync();
            
            var result = await _postgreSqlContainer.ExecAsync(new[]
                {
                    "/bin/sh", "-c",
                    $"psql -U {DatabaseUserName} -c \"CREATE EXTENSION plv8; SELECT extversion FROM pg_extensions WHERE extname = 'plv8';\""
                });
        }

        public Task DisposeAsync()
        {
            return _postgreSqlContainer.DisposeAsync().AsTask();
        }
    }
}