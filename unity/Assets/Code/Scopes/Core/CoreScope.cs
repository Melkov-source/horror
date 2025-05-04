using System.Threading;
using Code.Core.Chapters;
using Code.Shared;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace Code.Core
{
	[UsedImplicitly]
	public class CoreScope : IScope
	{
		private IChapter _chapter;
		
		public async UniTask InitializeAsync(CancellationToken token)
		{
			/*
			 
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
			
			*/
		}

		public async UniTask DisposeAsync(CancellationToken token)
		{
			throw new System.NotImplementedException();
		}
	}
}