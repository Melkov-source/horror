using System;

namespace Code.DI
{
	public static class DIContext
	{
		public static DIContainer Register<TContext>(DIContainer parent_container = null) where TContext : IDIContext
		{
			var context = Activator.CreateInstance<TContext>();
			var container = new DIContainer(parent_container);

			context.InstallBindings(container);
			container.Build();

			return container;
		}

		public static (DIContainer container, TResolve instance) Register<TContext, TResolve>(DIContainer parent_container = null)
			where TContext : IDIContext
			where TResolve : class
		{
			var container = Register<TContext>(parent_container);

			var instance = container.Resolve<TResolve>();
			return (container, instance);
		}
	}
}