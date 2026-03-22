using System.CommandLine;

namespace GFSWebTool.Commands;

internal interface ICommand
{
    Command GetCommand();
}
