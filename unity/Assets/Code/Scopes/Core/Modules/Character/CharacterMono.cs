using System;
using Code.DI;
using UnityEngine;
using UnityEngine.InputSystem;
using InputSystem = Code.Input.InputSystem;

namespace Code.Core.Character
{
	public class CharacterMono : MonoBehaviour
	{
		#region public fields

		public bool is_running { get; private set; }

		#endregion

		#region serialize fields

		[SerializeField] private Rigidbody _rigidbody;
		[SerializeField] private Camera _camera;

		#endregion

		#region inject fields

		private InputSystem _input;
		private CharacterConfig _config;

		#endregion

		#region private fields

		private bool _is_initialized;

		private Vector2 _camera_velocity;
		private Vector2 _camera_frame_velocity;

		private CharacterInteractive _interactive;

		#endregion

		[Inject]
		private void Constructor(InputSystem input, CharacterConfig config)
		{
			input.Enable();

			_input = input;
			_config = config;

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;

			_interactive = new CharacterInteractive(_camera, _config.interactive);

			_input.Player.Interact.started += OnInteractStarted;
			_interactive.on_hover += OnInteractHovered;

			_is_initialized = true;
		}

		private void Update()
		{
			if (_is_initialized == false)
			{
				return;
			}

			_interactive.Update();
		}

		private void FixedUpdate()
		{
			if (_is_initialized == false)
			{
				return;
			}

			Move();
		}

		private void LateUpdate()
		{
			if (_is_initialized == false)
			{
				return;
			}

			RotationCamera();
		}

		private void Move()
		{
			var axis = _input.Player.Move.ReadValue<Vector2>();

			is_running = _input.Player.Sprint.IsPressed();

			var speed = is_running ? _config.speed_run : _config.speed_walk;

			var velocity = new Vector2(axis.x * speed, axis.y * speed);

			var vector = new Vector3(velocity.x, _rigidbody.linearVelocity.y, velocity.y);

			var linear_velocity = transform.rotation * vector;

			_rigidbody.linearVelocity = linear_velocity;
		}

		private void RotationCamera()
		{
			var axis = _input.Player.Look.ReadValue<Vector2>();

			var raw_frame_velocity = Vector2.Scale(axis, Vector2.one * _config.camera_sensitivity);

			_camera_frame_velocity = Vector2.Lerp
			(
				_camera_frame_velocity,
				raw_frame_velocity,
				1 / _config.camera_smoothing
			);

			_camera_velocity += _camera_frame_velocity;
			_camera_velocity.y = Mathf.Clamp(_camera_velocity.y, _config.camera_clamp_min, _config.camera_clamp_max);

			_camera.transform.localRotation = Quaternion.AngleAxis(-_camera_velocity.y, Vector3.right);
			transform.localRotation = Quaternion.AngleAxis(_camera_velocity.x, Vector3.up);
		}
		
		private void OnInteractHovered(ICharacterInteractable interactable)
		{
			Debug.Log(interactable);
		}

		private void OnInteractStarted(InputAction.CallbackContext callback)
		{
			if (_interactive.TryInteract(out var interacted))
			{
				Debug.Log(interacted);
			}
		}
	}
}