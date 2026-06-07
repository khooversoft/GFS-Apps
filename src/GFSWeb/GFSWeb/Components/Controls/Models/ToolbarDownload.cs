using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Toolbox.Types;

namespace GFSWeb.Components.Controls;

public class ToolbarFileUpload : IToolbarElement
{
    public string Icon { get; init; } = null!;
    public Color Color { get; init; } = Color.Info;
    public EventCallback<IReadOnlyList<IBrowserFile>> OnClick { get; init; }
    public string? Text { get; set; }
    public bool Disabled { get; set; }
    public bool Show { get => OnClick.HasDelegate && field; set; } = true;
}


public static class ToolbarFileUploadTool
{
    private static EventCallback<IReadOnlyList<IBrowserFile>> CreateCallback(object receiver, Action<IReadOnlyList<IBrowserFile>> onClick) =>
        EventCallback.Factory.Create(receiver, onClick);

    private static EventCallback<IReadOnlyList<IBrowserFile>> CreateCallback(object receiver, Func<IReadOnlyList<IBrowserFile>, Task> onClick) =>
        EventCallback.Factory.Create(receiver, onClick);

    public static ToolbarFileUpload Create(EventCallback<IReadOnlyList<IBrowserFile>> onClick, bool disabled, string? text, bool show) => new()
    {
        Icon = Icons.Material.Outlined.FileUpload,
        Color = Color.Info,
        OnClick = onClick,
        Text = text,
        Disabled = disabled,
        Show = show,
    };

    public static ToolbarFileUpload Create(
        object receiver,
        Action<IReadOnlyList<IBrowserFile>> onClick,
        bool disabled = false,
        string? text = null,
        bool show = true)
        => Create(CreateCallback(receiver, onClick), disabled, text, show);

    public static ToolbarFileUpload Create(
        object receiver,
        Func<IReadOnlyList<IBrowserFile>, Task> onClick,
        bool disabled = false,
        string? text = null,
        bool show = true)
        => Create(CreateCallback(receiver, onClick), disabled, text, show);
}