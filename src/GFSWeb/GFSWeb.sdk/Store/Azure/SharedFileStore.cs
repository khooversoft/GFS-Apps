using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Store.Azure;

public class SharedFileStore
{
    private readonly ILogger<SharedFileStore> _logger;
    private readonly DatalakeStore _datalakeStore;

    public SharedFileStore(DatalakeOption datalakeOption, IServiceProvider serviceProvider, ILogger<SharedFileStore> logger)
    {
        datalakeOption.NotNull().Validate().ThrowOnError();
        _logger = logger.NotNull();

        _datalakeStore = ActivatorUtilities.CreateInstance<DatalakeStore>(serviceProvider, datalakeOption);
    }

    public async Task<Option<string>> Add(string path, DataETag data) => await _datalakeStore.Add(path, data);
    public async Task<Option> Delete(string path) => await _datalakeStore.Delete(path);
    public async Task<Option<DataETag>> Get(string path) => await _datalakeStore.Get(path);
    public async Task<IReadOnlyList<StorePathDetail>> Search(string path) => await _datalakeStore.Search(path);
}


public static class SharedFileStoreTool
{
    public const string SharedRootPath = "shared";
    public const string PersonalRootPath = "personal";
}