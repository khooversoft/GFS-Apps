namespace Toolbox.SAP.sdk.Abstractions;

/// <summary>
/// Abstraction of SAP RFC IRfcFunction.
/// Represents a callable remote function module.
/// </summary>
public interface ISapFunction : ISapDataContainer
{
    void Invoke(ISapDestination destination);
}
