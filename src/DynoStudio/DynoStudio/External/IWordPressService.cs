using System;
using CookComputing.XmlRpc;


namespace WordPressSharp
{
    public interface IWordPressService : IXmlRpcProxy
    {
        [XmlRpcMethod("reqLicStatus")]
        string ReqLicStatus(string username, string password, string product, string request);
    }
}
