using Code.DI;
using Code.PanelManager;
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
			
			var panel_manager = new PanelManager.PanelManager
			(
				new PanelControllerFactory(),
				new AddressablesPanelFactory(),
				null
			);
			
			container
				.Bind<PanelManager.PanelManager>()
				.FromInstance(panel_manager)
				.AsSingleton();
		}
	}
}