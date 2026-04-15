using System.Data;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Extensions;
using Toolbox.Tools;

namespace Toolbox.Test.Data.SQL;

public class SqlCrudTests
{
    private ITestOutputHelper _outputHelper;
    private const string _connectionString = "Data Source=localhost;Initial Catalog=GFSWeb;Integrated Security=True;TrustServerCertificate=False;Encrypt=False";

    public record TestPrincipal()
    {
        public string NameIdentifier { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public string Email { get; init; } = null!;

        public bool IsValid() => NameIdentifier.IsNotEmpty() && UserName.IsNotEmpty() && Email.IsNotEmpty();
    }


    public SqlCrudTests(ITestOutputHelper outputHelper) => _outputHelper = outputHelper;

    private async Task<IHost> BuildService([CallerMemberName] string function = "")
    {
        string basePath = nameof(SqlCrudTests) + "/" + function;

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddLogging(c => c.AddLambda(_outputHelper.WriteLine).AddDebug().AddFilter(_ => true));

                services.AddSqlClient<TestPrincipal>(config => config.ConnectionString = _connectionString);
            })
            .Build();

        return host;
    }

    [Fact]
    public async Task Connection()
    {
        using var host = await BuildService();
        var logger = host.Services.GetRequiredService<ILogger<SqlCrudTests>>();

        var sqlClient = host.Services.GetRequiredService<ISqlClient<TestPrincipal>>();
        sqlClient.TestConnection().BeTrue();
    }

    [Fact]
    public async Task Select()
    {
        using var host = await BuildService();
        var logger = host.Services.GetRequiredService<ILogger<SqlCrudTests>>();

        var sqlClient = host.Services.GetRequiredService<ISqlClient<TestPrincipal>>();

        string cmd = """
            select top(2) * from app.PrincipalIdentity 
            """;

        var result = await sqlClient.Query()
            .SetCommand(cmd, CommandType.Text)
            .Execute<TestPrincipal>();

        result.Count.Be(2);
        result.ForEach(x => x.IsValid().BeTrue());

        logger.LogInformation("Connected successfully");
    }
}
