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
				new DIPanelControllerFactory(container),
				new AddressablesPanelFactory(),
				null,
				"Menu"
			);
			
			container
				.Bind<IPanelManager, PanelManager.PanelManager>()
				.FromInstance(panel_manager)
				.AsSingleton();
		}
	}
}