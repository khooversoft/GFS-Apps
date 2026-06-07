using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace GFSWeb.Components.Controls;

public class ToolbarButton : IToolbarElement
{
    public string Icon { get; init; } = null!;
    public Color Color { get; init; } = Color.Info;
    public EventCallback OnClick { get; init; }
    public string? Text { get; set; }
    public bool Disabled { get; set; }
    public bool Show { get => OnClick.HasDelegate && field; set; } = true;
}

public static class ToolbarButtonTool
{
    public static ButtonBuilder ArrowBack { get; } = new(Icons.Material.Filled.ArrowBack, Color.Info, "Back");
    public static ButtonBuilder Add { get; } = new(Icons.Material.Outlined.Add, Color.Info, "Add");
    public static ButtonBuilder Edit { get; } = new(Icons.Material.Outlined.Edit, Color.Info, "Edit");
    public static ButtonBuilder Save { get; } = new(Icons.Material.Outlined.Save, Color.Info, "Save");
    public static ButtonBuilder Delete { get; } = new(Icons.Material.Outlined.Delete, Color.Error, "Delete");
    public static ButtonBuilder Link { get; } = new(Icons.Material.Outlined.AddLink, Color.Info, "Link");
    public static ButtonBuilder Linked { get; } = new(Icons.Custom.Uncategorized.AlertSuccess, Color.Surface, "Linked");
    public static ButtonBuilder Cancel { get; } = new(Icons.Material.Outlined.Cancel, Color.Surface, "Cancel");
    public static ButtonBuilder Run { get; } = new(Icons.Material.Outlined.PlayArrow, Color.Info, "Run");
    public static ButtonBuilder Refresh { get; } = new(Icons.Material.Outlined.Refresh, Color.Surface, "Refresh");
    public static ButtonBuilder Access { get; } = new(Icons.Material.Outlined.Security, Color.Info, "Access");
    public static ButtonBuilder Undo { get; } = new(Icons.Material.Outlined.Undo, Color.Info, "Undo");
    public static ButtonBuilder Close { get; } = new(Icons.Material.Outlined.Close, Color.Info, "Close");
    public static ButtonBuilder Expand { get; } = new(Icons.Material.Outlined.ExpandMore, Color.Info, null);
    public static ButtonBuilder Collapse { get; } = new(Icons.Material.Outlined.ExpandLess, Color.Info, null);
    public static ButtonBuilder ExpandAll { get; } = new(Icons.Material.Outlined.KeyboardDoubleArrowDown, Color.Info, null);
    public static ButtonBuilder CollapseAll { get; } = new(Icons.Material.Outlined.KeyboardDoubleArrowUp, Color.Info, null);
    public static ButtonBuilder Search { get; } = new(Icons.Material.Outlined.Search, Color.Info, null);
    public static ButtonBuilder Upload { get; } = new(Icons.Material.Outlined.FileUpload, Color.Info, "Upload");
    public static ButtonBuilder Download { get; } = new(Icons.Material.Outlined.FileDownload, Color.Info, "Download");

    public class ButtonBuilder
    {
        internal ButtonBuilder(string icon, Color color, string? text)
        {
            Icon = icon;
            Color = color;
            Text = text;
        }

        public string Icon { get; init; } = null!;
        public Color Color { get; init; } = Color.Info;
        public string? Text { get; set; }

        public ToolbarButton Create(EventCallback onClick, bool disabled = false, bool show = true) => CreateButton(onClick, disabled, show);

        public ToolbarButton Create(object receiver, Action onClick, bool disabled = false, bool show = true)
            => CreateButton(CreateCallback(receiver, onClick), disabled, show);

        public ToolbarButton Create(object receiver, Func<Task> onClick, bool disabled = false, bool show = true)
            => CreateButton(CreateCallback(receiver, onClick), disabled, show);

        private ToolbarButton CreateButton(EventCallback onClick, bool disabled, bool show) => new()
        {
            Icon = Icon,
            Text = Text,
            Color = Color,
            OnClick = onClick,
            Disabled = disabled,
            Show = show,
        };

        private static EventCallback CreateCallback(object receiver, Action onClick) => EventCallback.Factory.Create(receiver, onClick);
        private static EventCallback CreateCallback(object receiver, Func<Task> onClick) => EventCallback.Factory.Create(receiver, onClick);
    }
}