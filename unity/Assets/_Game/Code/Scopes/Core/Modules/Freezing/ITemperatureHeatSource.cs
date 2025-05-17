using UnityEngine;

namespace Code.Core.Modules.Freezing
{
	public interface ITemperatureHeatSource
	{
		float GetHeatAt(ITemperatureAffected target);
	}
}