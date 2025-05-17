using Code.Input;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Core.Character
{
	public class PlayerView : MonoBehaviour
	{
		[SerializeField] private Camera _camera;
		[SerializeField] private CharacterController _character_controller;
		[SerializeField] private CinemachineBasicMultiChannelPerlin _noise;

		private InputSystem _input;

		public float MoveSpeed = 4.0f;
		public float SprintSpeed = 6.0f;
		public float RotationSpeed = 1.0f;
		public float SpeedChangeRate = 10.0f;

		public float JumpHeight = 1.2f;
		public float _gravity = -15.0f;

		public float JumpTimeout = 0.1f;
		public float FallTimeout = 0.15f;

		public bool Grounded = true;
		public float GroundedOffset = -0.14f;
		public float GroundedRadius = 0.5f;
		public LayerMask GroundLayers;

		public GameObject CinemachineCameraTarget;
		public float TopClamp = 90.0f;
		public float BottomClamp = -90.0f;

		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		private const float _threshold = 0.01f;
		
		private bool _initialized;

		public void Initialize(InputSystem input)
		{
			_input = input;
			
			input.Enable();

			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;

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
			
			if (look.sqrMagnitude >= _threshold == false)
			{
				return;
			}
			
			const float DELTA_TIME_MULTIPLIER = 1.0f;
				
			_cinemachineTargetPitch += -look.y * RotationSpeed * DELTA_TIME_MULTIPLIER;
			_rotationVelocity = look.x * RotationSpeed * DELTA_TIME_MULTIPLIER;

			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

			CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

			transform.Rotate(Vector3.up * _rotationVelocity);
		}

		private void Move()
		{
			var move = _input.Player.Move.ReadValue<Vector2>();
			var sprint = _input.Player.Sprint.IsPressed();
			
			var target_speed = sprint ? SprintSpeed : MoveSpeed;

			
			if (move == Vector2.zero)
			{
				target_speed = 0.0f;
				_noise.FrequencyGain = 1;
				_noise.AmplitudeGain = 1;
			}
			else
			{
				if (sprint)
				{
					_noise.AmplitudeGain = 2;
					_noise.FrequencyGain = 5;
				}
				else
				{
					_noise.FrequencyGain = 3;
					_noise.AmplitudeGain = 1;
				}
			}

			var current_horizontal_speed = new Vector3(_character_controller.velocity.x, 0.0f, _character_controller.velocity.z).magnitude;

			const float SPEED_OFFSET = 0.1f;
			const float INPUT_MAGNITUDE = 1f;

			if (current_horizontal_speed < target_speed - SPEED_OFFSET ||
			    current_horizontal_speed > target_speed + SPEED_OFFSET)
			{
				_speed = Mathf.Lerp
				(
					current_horizontal_speed, 
					target_speed * INPUT_MAGNITUDE,
					Time.deltaTime * SpeedChangeRate
				);

				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = target_speed;
			}

			var input_direction = new Vector3(move.x, 0.0f, move.y).normalized;

			if (move != Vector2.zero)
			{
				input_direction = transform.right * move.x + transform.forward * move.y;
			}

			_character_controller.Move(input_direction.normalized * (_speed * Time.deltaTime) +
			                           new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void GroundedCheck()
		{
			var sphere_position = new Vector3
			(
				transform.position.x,
				transform.position.y - GroundedOffset,
				transform.position.z
			);

			Grounded = Physics.CheckSphere
			(
				sphere_position, 
				GroundedRadius, 
				GroundLayers,
				QueryTriggerInteraction.Ignore
			);
		}

		private void Gravity()
		{
			if (Grounded)
			{
				_fallTimeoutDelta = FallTimeout;

				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}
			}
			else
			{
				_jumpTimeoutDelta = JumpTimeout;

				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
			}

			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += _gravity * Time.deltaTime;
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
		
		private void OnDrawGizmosSelected()
		{
			var transparent_green = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			var transparent_red = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			Gizmos.color = Grounded ? transparent_green : transparent_red;

			var position = new Vector3
			(
				transform.position.x,
				transform.position.y - GroundedOffset,
				transform.position.z
			);

			Gizmos.DrawSphere(position, GroundedRadius);
		}
	}
}