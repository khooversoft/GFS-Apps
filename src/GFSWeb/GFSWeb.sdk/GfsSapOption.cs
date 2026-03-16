namespace GFSWeb.sdk;

public record GfsSapOption
{
    public IReadOnlyList<SapField> SapFields { get; init; } = null!;
}

public record SapField
{
    public string Name { get; init; } = null!;
    public IReadOnlyList<string> Operators { get; init; } = null!;
}
