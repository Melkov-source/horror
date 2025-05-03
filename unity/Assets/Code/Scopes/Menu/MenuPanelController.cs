using Code.PanelManager;
using Code.PanelManager.Attributes;
using UnityEngine;

namespace Code.Menu
{
	[Panel(PanelType = PanelType.WINDOW, Order = 0, AssetId = "Menu/Prefabs/MenuPanel.prefab")]
	public class MenuPanelController : PanelControllerBase<MenuPanel>
	{
		protected override void Initialize()
		{
			base.Initialize();
			
			Panel.new_game_button.onClick.AddListener(() =>
			{
				Debug.Log("New Game Button Clicked");
			});
		}
	}
}