using System.Data;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;

namespace Toolbox.Test.Data.SQL;

public class SqlCrudTests
{
    private ITestOutputHelper _outputHelper;
    private const string _connectionString = "Data Source=localhost;Initial Catalog=GFSWeb;Integrated Security=True;TrustServerCertificate=False;Encrypt=False";

    public record AppRole()
    {
        public int RoleId { get; init; }
        public string RoleCode { get; init; } = null!;
        public string Description { get; init; } = null!;
    }


    public SqlCrudTests(ITestOutputHelper outputHelper) => _outputHelper = outputHelper;

    private async Task<IHost> BuildService([CallerMemberName] string function = "")
    {
        string basePath = nameof(SqlCrudTests) + "/" + function;

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddLogging(c => c.AddLambda(_outputHelper.WriteLine).AddDebug().AddFilter(_ => true));

                services.AddSqlClient<AppRole>(config => config.ConnectionString = _connectionString);
            })
            .Build();

        return host;
    }

    [Fact]
    public async Task Connection()
    {
        using var host = await BuildService();
        var logger = host.Services.GetRequiredService<ILogger<SqlCrudTests>>();

        var sqlClient = host.Services.GetRequiredService<ISqlClient<AppRole>>();
        sqlClient.TestConnection().BeTrue();
    }

    [Fact]
    public async Task Select()
    {
        using var host = await BuildService();
        var logger = host.Services.GetRequiredService<ILogger<SqlCrudTests>>();

        var sqlClient = host.Services.GetRequiredService<ISqlClient<AppRole>>();

        var result = await sqlClient.Query()
            .SetCommand("select * from appdbo.approle", CommandType.Text)
            .Execute<AppRole>();

        result.Count.Be(5);
        string[] expectedRoleCodes = ["reader", "contributor", "owner", "parker", "parker-post"];

        var o1 = expectedRoleCodes.OrderBy(x => x).ToArray();
        var r1 = result.Select(x => x.RoleCode).OrderBy(x => x).ToArray();
        r1.SequenceEqual(o1).BeTrue();

        logger.LogInformation("Connected successfully");
    }
}
