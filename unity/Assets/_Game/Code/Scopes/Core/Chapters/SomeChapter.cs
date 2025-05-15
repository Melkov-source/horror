using System.Threading;
using Code.Core.Character;
using Code.DI;
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
		public UniTask InitializeAsync(CancellationToken token)
		{
			var character_config_text_asset = Addressables
				.LoadAssetAsync<TextAsset>("Character/Configs/CharacterConfig.json")
				.WaitForCompletion();
			
			var character_prefab = Addressables
				.LoadAssetAsync<GameObject>("Character/Prefabs/Character.prefab")
				.WaitForCompletion();

			var instance = Object.Instantiate(character_prefab, Camera.main.transform);

			var character = instance.GetComponent<CharacterMono>();

			var json = character_config_text_asset.text;

			var config = JsonConvert.DeserializeObject<CharacterConfig>(json);
			
			_container.Inject(character, config);
			
			return UniTask.CompletedTask;
		}

		public UniTask DisposeAsync(CancellationToken token)
		{
			throw new System.NotImplementedException();
		}
	}
}