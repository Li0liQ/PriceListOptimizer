using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Logging;
using Crawler;
using EntityFramework.Patterns;
using HtmlAgilityPack;
using OnlinerModel;
using OnlinerModel.Repositories;

namespace OnlinerParsers
{
	public class CategoriesParser
	{
		private readonly ILog logger;
		private readonly IWebCrawler crawler;
		private readonly ICategoryRepository categories;
		private readonly IUnitOfWork uow;

		// TODO: Provide baseUrl externally.
		private const string baseUrl = "http://catalog.onliner.by/";

		public CategoriesParser(ILog logger, IWebCrawler crawler, ICategoryRepository categories, IUnitOfWork uow)
		{
			this.logger = logger;
			this.crawler = crawler;
			this.categories = categories;
			this.uow = uow;
		}

		private static HtmlDocument GetDoc(TextReader reader)
		{
			HtmlDocument doc = new HtmlDocument();
			doc.Load(reader);
			return doc;
		}

		public void UpdateCategories()
		{
			TextReader reader = crawler.GetPageReader(baseUrl);
			HtmlDocument doc = GetDoc(reader);

			if (doc != null)
			{
				IList<Category> parsedCategories = ParseCategoriesList(doc);
				categories.UpdateCategories(parsedCategories);
				try
				{
					uow.Commit();
				}
				catch (Exception ex)
				{
					logger.Error(ex);
				}
			}
			else
			{
				logger.Warn("Unable to update categories: doc is null.");
			}
		}

		private IList<Category> ParseCategoriesList(HtmlDocument doc)
		{
			List<Category> categories = new List<Category>();

			// TODO: Extract XPATH generation to a separate utility class. Think about possible css selectors usage.
			HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(@"//ul[contains(@class, 'b-catalogitems')]/li/div[contains(@class, 'i')]/a[normalize-space(text())]");
			if (nodes != null)
			{
				foreach (HtmlNode node in nodes)
				{
					try
					{
						string name = node.InnerText;
						string urlPart = GetCategoryUrlPart(node.Attributes["href"].Value);
						if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(urlPart))
						{
							logger.WarnFormat("Empty values retrieved parsing a category node: {0}", node.OuterHtml);
						}
						else
						{
							categories.Add(new Category { Name = name, UrlPart = urlPart });
						}
					}
					catch (Exception)
					{
						logger.WarnFormat("Unable to parse a category node: {0}", node.OuterHtml);
					}
				}
			}
			return categories;
		}

		private static string GetCategoryUrlPart(string url)
		{
			if (string.IsNullOrWhiteSpace(url) || !url.StartsWith(baseUrl))
			{
				return null;
			}
			else
			{
				return url.Remove(0, baseUrl.Length).Split('/')[0];
			}
		}
	}
}
