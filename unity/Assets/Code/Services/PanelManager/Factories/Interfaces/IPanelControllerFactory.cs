using System;
using Code.PanelManager.Attributes;

namespace Code.PanelManager.Interfaces
{
    public interface IPanelControllerFactory
    {
        public TPanelController Create<TPanelController>(PanelAttribute meta) where TPanelController : IPanelController;
        public IPanelController Create(Type type_panel_controller, PanelAttribute meta);
    }
}