using System;
using Code.PanelManager.Attributes;
using Code.PanelManager.Interfaces;

namespace Code.PanelManager
{
    public class PanelControllerFactory : IPanelControllerFactory
    {
        public virtual TPanelController Create<TPanelController>(PanelAttribute meta) where TPanelController : IPanelController
        {
            var type = typeof(TPanelController);
            
            var controller = Create(type, meta);

            return (TPanelController)controller;
        }

        public IPanelController Create(Type type_panel_controller, PanelAttribute meta)
        {
            var controller = CreateInstance(type_panel_controller);

            return (IPanelController)controller;
        }

        protected virtual object CreateInstance(Type type, params object[] arguments)
        {
            var instance = Activator.CreateInstance(type, arguments);
            return instance;
        }
    }
}