using System.Runtime.Serialization;
using Code.Utils.Reactive;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Core.Modules.Freezing
{
	public class BodyTemperaturePart : ReactiveObject<float>
	{
		[CanBeNull] private IClothing _clothing;

		private readonly float _resistance;
		
		public BodyTemperaturePart(Data data) : base(data.value)
		{
			_resistance = data.resistance;
		}
		
		public void Apply(float temperature)
		{
			Next(temperature);
		}

		public void EquipClothing(IClothing clothing)
		{
			_clothing = clothing;
		}

		public void UnequipClothing()
		{
			_clothing = null;
		}

		public float CalculateResistance01()
		{
			var base_resistance = _resistance / 100f;
			
			var clothing_resistance = _clothing != null ? _clothing.resistance.value / 100f : 0f;
			
			var total_resistance = 1f - (1f - base_resistance) * (1f - clothing_resistance);

			return Mathf.Clamp01(total_resistance);
		}
		
		[DataContract]
		public struct Data
		{
			[DataMember] public float value;
			[DataMember] public float resistance;
		}
	}
}