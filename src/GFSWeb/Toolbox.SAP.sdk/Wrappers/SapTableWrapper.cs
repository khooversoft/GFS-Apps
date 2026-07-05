//using Toolbox.SAP.sdk.Abstractions;
//using SAP.Middleware.Connector;

//namespace Elim.Sap.Wrappers;

//internal sealed class SapTableWrapper : ISapTable
//{
//    private readonly IRfcTable _table;

//    public SapTableWrapper(IRfcTable table) => _table = table;

//    public int RowCount => _table.RowCount;

//    public int CurrentIndex
//    {
//        get => _table.CurrentIndex;
//        set => _table.CurrentIndex = value;
//    }

//    public ISapStructure this[int index] => new SapStructureWrapper(_table[index]);

//    public void Insert() => _table.Insert();
//    public void Append() => _table.Append();

//    public void SetValue(string name, string value) => _table.SetValue(name, value);
//    public string GetString(string name) => _table.GetString(name);
//    public ISapTable GetTable(string name) => new SapTableWrapper(_table.GetTable(name));
//    public ISapTable GetTable(string name, bool create) => new SapTableWrapper(_table.GetTable(name));
//}
