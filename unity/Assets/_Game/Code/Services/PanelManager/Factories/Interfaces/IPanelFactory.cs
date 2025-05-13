using Code.PanelManager.Attributes;

namespace Code.PanelManager.Interfaces
{
    public interface IPanelFactory
    {
        public IPanel Create(PanelAttribute meta);
    }
}