using System;
using System.Collections.Generic;

namespace Code.Utils.Reactive
{
	public class BehaviorSubject<TValue>
	{
		public TValue value { get; private set; }

		private readonly List<Action<TValue>> _callbacks = new();

		public BehaviorSubject(TValue value = default)
		{
			this.value = value;
		}

		public void Next(TValue new_value)
		{
			value = new_value;

			for (int index = 0, count = _callbacks.Count; index < count; ++index)
			{
				var callback = _callbacks[index];

				callback(new_value);
			}
		}

		public void On(Action<TValue> action, bool invoke = false)
		{
			_callbacks.Add(action);

			if (invoke)
			{
				action(value);
			}
		}

		public void Off(Action<TValue> action)
		{
			_callbacks.Remove(action);
		}
	}
}