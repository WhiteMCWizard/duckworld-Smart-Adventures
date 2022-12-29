using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.BeatTheBeagleBoys
{
	public class BTBGameController : GameController
	{
		public class ThiefExitedEvent
		{
			public BTBThief Thief;

			public BTBArea Area;
		}

		public class AnimalStolenEvent
		{
			public BTBThief Thief;

			public BTBArea Area;
		}

		public class CageClosedEvent
		{
		}

		public class MonitorAlarmedEvent
		{
			public BTBMonitor monitor;
		}

		[CompilerGenerated]
		private sealed class _003CswitchMonitor_003Ec__AnonStorey15D
		{
			internal BTBMonitor monitor;

			internal bool _003C_003Em__35(BTBArea ar)
			{
				return !ar.IsMonitored && !ar.HasThief && ar != monitor.Area;
			}
		}

		private const int POINTS_HEARTS_LEFT = 100;

		private const int POINTS_CAGES_CLOSED = 25;

		[SerializeField]
		[HideInInspector]
		private BTBArea[] areas;

		[SerializeField]
		private BTBMonitor[] monitors;

		[SerializeField]
		private GameObject[] thiefPrefabs;

		[SerializeField]
		private BTBGuard guard;

		[SerializeField]
		private BTBDifficultySetting[] levels;

		[SerializeField]
		private Alarm gameTimer;

		private int heartCount = 3;

		private int cagesClosed;

		private List<BTBThief> activeThieves = new List<BTBThief>();

		private List<BTBGuard> activeGuards = new List<BTBGuard>();

		private float nextMonitorSwitchTime;

		[SerializeField]
		private int gizmoDifficulty;

		[SerializeField]
		private float gizmoWidth = 10f;

		[CompilerGenerated]
		private static Func<BTBArea, bool> _003C_003Ef__am_0024cacheD;

		[CompilerGenerated]
		private static Func<BTBArea, bool> _003C_003Ef__am_0024cacheE;

		[CompilerGenerated]
		private static Func<BTBMonitor, bool> _003C_003Ef__am_0024cacheF;

		[CompilerGenerated]
		private static Func<BTBMonitor, int> _003C_003Ef__am_0024cache10;

		[CompilerGenerated]
		private static Func<BTBMonitor, bool> _003C_003Ef__am_0024cache11;

		[CompilerGenerated]
		private static Func<BTBMonitor, bool> _003C_003Ef__am_0024cache12;

		[CompilerGenerated]
		private static Func<BTBArea, bool> _003C_003Ef__am_0024cache13;

		[CompilerGenerated]
		private static Func<BTBArea, int> _003C_003Ef__am_0024cache14;

		[CompilerGenerated]
		private static Func<BTBArea, bool> _003C_003Ef__am_0024cache15;

		[CompilerGenerated]
		private static Func<BTBArea, bool> _003C_003Ef__am_0024cache16;

		public override int GameId
		{
			get
			{
				return 16;
			}
		}

		public override Dictionary<string, int> ScoreCategories
		{
			get
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				dictionary.Add(StringFormatter.GetLocalizationFormatted("BTB_VICTORYWINDOW_SCORE_HEARTS_LEFT", heartCount, 100), heartCount * 100);
				dictionary.Add(StringFormatter.GetLocalizationFormatted("BTB_VICTORYWINDOW_CAGES_CLOSED", cagesClosed, 25), cagesClosed * 25);
				return dictionary;
			}
		}

		public override string IntroNPCKey
		{
			get
			{
				return "NPC_NAME_DONALD";
			}
		}

		public override string IntroTextKey
		{
			get
			{
				return "BTB_CINEMATICINTRO_TEXT";
			}
		}

		public BTBDifficultySetting CurrentDifficultySetting
		{
			get
			{
				return SelectedLevel<BTBDifficultySetting>();
			}
		}

		public override LevelSetting[] Levels
		{
			get
			{
				return levels;
			}
		}

		public override Portrait DuckCharacter
		{
			get
			{
				return Portrait.DonaldDuck;
			}
		}

		protected override void Start()
		{
			base.Start();
			disableControls();
		}

		public override void Play(LevelSetting selectedLevel)
		{
			base.Play(selectedLevel);
			enableControls();
			for (int i = 0; i < monitors.Length; i++)
			{
				BTBArea[] collection = areas;
				if (_003C_003Ef__am_0024cacheD == null)
				{
					_003C_003Ef__am_0024cacheD = _003CPlay_003Em__2E;
				}
				BTBArea area = collection.FirstOrDefault(_003C_003Ef__am_0024cacheD);
				monitors[i].DisplayArea(area);
			}
			gameTimer = Alarm.Create();
			gameTimer.StartCountUp(CurrentDifficultySetting.LevelDuration, onGameTimeUp);
			CurrentDifficultySetting.SetGameTimer(gameTimer);
			GetView<BTBHudView>().SetInfo(gameTimer);
			OpenView<HeartsView>().SetTotalHeartCount(heartCount);
		}

		private void OnEnable()
		{
			GameEvents.Subscribe<AnimalStolenEvent>(onAnimalStolen);
			GameEvents.Subscribe<CageClosedEvent>(onCageClosed);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<AnimalStolenEvent>(onAnimalStolen);
			GameEvents.Unsubscribe<CageClosedEvent>(onCageClosed);
		}

		private void OnDrawGizmos()
		{
			BTBDifficultySetting obj = levels[gizmoDifficulty];
			AnimationCurve[] array = new AnimationCurve[2]
			{
				GetPrivateField<AnimationCurve>(obj, "thiefActiveCount"),
				GetPrivateField<AnimationCurve>(obj, "thiefIdleTime")
			};
			Color[] array2 = new Color[5]
			{
				Color.red,
				Color.green,
				Color.blue,
				Color.magenta,
				Color.yellow
			};
			for (int i = 0; i < array.Length; i++)
			{
				AnimationCurve animationCurve = array[i];
				for (int j = 1; j < animationCurve.keys.Length; j++)
				{
					Gizmos.color = array2[i];
					Gizmos.DrawLine(base.transform.position + Vector3.right * animationCurve.keys[j - 1].time * gizmoWidth + Vector3.up * animationCurve.keys[j - 1].value, base.transform.position + Vector3.right * animationCurve.keys[j].time * gizmoWidth + Vector3.up * animationCurve.keys[j].value);
				}
			}
		}

		private static T GetPrivateField<T>(object obj, string fieldname)
		{
			return (T)obj.GetType().GetField(fieldname, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj);
		}

		public override void Pause()
		{
			base.Pause();
			disableControls();
		}

		public override void Resume()
		{
			base.Resume();
			enableControls();
		}

		private void onGameTimeUp()
		{
			Finish(heartCount > 0);
		}

		protected override void WhileStateRunning()
		{
			base.WhileStateRunning();
			updateThiefs();
			updateMonitors();
			handleInput();
		}

		public override void Finish(bool succes)
		{
			BTBArea[] array = areas;
			foreach (BTBArea bTBArea in array)
			{
				if (bTBArea.Cage.IsOpened)
				{
					bTBArea.Cage.OnReset();
				}
			}
			for (int j = 0; j < activeThieves.Count; j++)
			{
				activeThieves[j].stopAI();
			}
			disableControls();
			gameTimer.Pause();
			base.Finish(succes);
		}

		private void onAnimalStolen(AnimalStolenEvent evt)
		{
			if (heartCount > 0)
			{
				GetView<HeartsView>().LoseHeart();
			}
			heartCount--;
			if (heartCount <= 0)
			{
				CoroutineUtils.WaitForObjectDestroyed(evt.Thief.gameObject, _003ConAnimalStolen_003Em__2F);
			}
		}

		private void onCageClosed(CageClosedEvent obj)
		{
			cagesClosed++;
		}

		private void updateMonitors()
		{
			BTBArea[] collection = areas;
			if (_003C_003Ef__am_0024cacheE == null)
			{
				_003C_003Ef__am_0024cacheE = _003CupdateMonitors_003Em__30;
			}
			IEnumerable<BTBArea> enumerable = collection.Where(_003C_003Ef__am_0024cacheE);
			BTBMonitor[] collection2 = monitors;
			if (_003C_003Ef__am_0024cacheF == null)
			{
				_003C_003Ef__am_0024cacheF = _003CupdateMonitors_003Em__31;
			}
			IEnumerable<BTBMonitor> source = collection2.Where(_003C_003Ef__am_0024cacheF);
			if (_003C_003Ef__am_0024cache10 == null)
			{
				_003C_003Ef__am_0024cache10 = _003CupdateMonitors_003Em__32;
			}
			Enumerable.IOrderedEnumerable<BTBMonitor> collection3 = source.OrderBy(_003C_003Ef__am_0024cache10);
			if (enumerable != null && monitors == null)
			{
				Debug.LogWarning("Hey budddy, there are not enought monitors to display all thiefs!");
			}
			foreach (BTBArea item in enumerable)
			{
				if (_003C_003Ef__am_0024cache11 == null)
				{
					_003C_003Ef__am_0024cache11 = _003CupdateMonitors_003Em__33;
				}
				collection3.Where(_003C_003Ef__am_0024cache11).GetRandom().DisplayArea(item);
			}
			if (Time.time > nextMonitorSwitchTime)
			{
				nextMonitorSwitchTime = Time.time + CurrentDifficultySetting.GetMonitorVisibleTime();
				switchMonitor();
			}
		}

		private void handleInput()
		{
			if (!Input.GetMouseButtonDown(0))
			{
				return;
			}
			RaycastHit hitInfo = default(RaycastHit);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.HasComponent<BTBMonitor>())
			{
				BTBMonitor component = hitInfo.collider.GetComponent<BTBMonitor>();
				if (component.Area != null && !component.AreControlsLocked && component.Area.OnInteract())
				{
					AudioController.Play("BTB_button_press");
					AudioController.Play("cage_close", component.transform);
				}
			}
		}

		private void disableControls()
		{
			BTBMonitor[] array = monitors;
			foreach (BTBMonitor bTBMonitor in array)
			{
				bTBMonitor.AreControlsLocked = true;
			}
		}

		private void enableControls()
		{
			BTBMonitor[] array = monitors;
			foreach (BTBMonitor bTBMonitor in array)
			{
				bTBMonitor.AreControlsLocked = false;
			}
		}

		private void switchMonitor()
		{
			_003CswitchMonitor_003Ec__AnonStorey15D _003CswitchMonitor_003Ec__AnonStorey15D = new _003CswitchMonitor_003Ec__AnonStorey15D();
			BTBMonitor[] collection = monitors;
			if (_003C_003Ef__am_0024cache12 == null)
			{
				_003C_003Ef__am_0024cache12 = _003CswitchMonitor_003Em__34;
			}
			_003CswitchMonitor_003Ec__AnonStorey15D.monitor = collection.Where(_003C_003Ef__am_0024cache12).GetRandom();
			if (_003CswitchMonitor_003Ec__AnonStorey15D.monitor != null)
			{
				BTBArea random = areas.Where(_003CswitchMonitor_003Ec__AnonStorey15D._003C_003Em__35).GetRandom();
				_003CswitchMonitor_003Ec__AnonStorey15D.monitor.DisplayArea(random);
			}
		}

		private void updateThiefs()
		{
			for (int i = 0; i < activeThieves.Count; i++)
			{
				if (!activeThieves[i].enabled)
				{
					UnityEngine.Object.Destroy(activeThieves[i].gameObject);
					activeThieves.RemoveAt(i);
					i--;
				}
			}
			if (activeThieves.Count >= CurrentDifficultySetting.GetThiefActiveCount())
			{
				return;
			}
			BTBArea[] collection = areas;
			if (_003C_003Ef__am_0024cache13 == null)
			{
				_003C_003Ef__am_0024cache13 = _003CupdateThiefs_003Em__36;
			}
			BTBArea bTBArea = collection.Where(_003C_003Ef__am_0024cache13).GetRandom();
			if (bTBArea == null)
			{
				BTBArea[] source = areas;
				if (_003C_003Ef__am_0024cache14 == null)
				{
					_003C_003Ef__am_0024cache14 = _003CupdateThiefs_003Em__37;
				}
				Enumerable.IOrderedEnumerable<BTBArea> collection2 = source.OrderBy(_003C_003Ef__am_0024cache14);
				if (_003C_003Ef__am_0024cache15 == null)
				{
					_003C_003Ef__am_0024cache15 = _003CupdateThiefs_003Em__38;
				}
				bTBArea = collection2.FirstOrDefault(_003C_003Ef__am_0024cache15);
			}
			if (bTBArea == null)
			{
				Debug.LogError("Couldn't spawn a thief because all areas are full!");
				return;
			}
			BTBThief componentInChildren = UnityEngine.Object.Instantiate(thiefPrefabs.GetRandom()).GetComponentInChildren<BTBThief>();
			componentInChildren.name += Time.time;
			bTBArea.OnThiefEntered(componentInChildren);
			componentInChildren.Initialize(bTBArea, CurrentDifficultySetting);
			addObjectToLayer(componentInChildren.gameObject, "Avatar", true);
			activeThieves.Add(componentInChildren);
		}

		private void addObjectToLayer(GameObject go, string layerName, bool recursivly)
		{
			go.layer = LayerMask.NameToLayer(layerName);
			if (!recursivly)
			{
				return;
			}
			foreach (Transform item in go.transform)
			{
				item.gameObject.layer = LayerMask.NameToLayer(layerName);
			}
		}

		private void updateGuard()
		{
			for (int i = 0; i < activeGuards.Count; i++)
			{
				if (!activeGuards[i].enabled)
				{
					UnityEngine.Object.Destroy(activeGuards[i].gameObject);
					activeGuards.RemoveAt(i);
					i--;
				}
			}
			if (activeGuards.Count == 0)
			{
				BTBArea[] collection = areas;
				if (_003C_003Ef__am_0024cache16 == null)
				{
					_003C_003Ef__am_0024cache16 = _003CupdateGuard_003Em__39;
				}
				BTBArea random = collection.Where(_003C_003Ef__am_0024cache16).GetRandom();
				if (random == null)
				{
					Debug.LogError("Couldnt spawn a guard because all areas are full!");
					return;
				}
				BTBGuard componentInChildren = UnityEngine.Object.Instantiate(guard.gameObject).GetComponentInChildren<BTBGuard>();
				componentInChildren.name += Time.time;
				random.OnGuardEntered(componentInChildren);
				componentInChildren.Initialize(random, CurrentDifficultySetting);
				addObjectToLayer(componentInChildren.gameObject, "Avatar", true);
				activeGuards.Add(componentInChildren);
			}
		}

		[CompilerGenerated]
		private static bool _003CPlay_003Em__2E(BTBArea a)
		{
			return !a.IsMonitored;
		}

		[CompilerGenerated]
		private void _003ConAnimalStolen_003Em__2F()
		{
			Finish(false);
		}

		[CompilerGenerated]
		private static bool _003CupdateMonitors_003Em__30(BTBArea ar)
		{
			return !ar.IsMonitored && ar.HasThief;
		}

		[CompilerGenerated]
		private static bool _003CupdateMonitors_003Em__31(BTBMonitor m)
		{
			return !m.Area.HasThief;
		}

		[CompilerGenerated]
		private static int _003CupdateMonitors_003Em__32(BTBMonitor m)
		{
			return m.UsedCount;
		}

		[CompilerGenerated]
		private static bool _003CupdateMonitors_003Em__33(BTBMonitor m)
		{
			return m.Area != null;
		}

		[CompilerGenerated]
		private static bool _003CswitchMonitor_003Em__34(BTBMonitor m)
		{
			return !m.Area.HasThief;
		}

		[CompilerGenerated]
		private static bool _003CupdateThiefs_003Em__36(BTBArea ar)
		{
			return !ar.Cage.IsHacked && ar.Cage.IsOpened && !ar.HasGuard && !ar.HasThief;
		}

		[CompilerGenerated]
		private static int _003CupdateThiefs_003Em__37(BTBArea ar)
		{
			return ar.ThiefsEncountered;
		}

		[CompilerGenerated]
		private static bool _003CupdateThiefs_003Em__38(BTBArea ar)
		{
			return !ar.HasGuard && !ar.HasThief && !ar.IsMonitored;
		}

		[CompilerGenerated]
		private static bool _003CupdateGuard_003Em__39(BTBArea ar)
		{
			return !ar.HasGuard && !ar.HasThief;
		}
	}
}
