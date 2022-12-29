using System;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.BuildSystem
{
	public class SceneDataLibrary : ScriptableObject
	{
		[Serializable]
		public class Version
		{
			public int Major;

			public int Minor;

			public int Build;

			public override string ToString()
			{
				return string.Format("{0}.{1}.{2}", Major, Minor, Build);
			}
		}

		[Serializable]
		public class LevelAssetVersion
		{
			public string LevelName;

			public int CRC;

			public int Version;

			[Popup(new string[] { "hub_loading_screen", "chapter1_loading_screen", "chapter2_loading_screen", "job_loading_screen", "none" })]
			public string LoadingScreenName;
		}

		[CompilerGenerated]
		private sealed class _003CGetVersionData_003Ec__AnonStorey17C
		{
			internal string levelName;

			internal bool _003C_003Em__C6(LevelAssetVersion l)
			{
				return l.LevelName == levelName;
			}
		}

		[SerializeField]
		public Version GameVersion;

		[SerializeField]
		private LevelAssetVersion[] levelAssets;

		public static SceneDataLibrary GetSceneDataLibrary()
		{
			return Resources.Load<SceneDataLibrary>("SceneDataLibrary");
		}

		public LevelAssetVersion GetVersionData(string levelName)
		{
			_003CGetVersionData_003Ec__AnonStorey17C _003CGetVersionData_003Ec__AnonStorey17C = new _003CGetVersionData_003Ec__AnonStorey17C();
			_003CGetVersionData_003Ec__AnonStorey17C.levelName = levelName;
			return levelAssets.FirstOrDefault(_003CGetVersionData_003Ec__AnonStorey17C._003C_003Em__C6);
		}
	}
}
