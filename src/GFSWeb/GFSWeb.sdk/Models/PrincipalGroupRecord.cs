using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Models;

public record PrincipalGroupRecord
{
    public string GroupName { get; init; } = null!;
    public string Description { get; set; } = null!;

    public static IValidator<PrincipalGroupRecord> Validator { get; } = new Validator<PrincipalGroupRecord>()
        .RuleFor(x => x.GroupName).NotEmpty()
        .RuleFor(x => x.Description).NotEmpty()
        .Build();
}

public static class PrincipalGroupRecordExtensions
{
    public static Option Validate(this PrincipalGroupRecord record) => PrincipalGroupRecord.Validator.Validate(record).ToOptionStatus();
}