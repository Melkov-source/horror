using System;
using System.Collections.Generic;
using Code.Core;
using Code.DI;
using Code.Menu;
using Code.Shared;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace Code.App
{
	[UsedImplicitly]
	public class ScopeDirector : IScopeDirector
	{
		private readonly Application.MonoHeart _heart;
		private readonly DIContainer _app_container;
		
		private IScope _scope;
		private DIContainer _container;
		
		// TODO: Create factory class
		private readonly Dictionary<AppScope, Func<DIContainer, (DIContainer, IScope)>> _scope_factories = new()
		{
			[AppScope.MENU] = container => DIContext.Register<MenuContext, MenuScope>(container),
			[AppScope.CORE] = container => DIContext.Register<CoreContext, CoreScope>(container),
		};

		public ScopeDirector(Application.MonoHeart heart, DIContainer app_container)
		{
			_heart = heart;
			_app_container = app_container;
		}
		
		public async UniTask ToScopeAsync(AppScope type)
		{
			if (_scope != null)
			{
				await _scope.DisposeAsync(_heart.destroyCancellationToken);
				_scope = null;
			}

			_container?.Dispose();
			_container = null;

			if (_scope_factories.TryGetValue(type, out var factory) == false)
			{
				throw new ArgumentOutOfRangeException(nameof(type));
			}

			var (container, scope) = factory(_app_container);
			
			_container = container;
			_scope = scope;

			_scope.InitializeAsync(_heart.destroyCancellationToken).Forget();
		}
	}
}