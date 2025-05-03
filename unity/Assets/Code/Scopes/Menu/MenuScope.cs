using System.Threading;
using Code.DI;
using Code.Shared;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Menu
{
	public class MenuScope : IScope
	{
		[Inject] private readonly PanelManager.PanelManager _panel_manager;
		
		public async UniTask InitializeAsync(CancellationToken token)
		{
			Debug.Log("MenuScope Initialize");

			var controller = _panel_manager.LoadPanel<MenuPanelController>();
			
			controller.Open();
		}

		public async UniTask DisposeAsync(CancellationToken token)
		{
			
		}
	}
}