using Microsoft.AspNetCore.Components.Forms;

namespace GFSWeb.Application.Models;

public static class IBrowserFileExtensions
{
    public static string GetSizeK(this IBrowserFile browerFile) => $"{browerFile.Size / 1024.0:0.00} KB";
}