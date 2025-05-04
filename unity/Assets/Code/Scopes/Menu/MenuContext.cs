using Code.DI;
using JetBrains.Annotations;

namespace Code.Menu
{
	[UsedImplicitly]
	public class MenuContext : IDIContext
	{
		public void InstallBindings(DIContainer container)
		{
			container
				.Bind<MenuScope>()
				.AsSingleton();
		}
	}
}