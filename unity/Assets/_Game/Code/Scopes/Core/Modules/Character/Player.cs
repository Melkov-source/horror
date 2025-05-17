using System.Threading;
using Code.Core.HUD;
using Code.Core.Interactive;
using Code.Core.Modules.Freezing;
using Code.Core.Modules.Inventory;
using Code.DI;
using Code.PanelManager;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using InputSystem = Code.Input.InputSystem;

namespace Code.Core.Character
{
	public class Player : ITemperatureAffected
	{
		public Vector3 Position => _view.transform.position;
		public BodyTemperature Body => _model.body;
		
		private InputSystem _input;
		private IPanelManager _panel_manager;
		private TemperatureController _temperature_controller;
		
		private CharacterMono _view;
		private CharacterModel _model;

		private HUDPanelController _hud;
		private InfoObjectPanelController _info_object;
		private InventoryPanelController _inventory;

		[Inject]
		public void Constructor
		(
			CharacterModel model, 
			CharacterMono view,
			
			InputSystem input,
			IPanelManager panel_manager,
			TemperatureController temperature_controller
		)
		{
			_view = view;
			_model = model;

			_input = input;
			_panel_manager = panel_manager;
			_temperature_controller = temperature_controller;
		}

		public UniTask LoadAsync(CancellationToken token)
		{
			_info_object = _panel_manager.LoadPanel<InfoObjectPanelController>();
			_inventory = _panel_manager.LoadPanel<InventoryPanelController>();
			_hud = _panel_manager.LoadPanel<HUDPanelController>();
			
			return UniTask.CompletedTask;
		}

		public UniTask InitializeAsync(CancellationToken token)
		{
			_input.Enable();
			
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			
			_input.Player.Inventory.started += OnInventoryStarted;
			
			_model.body.total.On(OnTemperatureTotalUpdated, true);
			
			_hud.SetBody(_model.body);
			
			_hud.Open();
			
			_temperature_controller.RegisterColdTarget(this);
			
			return UniTask.CompletedTask;
		}

		private void OnInteracted(IInteractable interactable)
		{
			switch (interactable)
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
						Object.Destroy(item.gameObject);
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
			}
			else
			{
				_inventory.Open();
				
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
		}

		private void OnTemperatureTotalUpdated(float value)
		{
			_hud.SetTotalTemperatureValue(value);
		}
	}
}