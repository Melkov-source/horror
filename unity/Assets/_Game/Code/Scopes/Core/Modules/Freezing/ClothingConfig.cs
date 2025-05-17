using System;
using Code.Core.Modules.Inventory;
using UnityEngine;

namespace Code.Core.Modules.Freezing
{
	[CreateAssetMenu(menuName = "Game/ClothingInfo")]
	public class ClothingConfig : ItemInfo
	{
		[field: SerializeField] public TemperatureResistance resistance { get; private set; }

		[Serializable]
		public struct TemperatureResistance
		{
			[field: SerializeField] public float head { get; private set; }
			[field: SerializeField] public float torso { get; private set; }
			[field: SerializeField] public float left_arm { get; private set; }
			[field: SerializeField] public float right_arm { get; private set; }
			[field: SerializeField] public float left_leg { get; private set; }
			[field: SerializeField] public float right_leg { get; private set; }
		}
	}
}