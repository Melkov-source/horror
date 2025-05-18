using System.Threading;
using Code.Core.Character;
using Code.Core.HistoryPreview;
using Code.DI;
using Code.Input;
using Code.PanelManager;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Code.Core.Chapters
{

	public class SomeChapter : IChapter
	{
		private DIContainer _container;

		public SomeChapter(DIContainer container)
		{
			_container = container;
		}
		public async UniTask InitializeAsync(CancellationToken token)
		{
			var scene = Addressables
				.LoadSceneAsync("Road/01_Road.unity", LoadSceneMode.Single, false)
				.WaitForCompletion();

			await scene
				.ActivateAsync()
				.ToUniTask(cancellationToken: token);
			
			var character_prefab = Addressables
				.LoadAssetAsync<GameObject>("Character/Prefabs/PlayerContainer.prefab")
				.WaitForCompletion();

			var point = Object.FindAnyObjectByType<PlayerSpawnPoint>();

			var instance = Object.Instantiate(character_prefab, Vector3.zero, Quaternion.identity);

			var view = instance.GetComponentInChildren<PlayerView>();
			
			view.transform.position = point.transform.position;
			
			var model = new PlayerModel();

			var controller = new Player();
			
			_container.Inject(controller, view, model);

			await controller.LoadAsync(token);
			await controller.InitializeAsync(token);

			var panel_manager = _container.Resolve<IPanelManager>()!;

			var p = panel_manager.LoadPanel<HistoryPreviewPanelController>();

			await p.StartText("16:47\nГде-то на севере.\nДорога в деревню \"Медвежье\"");
		}

		public UniTask DisposeAsync(CancellationToken token)
		{
			throw new System.NotImplementedException();
		}
	}
}