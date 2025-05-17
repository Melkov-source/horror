using UnityEngine;
using UnityEngine.UI;

namespace Code.Core.HUD
{
	public class ColdBar : MonoBehaviour
	{
		[SerializeField] private Image _image;
		[SerializeField] private Color[] _gradient_colors;
		
		private float _value = 1f;

		public void SetValue(float value)
		{
			var normalized = value / 100f;

			_value = Mathf.Clamp01(normalized);
		}

		private void Update()
		{
			_image.fillAmount = _value;

			if (_gradient_colors.Length == 0)
			{
				return;
			}
				
			var scaled = _value * (_gradient_colors.Length - 1);
			
			var index = Mathf.FloorToInt(scaled);
			
			var next_index = Mathf.Clamp(index + 1, 0, _gradient_colors.Length - 1);
			
			var t = scaled - index;

			var color = Color.Lerp(_gradient_colors[index], _gradient_colors[next_index], t);
			
			_image.color = color;
		}
	}
}