using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Engine
{
	public class ViewController : MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class _003COpenAndWait_003Ec__IteratorF<T> : IEnumerator, IDisposable, IEnumerator<object> where T : View
		{
			internal bool _003CisDoneOpening_003E__0;

			internal int _0024PC;

			internal object _0024current;

			internal ViewController _003C_003Ef__this;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return _0024current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return _0024current;
				}
			}

			public bool MoveNext()
			{
				//Discarded unreachable code: IL_006e
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_003CisDoneOpening_003E__0 = false;
					_003C_003Ef__this.OpenView<T>(_003C_003Em__C);
					goto case 1u;
				case 1u:
					if (!_003CisDoneOpening_003E__0)
					{
						_0024current = null;
						_0024PC = 1;
						return true;
					}
					_0024PC = -1;
					break;
				}
				return false;
			}

			[DebuggerHidden]
			public void Dispose()
			{
				_0024PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			internal void _003C_003Em__C(View v)
			{
				_003CisDoneOpening_003E__0 = true;
			}
		}

		[CompilerGenerated]
		private sealed class _003CCloseAndWait_003Ec__Iterator10<T> : IEnumerator, IDisposable, IEnumerator<object> where T : View
		{
			internal bool _003CisDoneClosing_003E__0;

			internal int _0024PC;

			internal object _0024current;

			internal ViewController _003C_003Ef__this;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return _0024current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return _0024current;
				}
			}

			public bool MoveNext()
			{
				//Discarded unreachable code: IL_006d
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_003CisDoneClosing_003E__0 = false;
					_003C_003Ef__this.CloseView<T>(_003C_003Em__D);
					goto case 1u;
				case 1u:
					if (!_003CisDoneClosing_003E__0)
					{
						_0024current = null;
						_0024PC = 1;
						return true;
					}
					_0024PC = -1;
					break;
				}
				return false;
			}

			[DebuggerHidden]
			public void Dispose()
			{
				_0024PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			internal void _003C_003Em__D(View v)
			{
				_003CisDoneClosing_003E__0 = true;
			}
		}

		private List<View> allViews = new List<View>();

		protected virtual void Start()
		{
		}

		protected void AddView(View view)
		{
			if (view == null)
			{
				UnityEngine.Debug.LogError("Cannot add view as it is null.");
			}
			else if (!allViews.Contains(view))
			{
				allViews.Add(view);
				view.Init(this);
				view.Close(true);
			}
			else
			{
				UnityEngine.Debug.LogError("Cannot add view: \"" + view.GetType().Name + "\" as it has already been added.", view);
			}
		}

		protected void AddViews(params View[] newViews)
		{
			for (int i = 0; i < newViews.Length; i++)
			{
				AddView(newViews[i]);
			}
		}

		protected void OpenView(View view)
		{
			OpenView(view, null);
		}

		protected virtual void OpenView(View view, View.Callback callback)
		{
			if (allViews.Contains(view))
			{
				if (!view.IsOpen)
				{
					view.Open(callback);
				}
				else
				{
					UnityEngine.Debug.LogError("View " + view.GetType().Name + " is already open.", view);
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Can't open view since its unknown to me." + view, this);
			}
		}

		protected T OpenView<T>(View.Callback callback) where T : View
		{
			View view = GetView<T>();
			OpenView(view, callback);
			return view as T;
		}

		protected T OpenView<T>() where T : View
		{
			return OpenView<T>(null);
		}

		protected void CloseView(View view)
		{
			CloseView(view, null);
		}

		protected virtual void CloseView(View view, View.Callback callback)
		{
			if (allViews.Contains(view))
			{
				if (view.IsOpen)
				{
					view.Close(callback);
				}
				else
				{
					UnityEngine.Debug.LogError("View " + view.GetType().Name + " is already closed.", view);
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Can't close view since its unknown to me." + view, this);
			}
		}

		protected void CloseView<T>(View.Callback callback) where T : View
		{
			View view = GetView<T>();
			if (!(view == null))
			{
				if (!view.IsOpen)
				{
					UnityEngine.Debug.LogError("Cannot close view since it's not open. " + view.GetType().Name, this);
				}
				else
				{
					CloseView(view, callback);
				}
			}
		}

		protected void CloseView<T>() where T : View
		{
			CloseView<T>(null);
		}

		protected void CloseAllViews()
		{
			foreach (View allView in allViews)
			{
				if (allView.IsOpen)
				{
					CloseView(allView);
				}
			}
		}

		protected void CloseAllViews(params Type[] excluded)
		{
			foreach (View allView in allViews)
			{
				if (!excluded.Contains(allView.GetType()) && allView.IsOpen)
				{
					CloseView(allView);
				}
			}
		}

		protected bool IsViewOpen<T>() where T : View
		{
			View view = GetView<T>();
			if (view != null)
			{
				return view.IsOpen;
			}
			return false;
		}

		protected T GetView<T>() where T : View
		{
			for (int i = 0; i < allViews.Count; i++)
			{
				if (allViews[i] is T)
				{
					return allViews[i] as T;
				}
			}
			UnityEngine.Debug.LogError("Can't find view: " + typeof(T).Name, this);
			return (T)null;
		}

		protected bool HasView<T>() where T : View
		{
			return allViews.Any(_003CHasView_00601_003Em__B<T>);
		}

		protected virtual IEnumerator OpenAndWait<T>() where T : View
		{
			bool isDoneOpening = false;
			OpenView<T>(((_003COpenAndWait_003Ec__IteratorF<T>)(object)this)._003C_003Em__C);
			while (!isDoneOpening)
			{
				yield return null;
			}
		}

		protected virtual IEnumerator CloseAndWait<T>() where T : View
		{
			bool isDoneClosing = false;
			CloseView<T>(((_003CCloseAndWait_003Ec__Iterator10<T>)(object)this)._003C_003Em__D);
			while (!isDoneClosing)
			{
				yield return null;
			}
		}

		[CompilerGenerated]
		private static bool _003CHasView_00601_003Em__B<T>(View view) where T : View
		{
			return view is T;
		}
	}
}
