using System.Collections.Generic;
using Code.PanelManager;
using Code.PanelManager.Attributes;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;

namespace Code.Core.Dialogue
{
	[Panel(PanelType = PanelType.OVERLAY, Order = 1, AssetId = "Dialogue/Prefabs/DialoguePanel.prefab")]
	public class DialoguePanelController : PanelControllerBase<DialoguePanel>
	{
		private GameObject _text_block_prefab;
		private readonly ObjectPool<DialogueTextBlock> _text_block_pool;
		private readonly List<DialogueTextBlock> _choice_text_blocks = new();

		private UniTaskCompletionSource<PlayerChoiceNode> _choice_completion_source;

		public DialoguePanelController()
		{
			_text_block_pool = new ObjectPool<DialogueTextBlock>
			(
				CreateTextBlock,
				OnGetTextBlock,
				OnReleaseTextBlock,
				OnDestroyTextBlock
			);
		}

		protected override void OnLoad()
		{
			base.OnLoad();

			_text_block_prefab = Addressables
				.LoadAssetAsync<GameObject>("Dialogue/Prefabs/DialogueTextBlock.prefab")
				.WaitForCompletion();
		}

		protected override void OnUnload()
		{
			base.OnUnload();

			Addressables.ReleaseInstance(_text_block_prefab);
		}

		public async UniTask StartDialogue(DialogueBase dialogue)
		{
			Open();

			for (int index = 0; index < dialogue.sequence.Count; ++index)
			{
				NPCNode currentNode = dialogue.sequence[index];

				while (currentNode != null)
				{
					currentNode = await ProcessNPCNode(currentNode);
				}
			}
		}

		private async UniTask<NPCNode> ProcessNPCNode(NPCNode npc_node)
		{
			// Очистка старых блоков
			if (_choice_text_blocks.Count > 0)
			{
				foreach (var block in _choice_text_blocks)
				{
					block.on_click -= OnChoiceSelected;
					_text_block_pool.Release(block);
				}
				_choice_text_blocks.Clear();
			}

			// Устанавливаем текст NPC
			Panel.npc_text_block.SetText(npc_node);

			// Создаём completion source ДО создания кнопок
			_choice_completion_source = new UniTaskCompletionSource<PlayerChoiceNode>();

			// Выводим варианты выбора
			foreach (var choice in npc_node.choices)
			{
				var block = _text_block_pool.Get();
				block.SetText(choice);
				block.on_click += OnChoiceSelected;

				block.transform.SetParent(Panel.choices_content, false);
				_choice_text_blocks.Add(block);
			}

			// Ожидаем выбор
			var selectedChoice = await _choice_completion_source.Task;

			// Очистка (на всякий случай)
			_choice_completion_source = null;

			if (selectedChoice.next != null)
			{
				Debug.Log($"[Dialogue] Есть next, продолжаем...");
				return selectedChoice.next;
			}
			else
			{
				Debug.Log($"[Dialogue] Нет next, завершаем шаг...");
				return null;
			}
		}

		private void OnChoiceSelected(DialogueNodeBase node)
		{
			var choice = (PlayerChoiceNode)node;
			Debug.Log($"[Dialogue] Выбран выбор: {choice.text}");

			foreach (var block in _choice_text_blocks)
			{
				block.on_click -= OnChoiceSelected;
				_text_block_pool.Release(block);
			}
			_choice_text_blocks.Clear();

			_choice_completion_source?.TrySetResult(choice);
		}

		private DialogueTextBlock CreateTextBlock()
		{
			var instance = Object.Instantiate(_text_block_prefab);
			return instance.GetComponent<DialogueTextBlock>();
		}

		private static void OnGetTextBlock(DialogueTextBlock block)
		{
			block.gameObject.SetActive(true);
		}

		private static void OnReleaseTextBlock(DialogueTextBlock block)
		{
			block.gameObject.SetActive(false);
		}

		private static void OnDestroyTextBlock(DialogueTextBlock block)
		{
			Object.Destroy(block.gameObject);
		}
	}
}
