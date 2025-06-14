﻿using System;
using System.Collections.Generic;
using _Game.Code.Scopes.Shared;
using _Game.Code.Scopes.Shared.Interfaces;
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
		private readonly MonoHeart _heart;
		private readonly DIContainer _app_container;
		
		private IScope _scope;
		private DIContainer _container;
		
		// TODO: Create factory class
		private readonly Dictionary<APP_SCOPE, Func<DIContainer, (DIContainer, IScope)>> _scope_factories = new()
		{
			[APP_SCOPE.MENU] = container => DIContext.Register<MenuContext, MenuScope>(container),
			[APP_SCOPE.CORE] = container => DIContext.Register<CoreContext, CoreScope>(container),
		};

		public ScopeDirector(MonoHeart heart, DIContainer app_container)
		{
			_heart = heart;
			_app_container = app_container;
		}
		
		public async UniTask ToScopeAsync(APP_SCOPE type)
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

			await _scope.InitializeAsync(_heart.destroyCancellationToken);
		}
	}
}