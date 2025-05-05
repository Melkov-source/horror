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
		private readonly ObjectPool<DialogueTextBlock> _text_block_pool;
		private readonly List<DialogueTextBlock> _choice_text_blocks = new();

		private UniTaskCompletionSource<PlayerChoiceNode> _choice_completion_source;
		private GameObject _text_block_prefab;

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

			for (int index = 0, count = dialogue.sequence.Count; index < count; ++index)
			{
				var node = dialogue.sequence[index];

				while (node != null)
				{
					node = await ProcessNode(node);
				}
			}

			Close();
		}

		private async UniTask<NPCNode> ProcessNode(NPCNode npc_node)
		{
			ClearChoices();

			await panel.npc_text_block.Setup(npc_node, 700);

			_choice_completion_source = new UniTaskCompletionSource<PlayerChoiceNode>();

			var index = 0;

			foreach (var choice in npc_node.choices)
			{
				var block = _text_block_pool.Get();
				
				block.transform.SetSiblingIndex(index);
				
				await block.Setup(choice, 200);

				block.on_click += OnChoiceSelected;

				_choice_text_blocks.Add(block);

				index++;
			}
			
			var selected_choice = await _choice_completion_source.Task;

			_choice_completion_source = null;

			return selected_choice.next;
		}

		private void OnChoiceSelected(DialogueNodeBase node)
		{
			var choice = (PlayerChoiceNode)node;

			ClearChoices();

			_choice_completion_source?.TrySetResult(choice);
		}

		private void ClearChoices()
		{
			if (_choice_text_blocks.Count <= 0)
			{
				return;
			}
			
			foreach (var block in _choice_text_blocks)
			{
				block.on_click -= OnChoiceSelected;
				_text_block_pool.Release(block);
			}

			_choice_text_blocks.Clear();
		}

		private DialogueTextBlock CreateTextBlock()
		{
			var instance = Object.Instantiate
			(
				_text_block_prefab,
				panel.choices_content,
				true
			);

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