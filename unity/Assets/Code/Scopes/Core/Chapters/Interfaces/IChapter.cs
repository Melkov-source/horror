using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Core.Chapters
{
	public interface IChapter
	{
		public UniTask InitializeAsync(CancellationToken token);
		public UniTask DisposeAsync(CancellationToken token);
	}
}