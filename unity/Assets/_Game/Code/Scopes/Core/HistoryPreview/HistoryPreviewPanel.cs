using Code.PanelManager;
using TMPro;
using UnityEngine;

namespace Code.Core.HistoryPreview
{
	public class HistoryPreviewPanel : PanelBase
	{
		[field: SerializeField] public TMP_Text tmp { get; private set; }
	}
}