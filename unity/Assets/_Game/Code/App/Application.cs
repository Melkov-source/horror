using Code.Debug.Console;
using Code.DI;
using Code.Input;
using Code.Shared;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.App
{
	[UsedImplicitly]
	public class Application
	{
		private readonly IScopeDirector _director;
		private readonly InputSystem _input;
		
		public Application(IScopeDirector director, InputSystem input, DIContainer container)
		{
			_director = director;
			_input = input;
		}

		public void Start()
		{
			_input.Enable();
			
			_director
				.ToScopeAsync(APP_SCOPE.CORE)
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