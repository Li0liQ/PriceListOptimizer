using System.Collections.Generic;
using EntityFramework.Patterns;

namespace OnlinerModel.Repositories
{
	public interface ICategoryRepository : IRepository<Category>
	{
		void UpdateCategories(IList<Category> latestValues);
	}
}