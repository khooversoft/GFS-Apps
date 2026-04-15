using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Models;

public record UserAccessRecord
{
    public int UserId { get; init; }
    public string PrincipalNameIdentity { get; init; } = null!;
    public string Table_ID { get; init; } = null!;
    public string ID { get; init; } = null!;
    public string Descr { get; init; } = null!;
    public string? Field1 { get; init; }
    public string? Field2 { get; init; }
    public string? Field3 { get; init; }
    public string? Field4 { get; init; }
    public string? Field5 { get; init; }
    public string? Field6 { get; init; }

    public static IValidator<UserAccessRecord> Validator { get; } = new Validator<UserAccessRecord>()
        .RuleFor(x => x.UserId).Must(x => x >= 0 ? StatusCode.OK : StatusCode.BadRequest)
        .RuleFor(x => x.PrincipalNameIdentity).NotEmpty()
        .RuleFor(x => x.Table_ID).NotEmpty()
        .RuleFor(x => x.ID).NotEmpty()
        .RuleFor(x => x.Descr).NotEmpty()
        .Build();

    public override string ToString() => $"UserId={UserId}, PrincipalNameIdentity={PrincipalNameIdentity}, Table_ID={Table_ID}, Id={ID}";
}

public static class UserAccessRecordExtensions
{
    public static Option Validate(this UserAccessRecord record) => UserAccessRecord.Validator.Validate(record).ToOptionStatus();

    public static UserAccessRecord ConvertTo(this MiscTablesRecord subject) => new UserAccessRecord
    {
        PrincipalNameIdentity = subject.ID,
        Table_ID = subject.Table_ID,
        ID = subject.ID,
        Descr = subject.Descr,
        Field1 = subject.Field1,
        Field2 = subject.Field2,
        Field3 = subject.Field3,
        Field4 = subject.Field4,
        Field5 = subject.Field5,
        Field6 = subject.Field6
    };
}
