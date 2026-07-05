using Toolbox.SAP.sdk.Abstractions;

namespace Toolbox.SAP.sdk.test.Fakes;

/// <summary>
/// In-memory fake of ISapTable backed by a list of FakeSapStructure rows.
/// </summary>
public sealed class FakeSapTable : ISapTable
{
    private readonly List<FakeSapStructure> _rows = new();

    public int RowCount => _rows.Count;

    public int CurrentIndex { get; set; }

    public ISapStructure this[int index] => _rows[index];

    public void Insert()
    {
        var row = new FakeSapStructure();
        _rows.Insert(0, row);
        CurrentIndex = 0;
    }

    public void Append()
    {
        var row = new FakeSapStructure();
        _rows.Add(row);
        CurrentIndex = _rows.Count - 1;
    }

    public void SetValue(string name, string value)
    {
        if (_rows.Count > 0)
            _rows[CurrentIndex].SetValue(name, value);
    }

    public string GetString(string name)
    {
        if (_rows.Count > 0)
            return _rows[CurrentIndex].GetString(name);
        return string.Empty;
    }

    public ISapTable GetTable(string name) => new FakeSapTable();
    public ISapTable GetTable(string name, bool create) => new FakeSapTable();

    /// <summary>
    /// Adds a pre-built row to the result table for test setup.
    /// </summary>
    public FakeSapTable AddRow(IDictionary<string, string> fields)
    {
        _rows.Add(new FakeSapStructure(fields));
        return this;
    }
}
