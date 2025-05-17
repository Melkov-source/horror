using System.Collections.Generic;
using _Game.Code.Scopes.Shared.Interfaces;
using UnityEngine;

namespace _Game.Code.Scopes.Shared
{
	public class MonoHeart : MonoBehaviour
	{
		private readonly List<IUpdatable> _updatables = new();
			
		public void Update()
		{
			for (int index = 0, count = _updatables.Count; index < count; ++index)
			{
				var updatable = _updatables[index];
					
				updatable.Update(Time.deltaTime);
			}
		}

		public void FixedUpdate()
		{
			
		}

		public void LateUpdate()
		{
			
		}

		public void RegisterUpdatable(IUpdatable updatable)
		{
			if (_updatables.Contains(updatable))
			{
				return;
			}
				
			_updatables.Add(updatable);
		}

		public void UnregisterUpdatable(IUpdatable updatable)
		{
			_updatables.Remove(updatable);
		}
	}
}