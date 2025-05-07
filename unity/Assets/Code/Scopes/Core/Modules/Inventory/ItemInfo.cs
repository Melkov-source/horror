using Code.Core.Modules.Inventory.Enums;
using UnityEngine;

namespace Code.Core.Modules.Inventory
{
	[CreateAssetMenu(menuName = "Game/ItemInfo")]
	public class ItemInfo : ScriptableObject
	{
		[field: SerializeField] public ITEM_TYPE type { get; private set; }
		[field: SerializeField] public ITEM_ACTION action { get; private set; }
		
		[field: SerializeField] public Sprite icon { get; private set; }
		
		[field: SerializeField] public string name_short { get; private set; }
		[field: SerializeField] public string description { get; private set; }
		
		[field: SerializeField] public int mass { get; private set; }
	}
}