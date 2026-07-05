namespace Toolbox.SAP.sdk.Abstractions;

/// <summary>
/// Abstraction of SAP RFC RfcDestinationManager.
/// Factory for creating SAP destinations from configuration.
/// </summary>
public interface ISapDestinationFactory
{
    ISapDestination GetDestination(SapOption option);
}
