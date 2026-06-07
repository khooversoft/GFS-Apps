using System.Text.Json.Serialization;

namespace GFSWeb.sdk.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(SapQueryActivity), typeDiscriminator: nameof(SapQueryActivity))]
[JsonDerivedType(typeof(SqlCommandActivity), typeDiscriminator: nameof(SqlCommandActivity))]
public interface IPackageActivity
{
    public string Id { get; }
    public string Type { get; }
    public string Description { get; }
}
