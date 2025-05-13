using UnityEngine;
using UnityEngine.UI;

namespace Code.Core.Modules
{
	public class GradientBar : MonoBehaviour
	{
		[Range(0f, 1f)]
		public float value = 1f;

		public Color[] gradientColors; // Массив цветов (например, от холодного к теплому)

		private Image image;

		void Awake()
		{
			image = GetComponent<Image>();
		}

		void Update()
		{
			image.fillAmount = value;

			if (gradientColors.Length == 0)
				return;

			// Вычисляем интервал
			float scaled = value * (gradientColors.Length - 1);
			int index = Mathf.FloorToInt(scaled);
			int nextIndex = Mathf.Clamp(index + 1, 0, gradientColors.Length - 1);
			float t = scaled - index;

			// Интерполяция между цветами
			Color color = Color.Lerp(gradientColors[index], gradientColors[nextIndex], t);
			image.color = color;
		}
	}
}