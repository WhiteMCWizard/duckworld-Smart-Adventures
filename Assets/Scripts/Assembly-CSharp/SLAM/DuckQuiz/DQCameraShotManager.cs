using System;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.DuckQuiz
{
	public class DQCameraShotManager : MonoBehaviour
	{
		[Serializable]
		public class CameraShotSetting
		{
			[SerializeField]
			[Popup(new string[] { "Show_Category", "Show_Question", "Show_Answer", "Question_Correct", "Question_Incorrect", "Show_Bonusround" })]
			private string stateName;

			[SerializeField]
			private Camera[] cameras;

			public string StateName
			{
				get
				{
					return stateName;
				}
			}

			public Camera[] Cameras
			{
				get
				{
					return cameras;
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CSwitchToCameraShot_003Ec__AnonStorey166
		{
			internal string stateName;

			internal bool _003C_003Em__51(CameraShotSetting s)
			{
				return s.StateName == stateName;
			}
		}

		[SerializeField]
		private CameraShotSetting[] settings;

		private Camera oldCamera;

		private float lastSwitchTime;

		[CompilerGenerated]
		private static Func<Camera, bool> _003C_003Ef__am_0024cache3;

		public float TimeSinceLastCameraSwitch
		{
			get
			{
				return Time.timeSinceLevelLoad - lastSwitchTime;
			}
		}

		private void OnEnable()
		{
			for (int i = 0; i < settings.Length; i++)
			{
				for (int j = 0; j < settings[i].Cameras.Length; j++)
				{
					settings[i].Cameras[j].gameObject.SetActive(false);
				}
			}
		}

		public void SwitchToCameraShot(string stateName)
		{
			_003CSwitchToCameraShot_003Ec__AnonStorey166 _003CSwitchToCameraShot_003Ec__AnonStorey = new _003CSwitchToCameraShot_003Ec__AnonStorey166();
			_003CSwitchToCameraShot_003Ec__AnonStorey.stateName = stateName;
			CameraShotSetting cameraShotSetting = settings.FirstOrDefault(_003CSwitchToCameraShot_003Ec__AnonStorey._003C_003Em__51);
			if (cameraShotSetting == null || cameraShotSetting.Cameras.Length <= 0)
			{
				return;
			}
			Camera[] cameras = cameraShotSetting.Cameras;
			if (_003C_003Ef__am_0024cache3 == null)
			{
				_003C_003Ef__am_0024cache3 = _003CSwitchToCameraShot_003Em__52;
			}
			Camera random = cameras.Where(_003C_003Ef__am_0024cache3).GetRandom();
			if (!(random == null))
			{
				if (oldCamera != null)
				{
					oldCamera.gameObject.SetActive(false);
				}
				random.gameObject.SetActive(true);
				oldCamera = random;
				lastSwitchTime = Time.timeSinceLevelLoad;
			}
		}

		[CompilerGenerated]
		private static bool _003CSwitchToCameraShot_003Em__52(Camera c)
		{
			return !c.gameObject.activeInHierarchy;
		}
	}
}
