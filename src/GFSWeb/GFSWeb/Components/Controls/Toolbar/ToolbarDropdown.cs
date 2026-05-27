using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Toolbox.Types;

namespace GFSWeb.Components.Controls;

public class ToolbarDropdownButton : IToolbarElement
{
    public string Text { get; init; } = string.Empty;
    public IReadOnlyList<KeyValue<string>> Options { get; init; } = [];
    public EventCallback<string> OnClick { get; init; }
    public string? CurrentValue { get; set; }
    public Color Color { get; init; } = Color.Info;
    public bool Disabled { get; set; }
    public bool Show { get => OnClick.HasDelegate && field; set; } = true;
}

public static class ToolbarDropdownTool
{
    private static ToolbarDropdownButton CreateDropdown(
        string text,
        IReadOnlyList<KeyValue<string>> options,
        EventCallback<string> onClick,
        string? currentValue,
        Color color,
        bool disabled,
        bool show)
        => new()
        {
            Text = text,
            Options = options,
            OnClick = onClick,
            Color = color,
            Disabled = disabled,
            Show = show,
            CurrentValue = currentValue,
        };

    private static EventCallback<string> CreateCallback(object receiver, Action<string> onClick) => EventCallback.Factory.Create(receiver, onClick);
    private static EventCallback<string> CreateCallback(object receiver, Func<string, Task> onClick) => EventCallback.Factory.Create(receiver, onClick);

    public static ToolbarDropdownButton Create(
        string text,
        IReadOnlyList<KeyValue<string>> options,
        EventCallback<string> onClick,
        string? currentValue = null,
        Color color = Color.Info,
        bool disabled = false,
        bool show = true)
        => CreateDropdown(text, options, onClick, currentValue, color, disabled, show);

    public static ToolbarDropdownButton Create(
        object receiver,
        string text,
        IReadOnlyList<KeyValue<string>> options,
        Action<string> onClick,
        string? currentValue = null,
        Color color = Color.Info,
        bool disabled = false,
        bool show = true)
        => Create(text, options, CreateCallback(receiver, onClick), currentValue, color, disabled, show);

    public static ToolbarDropdownButton Create(
        object receiver,
        string text,
        IReadOnlyList<KeyValue<string>> options,
        Func<string, Task> onClick,
        string? currentValue = null,
        Color color = Color.Info,
        bool disabled = false,
        bool show = true)
        => Create(text, options, CreateCallback(receiver, onClick), currentValue, color, disabled, show);
}
