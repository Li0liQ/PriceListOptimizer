using System.IO;
using System.Text;

namespace Crawler
{
	public interface IWebCrawler
	{
		string Cookie { get; set; }
		TextReader GetPageReader(string url, Encoding encoding = null);
	}
}