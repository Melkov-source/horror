﻿using System;
using Code.Core.Interactive;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Core.Character
{
	public class InteractiveController
	{
		public event Action<IInteractable> on_hover;
		
		private readonly Camera _camera;
		private readonly PlayerConfig _config;

		private Ray _ray;
		private RaycastHit _hit;

		private readonly Vector2 _screen_center;

		private Transform _current_interactable_transform;
		
		public InteractiveController(Camera camera, PlayerConfig config)
		{
			_camera = camera;
			_config = config;
			
			_screen_center = new Vector2(Screen.width / 2f, Screen.height / 2f);
		}

		public void Update()
		{
			_ray = _camera.ScreenPointToRay(_screen_center);

			if (Physics.Raycast(_ray, out _hit, _config.ray_length) == false)
			{
				_current_interactable_transform = null;
				return;
			}

			if (_current_interactable_transform == _hit.transform)
			{
				return;
			}
			
			_current_interactable_transform = _hit.transform;
				
			if(_hit.transform.gameObject.TryGetComponent<IInteractable>(out var interactable))
			{
				on_hover?.Invoke(interactable);
			}
		}

		public bool TryInteract([CanBeNull] out IInteractable interactable)
		{
			if (_hit.transform == null)
			{
				interactable = null;
				return false;
			}

			interactable = _hit.transform.gameObject.GetComponent<IInteractable>();
			
			Debug.Log(interactable);
			
			interactable?.Interact();
			
			return interactable != null;
		}
	}
}