using GFSWeb.sdk.Store;
using MudBlazor;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.Application;

public class StoreNofityAgent : IStoreNotify
{
    private readonly ISnackbar _snackbar;

    public StoreNofityAgent(ISnackbar snackbar) => _snackbar = snackbar.NotNull();

    public void Notify<T>(Option<T> option, string? success, string? error) => InternalNotify(option.ToOptionStatus(), success, error);

    public void Notify(Option option, string? success, string? error) => InternalNotify(option, success, error);

    private void InternalNotify(Option option, string? success, string? error)
    {
        (Severity severity, bool requireInteraction, string msg) = option switch
        {
            { StatusCode: StatusCode.OK } => (Severity.Success, false, success.NotEmpty()),
            _ => (Severity.Error, true, error.NotEmpty()),
        };

        string snakbarMsg = BuildMessage(option, msg);
        _snackbar.Add(snakbarMsg, severity, config =>
        {
            config.RequireInteraction = requireInteraction;
        });
    }

    private static string BuildMessage(Option option, string message) => option switch
    {
        { StatusCode: StatusCode.OK } => message,
        _ => $"{message} - Status: {option.StatusCode}, Error={option.Error}",
    };
}
