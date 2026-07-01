using System.Collections.Concurrent;
using GFSWeb.sdk.Models;
using Toolbox.Extensions;
using Toolbox.Tools;

namespace GFSWeb.Components.Pages.Package.Edit;

public class EditPackageContext
{
    public string PackageId { get; set; } = null!;
    public string Description { get; set; } = null!;
    public PackageType PackageType { get; set; }
    public bool Disabled { get; set; }
    public EliminationRecord? Elimination { get; init; }
    public List<ElimSelectRecord> ElimSelects { get; init; } = new();
    public List<MiscTablesRecord> MiscTables { get; init; } = new();
    public List<IPackageActivity> Activities { get; set; } = new();
    public ConcurrentDictionary<string, string> Properties { get; init; } = new(StringComparer.OrdinalIgnoreCase);
}

public static class EditPackageContextTool
{
    public static EditPackageContext ConvertTo(this PipelinePackageRecord subject)
    {
        subject.NotNull();

        return new EditPackageContext
        {
            PackageId = subject.PackageId.NotEmpty(),
            Description = subject.Description.NotEmpty(),
            PackageType = subject.PackageType.Assert(x => x.IsEnumValid(), "Invalid enum"),
            Disabled = subject.Disabled,
            Elimination = subject.Elimination,
            ElimSelects = subject.ElimSelects.NotNull().ToList(),
            MiscTables = subject.MiscTables.NotNull().ToList(),
            Activities = subject.Activities.NotNull().ToList(),
            Properties = new ConcurrentDictionary<string, string>(subject.Properties.Select(x => x.ToKeyValuePair()), StringComparer.OrdinalIgnoreCase),
        };
    }

    public static void SwapActivites(this EditPackageContext subject, int index1, int index2)
    {
        subject.NotNull();
        if (index1 < 0 || index1 >= subject.Activities.Count) throw new ArgumentOutOfRangeException(nameof(index1));
        if (index2 < 0 || index2 >= subject.Activities.Count) throw new ArgumentOutOfRangeException(nameof(index2));

        if (index1 == index2) return;
        (subject.Activities[index2], subject.Activities[index1]) = (subject.Activities[index1], subject.Activities[index2]);
    }
}