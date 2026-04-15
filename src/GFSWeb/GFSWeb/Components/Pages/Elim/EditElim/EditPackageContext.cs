using System.Collections.Concurrent;
using GFSWeb.sdk.Models;
using Toolbox.Extensions;

namespace GFSWeb.Components.Pages.Elim.EditElim;

public record EditPackageContext
{
    public string PackageId { get; set; } = null!;
    public string Description { get; set; } = null!;
    public PackageType PackageType { get; set; }

    public EliminationRecord? Elimination { get; init; }
    public IDictionary<string, ElimSelectRecord> ElimSelects { get; init; } = new ConcurrentDictionary<string, ElimSelectRecord>(StringComparer.OrdinalIgnoreCase);
    public IDictionary<string, MiscTablesRecord> MiscTables { get; init; } = new ConcurrentDictionary<string, MiscTablesRecord>(StringComparer.OrdinalIgnoreCase);

    public virtual bool Equals(EditPackageContext? context) => context is not null &&
        PackageId == context.PackageId &&
        Description == context.Description &&
        PackageType == context.PackageType &&
        Elimination == context.Elimination &&
        ElimSelects.DictionaryEquals(context.ElimSelects) &&
        MiscTables.DictionaryEquals(context.MiscTables);

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(EqualityContract);
        hash.Add(PackageId);
        hash.Add(Description);
        hash.Add(PackageType);
        hash.Add(Elimination);
        hash.Add(ElimSelects);
        hash.Add(MiscTables);
        return hash.ToHashCode();
    }
}

//public record Keyed<T> where T : notnull
//{
//    public string Key { get; init; } = Guid.NewGuid().ToString();
//    public T Value { get; set; } = default!;
//}
