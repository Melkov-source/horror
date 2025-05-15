using UnityEngine;

namespace Code.Core.Chapters.Road
{
	public class Car : MonoBehaviour
	{
		[field: SerializeField] public Radio radio { get; private set; }
	}
}