using System;
using CookComputing.XmlRpc;

namespace WordPressSharp
{
    internal class WordPressClient : IDisposable
    {
        protected WordPressSiteConfig WordPressSiteConfig { get; set; }
        
        public IWordPressService WordPressService { get; internal set; }

        public WordPressClient(WordPressSiteConfig siteConfig)
        {
            WordPressSiteConfig = siteConfig;

            WordPressService = (IWordPressService)XmlRpcProxyGen.Create(typeof(IWordPressService));
            WordPressService.Url = WordPressSiteConfig.FullUrl;
        }

        public void Dispose()
        {
            WordPressService = null;
        }
    }
}
