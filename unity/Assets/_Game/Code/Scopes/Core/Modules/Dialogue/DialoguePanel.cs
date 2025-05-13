using System.Collections.Generic;
using Code.PanelManager;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Core.Dialogue
{
	public class DialoguePanel : PanelBase
	{
		[field: SerializeField] public DialogueTextBlock npc_text_block { get; private set; }
		[field: SerializeField] public RectTransform choices_content { get; private set; }

		[SerializeField] private List<RectTransform> _rect_transforms; 

		public void ForceReloadRects()
		{
			_rect_transforms.ForEach(LayoutRebuilder.ForceRebuildLayoutImmediate);
		}
	}
}