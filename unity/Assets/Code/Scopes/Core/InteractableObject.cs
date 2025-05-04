using Code.Core.Character;
using UnityEngine;

namespace Code.Core
{
	public class InteractableObject : MonoBehaviour, ICharacterInteractable
	{
		public void Interact()
		{
			Debug.Log("Interacted");
		}
	}
}