using Code.Core.Modules.Freezing;

namespace Code.Core.Character
{
	public class CharacterModel
	{
		public BodyTemperature body { get; private set; }

		public CharacterModel()
		{
			body = new BodyTemperature(data: new BodyTemperature.Data()
			{
				head = new BodyTemperaturePart.Data()
				{
					value = 100,
					resistance = 0
				},
				torso = new BodyTemperaturePart.Data()
				{
					value = 100,
					resistance = 50
				},
				left_arm = new BodyTemperaturePart.Data()
				{
					value = 100,
					resistance = 0
				},
				right_arm = new BodyTemperaturePart.Data()
				{
					value = 100,
					resistance = 0
				},
				left_leg = new BodyTemperaturePart.Data()
				{
					value = 100,
					resistance = 0
				},
				right_leg = new BodyTemperaturePart.Data()
				{
					value = 100,
					resistance = 0
				},
			});
		}
	}
}