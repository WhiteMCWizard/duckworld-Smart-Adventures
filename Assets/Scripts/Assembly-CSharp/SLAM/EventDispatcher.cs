using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SLAM
{
	public class EventDispatcher
	{
		[CompilerGenerated]
		private sealed class _003CSubscribe_003Ec__AnonStorey18B<T> where T : class
		{
			internal Action<T> callback;

			internal void _003C_003Em__E9(object o)
			{
				callback((T)o);
			}
		}

		private Dictionary<Type, Action<object>> events = new Dictionary<Type, Action<object>>();

		private Dictionary<object, Action<object>> lookup = new Dictionary<object, Action<object>>();

		public void Subscribe<T>(Action<T> callback) where T : class
		{
			_003CSubscribe_003Ec__AnonStorey18B<T> _003CSubscribe_003Ec__AnonStorey18B = new _003CSubscribe_003Ec__AnonStorey18B<T>();
			_003CSubscribe_003Ec__AnonStorey18B.callback = callback;
			Action<object> action = _003CSubscribe_003Ec__AnonStorey18B._003C_003Em__E9;
			if (events.ContainsKey(typeof(T)))
			{
				events[typeof(T)] = (Action<object>)Delegate.Combine(events[typeof(T)], action);
			}
			else
			{
				events.Add(typeof(T), action);
			}
			lookup.Add(_003CSubscribe_003Ec__AnonStorey18B.callback, action);
		}

		public void Unsubscribe<T>(Action<T> callback) where T : class
		{
			Dictionary<Type, Action<object>> dictionary;
			Dictionary<Type, Action<object>> dictionary2 = (dictionary = events);
			Type typeFromHandle;
			Type key = (typeFromHandle = typeof(T));
			Action<object> source = dictionary[typeFromHandle];
			dictionary2[key] = (Action<object>)Delegate.Remove(source, lookup[callback]);
			lookup.Remove(callback);
		}

		public void Invoke<T>(T evt) where T : class
		{
			Action<object> value;
			if (events.TryGetValue(typeof(T), out value) && value != null)
			{
				value(evt);
			}
		}
	}
}
