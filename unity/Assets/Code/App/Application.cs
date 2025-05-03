using Code.Input;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.App
{
	[UsedImplicitly]
	public class Application
	{
		private readonly ScopeDirector _director;
		private readonly InputSystem _input;
		
		public Application(ScopeDirector director, InputSystem input)
		{
			_director = director;
			_input = input;
		}

		public void Main()
		{
			_input.Enable();
			
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