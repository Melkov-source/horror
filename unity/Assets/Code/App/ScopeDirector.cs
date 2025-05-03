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
	public class ScopeDirector
	{
		private readonly Application.MonoHeart _heart;
		private IScope _scope;
		
		private readonly Dictionary<AppScope, Func<IScope>> _scope_factories = new()
		{
			[AppScope.MENU] = DIContext.Register<MenuContext, MenuScope>,
			[AppScope.CORE] = DIContext.Register<CoreContext, CoreScope>,
		};

		public ScopeDirector(Application.MonoHeart heart)
		{
			_heart = heart;
		}
		
		public async UniTask ToScopeAsync(AppScope type)
		{
			//TODO: Handle exceptions try/catch for operation cancellation token
			
			if (_scope != null)
			{
				await _scope.DisposeAsync(_heart.destroyCancellationToken);
			}

			if (_scope_factories.TryGetValue(type, out var factory) == false)
			{
				throw new ArgumentOutOfRangeException(nameof(type));
			}

			_scope = factory();

			_scope
				.InitializeAsync(_heart.destroyCancellationToken)
				.Forget();
		}
	}
}