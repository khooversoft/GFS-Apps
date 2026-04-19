namespace GFSWeb.sdk;

public class FakeAuthAccess : IAuthAccess
{
    public Task<string> GetDisplayName()
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetEmail()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetUserName()
    {
        throw new NotImplementedException();
    }
}
