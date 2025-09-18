using CookComputing.XmlRpc;

namespace Dyno.External
{
    public interface IXmlRpcService : IXmlRpcProxy
    {
        [XmlRpcMethod("GetItemInfo")]
        string GetItemInfo(string product);
    }
}
