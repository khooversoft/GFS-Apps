namespace Toolbox.SAP.sdk.Abstractions;

/// <summary>
/// Abstraction of SAP RFC IRfcTable.
/// Represents a collection of rows with table-specific operations.
/// </summary>
public interface ISapTable : ISapStructure
{
    int RowCount { get; }
    int CurrentIndex { get; set; }
    ISapStructure this[int index] { get; }
    void Insert();
    void Append();
}
