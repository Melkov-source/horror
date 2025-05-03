using Code.DI;
using JetBrains.Annotations;

namespace Code.Core
{
	[UsedImplicitly]
	public class CoreContext : IDIContext
	{
		public void InstallBindings(DIContainer container)
		{
			container
				.Bind<CoreScope>()
				.AsSingleton();
		}
	}
}