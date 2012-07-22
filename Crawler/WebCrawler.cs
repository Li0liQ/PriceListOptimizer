using System;
using System.IO;
using System.Net;
using System.Text;
using Common.Logging;

namespace Crawler
{
	public class WebCrawler : IWebCrawler
	{
		protected ILog logger;

		public WebCrawler(ILog logger)
		{
			this.logger = logger;
		}


		#region Implementation of ICrawler

		public string Cookie {get; set;}

		public TextReader GetPageReader(string url, Encoding encoding = null)
		{
			StreamReader result = null;

			if(!Uri.IsWellFormedUriString(url, UriKind.Absolute))
			{
				// Trying to fix url by prefixing with "http://". 
				url = string.Format("http://{0}", url);
			}
			
			if(Uri.IsWellFormedUriString(url, UriKind.Absolute))
			{
				logger.InfoFormat("url = {0}.", url);
				try
				{
					WebRequest request = WebRequest.Create(url);
					request.Timeout = 10000;
					request.Headers["Cookie"] = this.Cookie;
					result = new StreamReader(request.GetResponse().GetResponseStream(), encoding ?? Encoding.GetEncoding(1251));
				}
				catch (Exception e)
				{
					logger.Fatal(String.Format("Unable to get page {0}", url), e);
				}
			}
			else
			{
				logger.WarnFormat("The url provided ({0}) is in incorrect format.", url);
			}

			return result;
		}

		#endregion
	}
}
