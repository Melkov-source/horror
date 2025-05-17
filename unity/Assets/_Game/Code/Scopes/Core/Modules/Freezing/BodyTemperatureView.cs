using TMPro;
using UnityEngine;

namespace Code.Core.Modules.Freezing
{
	public class BodyTemperatureView : MonoBehaviour
	{
		public TMP_Text _temperature_total;
		
		public BodyTemperaturePartView head;
		public BodyTemperaturePartView torso;
		public BodyTemperaturePartView left_arm;
		public BodyTemperaturePartView right_arm;
		public BodyTemperaturePartView left_leg;
		public BodyTemperaturePartView right_leg;

		public void Setup(BodyTemperature body)
		{
			body.total.On(v => _temperature_total.text = $"{Mathf.RoundToInt(v)}%");
			
			head.Setup(body.head);
			torso.Setup(body.torso);
			left_arm.Setup(body.left_arm);
			right_arm.Setup(body.right_arm);
			left_leg.Setup(body.left_leg);
			right_leg.Setup(body.right_leg);
		}
	}
}