using Code.Core.Modules.Freezing;
using Code.PanelManager;
using Code.PanelManager.Attributes;

namespace Code.Core.HUD
{
	[Panel(PanelType = PanelType.OVERLAY, Order = 0, AssetId = "HUD/Prefabs/HUDPanel.prefab")]
	public class HUDPanelController : PanelControllerBase<HUDPanel>
	{
		public void SetTotalTemperatureValue(float temperature)
		{
			panel.cold_bar.SetValue(temperature);
		}

		public void SetBody(BodyTemperature body)
		{
			panel.temperature_view.Setup(body);
		}
	}
}