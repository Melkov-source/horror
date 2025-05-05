using Code.PanelManager;
using UnityEngine;

namespace Code.Core.Dialogue
{
	public class DialoguePanel : PanelBase
	{
		[field: SerializeField] public DialogueTextBlock npc_text_block { get; private set; }
		[field: SerializeField] public RectTransform choices_content { get; private set; }
	}
}