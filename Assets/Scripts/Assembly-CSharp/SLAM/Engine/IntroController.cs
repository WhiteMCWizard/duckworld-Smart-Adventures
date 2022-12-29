using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Shared;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Engine
{
	public class IntroController : ViewController
	{
		[SerializeField]
		private BalloonType balloonType;

		[SerializeField]
		private GameObject balloonTarget;

		[SerializeField]
		private Camera balloonCamera;

		private SpeechBalloon balloon;

		private IEnumerable<Light> sceneLights;

		private IEnumerable<Camera> sceneCameras;

		[CompilerGenerated]
		private static Func<Light, bool> _003C_003Ef__am_0024cache6;

		[CompilerGenerated]
		private static Func<Camera, bool> _003C_003Ef__am_0024cache7;

		public void Enable(BalloonView v, string introText)
		{
			base.gameObject.SetActive(true);
			Light[] collection = UnityEngine.Object.FindObjectsOfType<Light>();
			if (_003C_003Ef__am_0024cache6 == null)
			{
				_003C_003Ef__am_0024cache6 = _003CEnable_003Em__F7;
			}
			sceneLights = collection.Where(_003C_003Ef__am_0024cache6);
			Camera[] collection2 = UnityEngine.Object.FindObjectsOfType<Camera>();
			if (_003C_003Ef__am_0024cache7 == null)
			{
				_003C_003Ef__am_0024cache7 = _003CEnable_003Em__F8;
			}
			sceneCameras = collection2.Where(_003C_003Ef__am_0024cache7);
			foreach (Light sceneLight in sceneLights)
			{
				sceneLight.enabled = (sceneLight.transform.IsChildOf(base.transform) ? true : false);
			}
			foreach (Camera sceneCamera in sceneCameras)
			{
				sceneCamera.enabled = (sceneCamera.transform.IsChildOf(base.transform) ? true : false);
			}
			balloon = v.CreateBalloon(balloonType);
			balloon.SetInfo(introText, balloonTarget, false, balloonCamera);
		}

		public void Disable()
		{
			UnityEngine.Object.Destroy(balloon);
			foreach (Light sceneLight in sceneLights)
			{
				sceneLight.enabled = ((!sceneLight.transform.IsChildOf(base.transform)) ? true : false);
			}
			foreach (Camera sceneCamera in sceneCameras)
			{
				sceneCamera.enabled = ((!sceneCamera.transform.IsChildOf(base.transform)) ? true : false);
			}
			base.gameObject.SetActive(false);
		}

		[CompilerGenerated]
		private static bool _003CEnable_003Em__F7(Light l)
		{
			return l != null && l.gameObject.layer != LayerMask.NameToLayer("GUI") && l.gameObject.layer != LayerMask.NameToLayer("PhotoBooth");
		}

		[CompilerGenerated]
		private static bool _003CEnable_003Em__F8(Camera c)
		{
			return c != null && c.gameObject.layer != LayerMask.NameToLayer("GUI") && c.gameObject.layer != LayerMask.NameToLayer("ErrorUI") && c.gameObject.layer != LayerMask.NameToLayer("PhotoBooth");
		}
	}
}
