using System;
using System.Collections.Generic;
using System.Text;

namespace GFSWeb.sdk;

public record GfsWebOption
{
    public string ConnectionString { get; init; } = null!;
}
