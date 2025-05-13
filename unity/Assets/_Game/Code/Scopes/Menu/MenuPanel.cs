using Code.PanelManager;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Menu
{
	public class MenuPanel : PanelBase
	{
		[field: SerializeField] public Button new_game_button { get; private set; }
	}
}