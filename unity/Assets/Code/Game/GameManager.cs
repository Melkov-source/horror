using Code.Character;
using Code.DI;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Game
{
	[UsedImplicitly]
	public class GameManager
	{
		private readonly Container _container;
		
		public GameManager(Container container)
		{
			_container = container;
		}

		public void Start()
		{
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
	}
}