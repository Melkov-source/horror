using System;
using Code.DI;
using Code.PanelManager.Attributes;
using Code.PanelManager.Interfaces;

namespace Code.PanelManager
{
	public class DIPanelControllerFactory : IPanelControllerFactory
	{
		private readonly DIContainer _container;
		
		public DIPanelControllerFactory(DIContainer container)
		{
			_container = container;
		}
		
		public TPanelController Create<TPanelController>(PanelAttribute meta) where TPanelController : IPanelController
		{
			var type = typeof(TPanelController);
			
			return (TPanelController)Create(type, meta);
		}

		public IPanelController Create(Type type_panel_controller, PanelAttribute meta)
		{
			return _container.Activate(type_panel_controller) as IPanelController;
		}
	}
}