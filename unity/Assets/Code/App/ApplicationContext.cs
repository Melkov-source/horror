using Code.DI;
using Code.Input;
using Code.PanelManager;
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
			var application = DIContext.Register<ApplicationContext, Application>();

			application.Main();
		}
		
		public void InstallBindings(DIContainer container)
		{
			container
				.Bind<Application>()
				.AsSingleton();
			
			container
				.Bind<ScopeDirector>()
				.AsSingleton();
			
			container
				.Bind<InputSystem>()
				.AsSingleton();

			var mono_heart = new GameObject("[App] [MonoHeart]").AddComponent<Application.MonoHeart>();
			
			Object.DontDestroyOnLoad(mono_heart);
			
			container
				.Bind<Application.MonoHeart>()
				.FromInstance(mono_heart)
				.AsSingleton();
		}
	}
}