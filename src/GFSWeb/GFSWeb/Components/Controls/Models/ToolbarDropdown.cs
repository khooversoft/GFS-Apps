using Microsoft.AspNetCore.Components;
using MudBlazor;
using Toolbox.Types;

namespace GFSWeb.Components.Controls;

public record ToolbarDropdown : IToolbarElement
{
    public string? Title { get; init; }
    public IReadOnlyList<KeyValue<string>> Options { get; init; } = [];
    public EventCallback<string> OnClick { get; init; }
    public string? CurrentValue { get; set; }
    public Color Color { get; init; } = Color.Info;
    public bool Disabled { get; set; }
    public string? MaxWidth { get; init; }

    public bool Show { get => OnClick.HasDelegate && field; set; } = true;

    public string GetStyle() => MaxWidth switch
    {
        null => "min-width:120px;margin: 0px 5px;",
        string v => $"min-width:120px;max-width:{v};margin: 0px 5px;"
    };
}

public static class ToolbarDropdownTool
{
    private static EventCallback<string> CreateCallback(object receiver, Action<string> onClick) => EventCallback.Factory.Create(receiver, onClick);
    private static EventCallback<string> CreateCallback(object receiver, Func<string, Task> onClick) => EventCallback.Factory.Create(receiver, onClick);

    public static ToolbarDropdown Create(
        IReadOnlyList<KeyValue<string>> options,
        EventCallback<string> onClick,
        string? currentValue = null,
        Color color = Color.Info,
        string? title = null,
        string? maxWidth = null,
        bool disabled = false,
        bool show = true)
        => new()
        {
            Title = title,
            Options = options,
            OnClick = onClick,
            CurrentValue = currentValue,
            Color = color,
            Disabled = disabled,
            Show = show,
            MaxWidth = maxWidth,
        };

    public static ToolbarDropdown Create(
        object receiver,
        IReadOnlyList<KeyValue<string>> options,
        Action<string> onClick,
        string? currentValue = null,
        Color color = Color.Info,
        string? title = null,
        string? maxWidth = null,
        bool disabled = false,
        bool show = true)
        => Create(options, CreateCallback(receiver, onClick), currentValue, color, title, maxWidth, disabled, show);

    public static ToolbarDropdown Create(
        object receiver,
        IReadOnlyList<KeyValue<string>> options,
        Func<string, Task> onClick,
        string? currentValue = null,
        Color color = Color.Info,
        string? title = null,
        string? maxWidth = null,
        bool disabled = false,
        bool show = true)
        => Create(options, CreateCallback(receiver, onClick), currentValue, color, title, maxWidth, disabled, show);
}
