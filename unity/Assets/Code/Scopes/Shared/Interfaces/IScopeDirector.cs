using Cysharp.Threading.Tasks;

namespace Code.Shared
{
	public interface IScopeDirector
	{
		UniTask ToScopeAsync(AppScope type);
	}
}