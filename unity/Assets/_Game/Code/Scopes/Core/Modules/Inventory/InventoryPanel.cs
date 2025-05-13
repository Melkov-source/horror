using Code.PanelManager;
using TMPro;
using UnityEngine;

namespace Code.Core.Modules.Inventory
{
	public class InventoryPanel : PanelBase
	{
		[field: SerializeField] public TMP_Text tittle_tmp { get; private set; }
		[field: SerializeField] public TMP_Text mass_total_tmp { get; private set; }
		[field: SerializeField] public RectTransform items_content { get; private set; }
		
	}
}