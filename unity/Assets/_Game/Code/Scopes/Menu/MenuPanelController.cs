using _Game.Code.Scopes.Shared.Interfaces;
using Code.PanelManager;
using Code.PanelManager.Attributes;
using Code.Shared;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Menu
{
	[Panel(PanelType = PanelType.WINDOW, Order = 0, AssetId = "Menu/Prefabs/MenuPanel.prefab")]
	public class MenuPanelController : PanelControllerBase<MenuPanel>
	{
		private readonly IScopeDirector _director;
		
		public MenuPanelController(IScopeDirector director)
		{
			_director = director;
		}
		
		protected override void Initialize()
		{
			base.Initialize();
			
			panel.new_game_button.onClick.AddListener(() =>
			{
				Debug.Log("New Game Button Clicked");
				
				_director.ToScopeAsync(APP_SCOPE.CORE).Forget();
			});
		}
	}
}