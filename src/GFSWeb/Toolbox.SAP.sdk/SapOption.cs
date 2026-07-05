namespace Toolbox.SAP.sdk;

public record SapOption
{
    public string User { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string Server { get; init; } = null!;
    public string Client { get; init; } = null!;
    public string Language { get; init; } = "EN";
    public string SystemNumber { get; init; } = "00";
    public string MaxPoolSize { get; init; } = "200";
    public string PoolSize { get; init; } = "1";
    public string IdleTimeout { get; init; } = "1";
}
