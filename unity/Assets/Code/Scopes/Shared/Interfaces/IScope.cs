using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Shared
{
	public interface IScope
	{
		public UniTask InitializeAsync(CancellationToken token);
		public UniTask DisposeAsync(CancellationToken token);
	}
}