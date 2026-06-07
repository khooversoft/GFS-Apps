using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Store.Azure;

public class UserFileStore
{
    private readonly ILogger<UserFileStore> _logger;
    private readonly DatalakeStore _datalakeStore;
    private readonly IAuthAccess _authAccess;

    public UserFileStore(DatalakeOption datalakeOption, IServiceProvider serviceProvider, IAuthAccess? authAccess, ILogger<UserFileStore> logger)
    {
        datalakeOption.NotNull().Validate().ThrowOnError();
        _logger = logger.NotNull();
        _authAccess = authAccess.NotNull();

        _datalakeStore = ActivatorUtilities.CreateInstance<DatalakeStore>(serviceProvider, datalakeOption);
    }

    public async Task<Option<string>> Add(DataETag data, string packageId, string description, string extension)
    {
        data.NotNull();

        string userEmail = (await _authAccess.GetEmail()).NotNull();
        string path = CreatePath(userEmail, packageId, description, extension);

        return await _datalakeStore.Add(path, data);
    }

    public async Task<Option> Delete(string path) => await _datalakeStore.Delete(path);

    public async Task<Option<DataETag>> Get(string path) => await _datalakeStore.Get(path);

    public async Task<IReadOnlyList<ProjectFileDetail>> ListFiles(string packageId)
    {
        string userEmail = (await _authAccess.NotNull("Autho required").GetEmail()).NotNull();

        var pattern = StorePathTool.ToSafePath(userEmail) + $"/{packageId}/**";
        var search = await _datalakeStore.Search(pattern);
        return search.Select(x => x.ConvertTo()).ToArray();
    }

    public string CreatePath(string userEmail, string packageId, string description, string extension)
    {
        userEmail.NotEmpty();
        packageId.NotEmpty();
        extension.NotEmpty();
        string rnd = RandomTool.RandomString();
        string path = $"{userEmail}/{packageId}/{packageId}-{description}_{DateTime.UtcNow:yyyyMMddHHmmss}_{rnd}";
        return StorePathTool.ToSafePath(path, extension);
    }
}

public static partial class UserFileStoreTool
{
    public static ProjectFileDetail ConvertTo(this StorePathDetail subject) => new ProjectFileDetail(subject.Path, subject.CreatedOn?.UtcDateTime ?? DateTime.UtcNow, subject.ContentLength);

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