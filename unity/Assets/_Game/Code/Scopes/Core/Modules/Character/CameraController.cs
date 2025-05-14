using Code.Input;
using UnityEngine;

namespace Code.Core.Character
{
	public class CameraController : MonoBehaviour
	{
		public float sensitivity = 2f;
		public float smoothTime = 0.05f;
		public float minY = -60f;
		public float maxY = 60f;

		private float yaw;   // Влево/вправо
		private float pitch; // Вверх/вниз

		private float yawSmooth;
		private float pitchSmooth;
		private float yawVelocity;
		private float pitchVelocity;

		private InputSystem _input;

		void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			_input = new InputSystem();
			_input.Enable();

			Vector3 euler = transform.eulerAngles;
			yaw = euler.y;
			pitch = euler.x;
			if (pitch > 180f) pitch -= 360f;

			yawSmooth = yaw;
			pitchSmooth = pitch;
		}

		void Update()
		{
			Vector2 look = _input.Player.Look.ReadValue<Vector2>();

			yaw += look.x * sensitivity;
			pitch -= look.y * sensitivity;
			pitch = Mathf.Clamp(pitch, minY, maxY);

			yawSmooth = Mathf.SmoothDampAngle(yawSmooth, yaw, ref yawVelocity, smoothTime);
			pitchSmooth = Mathf.SmoothDamp(pitchSmooth, pitch, ref pitchVelocity, smoothTime);

			transform.rotation = Quaternion.Euler(pitchSmooth, yawSmooth, 0f);
		}
	}
}