using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;

namespace GFSWebTool.Commands;

internal interface ICommand
{
    Command GetCommand();
}
