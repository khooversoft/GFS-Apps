using Microsoft.AspNetCore.Components;

namespace GFSWeb.Components.Controls;

public class ToolbarSearch : IToolbarElement
{
    public EventCallback<string> OnSearch { get; init; }
    public bool Disabled { get; set; }
    public bool Show { get => OnSearch.HasDelegate && field; set; } = true;
}


public static class ToolbarSearchTool
{
    public static ToolbarSearch Create(EventCallback<string> onSearch, bool disabled = false, bool show = true) => new() { OnSearch = onSearch, Disabled = disabled, Show = show };
    public static ToolbarSearch Create(object receiver, Action<string> onSearch, bool disabled = false, bool show = true)
        => Create(CreateCallback(receiver, onSearch), disabled, show);
    public static ToolbarSearch Create(object receiver, Func<string, Task> onSearch, bool disabled = false, bool show = true)
        => Create(CreateCallback(receiver, onSearch), disabled, show);

    private static EventCallback<string> CreateCallback(object receiver, Action<string> onClick) => EventCallback.Factory.Create(receiver, onClick);
    private static EventCallback<string> CreateCallback(object receiver, Func<string, Task> onClick) => EventCallback.Factory.Create(receiver, onClick);
}
