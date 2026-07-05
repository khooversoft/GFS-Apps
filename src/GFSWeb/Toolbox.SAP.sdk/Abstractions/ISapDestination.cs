namespace Toolbox.SAP.sdk.Abstractions;

/// <summary>
/// Abstraction of SAP RFC RfcDestination.
/// Represents an active connection to a SAP system.
/// </summary>
public interface ISapDestination
{
    ISapRepository Repository { get; }
    void Ping();
}
