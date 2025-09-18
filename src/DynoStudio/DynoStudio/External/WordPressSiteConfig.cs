using System.ComponentModel;
using CookComputing.XmlRpc;
using System;
using System.Configuration;

namespace WordPressSharp
{
	[XmlRpcMissingMapping(MappingAction.Ignore)]
	internal class WordPressSiteConfig
	{
		public string BaseUrl { get; set; }

		public string FullUrl { get { return string.Concat(BaseUrl, BaseUrl.EndsWith("/") ? "api/xmlrpc" : "/api/xmlrpc"); } }
	}
}