using CookComputing.XmlRpc;

namespace Dyno.External
{
	[XmlRpcMissingMapping(MappingAction.Ignore)]
	internal class SiteConfig
	{
		public string BaseUrl { get; set; }

		public string FullUrl => string.Concat(BaseUrl, BaseUrl.EndsWith("/") ? "api/xmlrpc" : "/api/xmlrpc");
	}
}