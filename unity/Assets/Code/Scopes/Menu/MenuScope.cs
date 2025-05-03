using System.Threading;
using Code.Shared;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Menu
{
	public class MenuScope : IScope
	{
		public async UniTask InitializeAsync(CancellationToken token)
		{
			Debug.Log("MenuScope Initialize");
		}

		public async UniTask DisposeAsync(CancellationToken token)
		{
			
		}
	}
}