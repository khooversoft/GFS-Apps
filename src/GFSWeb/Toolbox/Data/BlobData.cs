using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Text.Json.Serialization;
using Toolbox.Extensions;
using Toolbox.Tools;
using System.Linq;

namespace Toolbox.Data;

public sealed record BlobData
{
    public BlobData(IEnumerable<byte> data)
    {
        data.NotNull();
        Data = [.. data];
        ETag = data.ToHexHash();
    }

    [JsonConstructor]
    public BlobData(ImmutableArray<byte> data) => (Data, ETag) = (data.NotNull(), data.ToHexHash());

    public string? ETag { get; init; }
    public ImmutableArray<byte> Data { get; init; } = ImmutableArray<byte>.Empty;
    public BlobData Append(BlobData append) => Data.Concat(append.Data).ToBlobData();

    public bool Equals(BlobData? other) => other is not null && !other.Data.IsDefault && Data.SequenceEqual(other.Data);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var b in Data)
        {
            hash.Add(b);
        }
        hash.Add(ETag);
        return hash.ToHashCode();
    }

    public static implicit operator BlobData(byte[] data) => new([..data]);
    public static BlobData operator +(BlobData left, BlobData right) => left.Append(right);
}


public static class BlobDataExtensions
{
    public static BlobData ToBlobData<T>(this T value)
    {
        value.NotNull();
        if (value is BlobData blobData) return blobData;

        var bytes = value.ConvertToBytes();
        return new BlobData(bytes);
    }

    private static byte[] ConvertToBytes<T>(this T value)
    {
        value.NotNull();

        return value switch
        {
            null => throw new ArgumentNullException("value"),
            IEnumerable<BlobData> => throw new ArgumentException("No array are allowed"),
            IEnumerable<byte> v => v.ToArray(),
            string v => v.ToBytes(),
            Memory<byte> v => v.ToArray(),
            var v => v.ToJson().ToBytes(),
        };
    }
}
