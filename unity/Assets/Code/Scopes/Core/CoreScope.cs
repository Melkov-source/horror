using System;
using System.Collections.Generic;
using System.Threading;
using Code.Core.Chapters;
using Code.Core.Character;
using Code.Core.Dialogue;
using Code.Core.HUD;
using Code.DI;
using Code.PanelManager;
using Code.Shared;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Code.Core
{
	[UsedImplicitly]
	public class CoreScope : IScope
	{
		private readonly IPanelManager _panel_manager;
		private readonly DIContainer _container;
		private IChapter _chapter;
		
		public CoreScope(DIContainer container, IPanelManager panel_manager)
		{
			_container = container;
			_panel_manager = panel_manager;
		}
		
		public async UniTask InitializeAsync(CancellationToken token)
		{
			Debug.Log("CoreScope Initialize");
			
			/*var character_config_text_asset = Addressables
				.LoadAssetAsync<TextAsset>("Character/Configs/CharacterConfig.json")
				.WaitForCompletion();
			
			var character_prefab = Addressables
				.LoadAssetAsync<GameObject>("Character/Prefabs/Character.prefab")
				.WaitForCompletion();

			var instance = Object.Instantiate(character_prefab);

			var character = instance.GetComponent<CharacterMono>();

			var json = character_config_text_asset.text;

			var config = JsonConvert.DeserializeObject<CharacterConfig>(json);
			
			_container.Inject(character, config);*/

			var hud = _panel_manager.LoadPanel<HUDPanelController>();
			hud.Open();
			
			var dialogue = _panel_manager.LoadPanel<DialoguePanelController>();

			dialogue.StartDialogue(new MomDialogue_001()).Forget();
		}

		public async UniTask DisposeAsync(CancellationToken token)
		{
			throw new System.NotImplementedException();
		}
	}
	
	public class MomDialogue_001 : DialogueBase
	{
		public override NPCType type => NPCType.MOM;

		public override List<Func<bool>> conditions => new();

		public override List<NPCNode> sequence => new()
		{
			new NPCNode("...")
			{
				choices = new List<PlayerChoiceNode>
				{
					new("Ещё долго?"),
					new("А че мы так долго едем?"),
					new("Ёпта! Я проснулся, где мы?")
				}
			},

			new NPCNode("Почти приехали. Ещё пару километров и будем на развилке.")
			{
				choices = new List<PlayerChoiceNode>
				{
					new("Там хоть есть интернет?")
					{
						next = new("хуй его знает Вася!")
						{
							choices = new()
							{
								new("эй епта, без выражений"),
								new("пон")
								{
									next = new("Пошел ты!")
									{
										choices = new()
										{
											new("ок")
										}
									}
								},
								new("ну блиииин!"),
							}
						}
					}
				}
			},

			new NPCNode("Вряд ли. Но там спокойно. И деду будет приятно, что ты приехал.")
			{
				choices = new List<PlayerChoiceNode>
				{
					new("Вряд ли. Но там спокойно. И деду будет приятно, что ты приехал."),
					new("Вряд ли. Но там спокойно. И деду будет приятно, что ты приехал."),
					new("Вряд ли. Но там спокойно. И деду будет приятно, что ты приехал.")
				}
			},
			
			new NPCNode("Ну крышу уже починили. А вот с белками — не обещаю")
			{
				choices = new List<PlayerChoiceNode>
				{
					new("...")
				}
			}
		};
	}
}