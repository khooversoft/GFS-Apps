using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Models;

public record ElimSelectRecord
{
    public string ElimID { get; init; } = null!;
    public int Pass { get; init; }
    public int SubSeq { get; init; }
    public string FieldName { get; init; } = null!;
    public int FieldNbr { get; init; }
    public string? IncExcl { get; init; }
    public string? Oper { get; init; }
    public string? FromVal { get; init; }
    public string? ThruVal { get; init; }
    public string? GLSU { get; init; }

    public static IValidator<ElimSelectRecord> Validator { get; } = new Validator<ElimSelectRecord>()
        .RuleFor(x => x.ElimID).NotEmpty()
        .RuleFor(x => x.Pass).Must(x => x >= 0 ? StatusCode.OK : StatusCode.NotFound)
        .RuleFor(x => x.SubSeq).Must(x => x >= 0 ? StatusCode.OK : StatusCode.NotFound)
        .RuleFor(x => x.FieldNbr).Must(x => x >= 1 ? StatusCode.OK : StatusCode.NotFound)
        .RuleFor(x => x.FieldName).NotEmpty()
        .Build();
}
