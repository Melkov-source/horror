using JetBrains.Annotations;
using UnityEngine;

namespace Code.Core.Character
{
	public class CharacterInteractive
	{
		private readonly Camera _camera;
		private readonly CharacterConfig.Interactive _config;

		private Ray _ray;
		private RaycastHit _hit;

		private readonly Vector2 _screen_center;
		
		public CharacterInteractive(Camera camera, CharacterConfig.Interactive config)
		{
			_camera = camera;
			_config = config;
			
			_screen_center = new Vector2(Screen.width / 2f, Screen.height / 2f);
		}

		public void Update()
		{
			_ray = _camera.ScreenPointToRay(_screen_center);
			
			Physics.Raycast(_ray, out _hit, _config.ray_length);
		}

		public bool TryInteract([CanBeNull] out ICharacterInteractable character_interactable)
		{
			Debug.Log(_hit.transform);
			
			if (_hit.transform == null)
			{
				character_interactable = null;
				return false;
			}

			character_interactable = _hit.transform.gameObject.GetComponent<ICharacterInteractable>();
			
			Debug.Log(character_interactable);
			
			character_interactable?.Interact();
			
			return character_interactable != null;
		}
	}
}