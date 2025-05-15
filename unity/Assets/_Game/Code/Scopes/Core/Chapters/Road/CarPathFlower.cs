using UnityEngine;

namespace Code.Core.Chapters.Road
{
	public class CarPathFollower : MonoBehaviour
	{
		[SerializeField] private CarWaypoint[] _waypoints;
		[SerializeField] private float _rotation_speed = 5f;

		private int _current_waypoint_index = 0;
		private float _current_speed;

		private void Start()
		{
			if (_waypoints.Length > 0)
			{
				_current_speed = _waypoints[0].speedToNext;
			}
		}

		private void Update()
		{
			if (_waypoints.Length == 0) return;

			var target = _waypoints[_current_waypoint_index].transform;
			var distance_to_target = Vector3.Distance(transform.position, target.position);

			if (distance_to_target < 1f)
			{
				_current_waypoint_index = (_current_waypoint_index + 1) % _waypoints.Length;
				_current_speed = _waypoints[_current_waypoint_index].speedToNext;
				target = _waypoints[_current_waypoint_index].transform;
			}

			var direction = (target.position - transform.position).normalized;
			var look_rotation = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(transform.rotation, look_rotation, _rotation_speed * Time.deltaTime);
			transform.position += transform.forward * (_current_speed * Time.deltaTime);
		}
	}
}