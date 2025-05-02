using Code.DI;
using Code.Game;
using Code.Input;
using UnityEngine;

namespace Code.App
{
	public static class Application
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		public static void Run()
		{
			var container = new Container();

			container
				.Bind<InputSystem>()
				.AsSingleton();
			
			container
				.Bind<GameManager>()
				.AsSingleton();
			
			container.Build();

			var game = container.Resolve<GameManager>()!;
			
			game.Start();
		}
	}
}