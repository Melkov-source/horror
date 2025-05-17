using Code.Shared;
using Cysharp.Threading.Tasks;

namespace _Game.Code.Scopes.Shared.Interfaces
{
	public interface IScopeDirector
	{
		UniTask ToScopeAsync(APP_SCOPE type);
	}
}