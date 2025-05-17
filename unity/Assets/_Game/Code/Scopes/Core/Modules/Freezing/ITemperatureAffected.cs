using UnityEngine;

namespace Code.Core.Modules.Freezing
{
	public interface ITemperatureAffected
	{
		public Vector3 Position { get; }
		public BodyTemperature Body { get; }
	}
}