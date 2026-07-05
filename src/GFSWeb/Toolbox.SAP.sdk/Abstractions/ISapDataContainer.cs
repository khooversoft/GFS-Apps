namespace Toolbox.SAP.sdk.Abstractions;

/// <summary>
/// Abstraction of SAP RFC IRfcDataContainer.
/// Provides shared data access methods for functions, structures, and tables.
/// </summary>
public interface ISapDataContainer
{
    void SetValue(string name, string value);
    string GetString(string name);
    ISapTable GetTable(string name);
    ISapTable GetTable(string name, bool create);
}
