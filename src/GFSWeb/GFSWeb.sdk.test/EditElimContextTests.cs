using GFSWeb.Components.Pages.Elim.EditElim;
using GFSWeb.sdk.Models;
using Toolbox.Tools;

namespace GFSWeb.sdk.test;

public class EditElimContextTests
{
    [Fact]
    public void EmptyEqual()
    {
        var v1 = new EditPackageContext();
        var v2 = new EditPackageContext();
        (v1 == v2).BeTrue();
    }

    [Fact]
    public void Equal()
    {
        var v1 = new EditPackageContext
        {
            PackageId = "PackageId",
            Description = "Description",
            PackageType = PackageType.GLSUs,
            Elimination = new EliminationRecord { ID = 1 },
            ElimSelects = new Dictionary<string, ElimSelectRecord>
            {
                ["key"] = new ElimSelectRecord { ElimID = "2" }
            },
            MiscTables = new Dictionary<string, MiscTablesRecord>
            {
                ["key"] = new MiscTablesRecord { ID = "3" }
            }
        };
        var v2 = new EditPackageContext
        {
            PackageId = "PackageId",
            Description = "Description",
            PackageType = PackageType.GLSUs,
            Elimination = new EliminationRecord { ID = 1 },
            ElimSelects = new Dictionary<string, ElimSelectRecord>
            {
                ["key"] = new ElimSelectRecord { ElimID = "2" }
            },
            MiscTables = new Dictionary<string, MiscTablesRecord>
            {
                ["key"] = new MiscTablesRecord { ID = "3" }
            }
        };

        (v1 == v2).BeTrue();
    }

    [Fact]
    public void WithCopyIsEqual()
    {
        var v1 = CreateFull();
        var v2 = v1 with { };
        (v1 == v2).BeTrue();
    }

    [Fact]
    public void NullIsNotEqual()
    {
        var v1 = CreateFull();
        (v1 == null).BeFalse();
    }

    [Fact]
    public void DifferentPackageId()
    {
        var v1 = CreateFull();
        var v2 = CreateFull() with { PackageId = "Other" };
        (v1 == v2).BeFalse();
    }

    [Fact]
    public void DifferentDescription()
    {
        var v1 = CreateFull();
        var v2 = CreateFull() with { Description = "Other" };
        (v1 == v2).BeFalse();
    }

    [Fact]
    public void DifferentPackageType()
    {
        var v1 = CreateFull();
        var v2 = CreateFull() with { PackageType = PackageType.Recons };
        (v1 == v2).BeFalse();
    }

    [Fact]
    public void DifferentElimination()
    {
        var v1 = CreateFull();
        var v2 = CreateFull() with { Elimination = new EliminationRecord { ID = 99 } };
        (v1 == v2).BeFalse();
    }

    [Fact]
    public void NullVsNonNullElimination()
    {
        var v1 = CreateFull() with { Elimination = null };
        var v2 = CreateFull();
        (v1 == v2).BeFalse();
    }

    [Fact]
    public void DifferentElimSelectValue()
    {
        var v1 = CreateFull();
        var v2 = CreateFull() with
        {
            ElimSelects = new Dictionary<string, ElimSelectRecord>
            {
                ["key"] = new ElimSelectRecord { ElimID = "changed" }
            }
        };
        (v1 == v2).BeFalse();
    }

    [Fact]
    public void DifferentElimSelectCount()
    {
        var v1 = CreateFull();
        var v2 = CreateFull() with
        {
            ElimSelects = new Dictionary<string, ElimSelectRecord>
            {
                ["key"] = new ElimSelectRecord { ElimID = "2" },
                ["key2"] = new ElimSelectRecord { ElimID = "4" }
            }
        };
        (v1 == v2).BeFalse();
    }

    [Fact]
    public void DifferentMiscTablesValue()
    {
        var v1 = CreateFull();
        var v2 = CreateFull() with
        {
            MiscTables = new Dictionary<string, MiscTablesRecord>
            {
                ["key"] = new MiscTablesRecord { ID = "changed" }
            }
        };
        (v1 == v2).BeFalse();
    }

    [Fact]
    public void DifferentMiscTablesCount()
    {
        var v1 = CreateFull();
        var v2 = CreateFull() with
        {
            MiscTables = new Dictionary<string, MiscTablesRecord>
            {
                ["key"] = new MiscTablesRecord { ID = "3" },
                ["key10"] = new MiscTablesRecord { ID = "5" }
            }
        };
        (v1 == v2).BeFalse();
    }

    [Fact]
    public void EmptyDictionariesAreEqual()
    {
        var v1 = new EditPackageContext { PackageId = "p", Description = "d" };
        var v2 = new EditPackageContext { PackageId = "p", Description = "d" };
        (v1 == v2).BeTrue();
    }

    private static EditPackageContext CreateFull() => new()
    {
        PackageId = "PackageId",
        Description = "Description",
        PackageType = PackageType.GLSUs,
        Elimination = new EliminationRecord { ID = 1 },
        ElimSelects = new Dictionary<string, ElimSelectRecord>
        {
            ["key"] = new ElimSelectRecord { ElimID = "2" },
            ["key2"] = new ElimSelectRecord { ElimID = "3" },
            ["key3"] = new ElimSelectRecord { ElimID = "4" },
        },
        MiscTables = new Dictionary<string, MiscTablesRecord>
        {
            ["key"] = new MiscTablesRecord { ID = "5" },
            ["key21"] = new MiscTablesRecord { ID = "6" },
            ["key31"] = new MiscTablesRecord { ID = "7" }
        }
    };
}
