using System.Collections.Concurrent;

namespace GFSWeb.sdk.Models;

public record SapQueryActivity : IPackageActivity
{
    public string Id { get; } = Guid.NewGuid().ToString();

    public List<SapQueryMapping> SapQueryMappings { get; init; } = new();
    public List<WhereEditRecord> SapQueries { get; init; } = new();
}
