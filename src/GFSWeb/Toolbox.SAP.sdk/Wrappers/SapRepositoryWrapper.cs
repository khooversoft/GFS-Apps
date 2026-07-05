//using Toolbox.SAP.sdk.Abstractions;
//using SAP.Middleware.Connector;

//namespace Elim.Sap.Wrappers;

//internal sealed class SapRepositoryWrapper : ISapRepository
//{
//    private readonly RfcRepository _repository;

//    public SapRepositoryWrapper(RfcRepository repository) => _repository = repository;

//    public ISapFunction CreateFunction(string functionName) =>
//        new SapFunctionWrapper(_repository.CreateFunction(functionName));
//}
