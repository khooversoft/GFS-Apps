using Microsoft.Extensions.Logging;
using Toolbox.Extensions;
using Toolbox.SAP.sdk.Abstractions;
using Toolbox.Tools;

namespace Toolbox.SAP.sdk;

public sealed class SapQueryBuilder
{
    private readonly string _functionName;
    private readonly ISapDestination _destination;
    private readonly ILogger _logger;
    private readonly List<SapQuery> _parameters = new();

    internal SapQueryBuilder(string functionName, ISapDestination destination, ILogger logger)
    {
        _functionName = functionName;
        _destination = destination;
        _logger = logger;
    }

    public SapQueryBuilder WithParameter(string name, string value)
    {
        _parameters.Add(new SapQuery { Name = name, Value = value });
        return this;
    }

    public SapQueryBuilder WithParameter(string name, string value, string value2)
    {
        _parameters.Add(new SapQuery { Name = name, Value = value, Value2 = value2 });
        return this;
    }

    public IReadOnlyList<T> Execute<T>() where T : new()
    {
        var function = _destination.Repository.CreateFunction(_functionName);

        foreach (var param in _parameters)
        {
            if (param.Value2.IsNotEmpty())
            {
                function.SetValue(param.Name, param.Value);
            }
            else
            {
                var table = function.GetTable(param.Name, true);
                table.Insert();
                var row = table[0];
                row.SetValue("SIGN", "I");
                row.SetValue("OPTION", "BT");
                row.SetValue("LOW", param.Value);
                row.SetValue("HIGH", param.Value2.NotEmpty());
            }
        }

        _logger.LogDebug("Invoking SAP function {Function} with {Count} parameters", _functionName, _parameters.Count);

        function.Invoke(_destination);

        var resultTable = function.GetTable("EX_RESULT");
        var list = new List<T>(resultTable.RowCount);

        for (int i = 0; i < resultTable.RowCount; i++)
        {
            list.Add(SapRowMapper.Map<T>(resultTable[i]));
        }

        _logger.LogInformation("SAP function {Function} returned {Count} rows", _functionName, list.Count);
        return list;
    }
}
