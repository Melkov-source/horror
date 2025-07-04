﻿using System.Threading;
using _Game.Code.Scopes.Shared.Interfaces;
using Code.Core.Chapters;
using Code.DI;
using Code.PanelManager;
using Code.Shared;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Core
{
	[UsedImplicitly]
	public class CoreScope : IScope
	{
		private readonly DIContainer _container;
		private IChapter _chapter;
		
		public CoreScope(DIContainer container)
		{
			_container = container;
		}
		
		public async UniTask InitializeAsync(CancellationToken token)
		{
			Debug.Log("CoreScope Initialize");

			//var chapter = new RoadChapter(_panel_manager);
			var chapter = new SomeChapter(_container);
			
			await chapter.InitializeAsync(token);
		}

		public async UniTask DisposeAsync(CancellationToken token)
		{
			throw new System.NotImplementedException();
		}
	}
}