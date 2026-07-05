using Toolbox.SAP.sdk.Abstractions;

namespace Toolbox.SAP.sdk.test.Fakes;

/// <summary>
/// In-memory fake of ISapStructure backed by a dictionary.
/// </summary>
public sealed class FakeSapStructure : ISapStructure
{
    private readonly Dictionary<string, string> _fields = new(StringComparer.OrdinalIgnoreCase);

    public FakeSapStructure() { }

    public FakeSapStructure(IDictionary<string, string> fields)
    {
        foreach (var kvp in fields)
            _fields[kvp.Key] = kvp.Value;
    }

    public void SetValue(string name, string value) => _fields[name] = value;

    public string GetString(string name) =>
        _fields.TryGetValue(name, out var value) ? value : string.Empty;

    public ISapTable GetTable(string name) => new FakeSapTable();
    public ISapTable GetTable(string name, bool create) => new FakeSapTable();
}
