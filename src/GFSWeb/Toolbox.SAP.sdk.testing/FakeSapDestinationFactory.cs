using Elim.Sap.Abstractions;

namespace Elim.Sap.Testing;

/// <summary>
/// Test implementation of ISapDestinationFactory.
/// Returns a FakeSapDestination with pre-registered functions and static result data.
/// No SAP connection is made.
/// </summary>
public sealed class FakeSapDestinationFactory : ISapDestinationFactory
{
    private readonly FakeSapRepository _repository = new();

    public ISapDestination GetDestination(SapOption option) => new FakeSapDestination(_repository);

    /// <summary>
    /// Registers a pre-configured fake function that will be returned when CreateFunction is called.
    /// </summary>
    public FakeSapDestinationFactory WithFunction(string functionName, FakeSapFunction function)
    {
        _repository.WithFunction(functionName, function);
        return this;
    }
}
