using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SLAM.Analytics;
using SLAM.Avatar;
using SLAM.BuildSystem;
using SLAM.Smartphone;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.FirstResponse
{
	public class FirstResponse : MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class _003CStart_003Ec__Iterator82 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal int _0024PC;

			internal object _0024current;

			internal FirstResponse _003C_003Ef__this;

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
				//Discarded unreachable code: IL_0060
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_003C_003Ef__this.checkForExistingProcess();
					SettingsView.InitializeSettings();
					UpdateSystem.HasLatestVersion(_003C_003Em__5C);
					_0024current = null;
					_0024PC = 1;
					return true;
				case 1u:
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

			internal void _003C_003Em__5C(bool hasLatest)
			{
				if (hasLatest)
				{
					if (SingletonMonobehaviour<Webservice>.Instance.HasAuthenticationToken())
					{
						UserProfile.GetCurrentProfileData(_003C_003Ef__this.gotUserProfile);
					}
					else
					{
						_003C_003Ef__this.gotUserProfile(null);
					}
				}
				else
				{
					UpdateSystem.UpdateToLatestVersion();
				}
			}
		}

		[CompilerGenerated]
		private static Action<AsyncOperation> _003C_003Ef__am_0024cache0;

		private IEnumerator Start()
		{
			checkForExistingProcess();
			SettingsView.InitializeSettings();
			UpdateSystem.HasLatestVersion(((_003CStart_003Ec__Iterator82)(object)this)._003C_003Em__5C);
			yield return null;
		}

		private void gotUserProfile(UserProfile profile)
		{
			if (profile == null)
			{
				SceneManager.Load("Login");
				return;
			}
			AvatarSystem.LoadPlayerConfiguration();
			if (_003C_003Ef__am_0024cache0 == null)
			{
				_003C_003Ef__am_0024cache0 = _003CgotUserProfile_003Em__5B;
			}
			SceneManager.Load("Hub", _003C_003Ef__am_0024cache0);
		}

		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		private void checkForExistingProcess()
		{
			Process currentProcess = Process.GetCurrentProcess();
			Process[] processesByName = Process.GetProcessesByName(currentProcess.ProcessName);
			if (processesByName.Length <= 1)
			{
				return;
			}
			foreach (Process process in processesByName)
			{
				if (process.Id != currentProcess.Id)
				{
					SetForegroundWindow(process.MainWindowHandle);
					Application.Quit();
				}
			}
		}

		private static void trackLoadComplete()
		{
			TrackingEvent trackingEvent = new TrackingEvent();
			trackingEvent.Type = TrackingEvent.TrackingType.LoadComplete;
			trackingEvent.Arguments = new Dictionary<string, object>
			{
				{ "UserLoggedIn", "1" },
				{
					"LoadingTime",
					Time.realtimeSinceStartup
				}
			};
			GameEvents.Invoke(trackingEvent);
		}

		[CompilerGenerated]
		private static void _003CgotUserProfile_003Em__5B(AsyncOperation op)
		{
			trackLoadComplete();
		}
	}
}
