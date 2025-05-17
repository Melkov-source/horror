using TMPro;
using UnityEngine;

namespace Code.Core.Modules.Freezing
{
	public class BodyTemperaturePartView : MonoBehaviour
	{
		public TMP_Text _temperature;

		public void Setup(BodyTemperaturePart part)
		{
			part.On(v => _temperature.text = $"{Mathf.RoundToInt(v)}%");
		}
	}
}