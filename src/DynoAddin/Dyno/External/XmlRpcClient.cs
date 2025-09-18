using System;
using System.Net;
using System.Threading.Tasks;
using CookComputing.XmlRpc;

namespace Dyno.External
{
    internal class XmlRpcClient : IDisposable
    {
        protected SiteConfig SiteConfig { get; set; }

        public IXmlRpcService XmlRpcService { get; internal set; }

        public XmlRpcClient(SiteConfig siteConfig)
        {
            SiteConfig = siteConfig;

            XmlRpcService = (IXmlRpcService)XmlRpcProxyGen.Create(typeof(IXmlRpcService));
            XmlRpcService.Url = SiteConfig.FullUrl;
        }

        public void Dispose()
        {
            XmlRpcService = null;
        }

        internal static Task CheckInternetConnection()
        {
            try
            {
                Dns.GetHostEntry("content.prorubim.com");
            }
            catch (Exception)
            {
                throw new Exception("No internet connection or bad connect quality");
            }
            return Task.CompletedTask;
        }
    }
}
