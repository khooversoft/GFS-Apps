using System.Text.RegularExpressions;
using GFSWeb.sdk.Store.Azure;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Store;

public partial class UserDatalakeStore : DatalakeStore
{
    private readonly IAuthAccess _authAccess;

    public UserDatalakeStore(DatalakeOption datalakeOption, IAuthAccess authAccess, ILogger<DatalakeStore> logger) : base(datalakeOption, logger)
    {
        _authAccess = authAccess.NotNull();
    }

    public async Task<Option<string>> Add(DataETag data, string packageId, string description, string extension)
    {
        data.NotNull();
        packageId.NotEmpty();
        extension.NotEmpty();

        string userEmail = (await _authAccess.GetEmail()).NotNull();

        string rnd = RandomTool.RandomString();
        string path = $"{userEmail}/{packageId}/{packageId}-{description}_{DateTime.UtcNow:yyyyMMddHHmmss}_{rnd}";
        path = StorePathTool.ToSafePath(path, extension);

        return await Add(path, data);
    }

    public async Task<IReadOnlyList<ReportFileDetail>> ListReportFiles(string packageId)
    {
        string userEmail = (await _authAccess.GetEmail()).NotNull();

        var pattern = StorePathTool.ToSafePath(userEmail) + $"/{packageId}/**";
        var search = await Search(pattern);
        return search.Select(x => x.ConvertTo()).ToArray();
    }
}

public static partial class UserDatalakeStoreTool
{
    public static ReportFileDetail ConvertTo(this StorePathDetail subject) => new ReportFileDetail(subject.Path, subject.CreatedOn?.UtcDateTime ?? DateTime.UtcNow, subject.ContentLength);

    public static (string UserEmail, string PackageId, string FileName) ParsePath(string path)
    {
        path.NotEmpty();

        var match = PathPattern().Match(path);
        match.Success.Assert(x => x, $"Path '{path}' does not match expected pattern: userEmail/packageId/fileName", nameof(path));

        return (
            UserEmail: match.Groups["userEmail"].Value,
            PackageId: match.Groups["packageId"].Value,
            FileName: match.Groups["fileName"].Value
        );
    }

    [GeneratedRegex(@"^(?<userEmail>[^/]+)/(?<packageId>[^/]+)/(?<fileName>.+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    internal static partial Regex PathPattern();
}
