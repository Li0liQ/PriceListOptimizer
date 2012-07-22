using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace OnlinerModel
{
	public class OnlinerDb : DbContext
	{
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}

		public OnlinerDb()
		{
			Database.SetInitializer(new DropCreateDatabaseIfModelChanges<OnlinerDb>());
		}
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Manufacturer> Manufacturers { get; set; }
	}
}