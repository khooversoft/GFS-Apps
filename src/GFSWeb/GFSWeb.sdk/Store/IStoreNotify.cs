using Toolbox.Types;

namespace GFSWeb.sdk.Store;

public interface IStoreNotify
{
    void Notify(Option option, string? success, string? error);
    void Notify<T>(Option<T> option, string? success, string? error);
}
