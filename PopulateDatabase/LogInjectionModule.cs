using System;
using System.Linq;
using Autofac;
using Autofac.Core;
using Common.Logging;

namespace PopulateDatabase
{
	public class LogInjectionModule : Module
	{
		protected override void AttachToComponentRegistration(IComponentRegistry registry, IComponentRegistration registration)
		{
			registration.Preparing += OnComponentPreparing;
		}

		static void OnComponentPreparing(object sender, PreparingEventArgs e)
		{
			Type t = e.Component.Activator.LimitType;
			e.Parameters = e.Parameters.Union(new[]
			{
				new ResolvedParameter((p, i) => p.ParameterType == typeof(ILog), (p, i) => LogManager.GetLogger(t))
			});
		}
	}
}
