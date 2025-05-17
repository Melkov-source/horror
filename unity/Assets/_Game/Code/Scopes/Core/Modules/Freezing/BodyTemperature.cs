using System.Collections.Generic;
using System.Runtime.Serialization;
using Code.Utils.Reactive;

namespace Code.Core.Modules.Freezing
{
	public class BodyTemperature
	{
		public BehaviorSubject<float> total { get; }
		
		public readonly List<BodyTemperaturePart> parts;
		public BodyTemperaturePart head { get; }
		public BodyTemperaturePart torso { get; }
		public BodyTemperaturePart left_arm { get; }
		public BodyTemperaturePart right_arm { get; }
		public BodyTemperaturePart left_leg { get; }
		public BodyTemperaturePart right_leg { get; }
		
		
		public BodyTemperature(Data data = default)
		{
			head = new BodyTemperaturePart(data.head);
			torso = new BodyTemperaturePart(data.torso);
			left_arm = new BodyTemperaturePart(data.left_arm);
			right_arm = new BodyTemperaturePart(data.right_arm);
			left_leg = new BodyTemperaturePart(data.left_leg);
			right_leg = new BodyTemperaturePart(data.right_leg);
			
			total = new BehaviorSubject<float>(CalculateTotal());
			
			head.On(OnTemperatureUpdated);
			torso.On(OnTemperatureUpdated);
			left_arm.On(OnTemperatureUpdated);
			right_arm.On(OnTemperatureUpdated);
			left_leg.On(OnTemperatureUpdated);
			right_leg.On(OnTemperatureUpdated);

			parts = new List<BodyTemperaturePart>
			{
				head,
				torso,
				left_arm,
				right_arm,
				left_leg,
				right_leg
			};
		}

		private void OnTemperatureUpdated(float _)
		{
			var calculated_total = CalculateTotal();
			
			total.Next(calculated_total);
		}

		private float CalculateTotal()
		{
			const float HEAD_MASS = 3f;
			const float TORSO_MASS = 2f;
			const float DEFAULT_MASS = 1f;

			const float TOTAL_MASS = HEAD_MASS + TORSO_MASS + DEFAULT_MASS * 4;

			var total_weight = head.value * HEAD_MASS +
			                   torso.value * TORSO_MASS +
			                   left_arm.value * DEFAULT_MASS +
			                   right_arm.value * DEFAULT_MASS +
			                   left_leg.value * DEFAULT_MASS +
			                   right_leg.value * DEFAULT_MASS;

			return total_weight / TOTAL_MASS;
		}

		[DataContract]
		public struct Data
		{
			[DataMember] public BodyTemperaturePart.Data head;
			[DataMember] public BodyTemperaturePart.Data torso;
			[DataMember] public BodyTemperaturePart.Data left_arm;
			[DataMember] public BodyTemperaturePart.Data right_arm;
			[DataMember] public BodyTemperaturePart.Data left_leg;
			[DataMember] public BodyTemperaturePart.Data right_leg;
		}
	}
}