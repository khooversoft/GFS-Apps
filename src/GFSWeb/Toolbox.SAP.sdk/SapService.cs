using Microsoft.Extensions.Logging;
using Toolbox.SAP.sdk.Abstractions;

namespace Toolbox.SAP.sdk;

public sealed class SapService : ISapService
{
    private readonly ILogger<SapService> _logger;
    private readonly ISapDestination _destination;

    public SapService(SapOption option, ISapDestinationFactory factory, ILogger<SapService> logger)
    {
        ArgumentNullException.ThrowIfNull(option);
        ArgumentNullException.ThrowIfNull(factory);
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _destination = factory.GetDestination(option);
        _destination.Ping();
        _logger.LogInformation("SAP connection established to {Server}", option.Server);
    }

    public SapQueryBuilder Query(string functionName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(functionName);
        return new SapQueryBuilder(functionName, _destination, _logger);
    }
}
