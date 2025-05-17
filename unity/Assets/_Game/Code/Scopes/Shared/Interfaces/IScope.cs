using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Game.Code.Scopes.Shared.Interfaces
{
	public interface IScope
	{
		public UniTask InitializeAsync(CancellationToken token);
		public UniTask DisposeAsync(CancellationToken token);
	}
}