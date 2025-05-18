using System;
using UnityEngine;

namespace Code.Core.Character
{
	[CreateAssetMenu(menuName = "Game/PlayerConfig")]
	public class PlayerConfig : ScriptableObject
	{
		[field: SerializeField] public float move_speed { get; private set; } = 4.0f;
		[field: SerializeField] public float sprint_speed { get; private set; } = 6.0f;
		[field: SerializeField] public float rotation_speed { get; private set; } = 1.0f;
		[field: SerializeField] public float speed_change_rate { get; private set; } = 10.0f;

		[field: SerializeField] public float gravity { get; private set; } = -15.0f;

		[field: SerializeField] public float fall_timeout { get; private set; } = 0.15f;
		
		[field: SerializeField] public float grounded_offset { get; private set; } = -0.14f;
		[field: SerializeField] public float grounded_radius { get; private set; } = 0.5f;
		[field: SerializeField] public LayerMask ground_layers { get; private set; }

		[field: SerializeField] public float top_clamp { get; private set; } = 90.0f;
		[field: SerializeField] public float bottom_clamp { get; private set; } = -90.0f;

		[field: SerializeField] public float threshold { get; private set; } = 0.01f;
		[field: SerializeField] public float terminal_velocity { get; private set; } = 53.0f;
		
		[field: SerializeField] public CameraNoiseInfo default_camera_noise { get; private set; }
		[field: SerializeField] public CameraNoiseInfo sprint_camera_noise { get; private set; }
		[field: SerializeField] public CameraNoiseInfo move_camera_noise { get; private set; }
		[field: SerializeField] public float ray_length { get; private set; } = 4f;
		
		
		[Serializable]
		public struct CameraNoiseInfo
		{
			[field: SerializeField] public float frequency_gain { get; private set; }
			[field: SerializeField] public float amplitude_gain { get; private set; }
		}
	}
}