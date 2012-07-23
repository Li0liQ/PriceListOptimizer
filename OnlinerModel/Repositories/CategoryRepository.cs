using System.Collections.Generic;
using System.Linq;
using EntityFramework.Patterns;

namespace OnlinerModel.Repositories
{
	public class CategoryRepository : Repository<Category>, ICategoryRepository
	{
		public CategoryRepository(IObjectSetFactory objectSetFactory) : base(objectSetFactory)
		{
		}

		#region Implementation of ICategoryRepository

		public void UpdateCategories(IList<Category> latestValues)
		{
			// 1. Remove the categories UrlPart for which no longer appears on the category page.
			var urlParts = latestValues.Select(c => c.UrlPart);

			foreach (Category category in this.AsQueryable().Where(category => !urlParts.Contains(category.UrlPart)))
			{
				this.Delete(category);
			}
				
			// 2. Update Name for remaining categories and insert new ones.
			IList<Category> existingCategories = this.AsQueryable().ToList();
			foreach (Category parsedCategory in latestValues)
			{
				Category foundItem = existingCategories.FirstOrDefault(c => c.UrlPart == parsedCategory.UrlPart);
				if (foundItem != null)
				{
					foundItem.Name = parsedCategory.Name;
				}
				else
				{
					this.Insert(parsedCategory);
				}
			}
		}

		#endregion
	}
}
