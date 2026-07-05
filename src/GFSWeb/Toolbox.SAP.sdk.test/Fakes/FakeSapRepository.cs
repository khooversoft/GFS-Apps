using Toolbox.SAP.sdk.Abstractions;

namespace Toolbox.SAP.sdk.test.Fakes;

/// <summary>
/// In-memory fake of ISapRepository. Holds pre-registered FakeSapFunction instances by name.
/// </summary>
public sealed class FakeSapRepository : ISapRepository
{
    private readonly Dictionary<string, FakeSapFunction> _functions = new(StringComparer.OrdinalIgnoreCase);

    public ISapFunction CreateFunction(string functionName)
    {
        if (_functions.TryGetValue(functionName, out var function))
            return function;

        throw new InvalidOperationException(
            $"No fake function registered for '{functionName}'. Call WithFunction() during test setup.");
    }

    /// <summary>
    /// Registers a pre-configured fake function for the given RFC name.
    /// </summary>
    public FakeSapRepository WithFunction(string functionName, FakeSapFunction function)
    {
        _functions[functionName] = function;
        return this;
    }
}
