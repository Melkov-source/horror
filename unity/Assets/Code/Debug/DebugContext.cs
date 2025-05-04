using Code.DI;
using Code.PanelManager;
using JetBrains.Annotations;

namespace Code.Debug
{
	[UsedImplicitly]
	public class DebugContext : IDIContext
	{
		public void InstallBindings(DIContainer container)
		{
			var panel_manager = new PanelManager.PanelManager
			(
				new DIPanelControllerFactory(container),
				new AddressablesPanelFactory(),
				null,
				"Debug"
			);
			
			container
				.Bind<IPanelManager, PanelManager.PanelManager>()
				.FromInstance(panel_manager)
				.AsSingleton();
		}
	}
}