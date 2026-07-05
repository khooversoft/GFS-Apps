using Elim.Sap.Abstractions;

namespace Elim.Sap.Testing;

/// <summary>
/// In-memory fake of ISapDestination. Returns a FakeSapRepository.
/// </summary>
public sealed class FakeSapDestination : ISapDestination
{
    private readonly FakeSapRepository _repository;

    public FakeSapDestination(FakeSapRepository repository)
    {
        _repository = repository;
    }

    public ISapRepository Repository => _repository;

    public void Ping() { }
}
