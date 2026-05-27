using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GFSWeb.sdk.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(SapQueryActivity), typeDiscriminator: nameof(SapQueryActivity))]
[JsonDerivedType(typeof(SqlCommandActivity), typeDiscriminator: nameof(SqlCommandActivity))]
public interface IPackageActivity
{
    public string Id { get; }
}
