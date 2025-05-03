using System;

namespace Code.DI
{
	public static class DIContext
	{
		public static TResolve Register<TContext, TResolve>() 
			where TContext : IDIContext 
			where TResolve : class
		{
			var context = Activator.CreateInstance<TContext>();

			var container = new DIContainer();
			
			context.InstallBindings(container);
			
			container.Build();

			return container.Resolve<TResolve>();
		}
	}
}