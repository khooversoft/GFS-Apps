//using Toolbox.SAP.sdk.Abstractions;
//using SAP.Middleware.Connector;

//namespace Elim.Sap.Wrappers;

//internal sealed class SapDestinationWrapper : ISapDestination
//{
//    internal RfcDestination Inner { get; }

//    public SapDestinationWrapper(RfcDestination destination) => Inner = destination;

//    public ISapRepository Repository => new SapRepositoryWrapper(Inner.Repository);

//    public void Ping() => Inner.Ping();
//}
