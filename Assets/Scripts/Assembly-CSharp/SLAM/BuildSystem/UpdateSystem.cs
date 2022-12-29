using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.BuildSystem
{
	public static class UpdateSystem
	{
		[CompilerGenerated]
		private sealed class _003CHasLatestVersion_003Ec__AnonStorey17D
		{
			internal Action<bool> callback;

			private static Func<string, bool> _003C_003Ef__am_0024cache1;

			private static Func<string, bool> _003C_003Ef__am_0024cache2;

			internal void _003C_003Em__C9(WebConfiguration config)
			{
				if (config == null)
				{
					callback(true);
					return;
				}
				bool flag = Regex.IsMatch(SceneDataLibrary.GetSceneDataLibrary().GameVersion.ToString(), config.Version);
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				if (_003C_003Ef__am_0024cache1 == null)
				{
					_003C_003Ef__am_0024cache1 = _003C_003Em__CC;
				}
				bool flag2 = commandLineArgs.Any(_003C_003Ef__am_0024cache1);
				string[] commandLineArgs2 = Environment.GetCommandLineArgs();
				if (_003C_003Ef__am_0024cache2 == null)
				{
					_003C_003Ef__am_0024cache2 = _003C_003Em__CD;
				}
				bool flag3 = commandLineArgs2.Any(_003C_003Ef__am_0024cache2);
				bool flag4 = (!flag && !flag3) || flag2;
				Debug.LogFormat("Version check: local version: {0}, online version: {1}, versionIsMatched: {2}, forceUpdate: {3}, skipUpdate: {4}, shouldUpdate: {5}", SceneDataLibrary.GetSceneDataLibrary().GameVersion, config.Version, flag, flag2, flag3, flag4);
				if (callback != null)
				{
					callback(!flag4);
				}
			}

			private static bool _003C_003Em__CC(string cmd)
			{
				return cmd == "-force-update";
			}

			private static bool _003C_003Em__CD(string cmd)
			{
				return cmd == "-skip-update";
			}
		}

		[CompilerGenerated]
		private static Action<WebConfiguration> _003C_003Ef__am_0024cache0;

		[CompilerGenerated]
		private static Action _003C_003Ef__am_0024cache1;

		public static void HasLatestVersion(Action<bool> callback)
		{
			_003CHasLatestVersion_003Ec__AnonStorey17D _003CHasLatestVersion_003Ec__AnonStorey17D = new _003CHasLatestVersion_003Ec__AnonStorey17D();
			_003CHasLatestVersion_003Ec__AnonStorey17D.callback = callback;
			DataStorage.GetWebConfiguration(_003CHasLatestVersion_003Ec__AnonStorey17D._003C_003Em__C9, true);
		}

		public static void UpdateToLatestVersion()
		{
			if (_003C_003Ef__am_0024cache0 == null)
			{
				_003C_003Ef__am_0024cache0 = _003CUpdateToLatestVersion_003Em__CA;
			}
			DataStorage.GetWebConfiguration(_003C_003Ef__am_0024cache0);
		}

		[CompilerGenerated]
		private static void _003CUpdateToLatestVersion_003Em__CA(WebConfiguration config)
		{
			string title = Localization.Get("ERROR_UPDATE_TITLE");
			string message = string.Format(Localization.Get("ERROR_UPDATE_BODY"), SceneDataLibrary.GetSceneDataLibrary().GameVersion.ToString(), config.Version);
			string okButtonText = Localization.Get("UI_OK");
			if (_003C_003Ef__am_0024cache1 == null)
			{
				_003C_003Ef__am_0024cache1 = _003CUpdateToLatestVersion_003Em__CB;
			}
			GameEvents.Invoke(new PopupEvent(title, message, okButtonText, _003C_003Ef__am_0024cache1));
		}

		[CompilerGenerated]
		private static void _003CUpdateToLatestVersion_003Em__CB()
		{
			SceneManager.Load("UpdateGame");
		}
	}
}
