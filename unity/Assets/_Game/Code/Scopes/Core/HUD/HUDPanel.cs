using Code.Core.Modules.Freezing;
using Code.PanelManager;
using UnityEngine;

namespace Code.Core.HUD
{
	public class HUDPanel : PanelBase
	{
		[field: SerializeField] public ColdBar cold_bar { get; private set; }
		[field: SerializeField] public BodyTemperatureView temperature_view { get; private set; }
	}
}