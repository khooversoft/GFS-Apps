//using Toolbox.SAP.sdk.Abstractions;
//using SAP.Middleware.Connector;

//namespace Elim.Sap.Wrappers;

//internal sealed class SapStructureWrapper : ISapStructure
//{
//    private readonly IRfcStructure _structure;

//    public SapStructureWrapper(IRfcStructure structure) => _structure = structure;

//    public void SetValue(string name, string value) => _structure.SetValue(name, value);
//    public string GetString(string name) => _structure.GetString(name);
//    public ISapTable GetTable(string name) => new SapTableWrapper(_structure.GetTable(name));
//    public ISapTable GetTable(string name, bool create) => new SapTableWrapper(_structure.GetTable(name));
//}
