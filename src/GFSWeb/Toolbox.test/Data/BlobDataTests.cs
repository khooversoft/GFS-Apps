using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Toolbox.Data;
using Toolbox.Tools;

namespace Toolbox.test.Data;

public class BlobDataTests
{
    [Fact]
    public void Empty()
    {
        var blobData = new BlobData(Array.Empty<byte>());
        blobData.Data.NotNull();
        blobData.Data.Length.Be(0);
        blobData.ETag.NotEmpty();
    }

    [Fact]
    public void ConstructorWithByteArray_ShouldCreateBlobData()
    {
        byte[] data = [1, 2, 3, 4, 5];

        var blobData = new BlobData(data);

        blobData.Data.Length.Be(5);
        blobData.Data.SequenceEqual(data).BeTrue();
        blobData.ETag.NotEmpty();
    }

    [Fact]
    public void ConstructorWithImmutableArray_ShouldCreateBlobData()
    {
        var immutableData = ImmutableArray.Create<byte>(1, 2, 3, 4, 5);

        var blobData = new BlobData(immutableData);

        blobData.Data.Length.Be(5);
        blobData.Data.SequenceEqual(immutableData).BeTrue();
        blobData.ETag.NotEmpty();
    }

    [Fact]
    public void ETag_ShouldBeConsistent_ForSameData()
    {
        byte[] data = [10, 20, 30];

        var blobData1 = new BlobData(data);
        var blobData2 = new BlobData(data);

        blobData1.ETag.Be(blobData2.ETag);
    }

    [Fact]
    public void ETag_ShouldBeDifferent_ForDifferentData()
    {
        byte[] data1 = [1, 2, 3];
        byte[] data2 = [4, 5, 6];
        var blobData1 = new BlobData(data1);
        var blobData2 = new BlobData(data2);

        (blobData1.ETag != blobData2.ETag).BeTrue();
    }

    [Fact]
    public void Append_ShouldCombineTwoBlobData()
    {
        byte[] data1 = [1, 2, 3];
        byte[] data2 = [4, 5, 6];
        var blobData1 = new BlobData(data1);
        var blobData2 = new BlobData(data2);

        var result = blobData1.Append(blobData2);

        byte[] expected = [1, 2, 3, 4, 5, 6];
        result.Data.Length.Be(6);
        result.Data.SequenceEqual(expected).BeTrue();
    }

    [Fact]
    public void PlusOperator_ShouldCombineTwoBlobData()
    {
        byte[] data1 = [1, 2, 3];
        byte[] data2 = [4, 5, 6];
        var blobData1 = new BlobData(data1);
        var blobData2 = new BlobData(data2);

        var result = blobData1 + blobData2;

        byte[] expected = [1, 2, 3, 4, 5, 6];
        result.Data.Length.Be(6);
        result.Data.SequenceEqual(expected).BeTrue();
    }

    [Fact]
    public void ImplicitConversion_FromByteArray_ShouldCreateBlobData()
    {
        byte[] data = [7, 8, 9];

        BlobData blobData = data;

        blobData.Data.Length.Be(3);
        blobData.Data.SequenceEqual(data).BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnTrue_ForSameData()
    {
        byte[] data1 = [1, 2, 3];
        byte[] data2 = [1, 2, 3];
        var blobData1 = new BlobData(data1);
        var blobData2 = new BlobData(data2);

        blobData1.Equals(blobData2).BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_ForDifferentData()
    {
        byte[] data1 = [1, 2, 3];
        byte[] data2 = [4, 5, 6];
        var blobData1 = new BlobData(data1);
        var blobData2 = new BlobData(data2);

        blobData1.Equals(blobData2).BeFalse();
    }

    [Fact]
    public void EqualOperator_ShouldReturnTrue_ForSameData()
    {
        byte[] data1 = [1, 2, 3];
        byte[] data2 = [1, 2, 3];
        var blobData1 = new BlobData(data1);
        var blobData2 = new BlobData(data2);

        (blobData1 == blobData2).BeTrue();
    }

    [Fact]
    public void EqualOperator_ShouldReturnFalse_ForDifferentData()
    {
        byte[] data1 = [1, 2, 3];
        byte[] data2 = [4, 5, 6];
        var blobData1 = new BlobData(data1);
        var blobData2 = new BlobData(data2);

        (blobData1 == blobData2).BeFalse();
    }

    [Fact]
    public void EqualOperator_ShouldHandleNullComparisons()
    {
        byte[] data = [1, 2, 3];
        var blobData = new BlobData(data);
        BlobData? nullBlobData = null;

        (blobData == null).BeFalse();
        (null == blobData).BeFalse();
        (nullBlobData == null).BeTrue();
    }

    [Fact]
    public void NotEqualOperator_ShouldReturnFalse_ForSameData()
    {
        byte[] data1 = [1, 2, 3];
        byte[] data2 = [1, 2, 3];
        var blobData1 = new BlobData(data1);
        var blobData2 = new BlobData(data2);

        (blobData1 != blobData2).BeFalse();
    }

    [Fact]
    public void NotEqualOperator_ShouldReturnTrue_ForDifferentData()
    {
        byte[] data1 = [1, 2, 3];
        byte[] data2 = [4, 5, 6];
        var blobData1 = new BlobData(data1);
        var blobData2 = new BlobData(data2);

        (blobData1 != blobData2).BeTrue();
    }

    [Fact]
    public void NotEqualOperator_ShouldHandleNullComparisons()
    {
        byte[] data = [1, 2, 3];
        var blobData = new BlobData(data);
        BlobData? nullBlobData = null;

        (blobData != null).BeTrue();
        (null != blobData).BeTrue();
        (nullBlobData != null).BeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_ForNull()
    {
        byte[] data = [1, 2, 3];
        var blobData = new BlobData(data);

        blobData.Equals(null).BeFalse();
    }

    [Fact]
    public void GetHashCode_ShouldBeConsistent_ForSameData()
    {
        byte[] data1 = [1, 2, 3];
        byte[] data2 = [1, 2, 3];
        var blobData1 = new BlobData(data1);
        var blobData2 = new BlobData(data2);

        blobData1.GetHashCode().Be(blobData2.GetHashCode());
    }

    [Fact]
    public void ToBlobData_FromByteEnumerable_ShouldCreateBlobData()
    {
        IEnumerable<byte> data = new byte[] { 1, 2, 3 };

        var blobData = data.ToBlobData();

        byte[] expected = [1, 2, 3];
        blobData.Data.Length.Be(3);
        blobData.Data.SequenceEqual(expected).BeTrue();
    }

    [Fact]
    public void ToBlobData_FromString_ShouldCreateBlobData()
    {
        string text = "Hello";

        var blobData = text.ToBlobData();

        (blobData.Data.Length > 0).BeTrue();
        blobData.ETag.NotEmpty();
    }

    [Fact]
    public void ToBlobData_FromBlobData_ShouldReturnSameInstance()
    {
        byte[] data = [1, 2, 3];
        var original = new BlobData(data);

        var result = original.ToBlobData();

        ReferenceEquals(original, result).BeTrue();
    }

    [Fact]
    public void ToBlobData_FromMemory_ShouldCreateBlobData()
    {
        Memory<byte> memory = new byte[] { 1, 2, 3 };

        var blobData = memory.ToBlobData();

        byte[] expected = [1, 2, 3];
        blobData.Data.Length.Be(3);
        blobData.Data.SequenceEqual(expected).BeTrue();
    }

    [Fact]
    public void Append_WithEmptyBlobData_ShouldReturnOriginalData()
    {
        byte[] data = [1, 2, 3];
        var blobData = new BlobData(data);
        var empty = new BlobData(Array.Empty<byte>());

        var result = blobData.Append(empty);

        byte[] expected = [1, 2, 3];
        result.Data.Length.Be(3);
        result.Data.SequenceEqual(expected).BeTrue();
    }

    [Fact]
    public void PlusOperator_ChainedAppends_ShouldCombineAll()
    {
        byte[] d1 = [1];
        byte[] d2 = [2];
        byte[] d3 = [3];
        var blob1 = new BlobData(d1);
        var blob2 = new BlobData(d2);
        var blob3 = new BlobData(d3);

        var result = blob1 + blob2 + blob3;

        byte[] expected = [1, 2, 3];
        result.Data.Length.Be(3);
        result.Data.SequenceEqual(expected).BeTrue();
    }
}
