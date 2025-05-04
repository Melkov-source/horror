using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Core.Chapters
{
	public class RoadChapter : IChapter
	{
		public UniTask InitializeAsync(CancellationToken token)
		{
			throw new System.NotImplementedException();
		}

		public UniTask DisposeAsync(CancellationToken token)
		{
			throw new System.NotImplementedException();
		}
	}
}