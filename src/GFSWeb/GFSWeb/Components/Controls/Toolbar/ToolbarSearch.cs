using Microsoft.AspNetCore.Components;

namespace GFSWeb.Components.Controls;

public class ToolbarSearch : IToolbarElement
{
    public string Icon { get; init; } = null!;
    public EventCallback<string> OnSearch { get; init; }
    public bool Disabled { get; set; }
    public bool Show { get => OnSearch.HasDelegate && field; set; } = true;
}
