using Elim.Sap.Abstractions;

namespace Elim.Sap.Testing;

/// <summary>
/// In-memory fake of ISapFunction. Captures input parameters and returns pre-configured result data.
/// </summary>
public sealed class FakeSapFunction : ISapFunction
{
    private readonly Dictionary<string, string> _scalarValues = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, FakeSapTable> _tables = new(StringComparer.OrdinalIgnoreCase);
    private readonly FakeSapTable _resultTable = new();
    private bool _invoked;

    /// <summary>
    /// Whether Invoke was called on this function.
    /// </summary>
    public bool WasInvoked => _invoked;

    /// <summary>
    /// Scalar values that were set via SetValue during query execution.
    /// </summary>
    public IReadOnlyDictionary<string, string> CapturedScalars => _scalarValues;

    /// <summary>
    /// Tables that were populated during query execution (input range tables).
    /// </summary>
    public IReadOnlyDictionary<string, FakeSapTable> CapturedTables => _tables;

    public void Invoke(ISapDestination destination) => _invoked = true;

    public void SetValue(string name, string value) => _scalarValues[name] = value;

    public string GetString(string name) =>
        _scalarValues.TryGetValue(name, out var value) ? value : string.Empty;

    public ISapTable GetTable(string name)
    {
        if (string.Equals(name, "EX_RESULT", StringComparison.OrdinalIgnoreCase))
            return _resultTable;

        return GetOrCreateInputTable(name);
    }

    public ISapTable GetTable(string name, bool create)
    {
        if (string.Equals(name, "EX_RESULT", StringComparison.OrdinalIgnoreCase))
            return _resultTable;

        return GetOrCreateInputTable(name);
    }

    /// <summary>
    /// Adds a row to the EX_RESULT table that will be returned after Invoke.
    /// </summary>
    public FakeSapFunction WithResultRow(IDictionary<string, string> fields)
    {
        _resultTable.AddRow(fields);
        return this;
    }

    private FakeSapTable GetOrCreateInputTable(string name)
    {
        if (!_tables.TryGetValue(name, out var table))
        {
            table = new FakeSapTable();
            _tables[name] = table;
        }
        return table;
    }
}
