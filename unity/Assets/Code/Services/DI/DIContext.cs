using System;

namespace Code.DI
{
	public static class DIContext
	{
		public static (DIContainer container, TResolve instance) Register<TContext, TResolve>(DIContainer parent_container = null)
			where TContext : IDIContext
			where TResolve : class
		{
			var context = Activator.CreateInstance<TContext>();
			var container = new DIContainer(parent_container);

			context.InstallBindings(container);
			container.Build();

			var instance = container.Resolve<TResolve>();
			return (container, instance);
		}
	}
}