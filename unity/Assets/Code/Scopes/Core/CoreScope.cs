using System.Threading;
using Code.Core.Chapters;
using Code.Core.Character;
using Code.DI;
using Code.Shared;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Core
{
	[UsedImplicitly]
	public class CoreScope : IScope
	{
		private readonly DIContainer _container;
		private IChapter _chapter;
		
		public CoreScope(DIContainer container)
		{
			_container = container;
		}
		
		public async UniTask InitializeAsync(CancellationToken token)
		{
			Debug.Log("CoreScope Initialize");
			
			var character_config_text_asset = Addressables
				.LoadAssetAsync<TextAsset>("Character/Configs/CharacterConfig.json")
				.WaitForCompletion();
			
			var character_prefab = Addressables
				.LoadAssetAsync<GameObject>("Character/Prefabs/Character.prefab")
				.WaitForCompletion();

			var instance = Object.Instantiate(character_prefab);

			var character = instance.GetComponent<CharacterMono>();

			var json = character_config_text_asset.text;

			var config = JsonConvert.DeserializeObject<CharacterConfig>(json);
			
			_container.Inject(character, config);
		}

		public async UniTask DisposeAsync(CancellationToken token)
		{
			throw new System.NotImplementedException();
		}
	}
}