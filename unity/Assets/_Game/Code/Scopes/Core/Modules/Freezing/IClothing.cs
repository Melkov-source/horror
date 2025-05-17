using Code.Utils.Reactive;

namespace Code.Core.Modules.Freezing
{
	public interface IClothing
	{
		BehaviorSubject<float> resistance { get; }
	}
}