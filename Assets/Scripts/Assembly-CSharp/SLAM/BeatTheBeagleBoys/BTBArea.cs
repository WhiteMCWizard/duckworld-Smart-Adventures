using System;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.BeatTheBeagleBoys
{
	public class BTBArea : MonoBehaviour
	{
		public enum BTBAreaTheme
		{
			Bird = 0,
			Reptile = 1,
			Primate = 2
		}

		[Serializable]
		public class RoutePoints
		{
			public Transform entryPoint;

			public Transform stealPoint;

			public Transform animalPoint;

			public Transform exitPoint;
		}

		[SerializeField]
		private BTBCage cage;

		[SerializeField]
		private BTBAreaTheme theme;

		[SerializeField]
		private BTBCamera[] cameras;

		[SerializeField]
		private RoutePoints[] routes;

		[CompilerGenerated]
		private static Func<BTBCamera, bool> _003C_003Ef__am_0024cache7;

		[CompilerGenerated]
		private static Func<BTBCamera, int> _003C_003Ef__am_0024cache8;

		public int ThiefsEncountered { get; private set; }

		public BTBCage Cage
		{
			get
			{
				return cage;
			}
		}

		public BTBAreaTheme Theme
		{
			get
			{
				return theme;
			}
		}

		public RoutePoints[] Routes
		{
			get
			{
				return routes;
			}
		}

		public bool CanSteal
		{
			get
			{
				return cage != null && cage.IsOpened;
			}
		}

		public bool IsMonitored
		{
			get
			{
				BTBCamera[] collection = cameras;
				if (_003C_003Ef__am_0024cache7 == null)
				{
					_003C_003Ef__am_0024cache7 = _003Cget_IsMonitored_003Em__2C;
				}
				return collection.Count(_003C_003Ef__am_0024cache7) > 0;
			}
		}

		public BTBThief CurrentThief { get; protected set; }

		public BTBGuard CurrentGuard { get; protected set; }

		public bool HasThief
		{
			get
			{
				return CurrentThief != null;
			}
		}

		public bool HasGuard
		{
			get
			{
				return CurrentGuard != null;
			}
		}

		private void Start()
		{
			for (int i = 0; i < cameras.Length; i++)
			{
				cameras[i].enabled = false;
				cameras[i].CameraComp.enabled = false;
			}
		}

		public void OnThiefEntered(BTBThief thief)
		{
			ThiefsEncountered++;
			CurrentThief = thief;
		}

		public void OnThiefExited(BTBThief thief)
		{
			BTBGameController.ThiefExitedEvent thiefExitedEvent = new BTBGameController.ThiefExitedEvent();
			thiefExitedEvent.Thief = CurrentThief;
			thiefExitedEvent.Area = this;
			GameEvents.Invoke(thiefExitedEvent);
			CurrentThief = null;
			if (cage.IsHacked || cage.IsOpened)
			{
				cage.OnReset();
			}
		}

		public void OnGuardEntered(BTBGuard guard)
		{
			CurrentGuard = guard;
		}

		public void OnGuardExited(BTBGuard guard)
		{
			CurrentGuard = null;
		}

		public BTBCamera ActivateCamera()
		{
			BTBCamera[] source = cameras;
			if (_003C_003Ef__am_0024cache8 == null)
			{
				_003C_003Ef__am_0024cache8 = _003CActivateCamera_003Em__2D;
			}
			BTBCamera bTBCamera = source.OrderBy(_003C_003Ef__am_0024cache8).First();
			if (HasThief)
			{
				bTBCamera.ThiefsEncountered++;
			}
			bTBCamera.enabled = true;
			bTBCamera.CameraComp.enabled = true;
			return bTBCamera;
		}

		public bool OnInteract()
		{
			if (!HasGuard && HasThief && CurrentThief.IsRunningToCage && cage.IsOpened && cage.IsHacked)
			{
				cage.CloseCage();
				GameEvents.Invoke(new BTBGameController.CageClosedEvent());
				return true;
			}
			return false;
		}

		[CompilerGenerated]
		private static bool _003Cget_IsMonitored_003Em__2C(BTBCamera c)
		{
			return c.enabled;
		}

		[CompilerGenerated]
		private static int _003CActivateCamera_003Em__2D(BTBCamera c)
		{
			return c.ThiefsEncountered;
		}
	}
}
