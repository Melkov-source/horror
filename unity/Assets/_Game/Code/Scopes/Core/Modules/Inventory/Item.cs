using Code.Core.Interactive;
using UnityEngine;

namespace Code.Core.Modules.Inventory
{
	public class Item : MonoBehaviour, IInteractable
	{
		[field: SerializeField] public ItemInfo info { get; private set; }
		
		public void Interact()
		{
			
		}
	}
}