//using Toolbox.SAP.sdk.Abstractions;
//using SAP.Middleware.Connector;

//namespace Elim.Sap.Wrappers;

//internal sealed class SapDestinationFactory : ISapDestinationFactory
//{
//    public ISapDestination GetDestination(SapOption option)
//    {
//        var config = new RfcConfigParameters
//        {
//            { "NAME", Guid.NewGuid().ToString() },
//            { "USER", option.User },
//            { "PASSWD", option.Password },
//            { "CLIENT", option.Client },
//            { "LANG", option.Language },
//            { "ASHOST", option.Server },
//            { "SYSNR", option.SystemNumber },
//            { "MAX_POOL_SIZE", option.MaxPoolSize },
//            { "POOL_SIZE", option.PoolSize },
//            { "IDLE_TIMEOUT", option.IdleTimeout },
//        };

//        var destination = RfcDestinationManager.GetDestination(config);
//        return new SapDestinationWrapper(destination);
//    }
//}
