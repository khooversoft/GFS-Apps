using System;
using System.Collections.Generic;
using System.Text;

namespace GFSWeb.sdk;

public interface IAuthAccess
{
    Task<string> GetDisplayName();
    Task<string> GetUserName();
    Task<string?> GetEmail();
}
