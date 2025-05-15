using UnityEngine;

namespace Code.Core.Chapters.Road
{
	public class CarWaypoint : MonoBehaviour
	{
		public float speedToNext = 5f;

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(transform.position, 0.3f);
		}
	}
}