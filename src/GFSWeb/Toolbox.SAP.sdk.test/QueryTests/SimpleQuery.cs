using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Toolbox.SAP.sdk.Abstractions;
using Toolbox.SAP.sdk.test.Fakes;

namespace Toolbox.SAP.sdk.test.QueryTests;

public record ScalarResultRow
{
    [SapField("CO")] public string Company { get; set; } = "";
    [SapField("DOC_NO")] public string DocNumber { get; set; } = "";
    [SapField("GL")] public string GlAccount { get; set; } = "";
    [SapField("LC_AMT")] public double LocalAmount { get; set; }
    [SapField("PERIOD")] public int Period { get; set; }
}

public record TypeConversionRow
{
    [SapField("STR_FIELD")] public string StringVal { get; set; } = "";
    [SapField("INT_FIELD")] public int IntVal { get; set; }
    [SapField("DBL_FIELD")] public double DoubleVal { get; set; }
    [SapField("DEC_FIELD")] public decimal DecimalVal { get; set; }
    [SapField("LNG_FIELD")] public long LongVal { get; set; }
    [SapField("DT_FIELD")] public DateTime DateVal { get; set; }
    [SapField("BOOL_FIELD")] public bool BoolVal { get; set; }
}

public record NullableRow
{
    [SapField("NULLABLE_INT")] public int? NullableInt { get; set; }
    [SapField("NULLABLE_DBL")] public double? NullableDouble { get; set; }
    [SapField("NULLABLE_DT")] public DateTime? NullableDate { get; set; }
    [SapField("STR_FIELD")] public string StringVal { get; set; } = "";
}

public record PartialMappingRow
{
    [SapField("CO")] public string Company { get; set; } = "";
    public string UnmappedProperty { get; set; } = "default";
    [SapField("GL")] public string GlAccount { get; set; } = "";
}

public class SimpleQuery
{
    private static readonly SapOption TestOption = new()
    {
        User = "TEST",
        Password = "neverguess",
        Server = "test.server",
        Client = "100",
    };

    private (IHost Host, FakeSapDestinationFactory Factory) BuildService(FakeSapDestinationFactory factory)
    {
        IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(TestOption);
                services.AddSingleton<ISapDestinationFactory>(factory);
                services.AddSingleton<ISapService, SapService>();
                services.AddLogging(config => config.AddDebug());
            })
            .Build();

        return (host, factory);
    }

    [Fact]
    public void Query_WithScalarParameters_SetsValuesOnFunction()
    {
        var function = new FakeSapFunction()
            .WithResultRow(new Dictionary<string, string>
            {
                ["CO"] = "2008",
                ["DOC_NO"] = "100001234",
                ["GL"] = "4100000",
                ["LC_AMT"] = "5000.50",
                ["PERIOD"] = "6",
            });

        var factory = new FakeSapDestinationFactory()
            .WithFunction("ZODY_NEWLINE_EXTRACT", function);

        var (host, _) = BuildService(factory);
        var service = host.Services.GetRequiredService<ISapService>();

        var results = service.Query("ZODY_NEWLINE_EXTRACT")
            .WithParameter("IN_CPUDT_START", "20240601")
            .WithParameter("IN_CPUDT_END", "20240630")
            .WithParameter("IN_BUKRS_START", "2008")
            .WithParameter("IN_BUKRS_END", "2008")
            .Execute<ScalarResultRow>();

        Assert.Single(results);
        Assert.Equal("2008", results[0].Company);
        Assert.Equal("100001234", results[0].DocNumber);
        Assert.Equal("4100000", results[0].GlAccount);
        Assert.Equal(5000.50, results[0].LocalAmount);
        Assert.Equal(6, results[0].Period);

        // Verify scalar parameters were captured
        Assert.True(function.WasInvoked);
        Assert.Equal("20240601", function.CapturedScalars["IN_CPUDT_START"]);
        Assert.Equal("20240630", function.CapturedScalars["IN_CPUDT_END"]);
        Assert.Equal("2008", function.CapturedScalars["IN_BUKRS_START"]);
        Assert.Equal("2008", function.CapturedScalars["IN_BUKRS_END"]);
    }

    [Fact]
    public void Query_WithRangeParameters_CreatesTableWithBetween()
    {
        var function = new FakeSapFunction()
            .WithResultRow(new Dictionary<string, string>
            {
                ["CO"] = "1000",
                ["DOC_NO"] = "200005678",
                ["GL"] = "5000000",
                ["LC_AMT"] = "12345.00",
                ["PERIOD"] = "3",
            });

        var factory = new FakeSapDestinationFactory()
            .WithFunction("ZODY_FLEXA_EXTRACT", function);

        var (host, _) = BuildService(factory);
        var service = host.Services.GetRequiredService<ISapService>();

        var results = service.Query("ZODY_FLEXA_EXTRACT")
            .WithParameter("COCODE_RANGE", "1000", "2000")
            .Execute<ScalarResultRow>();

        Assert.Single(results);
        Assert.Equal("1000", results[0].Company);

        // Verify range table was populated
        Assert.True(function.WasInvoked);
        Assert.True(function.CapturedTables.ContainsKey("COCODE_RANGE"));
        var rangeTable = function.CapturedTables["COCODE_RANGE"];
        Assert.Equal(1, rangeTable.RowCount);

        var row = rangeTable[0];
        Assert.Equal("I", row.GetString("SIGN"));
        Assert.Equal("BT", row.GetString("OPTION"));
        Assert.Equal("1000", row.GetString("LOW"));
        Assert.Equal("2000", row.GetString("HIGH"));
    }

    [Fact]
    public void Query_WithMixedParameters_HandlesScalarAndRangeTogether()
    {
        var function = new FakeSapFunction()
            .WithResultRow(new Dictionary<string, string>
            {
                ["CO"] = "2008",
                ["DOC_NO"] = "300009999",
                ["GL"] = "6100000",
                ["LC_AMT"] = "-750.25",
                ["PERIOD"] = "12",
            });

        var factory = new FakeSapDestinationFactory()
            .WithFunction("ZODY_FLEXT_EXTRACT2", function);

        var (host, _) = BuildService(factory);
        var service = host.Services.GetRequiredService<ISapService>();

        var results = service.Query("ZODY_FLEXT_EXTRACT2")
            .WithParameter("FIYEAR", "2024")
            .WithParameter("LEDGER", "0L")
            .WithParameter("COCODE_RANGE", "2000", "2999")
            .WithParameter("CURR_TYPE", "30")
            .Execute<ScalarResultRow>();

        Assert.Single(results);
        Assert.Equal(-750.25, results[0].LocalAmount);
        Assert.Equal(12, results[0].Period);

        // Scalars
        Assert.Equal("2024", function.CapturedScalars["FIYEAR"]);
        Assert.Equal("0L", function.CapturedScalars["LEDGER"]);
        Assert.Equal("30", function.CapturedScalars["CURR_TYPE"]);

        // Range
        Assert.True(function.CapturedTables.ContainsKey("COCODE_RANGE"));
    }

    [Fact]
    public void Query_ReturnsMultipleRows_AllDeserialized()
    {
        var function = new FakeSapFunction()
            .WithResultRow(new Dictionary<string, string>
            {
                ["CO"] = "2008",
                ["DOC_NO"] = "100000001",
                ["GL"] = "4100000",
                ["LC_AMT"] = "100.00",
                ["PERIOD"] = "1",
            })
            .WithResultRow(new Dictionary<string, string>
            {
                ["CO"] = "2008",
                ["DOC_NO"] = "100000002",
                ["GL"] = "4200000",
                ["LC_AMT"] = "200.00",
                ["PERIOD"] = "2",
            })
            .WithResultRow(new Dictionary<string, string>
            {
                ["CO"] = "3010",
                ["DOC_NO"] = "100000003",
                ["GL"] = "4300000",
                ["LC_AMT"] = "-300.00",
                ["PERIOD"] = "3",
            });

        var factory = new FakeSapDestinationFactory()
            .WithFunction("ZODY_NEWLINE_EXTRACT", function);

        var (host, _) = BuildService(factory);
        var service = host.Services.GetRequiredService<ISapService>();

        var results = service.Query("ZODY_NEWLINE_EXTRACT")
            .WithParameter("IN_CPUDT_START", "20240101")
            .WithParameter("IN_CPUDT_END", "20241231")
            .WithParameter("IN_BUKRS_START", "1000")
            .WithParameter("IN_BUKRS_END", "9999")
            .Execute<ScalarResultRow>();

        Assert.Equal(3, results.Count);
        Assert.Equal("100000001", results[0].DocNumber);
        Assert.Equal("100000002", results[1].DocNumber);
        Assert.Equal("100000003", results[2].DocNumber);
        Assert.Equal("3010", results[2].Company);
        Assert.Equal(-300.00, results[2].LocalAmount);
    }

    [Fact]
    public void Query_ReturnsEmptyResult_ReturnsEmptyList()
    {
        var function = new FakeSapFunction(); // no rows added

        var factory = new FakeSapDestinationFactory()
            .WithFunction("ZODY_NEWLINE_EXTRACT", function);

        var (host, _) = BuildService(factory);
        var service = host.Services.GetRequiredService<ISapService>();

        var results = service.Query("ZODY_NEWLINE_EXTRACT")
            .WithParameter("IN_CPUDT_START", "20240101")
            .WithParameter("IN_CPUDT_END", "20240101")
            .WithParameter("IN_BUKRS_START", "9999")
            .WithParameter("IN_BUKRS_END", "9999")
            .Execute<ScalarResultRow>();

        Assert.Empty(results);
        Assert.True(function.WasInvoked);
    }

    [Fact]
    public void Query_DeserializesAllSupportedTypes()
    {
        var function = new FakeSapFunction()
            .WithResultRow(new Dictionary<string, string>
            {
                ["STR_FIELD"] = "hello",
                ["INT_FIELD"] = "42",
                ["DBL_FIELD"] = "3.14159",
                ["DEC_FIELD"] = "99999.99",
                ["LNG_FIELD"] = "9876543210",
                ["DT_FIELD"] = "2024-06-15",
                ["BOOL_FIELD"] = "1",
            });

        var factory = new FakeSapDestinationFactory()
            .WithFunction("TEST_TYPES", function);

        var (host, _) = BuildService(factory);
        var service = host.Services.GetRequiredService<ISapService>();

        var results = service.Query("TEST_TYPES")
            .WithParameter("DUMMY", "X")
            .Execute<TypeConversionRow>();

        Assert.Single(results);
        var row = results[0];
        Assert.Equal("hello", row.StringVal);
        Assert.Equal(42, row.IntVal);
        Assert.Equal(3.14159, row.DoubleVal);
        Assert.Equal(99999.99m, row.DecimalVal);
        Assert.Equal(9876543210L, row.LongVal);
        Assert.Equal(new DateTime(2024, 6, 15), row.DateVal);
        Assert.True(row.BoolVal);
    }

    [Fact]
    public void Query_BoolField_FalseWhenZero()
    {
        var function = new FakeSapFunction()
            .WithResultRow(new Dictionary<string, string>
            {
                ["STR_FIELD"] = "",
                ["INT_FIELD"] = "0",
                ["DBL_FIELD"] = "0",
                ["DEC_FIELD"] = "0",
                ["LNG_FIELD"] = "0",
                ["DT_FIELD"] = "2024-01-01",
                ["BOOL_FIELD"] = "0",
            });

        var factory = new FakeSapDestinationFactory()
            .WithFunction("TEST_TYPES", function);

        var (host, _) = BuildService(factory);
        var service = host.Services.GetRequiredService<ISapService>();

        var results = service.Query("TEST_TYPES")
            .WithParameter("DUMMY", "X")
            .Execute<TypeConversionRow>();

        Assert.False(results[0].BoolVal);
    }

    [Fact]
    public void Query_NullableFields_ReturnsNullWhenEmpty()
    {
        var function = new FakeSapFunction()
            .WithResultRow(new Dictionary<string, string>
            {
                ["NULLABLE_INT"] = "",
                ["NULLABLE_DBL"] = "",
                ["NULLABLE_DT"] = "",
                ["STR_FIELD"] = "present",
            });

        var factory = new FakeSapDestinationFactory()
            .WithFunction("TEST_NULLABLE", function);

        var (host, _) = BuildService(factory);
        var service = host.Services.GetRequiredService<ISapService>();

        var results = service.Query("TEST_NULLABLE")
            .WithParameter("DUMMY", "X")
            .Execute<NullableRow>();

        Assert.Single(results);
        Assert.Null(results[0].NullableInt);
        Assert.Null(results[0].NullableDouble);
        Assert.Null(results[0].NullableDate);
        Assert.Equal("present", results[0].StringVal);
    }

    [Fact]
    public void Query_NullableFields_ReturnsValueWhenPopulated()
    {
        var function = new FakeSapFunction()
            .WithResultRow(new Dictionary<string, string>
            {
                ["NULLABLE_INT"] = "7",
                ["NULLABLE_DBL"] = "2.5",
                ["NULLABLE_DT"] = "2024-12-25",
                ["STR_FIELD"] = "test",
            });

        var factory = new FakeSapDestinationFactory()
            .WithFunction("TEST_NULLABLE", function);

        var (host, _) = BuildService(factory);
        var service = host.Services.GetRequiredService<ISapService>();

        var results = service.Query("TEST_NULLABLE")
            .WithParameter("DUMMY", "X")
            .Execute<NullableRow>();

        Assert.Equal(7, results[0].NullableInt);
        Assert.Equal(2.5, results[0].NullableDouble);
        Assert.Equal(new DateTime(2024, 12, 25), results[0].NullableDate);
    }

    [Fact]
    public void Query_PropertiesWithoutAttribute_AreNotMapped()
    {
        var function = new FakeSapFunction()
            .WithResultRow(new Dictionary<string, string>
            {
                ["CO"] = "5000",
                ["GL"] = "7000000",
            });

        var factory = new FakeSapDestinationFactory()
            .WithFunction("TEST_PARTIAL", function);

        var (host, _) = BuildService(factory);
        var service = host.Services.GetRequiredService<ISapService>();

        var results = service.Query("TEST_PARTIAL")
            .WithParameter("DUMMY", "X")
            .Execute<PartialMappingRow>();

        Assert.Single(results);
        Assert.Equal("5000", results[0].Company);
        Assert.Equal("7000000", results[0].GlAccount);
        Assert.Equal("default", results[0].UnmappedProperty); // untouched
    }

    [Fact]
    public void Query_MultipleRangeParameters_EachGetsOwnTable()
    {
        var function = new FakeSapFunction()
            .WithResultRow(new Dictionary<string, string>
            {
                ["CO"] = "2008",
                ["DOC_NO"] = "400000001",
                ["GL"] = "4100000",
                ["LC_AMT"] = "0",
                ["PERIOD"] = "1",
            });

        var factory = new FakeSapDestinationFactory()
            .WithFunction("ZODY_FLEXA_EXTRACT", function);

        var (host, _) = BuildService(factory);
        var service = host.Services.GetRequiredService<ISapService>();

        var results = service.Query("ZODY_FLEXA_EXTRACT")
            .WithParameter("COCODE_RANGE", "1000", "3000")
            .WithParameter("ACCOUNT_RANGE", "4000000", "4999999")
            .Execute<ScalarResultRow>();

        Assert.Single(results);
        Assert.True(function.CapturedTables.ContainsKey("COCODE_RANGE"));
        Assert.True(function.CapturedTables.ContainsKey("ACCOUNT_RANGE"));

        var acctRow = function.CapturedTables["ACCOUNT_RANGE"][0];
        Assert.Equal("4000000", acctRow.GetString("LOW"));
        Assert.Equal("4999999", acctRow.GetString("HIGH"));
    }

    [Fact]
    public void Query_NullFunctionName_ThrowsArgumentException()
    {
        var factory = new FakeSapDestinationFactory()
            .WithFunction("DUMMY", new FakeSapFunction());

        var (host, _) = BuildService(factory);
        var service = host.Services.GetRequiredService<ISapService>();

        Assert.ThrowsAny<ArgumentException>(() =>
            service.Query(null!));
    }

    [Fact]
    public void Query_EmptyFunctionName_ThrowsArgumentException()
    {
        var factory = new FakeSapDestinationFactory()
            .WithFunction("DUMMY", new FakeSapFunction());

        var (host, _) = BuildService(factory);
        var service = host.Services.GetRequiredService<ISapService>();

        Assert.Throws<ArgumentException>(() =>
            service.Query(""));
    }

    [Fact]
    public void Query_NoParameters_InvokesFunctionWithNoInputs()
    {
        var function = new FakeSapFunction()
            .WithResultRow(new Dictionary<string, string>
            {
                ["CO"] = "1000",
                ["DOC_NO"] = "X",
                ["GL"] = "Y",
                ["LC_AMT"] = "0",
                ["PERIOD"] = "0",
            });

        var factory = new FakeSapDestinationFactory()
            .WithFunction("TEST_EMPTY_PARAMS", function);

        var (host, _) = BuildService(factory);
        var service = host.Services.GetRequiredService<ISapService>();

        var results = service.Query("TEST_EMPTY_PARAMS")
            .Execute<ScalarResultRow>();

        Assert.Single(results);
        Assert.True(function.WasInvoked);
        Assert.Empty(function.CapturedScalars);
        Assert.Empty(function.CapturedTables);
    }

    [Fact]
    public void Query_UnregisteredFunction_ThrowsInvalidOperation()
    {
        var factory = new FakeSapDestinationFactory()
            .WithFunction("REGISTERED", new FakeSapFunction());

        var (host, _) = BuildService(factory);
        var service = host.Services.GetRequiredService<ISapService>();

        Assert.Throws<InvalidOperationException>(() =>
            service.Query("NOT_REGISTERED").Execute<ScalarResultRow>());
    }
}
