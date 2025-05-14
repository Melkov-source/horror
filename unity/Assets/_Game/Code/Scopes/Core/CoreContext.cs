using Code.DI;
using Code.PanelManager;
using JetBrains.Annotations;

namespace Code.Core
{
	[UsedImplicitly]
	public class CoreContext : IDIContext
	{
		public static DIContainer di;
		
		public void InstallBindings(DIContainer container)
		{
			di = container;
			
			container
				.Bind<CoreScope>()
				.AsSingleton();
			
			var panel_manager = new PanelManager.PanelManager
			(
				new DIPanelControllerFactory(container),
				new AddressablesPanelFactory(),
				null,
				"Core"
			);
			
			container
				.Bind<IPanelManager, PanelManager.PanelManager>()
				.FromInstance(panel_manager)
				.AsSingleton();
		}
	}
}