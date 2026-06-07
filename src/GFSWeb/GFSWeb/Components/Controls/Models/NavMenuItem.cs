using Microsoft.AspNetCore.Components;

namespace GFSWeb.Components.Controls;

public record NavMenuItem
{
    public NavMenuItem() { }

    public NavMenuItem(string text, EventCallback onClick, string? icon = null, bool disabled = false, bool show = true)
    {
        Text = text;
        OnClick = onClick;
        Icon = icon;
        Disabled = disabled;
        Show = show;
    }

    public NavMenuItem(string text, object receiver, Action onClick, string? icon = null, bool disabled = false, bool show = true)
    {
        Text = text;
        OnClick = EventCallback.Factory.Create(receiver, onClick);
        Icon = icon;
        Disabled = disabled;
        Show = show;
    }

    public NavMenuItem(string text, object receiver, Func<Task> onClick, string? icon = null, bool disabled = false, bool show = true)
    {
        Text = text;
        OnClick = EventCallback.Factory.Create(receiver, onClick);
        Icon = icon;
        Disabled = disabled;
        Show = show;
    }

    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string? Text { get; init; }
    public string? Icon { get; init; }
    public EventCallback OnClick { get; init; }
    public bool Disabled { get; set; }
    public bool Show { get => OnClick.HasDelegate && field; set; } = true;
}
