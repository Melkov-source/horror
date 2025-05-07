using Code.Core.Interactive;
using Code.Core.Modules.Inventory;
using Code.DI;
using Code.PanelManager;
using Cysharp.Threading.Tasks;
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
		private IPanelManager _panel_manager;

		#endregion

		#region private fields

		private bool _is_initialized;

		private Vector2 _camera_velocity;
		private Vector2 _camera_frame_velocity;

		private CharacterInteractive _interactive;

		private InfoObjectPanelController _info_object;
		private InventoryPanelController _inventory;

		private bool _is_active = true;

		#endregion

		[Inject]
		private void Constructor
		(
			InputSystem input, 
			CharacterConfig config,
			IPanelManager panel_manager
		)
		{
			input.Enable();

			_input = input;
			_config = config;
			_panel_manager = panel_manager;

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;

			_interactive = new CharacterInteractive(_camera, _config.interactive);

			_input.Player.Interact.started += OnInteractStarted;
			_input.Player.Inventory.started += OnInventoryStarted;
			
			_interactive.on_hover += OnInteractHovered;

			// ✅ Включаем интерполяцию для Rigidbody
			_rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

			_info_object = panel_manager.LoadPanel<InfoObjectPanelController>();
			_inventory = panel_manager.LoadPanel<InventoryPanelController>();

			_is_initialized = true;
		}

		private void Update()
		{
			if (_is_initialized == false) return;
			
			if(_is_active == false) return;

			_interactive.Update();
		}

		private void FixedUpdate()
		{
			if (_is_initialized == false) return;
			
			if(_is_active == false) return;

			Move();
		}

		private void LateUpdate()
		{
			if (_is_initialized == false) return;
			
			if(_is_active == false) return;

			RotationCamera();
		}

		private void Move()
		{
			var axis = _input.Player.Move.ReadValue<Vector2>();
			is_running = _input.Player.Sprint.IsPressed();

			float speed = is_running ? _config.speed_run : _config.speed_walk;

			Vector2 planarVelocity = axis * speed;

			// ✅ Учитываем текущую вертикальную скорость (прыжки, падения)
			Vector3 targetVelocity = new Vector3(planarVelocity.x, _rigidbody.linearVelocity.y, planarVelocity.y);

			// ✅ Поворот игрока влияет на направление движения
			_rigidbody.linearVelocity = transform.rotation * targetVelocity;
		}

		private void RotationCamera()
		{
			Vector2 axis = _input.Player.Look.ReadValue<Vector2>();
			Vector2 rawFrameVelocity = Vector2.Scale(axis, Vector2.one * _config.camera_sensitivity);

			_camera_frame_velocity = Vector2.Lerp(_camera_frame_velocity, rawFrameVelocity, 1f / _config.camera_smoothing);
			_camera_velocity += _camera_frame_velocity;

			_camera_velocity.y = Mathf.Clamp(_camera_velocity.y, _config.camera_clamp_min, _config.camera_clamp_max);

			// ✅ Камера вращается только по X (вверх/вниз), персонаж — по Y
			_camera.transform.localRotation = Quaternion.AngleAxis(-_camera_velocity.y, Vector3.right);
			transform.localRotation = Quaternion.AngleAxis(_camera_velocity.x, Vector3.up);
		}

		private void OnInteractHovered(IInteractable interactable)
		{
			Debug.Log(interactable);
		}

		private void OnInteractStarted(InputAction.CallbackContext callback)
		{
			if (_interactive.TryInteract(out var interacted) == false)
			{
				return;
			}
			
			Debug.Log(interacted);

			switch (interacted)
			{
				case InfoObject info_object:
					_info_object
						.StartInfo(info_object.info)
						.Forget();
					break;
				case Item item:
					if (_inventory.TryAddItem(item.info))
					{
						Debug.Log(item.info);
						Destroy(item.gameObject);
					}
					break;
			}
		}

		private void OnInventoryStarted(InputAction.CallbackContext callback)
		{
			if (_inventory.panel.State == PanelState.OPENED)
			{
				_inventory.Close();
				
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
				_is_active = true;
			}
			else
			{
				_inventory.Open();
				
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				_is_active = false;
			}
		}
	}
}
