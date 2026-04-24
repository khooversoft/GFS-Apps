using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Store;

public class ScheduleDatalakeStore : DatalakeStore
{
    public ScheduleDatalakeStore(DatalakeOption datalakeOption, ILogger<DatalakeStore> logger) : base(datalakeOption, logger)
    {
    }

    public async Task<Option<string>> Add(DataETag data, string reportId, string fileName, string extension)
    {
        data.NotNull();
        reportId.NotEmpty();
        fileName.NotEmpty();
        extension.NotEmpty();

        string rnd = RandomTool.RandomString();
        string path = $"{reportId}/{fileName}_{DateTime.UtcNow:yyyyMMddHHmmss}_{rnd}";
        path = StorePathTool.ToSafePath(path, extension);

        return await Add(path, data);
    }

    public async Task<IReadOnlyList<StorePathDetail>> List(string reportId)
    {
        reportId.NotEmpty();

        var pattern = StorePathTool.ToSafePath(reportId) + "/*";
        var search = await Search(pattern);
        return search;
    }
}