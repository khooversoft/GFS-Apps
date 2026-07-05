namespace Toolbox.SAP.sdk;

public interface ISapService
{
    SapQueryBuilder Query(string functionName);
}
