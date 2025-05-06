using Code.PanelManager;
using TMPro;
using UnityEngine;

namespace Code.Core.Interactive
{
	public class InfoObjectPanel : PanelBase
	{
		[field: SerializeField] public TMP_Text info_tmp { get; private set; }
	}
}