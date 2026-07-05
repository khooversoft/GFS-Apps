//using Toolbox.SAP.sdk.Abstractions;
//using SAP.Middleware.Connector;

//namespace Elim.Sap.Wrappers;

//internal sealed class SapFunctionWrapper : ISapFunction
//{
//    private readonly IRfcFunction _function;

//    public SapFunctionWrapper(IRfcFunction function) => _function = function;

//    public void Invoke(ISapDestination destination)
//    {
//        var wrapper = (SapDestinationWrapper)destination;
//        _function.Invoke(wrapper.Inner);
//    }

//    public void SetValue(string name, string value) => _function.SetValue(name, value);
//    public string GetString(string name) => _function.GetString(name);
//    public ISapTable GetTable(string name) => new SapTableWrapper(_function.GetTable(name));
//    public ISapTable GetTable(string name, bool create) => new SapTableWrapper(_function.GetTable(name, create));
//}
