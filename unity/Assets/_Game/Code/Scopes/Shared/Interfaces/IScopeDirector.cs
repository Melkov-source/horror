using Cysharp.Threading.Tasks;

namespace Code.Shared
{
	public interface IScopeDirector
	{
		UniTask ToScopeAsync(APP_SCOPE type);
	}
}