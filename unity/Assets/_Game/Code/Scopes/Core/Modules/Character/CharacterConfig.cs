using System.Runtime.Serialization;

namespace Code.Core.Character
{
	[DataContract]
	public class CharacterConfig
	{
		[DataMember] public Interactive interactive { get; private set; }
		
		[DataMember] public float position_threshold { get; private set; }
		[DataMember] public float speed_walk { get; private set; }
		[DataMember] public float speed_run { get; private set; }
		
		[DataMember] public float camera_sensitivity { get; private set; }
		[DataMember] public float camera_smoothing { get; private set; }
		
		[DataMember] public float camera_clamp_min { get; private set; }
		[DataMember] public float camera_clamp_max { get; private set; }
		
		[DataContract]
		public class Interactive
		{
			[DataMember] public float ray_length { get; private set; }
		}
	}
}