using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Toolbox.Data;
using Toolbox.Extensions;
using Toolbox.Tools;

namespace Toolbox.test.Application;

internal static class TestApplication
{
    public static DatalakeStore GetDatalake(string basePath) => new DatalakeStore(ReadOption(basePath), new NullLogger<DatalakeStore>());

    public static DatalakeOption ReadOption(string basePath) => new ConfigurationBuilder()
        .AddJsonFile("TestSettings.json")
        .AddUserSecrets("gfs-web-secrets")
        .Build()
        .Get<DatalakeOption>().NotNull()
        .Func(x => x with { BasePath = basePath });
}
