namespace Toolbox.SAP.sdk;

public record SapQuery
{
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
    public string? Value2 { get; set; }
}
