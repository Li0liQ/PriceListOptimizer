using System.Data.Entity;
using Autofac;
using Crawler;
using EntityFramework.Patterns;
using OnlinerModel;
using OnlinerParsers;

namespace PopulateDatabase
{
	class Program
	{
		private static IContainer GetContainer()
		{
			var builder = new ContainerBuilder();
			builder.RegisterModule(new LogInjectionModule());

			builder.RegisterType<WebCrawler>().As<IWebCrawler>();
			builder.RegisterType<CategoriesParser>().As<CategoriesParser>();

			builder.RegisterType<OnlinerDb>().As<DbContext>();
			builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
			builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
			
            // Repositories and units of work will be sharing the same instance of DbContextAdapter.
			builder.Register(c => new DbContextAdapter(c.Resolve<DbContext>())).SingleInstance();

			builder.Register(c => (IObjectSetFactory)(c.Resolve<DbContextAdapter>()));
			builder.Register(c => (IObjectContext)(c.Resolve<DbContextAdapter>()));

			IContainer container = builder.Build();
			return container;
		}


		static void Main(string[] args)
		{
			var container = GetContainer();
			CategoriesParser parser = container.Resolve<CategoriesParser>();
			parser.UpdateCategories();
		}
	}
}
