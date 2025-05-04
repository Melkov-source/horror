using UnityEngine;

namespace Code.Core.Character
{
	public class CharacterInteractive
	{
		private readonly Camera _camera;
		private readonly CharacterConfig.Interactive _config;

		private Ray _ray;
		private RaycastHit _hit;
		
		public CharacterInteractive(Camera camera, CharacterConfig.Interactive config)
		{
			_camera = camera;
			_config = config;
		}

		public void Update()
		{
			Ray();
			CheckRayCast();
		}
		
		private void Ray()
		{
			var screen = new Vector2(Screen.width / 2f, Screen.height / 2f);

			_ray = _camera.ScreenPointToRay(screen);
		}

		private void CheckRayCast()
		{
			var direction = _ray.direction * _config.ray_length;
			var point = _camera.transform.position;
			
			if (Physics.Raycast(_ray, out _hit, _config.ray_length))
			{
				Debug.DrawRay(point, direction, Color.blue);
			}
			
			if(_hit.transform == null)
			{
				Debug.DrawRay(point, direction, Color.red);
			}
			else
			{
				Debug.DrawRay(point, direction, Color.green);
			}
		}
	}
}