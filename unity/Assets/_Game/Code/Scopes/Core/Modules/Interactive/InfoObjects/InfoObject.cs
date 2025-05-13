using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Core.Interactive
{
	public class InfoObject : MonoBehaviour, IInteractable
	{
		[field: SerializeField] public Info info { get; private set; }
		
		public void Interact()
		{
		}
		
		[Serializable]
		public class Info
		{
			[field: SerializeField] public List<string> sequence { get; private set; }
		}
	}
}