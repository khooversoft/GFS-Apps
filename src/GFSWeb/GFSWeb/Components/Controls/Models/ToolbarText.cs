namespace GFSWeb.Components.Controls;

public class ToolbarText : IToolbarElement
{
    public string Text { get; init; } = string.Empty;
    public string? SubText { get; init; }
    public bool Disabled { get; set; }
    public bool Show { get; set; } = true;
}
