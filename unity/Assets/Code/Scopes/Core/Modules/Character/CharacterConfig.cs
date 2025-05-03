using System.Runtime.Serialization;

namespace Code.Core.Character
{
	[DataContract]
	public class CharacterConfig
	{
		[DataMember] public float position_threshold { get; private set; } = 0.01f;
		[DataMember] public float speed_walk { get; private set; } = 5f;
		[DataMember] public float speed_run { get; private set; } = 9f;
		
		[DataMember] public float camera_sensitivity { get; private set; } = 0.2f;
		[DataMember] public float camera_smoothing { get; private set; } = 7f;
		
		[DataMember] public float camera_clamp_min { get; private set; } = -90;
		[DataMember] public float camera_clamp_max { get; private set; } = 90;
	}
}