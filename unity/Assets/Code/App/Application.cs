using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.App
{
	[UsedImplicitly]
	public class Application
	{
		private readonly ScopeDirector _director;
		
		public Application(ScopeDirector director)
		{
			_director = director;
		}

		public void Main()
		{
			_director
				.ToScopeAsync(AppScope.MENU)
				.Forget();
		}
		
		public class MonoHeart : MonoBehaviour
		{
			public void Update()
			{
			
			}

			public void FixedUpdate()
			{
			
			}

			public void LateUpdate()
			{
			
			}
		}
	}
}