using System.Threading;
using Code.Core.Character;
using Code.DI;
using Code.Input;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
			/*var character_config_text_asset = Addressables
				.LoadAssetAsync<TextAsset>("Character/Configs/CharacterConfig.json")
				.WaitForCompletion();
			
			var character_prefab = Addressables
				.LoadAssetAsync<GameObject>("Character/Prefabs/Character.prefab")
				.WaitForCompletion();

			var point = Object.FindAnyObjectByType<PlayerSpawnPoint>();

			var instance = Object.Instantiate(character_prefab, point.transform.position, Quaternion.identity);

			var view = instance.GetComponent<CharacterMono>();

			var json = character_config_text_asset.text;

			var config = JsonConvert.DeserializeObject<CharacterConfig>(json);
			
			_container.Inject(view, config);*/


			var input = _container.Resolve<InputSystem>();
			Object.FindFirstObjectByType<PlayerView>().Initialize(input);

			//var model = new CharacterModel();

			//var controller = new Player();
			
			//_container.Inject(controller, view, model);

			//await controller.LoadAsync(token);
			//await controller.InitializeAsync(token);
		}

		public UniTask DisposeAsync(CancellationToken token)
		{
			throw new System.NotImplementedException();
		}
	}
}