using _Game.Code.Scopes.Shared;
using _Game.Code.Scopes.Shared.Interfaces;
using Code.Debug;
using Code.DI;
using Code.Input;
using Code.Shared;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.App
{
	[UsedImplicitly]
	public class ApplicationContext : IDIContext
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void OnLoad()
		{
			var (container, application) = DIContext.Register<ApplicationContext, Application>();
			
			DIContext.Register<DebugContext>(container);

			application.Start();
		}
		
		public void InstallBindings(DIContainer container)
		{
			container
				.Bind<Application>()
				.AsSingleton();
			
			container
				.Bind<IScopeDirector, ScopeDirector>()
				.AsSingleton();
			
			container
				.Bind<InputSystem>()
				.AsSingleton();

			var mono_heart = new GameObject("[App] [MonoHeart]").AddComponent<MonoHeart>();
			
			Object.DontDestroyOnLoad(mono_heart);
			
			container
				.Bind<MonoHeart>()
				.FromInstance(mono_heart)
				.AsSingleton();
		}
	}
}