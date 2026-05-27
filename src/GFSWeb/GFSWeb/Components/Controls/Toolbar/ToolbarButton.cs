using System;
using System.Threading.Tasks;
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
    private static ToolbarButton CreateButton(string icon, Color color, EventCallback onClick, string? text, bool disabled, bool show) => new()
    {
        Icon = icon,
        Text = text,
        Color = color,
        OnClick = onClick,
        Disabled = disabled,
        Show = show,
    };

    private static EventCallback CreateCallback(object receiver, Action onClick) => EventCallback.Factory.Create(receiver, onClick);
    private static EventCallback CreateCallback(object receiver, Func<Task> onClick) => EventCallback.Factory.Create(receiver, onClick);

    public static ToolbarButton ArrowBack(EventCallback onClick, bool disabled = false, bool show = true) => new()
    {
        Icon = Icons.Material.Filled.ArrowBack,
        Text = "Back",
        Color = Color.Info,
        OnClick = onClick,
        Disabled = disabled,
        Show = show,
    };

    public static ToolbarButton ArrowBack(object receiver, Action onClick, bool disabled = false, bool show = true)
        => ArrowBack(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton ArrowBack(object receiver, Func<Task> onClick, bool disabled = false, bool show = true)
        => ArrowBack(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Add(EventCallback onClick, bool disabled = false, bool show = true)
        => CreateButton(Icons.Material.Outlined.Add, Color.Info, onClick, "Add", disabled, show);

    public static ToolbarButton Add(object receiver, Action onClick, bool disabled = false, bool show = true)
        => Add(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Add(object receiver, Func<Task> onClick, bool disabled = false, bool show = true)
        => Add(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Edit(EventCallback onClick, bool disabled = false, bool show = true)
        => CreateButton(Icons.Material.Outlined.Edit, Color.Info, onClick, "Edit", disabled, show);

    public static ToolbarButton Edit(object receiver, Action onClick, bool disabled = false, bool show = true)
        => Edit(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Edit(object receiver, Func<Task> onClick, bool disabled = false, bool show = true)
        => Edit(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Save(EventCallback onClick, bool disabled = false, bool show = true)
        => CreateButton(Icons.Material.Outlined.Save, Color.Info, onClick, "Save", disabled, show);

    public static ToolbarButton Save(object receiver, Action onClick, bool disabled = false, bool show = true)
        => Save(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Save(object receiver, Func<Task> onClick, bool disabled = false, bool show = true)
        => Save(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Delete(EventCallback onClick, bool disabled = false, bool show = true)
        => CreateButton(Icons.Material.Outlined.Delete, Color.Error, onClick, "Delete", disabled, show);

    public static ToolbarButton Delete(object receiver, Action onClick, bool disabled = false, bool show = true)
        => Delete(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Delete(object receiver, Func<Task> onClick, bool disabled = false, bool show = true)
        => Delete(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Link(EventCallback onClick, bool disabled = false, bool show = true)
        => CreateButton(Icons.Material.Outlined.Link, Color.Surface, onClick, "Link", disabled, show);

    public static ToolbarButton Link(object receiver, Action onClick, bool disabled = false, bool show = true)
        => Link(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Link(object receiver, Func<Task> onClick, bool disabled = false, bool show = true)
        => Link(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Cancel(EventCallback onClick, bool disabled = false, bool show = true)
        => CreateButton(Icons.Material.Outlined.Cancel, Color.Surface, onClick, "Cancel", disabled, show);

    public static ToolbarButton Cancel(object receiver, Action onClick, bool disabled = false, bool show = true)
        => Cancel(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Cancel(object receiver, Func<Task> onClick, bool disabled = false, bool show = true)
        => Cancel(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Run(EventCallback onClick, bool disabled = false, bool show = true)
        => CreateButton(Icons.Material.Outlined.PlayArrow, Color.Info, onClick, "Run", disabled, show);

    public static ToolbarButton Run(object receiver, Action onClick, bool disabled = false, bool show = true)
        => Run(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Run(object receiver, Func<Task> onClick, bool disabled = false, bool show = true)
        => Run(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Refresh(EventCallback onClick, bool disabled = false, bool show = true)
        => CreateButton(Icons.Material.Outlined.Refresh, Color.Surface, onClick, "Refresh", disabled, show);

    public static ToolbarButton Refresh(object receiver, Action onClick, bool disabled = false, bool show = true)
        => Refresh(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Refresh(object receiver, Func<Task> onClick, bool disabled = false, bool show = true)
        => Refresh(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Access(EventCallback onClick, bool disabled = false, bool show = true)
        => CreateButton(Icons.Material.Outlined.Security, Color.Info, onClick, "Access", disabled, show);

    public static ToolbarButton Access(object receiver, Action onClick, bool disabled = false, bool show = true)
        => Access(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Access(object receiver, Func<Task> onClick, bool disabled = false, bool show = true)
        => Access(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Undo(EventCallback onClick, bool disabled = false, bool show = true)
        => CreateButton(Icons.Material.Outlined.Undo, Color.Info, onClick, "Undo", disabled, show);

    public static ToolbarButton Undo(object receiver, Action onClick, bool disabled = false, bool show = true)
        => Undo(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Undo(object receiver, Func<Task> onClick, bool disabled = false, bool show = true)
        => Undo(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Close(EventCallback onClick, bool disabled = false, bool show = true)
        => CreateButton(Icons.Material.Outlined.Close, Color.Info, onClick, "Close", disabled, show);

    public static ToolbarButton Close(object receiver, Action onClick, bool disabled = false, bool show = true)
        => Close(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Close(object receiver, Func<Task> onClick, bool disabled = false, bool show = true)
        => Close(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Expand(EventCallback onClick, bool disabled = false, bool show = true)
        => CreateButton(Icons.Material.Outlined.ExpandMore, Color.Info, onClick, null, disabled, show);

    public static ToolbarButton Expand(object receiver, Action onClick, bool disabled = false, bool show = true)
        => Expand(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Expand(object receiver, Func<Task> onClick, bool disabled = false, bool show = true)
        => Expand(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Collapse(EventCallback onClick, bool disabled = false, bool show = true)
        => CreateButton(Icons.Material.Outlined.ExpandLess, Color.Info, onClick, null, disabled, show);

    public static ToolbarButton Collapse(object receiver, Action onClick, bool disabled = false, bool show = true)
        => Collapse(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton Collapse(object receiver, Func<Task> onClick, bool disabled = false, bool show = true)
        => Collapse(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton ExpandAll(EventCallback onClick, bool disabled = false, bool show = true)
        => CreateButton(Icons.Material.Outlined.KeyboardDoubleArrowDown, Color.Info, onClick, null, disabled, show);

    public static ToolbarButton ExpandAll(object receiver, Action onClick, bool disabled = false, bool show = true)
        => ExpandAll(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton ExpandAll(object receiver, Func<Task> onClick, bool disabled = false, bool show = true)
        => ExpandAll(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton CollapseAll(EventCallback onClick, bool disabled = false, bool show = true)
        => CreateButton(Icons.Material.Outlined.KeyboardDoubleArrowUp, Color.Info, onClick, null, disabled, show);

    public static ToolbarButton CollapseAll(object receiver, Action onClick, bool disabled = false, bool show = true)
        => CollapseAll(CreateCallback(receiver, onClick), disabled, show);

    public static ToolbarButton CollapseAll(object receiver, Func<Task> onClick, bool disabled = false, bool show = true)
        => CollapseAll(CreateCallback(receiver, onClick), disabled, show);
}