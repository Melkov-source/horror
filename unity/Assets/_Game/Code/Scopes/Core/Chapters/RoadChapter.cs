using System.Threading;
using Code.Core.Chapters.Road;
using Code.Core.HUD;
using Code.PanelManager;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Code.Core.Chapters
{
	public class RoadChapter : IChapter
	{
		private SceneInstance _scene;

		private readonly IPanelManager _panel_manager;

		public RoadChapter(IPanelManager panel_manager)
		{
			_panel_manager = panel_manager;
		}
		
		public async UniTask InitializeAsync(CancellationToken token)
		{
			_scene = Addressables
				.LoadSceneAsync("Road/01_Road.unity", LoadSceneMode.Single, false)
				.WaitForCompletion();

			await _scene
				.ActivateAsync()
				.ToUniTask(cancellationToken: token);

			var car = Object.FindAnyObjectByType<Car>();

			car.radio.Enable = true;
		}

		public UniTask DisposeAsync(CancellationToken token)
		{
			return UniTask.CompletedTask;
		}
	}
}