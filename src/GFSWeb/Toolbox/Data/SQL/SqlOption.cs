using System;
using System.Collections.Generic;
using System.Text;

namespace Toolbox.Data;

public record SqlOption
{
    public string ConnectionString { get; set; } = null!;
}
