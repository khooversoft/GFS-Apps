namespace Toolbox.SAP.sdk.Abstractions;

/// <summary>
/// Abstraction of SAP RFC RfcRepository.
/// Provides creation of function module instances.
/// </summary>
public interface ISapRepository
{
    ISapFunction CreateFunction(string functionName);
}
