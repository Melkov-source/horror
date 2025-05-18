using Code.Input;
using Unity.Cinemachine;
using UnityEngine;

namespace Code.Core.Character
{
	public class PlayerView : MonoBehaviour
	{
		public Camera camera => _camera;
		
		[SerializeField] private Camera _camera;
		[SerializeField] private CharacterController _character_controller;
		[SerializeField] private CinemachineBasicMultiChannelPerlin _noise;
		[SerializeField] private Transform _camera_root;

		private PlayerConfig _config;
		private InputSystem _input;
		
		private bool _initialized;
		private bool _grounded = true;
		private float _cinemachine_target_pitch;
		private float _current_speed;
		private float _rotation_velocity;
		private float _vertical_velocity;
		private float _fall_timeout_delta;
		
		private const float DELTA_TIME_MULTIPLIER = 1.0f;
		private const float SPEED_OFFSET = 0.1f;
		private const float INPUT_MAGNITUDE = 1f;

		public void Initialize(PlayerConfig config, InputSystem input)
		{
			_config = config;
			_input = input;

			_fall_timeout_delta = config.fall_timeout;

			_initialized = true;
		}

		private void Update()
		{
			if (_initialized == false)
			{
				return;
			}
			
			Gravity();
			GroundedCheck();
			Move();
		}

		private void LateUpdate()
		{
			if (_initialized == false)
			{
				return;
			}
			
			CameraRotation();
		}
		
		private void CameraRotation()
		{
			var look = _input.Player.Look.ReadValue<Vector2>();
			
			if (look.sqrMagnitude >= _config.threshold == false)
			{
				return;
			}
				
			_cinemachine_target_pitch += -look.y * _config.rotation_speed * DELTA_TIME_MULTIPLIER;
			_rotation_velocity = look.x * _config.rotation_speed * DELTA_TIME_MULTIPLIER;

			_cinemachine_target_pitch = ClampAngle(_cinemachine_target_pitch, _config.bottom_clamp, _config.top_clamp);

			_camera_root.transform.localRotation = Quaternion.Euler(_cinemachine_target_pitch, 0.0f, 0.0f);

			transform.Rotate(Vector3.up * _rotation_velocity);
		}

		private void Move()
		{
			var move = _input.Player.Move.ReadValue<Vector2>();
			var sprint = _input.Player.Sprint.IsPressed();
			
			var target_speed = sprint ? _config.sprint_speed : _config.move_speed;
			
			if (move == Vector2.zero)
			{
				target_speed = 0.0f;
				_noise.FrequencyGain = _config.default_camera_noise.frequency_gain;
				_noise.AmplitudeGain = _config.default_camera_noise.amplitude_gain;
			}
			else
			{
				if (sprint)
				{
					_noise.FrequencyGain = _config.sprint_camera_noise.frequency_gain;
					_noise.AmplitudeGain = _config.sprint_camera_noise.amplitude_gain;
				}
				else
				{
					_noise.FrequencyGain = _config.move_camera_noise.frequency_gain;
					_noise.AmplitudeGain = _config.move_camera_noise.amplitude_gain;
				}
			}

			var current_horizontal_speed = new Vector3(_character_controller.velocity.x, 0.0f, _character_controller.velocity.z).magnitude;

			if (current_horizontal_speed < target_speed - SPEED_OFFSET ||
			    current_horizontal_speed > target_speed + SPEED_OFFSET)
			{
				_current_speed = Mathf.Lerp
				(
					current_horizontal_speed, 
					target_speed * INPUT_MAGNITUDE,
					Time.deltaTime * _config.speed_change_rate
				);

				_current_speed = Mathf.Round(_current_speed * 1000f) / 1000f;
			}
			else
			{
				_current_speed = target_speed;
			}

			var input_direction = new Vector3(move.x, 0.0f, move.y).normalized;

			if (move != Vector2.zero)
			{
				input_direction = transform.right * move.x + transform.forward * move.y;
			}

			_character_controller.Move(input_direction.normalized * (_current_speed * Time.deltaTime) +
			                           new Vector3(0.0f, _vertical_velocity, 0.0f) * Time.deltaTime);
		}

		private void GroundedCheck()
		{
			var sphere_position = new Vector3
			(
				transform.position.x,
				transform.position.y - _config.grounded_offset,
				transform.position.z
			);

			_grounded = Physics.CheckSphere
			(
				sphere_position, 
				_config.grounded_radius, 
				_config.ground_layers,
				QueryTriggerInteraction.Ignore
			);
		}

		private void Gravity()
		{
			if (_grounded)
			{
				_fall_timeout_delta = _config.fall_timeout;

				if (_vertical_velocity < 0.0f)
				{
					_vertical_velocity = -2f;
				}
			}
			else
			{
				if (_fall_timeout_delta >= 0.0f)
				{
					_fall_timeout_delta -= Time.deltaTime;
				}
			}

			if (_vertical_velocity < _config.terminal_velocity)
			{
				_vertical_velocity += _config.gravity * Time.deltaTime;
			}
		}
		
		private static float ClampAngle(float lf_angle, float lf_min, float lf_max)
		{
			if (lf_angle < -360f)
			{
				lf_angle += 360f;
			}

			if (lf_angle > 360f)
			{
				lf_angle -= 360f;
			}
			
			return Mathf.Clamp(lf_angle, lf_min, lf_max);
		}
	}
}