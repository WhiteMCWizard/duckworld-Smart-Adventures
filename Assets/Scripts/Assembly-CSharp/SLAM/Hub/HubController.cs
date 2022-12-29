using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Analytics;
using SLAM.Avatar;
using SLAM.BuildSystem;
using SLAM.Engine;
using SLAM.MotionComics._3D;
using SLAM.Slinq;
using SLAM.Smartphone;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Hub
{
	public class HubController : ViewController
	{
		[CompilerGenerated]
		private sealed class _003CStart_003Ec__IteratorA3 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal int _0024PC;

			internal object _0024current;

			internal HubController _003C_003Ef__this;

			public static Func<HubLocationProvider, bool> _003C_003Ef__am_0024cache3;

			private static Func<HubLocationProvider, bool> _003C_003Ef__am_0024cache4;

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
				//Discarded unreachable code: IL_0276
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					if (lastSelectedLocationId <= 0)
					{
						AudioController.PlayMusic(_003C_003Ef__this.musicClipIntro.name);
						_003C_003Ef__this.hubCameraManager.PlayIntroAnimation();
						if (_003C_003Ef__this.introAmbientClip != null)
						{
							_003C_003Ef__this.playingAmbientObject = AudioController.Play(_003C_003Ef__this.introAmbientClip.name);
							_003C_003Ef__this.playingAmbientObject.FadeOut(8.133f);
							_003C_003Ef__this.playingAmbientObject.SwitchAudioSources();
						}
						_003C_003Ef__this.playingAmbientObject.PlayNow(_003C_003Ef__this.defaultAmbientClip.name, 0f, 1f, 0f);
						_003C_003Ef__this.playingAmbientObject.FadeIn(8.133f);
					}
					else
					{
						AudioController.PlayMusic(_003C_003Ef__this.musicClip.name);
						HubController hubController = _003C_003Ef__this;
						HubLocationProvider[] collection = UnityEngine.Object.FindObjectsOfType<HubLocationProvider>();
						if (_003C_003Ef__am_0024cache3 == null)
						{
							_003C_003Ef__am_0024cache3 = _003C_003Em__84;
						}
						hubController.selectedLocation = collection.FirstOrDefault(_003C_003Ef__am_0024cache3);
						_003C_003Ef__this.hubCameraManager.WarpToLocation(_003C_003Ef__this.selectedLocation);
						_003C_003Ef__this.playingAmbientObject = AudioController.Play(_003C_003Ef__this.selectedLocation.AmbientLoop.name);
					}
					_0024current = _003C_003Ef__this.StartCoroutine(_003C_003Ef__this.waitForDataLoadAndIntroAnimationToContinue());
					_0024PC = 1;
					return true;
				case 1u:
					if (AvatarSystem.GetPlayerConfiguration() == null)
					{
						_003C_003Ef__this.OpenView<HubFirstPlayView>();
					}
					else if (lastSelectedLocationId > 0)
					{
						_003C_003Ef__this.OpenView<HubLocationView>().UpdateInfo(Location.GetById(_003C_003Ef__this.selectedLocation.LocationId, _003C_003Ef__this.hubData), _003C_003Ef__this.selectedLocation);
					}
					else
					{
						_003C_003Ef__this.OpenView<HubHudView>();
					}
					if (pendingLocationId > -1)
					{
						_003C_003Ef__this.DeselectLocation(_003C_003Em__85);
					}
					_003C_003Ef__this.smartphone2DRoot.SetActive(!_003C_003Ef__this.IsViewOpen<HubFirstPlayView>() && !playedGameIds.Contains(-1));
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

			public static bool _003C_003Em__84(HubLocationProvider hlp)
			{
				return hlp.LocationId == lastSelectedLocationId;
			}

			internal void _003C_003Em__85()
			{
				HubController hubController = _003C_003Ef__this;
				HubLocationProvider[] collection = UnityEngine.Object.FindObjectsOfType<HubLocationProvider>();
				if (_003C_003Ef__am_0024cache4 == null)
				{
					_003C_003Ef__am_0024cache4 = _003C_003Em__86;
				}
				hubController.SelectLocation(collection.FirstOrDefault(_003C_003Ef__am_0024cache4));
				pendingLocationId = -1;
			}

			private static bool _003C_003Em__86(HubLocationProvider hlp)
			{
				return hlp.LocationId == pendingLocationId;
			}
		}

		[CompilerGenerated]
		private sealed class _003CwaitForDataLoadAndIntroAnimationToContinue_003Ec__IteratorA4 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal int _0024PC;

			internal object _0024current;

			internal HubController _003C_003Ef__this;

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
				//Discarded unreachable code: IL_00ac
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_003C_003Ef__this.smartphone2DRoot.SetActive(false);
					AvatarSystem.LoadPlayerConfiguration();
					DataStorage.GetLocationsData(_003C_003Em__87);
					goto case 1u;
				case 1u:
					if (!_003C_003Ef__this.hasDataBeenLoadedToBegin())
					{
						_0024current = null;
						_0024PC = 1;
						break;
					}
					goto case 2u;
				case 2u:
					if (_003C_003Ef__this.hubCameraManager.IsPlayingIntroAnimation)
					{
						_0024current = null;
						_0024PC = 2;
						break;
					}
					_0024PC = -1;
					goto default;
				default:
					return false;
				}
				return true;
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

			internal void _003C_003Em__87(Location[] locations)
			{
				_003C_003Ef__this.hubData = locations;
			}
		}

		[CompilerGenerated]
		private sealed class _003COpenPremiumGameLockedWindow_003Ec__AnonStorey170
		{
			internal int gameId;

			internal HubController _003C_003Ef__this;

			internal void _003C_003Em__7E()
			{
				ApiClient.OpenPropositionPage(gameId);
			}

			internal void _003C_003Em__7F()
			{
				_003C_003Ef__this.OpenView<TipParentView>();
			}

			internal void _003C_003Em__80()
			{
				ApiClient.OpenPropositionPage(gameId);
			}

			internal void _003C_003Em__81()
			{
				_003C_003Ef__this.OpenView<TipParentView>();
			}
		}

		[CompilerGenerated]
		private sealed class _003CDeselectLocation_003Ec__AnonStorey171
		{
			internal Action callback;

			internal HubController _003C_003Ef__this;

			internal void _003C_003Em__83()
			{
				if (!_003C_003Ef__this.IsSmartphoneVisible)
				{
					_003C_003Ef__this.OpenView<HubHudView>();
				}
				_003C_003Ef__this.smartphone2DRoot.SetActive(true);
				GameEvents.Invoke(new TrackingEvent
				{
					Type = TrackingEvent.TrackingType.HubOpened,
					Arguments = new Dictionary<string, object>()
				});
				if (callback != null)
				{
					callback();
				}
			}
		}

		private static int lastSelectedLocationId = -1;

		private static int pendingLocationId = -1;

		private static List<int> playedGameIds = new List<int>();

		public bool TrialHasEnded;

		[SerializeField]
		private GameProgressionManager progressionManager;

		[SerializeField]
		private View[] hubViews;

		[SerializeField]
		private AudioClip musicClip;

		[SerializeField]
		private AudioClip musicClipIntro;

		[SerializeField]
		private AudioClip defaultAmbientClip;

		[SerializeField]
		private AudioClip introAmbientClip;

		[SerializeField]
		private AudioClip idleAmbienceClip;

		[SerializeField]
		private float ambientFadeLength = 2f;

		[SerializeField]
		private HubCameraManager hubCameraManager;

		[SerializeField]
		private GameObject smartphone2DRoot;

		[SerializeField]
		private GameObject smartphone3DRoot;

		[SerializeField]
		private UIAtlas screenshotAtlas;

		private Location[] hubData;

		private HubLocationProvider selectedLocation;

		private AudioObject playingAmbientObject;

		[CompilerGenerated]
		private static Func<Game, bool> _003C_003Ef__am_0024cache14;

		public static int LastSelectedLocationId
		{
			get
			{
				return lastSelectedLocationId;
			}
		}

		public static int PendingLocationId
		{
			get
			{
				return pendingLocationId;
			}
		}

		public bool IsSmartphoneVisible { get; protected set; }

		protected bool isShowingLocation
		{
			get
			{
				return selectedLocation == null;
			}
		}

		public GameProgressionManager ProgressionManager
		{
			get
			{
				return progressionManager;
			}
		}

		public bool IsFirstPlayViewOpen
		{
			get
			{
				return IsViewOpen<HubFirstPlayView>();
			}
		}

		protected void Awake()
		{
			AddViews(hubViews);
			TrackingEvent trackingEvent = new TrackingEvent();
			trackingEvent.Type = TrackingEvent.TrackingType.HubOpened;
			trackingEvent.Arguments = new Dictionary<string, object>();
			GameEvents.Invoke(trackingEvent);
		}

		private new IEnumerator Start()
		{
			if (lastSelectedLocationId <= 0)
			{
				AudioController.PlayMusic(musicClipIntro.name);
				hubCameraManager.PlayIntroAnimation();
				if (introAmbientClip != null)
				{
					playingAmbientObject = AudioController.Play(introAmbientClip.name);
					playingAmbientObject.FadeOut(8.133f);
					playingAmbientObject.SwitchAudioSources();
				}
				playingAmbientObject.PlayNow(defaultAmbientClip.name, 0f, 1f, 0f);
				playingAmbientObject.FadeIn(8.133f);
			}
			else
			{
				AudioController.PlayMusic(musicClip.name);
				HubController hubController = this;
				HubLocationProvider[] collection = UnityEngine.Object.FindObjectsOfType<HubLocationProvider>();
				if (_003CStart_003Ec__IteratorA3._003C_003Ef__am_0024cache3 == null)
				{
					_003CStart_003Ec__IteratorA3._003C_003Ef__am_0024cache3 = _003CStart_003Ec__IteratorA3._003C_003Em__84;
				}
				hubController.selectedLocation = collection.FirstOrDefault(_003CStart_003Ec__IteratorA3._003C_003Ef__am_0024cache3);
				hubCameraManager.WarpToLocation(selectedLocation);
				playingAmbientObject = AudioController.Play(selectedLocation.AmbientLoop.name);
			}
			yield return StartCoroutine(waitForDataLoadAndIntroAnimationToContinue());
			if (AvatarSystem.GetPlayerConfiguration() == null)
			{
				OpenView<HubFirstPlayView>();
			}
			else if (lastSelectedLocationId > 0)
			{
				OpenView<HubLocationView>().UpdateInfo(Location.GetById(selectedLocation.LocationId, hubData), selectedLocation);
			}
			else
			{
				OpenView<HubHudView>();
			}
			if (pendingLocationId > -1)
			{
				DeselectLocation(((_003CStart_003Ec__IteratorA3)(object)this)._003C_003Em__85);
			}
			smartphone2DRoot.SetActive(!IsViewOpen<HubFirstPlayView>() && !playedGameIds.Contains(-1));
		}

		private void OnEnable()
		{
			GameEvents.Subscribe<SmartphoneVisibilityChangedEvent>(onSmartphoneVisibilityChanged);
			GameEvents.Subscribe<Webservice.LogoutEvent>(onLogout);
			GameEvents.Subscribe<Webservice.TrialEndedEvent>(onTrialEnded);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<SmartphoneVisibilityChangedEvent>(onSmartphoneVisibilityChanged);
			GameEvents.Unsubscribe<Webservice.LogoutEvent>(onLogout);
			GameEvents.Unsubscribe<Webservice.TrialEndedEvent>(onTrialEnded);
		}

		private void onLogout(Webservice.LogoutEvent evt)
		{
			lastSelectedLocationId = -1;
			playedGameIds = new List<int>();
		}

		private void onTrialEnded(Webservice.TrialEndedEvent evt)
		{
			TrialHasEnded = true;
			CloseAllViews();
			smartphone2DRoot.SetActive(false);
			smartphone3DRoot.SetActive(false);
			if (selectedLocation != null)
			{
				DeselectLocation(_003ConTrialEnded_003Em__7C);
			}
			else
			{
				GetView<HubHudView>().Close();
			}
			GameEvents.Invoke(new PopupEvent(Localization.Get("HUB_POPUP_TRIAL_PERIOD_ENDED_TITLE"), Localization.Get("HUB_POPUP_TRIAL_PERIOD_ENDED_DESCRIPTION"), Localization.Get("UI_OK"), string.Empty, _003ConTrialEnded_003Em__7D, null, false));
		}

		private void onSmartphoneVisibilityChanged(SmartphoneVisibilityChangedEvent evt)
		{
			IsSmartphoneVisible = evt.IsVisible;
			if (selectedLocation == null)
			{
				View view = GetView<HubHudView>();
				if (IsSmartphoneVisible && view.IsOpen)
				{
					CloseView(view);
				}
				else if (!view.IsOpen)
				{
					OpenView(view);
				}
			}
			else if (IsSmartphoneVisible)
			{
				DeselectLocation();
			}
		}

		private IEnumerator waitForDataLoadAndIntroAnimationToContinue()
		{
			smartphone2DRoot.SetActive(false);
			AvatarSystem.LoadPlayerConfiguration();
			DataStorage.GetLocationsData(((_003CwaitForDataLoadAndIntroAnimationToContinue_003Ec__IteratorA4)(object)this)._003C_003Em__87);
			while (!hasDataBeenLoadedToBegin())
			{
				yield return null;
			}
			while (hubCameraManager.IsPlayingIntroAnimation)
			{
				yield return null;
			}
		}

		private bool hasDataBeenLoadedToBegin()
		{
			return hubData != null && progressionManager.GameDetails != null;
		}

		public void LogoutCurrentUser()
		{
			GameEvents.Invoke(new Webservice.LogoutEvent());
		}

		public void OpenSettingsWindow()
		{
			if (!IsFirstPlayViewOpen)
			{
				UnityEngine.Object.FindObjectOfType<SmartphoneController>().HideNotificationCenter();
				SmartphoneVisibilityChangedEvent smartphoneVisibilityChangedEvent = new SmartphoneVisibilityChangedEvent();
				smartphoneVisibilityChangedEvent.IsVisible = true;
				GameEvents.Invoke(smartphoneVisibilityChangedEvent);
			}
			if (!IsViewOpen<HubMenuView>())
			{
				OpenView<HubMenuView>();
			}
		}

		public void CloseSettingsWindow()
		{
			if (!IsFirstPlayViewOpen)
			{
				UnityEngine.Object.FindObjectOfType<SmartphoneController>().ShowNotificationCenter();
				SmartphoneVisibilityChangedEvent smartphoneVisibilityChangedEvent = new SmartphoneVisibilityChangedEvent();
				smartphoneVisibilityChangedEvent.IsVisible = false;
				GameEvents.Invoke(smartphoneVisibilityChangedEvent);
			}
			CloseView<HubMenuView>();
		}

		public void OpenPremiumGameLockedWindow(int gameId)
		{
			_003COpenPremiumGameLockedWindow_003Ec__AnonStorey170 _003COpenPremiumGameLockedWindow_003Ec__AnonStorey = new _003COpenPremiumGameLockedWindow_003Ec__AnonStorey170();
			_003COpenPremiumGameLockedWindow_003Ec__AnonStorey.gameId = gameId;
			_003COpenPremiumGameLockedWindow_003Ec__AnonStorey._003C_003Ef__this = this;
			if (screenshotAtlas.GetSprite(_003COpenPremiumGameLockedWindow_003Ec__AnonStorey.gameId.ToString()) != null)
			{
				HubPremiumGameView hubPremiumGameView = (IsViewOpen<HubPremiumGameView>() ? GetView<HubPremiumGameView>() : OpenView<HubPremiumGameView>());
				hubPremiumGameView.SetInfo(_003COpenPremiumGameLockedWindow_003Ec__AnonStorey.gameId, Localization.Get("HUB_POPUP_PREMIUMGAME_TITLE"), Localization.Get("HUB_POPUP_PREMIUMGAME_DESCRIPTION"), Localization.Get("UI_REGISTER"), Localization.Get("UI_TIP_PARENTS"), _003COpenPremiumGameLockedWindow_003Ec__AnonStorey._003C_003Em__7E, _003COpenPremiumGameLockedWindow_003Ec__AnonStorey._003C_003Em__7F);
			}
			else
			{
				GameEvents.Invoke(new PopupEvent(Localization.Get("HUB_POPUP_PREMIUMGAME_TITLE"), Localization.Get("HUB_POPUP_PREMIUMGAME_DESCRIPTION"), Localization.Get("UI_REGISTER"), Localization.Get("UI_TIP_PARENTS"), _003COpenPremiumGameLockedWindow_003Ec__AnonStorey._003C_003Em__80, _003COpenPremiumGameLockedWindow_003Ec__AnonStorey._003C_003Em__81));
			}
		}

		public void OpenInstructionsWindow()
		{
			CloseView<HubHudView>();
			OpenView<PauseView>();
		}

		public void CloseInstructionsWindow()
		{
			OpenView<HubHudView>();
			CloseView<PauseView>();
		}

		public void OnCameraIdleStart()
		{
			CloseAllViews();
			smartphone2DRoot.SetActive(false);
			smartphone3DRoot.SetActive(false);
			playingAmbientObject.FadeOut(4f);
			playingAmbientObject.SwitchAudioSources();
			playingAmbientObject.PlayNow(idleAmbienceClip.name, 0f, 1f, 0f);
			playingAmbientObject.FadeIn(4f);
		}

		public void FadeToOverview()
		{
			playingAmbientObject.FadeOut(3f);
			playingAmbientObject.SwitchAudioSources();
			playingAmbientObject.PlayNow(defaultAmbientClip.name, 0f, 1f, 0f);
			playingAmbientObject.FadeIn(3f);
		}

		public void OnCameraIdleStop()
		{
			OpenView<HubHudView>();
			smartphone2DRoot.SetActive(true);
			smartphone3DRoot.SetActive(true);
		}

		public void SelectLocation(HubLocationProvider location)
		{
			Location location2 = GetLocation(location);
			Game[] games = location2.Games;
			if (_003C_003Ef__am_0024cache14 == null)
			{
				_003C_003Ef__am_0024cache14 = _003CSelectLocation_003Em__82;
			}
			if (games.All(_003C_003Ef__am_0024cache14) && UserProfile.Current != null && UserProfile.Current.IsSA)
			{
				GameEvents.Invoke(new PopupEvent(Localization.Get("HUB_POPUP_LOCKEDGAME_SA_TITLE"), StringFormatter.GetLocalizationFormatted("HUB_POPUP_LOCKEDGAME_SA_DESCRIPTION"), Localization.Get("HUB_POPUP_LOCKEDGAME_SA_BUTTON_CONTINUE"), null, null, null));
			}
			AnimateToLocation(location, OnArriveAtLocation);
			playingAmbientObject.FadeOut(ambientFadeLength);
			playingAmbientObject.SwitchAudioSources();
			playingAmbientObject.PlayNow(selectedLocation.AmbientLoop.name, 0f, 1f, 0f);
			playingAmbientObject.FadeIn(ambientFadeLength);
			if (IsViewOpen<HubHudView>())
			{
				CloseView<HubHudView>();
			}
		}

		public float AnimateToLocation(HubLocationProvider location, Action callback)
		{
			selectedLocation = location;
			smartphone2DRoot.SetActive(false);
			return hubCameraManager.AnimateToLocation(location, callback);
		}

		private void OnArriveAtLocation()
		{
			Location byId = Location.GetById(selectedLocation.LocationId, hubData);
			OpenView<HubLocationView>().UpdateInfo(byId, selectedLocation);
			TrackingEvent trackingEvent = new TrackingEvent();
			trackingEvent.Type = TrackingEvent.TrackingType.LocationOpened;
			trackingEvent.Arguments = new Dictionary<string, object> { { "LocationName", byId.Name } };
			GameEvents.Invoke(trackingEvent);
			smartphone2DRoot.SetActive(true);
		}

		public void DeselectLocation(Action callback = null)
		{
			_003CDeselectLocation_003Ec__AnonStorey171 _003CDeselectLocation_003Ec__AnonStorey = new _003CDeselectLocation_003Ec__AnonStorey171();
			_003CDeselectLocation_003Ec__AnonStorey.callback = callback;
			_003CDeselectLocation_003Ec__AnonStorey._003C_003Ef__this = this;
			CloseView<HubLocationView>();
			selectedLocation = null;
			smartphone2DRoot.SetActive(false);
			if (playingAmbientObject != null)
			{
				playingAmbientObject.FadeOut(ambientFadeLength);
				playingAmbientObject.SwitchAudioSources();
				playingAmbientObject.PlayNow(defaultAmbientClip.name, 0f, 1f, 0f);
				playingAmbientObject.FadeIn(ambientFadeLength);
			}
			else
			{
				UnityEngine.Debug.LogWarning("Hey buddy, ambient not looping?");
			}
			hubCameraManager.AnimateToOverview(_003CDeselectLocation_003Ec__AnonStorey._003C_003Em__83);
		}

		public void Play(Game game)
		{
			if (IsSmartphoneVisible)
			{
				return;
			}
			if (base.enabled)
			{
				string text = game.SceneName;
				if (!string.IsNullOrEmpty(game.SceneMotionComicName) && ((!playedGameIds.Contains(game.Id) && !UserProfile.Current.IsFree) || text == Application.loadedLevelName))
				{
					text = game.SceneMotionComicName;
					MotionComicPlayer.SetSceneToLoad(game.SceneName);
				}
				playedGameIds.Add(game.Id);
				lastSelectedLocationId = ((!(selectedLocation != null) || game.Id < 0) ? (-1) : selectedLocation.LocationId);
				if (IsViewOpen<HubLocationView>())
				{
					CloseView<HubLocationView>();
				}
				SceneManager.Load(text);
			}
			base.enabled = false;
		}

		public Location GetLocation(HubLocationProvider provider)
		{
			return Location.GetById(provider.LocationId, hubData);
		}

		public static void ZoomOutNextVisit()
		{
			lastSelectedLocationId = -1;
		}

		public static void ZoomInNextVisit(int locationId)
		{
			pendingLocationId = locationId;
		}

		[CompilerGenerated]
		private void _003ConTrialEnded_003Em__7C()
		{
			GetView<HubHudView>().Close();
		}

		[CompilerGenerated]
		private void _003ConTrialEnded_003Em__7D()
		{
			LogoutCurrentUser();
		}

		[CompilerGenerated]
		private static bool _003CSelectLocation_003Em__82(Game g)
		{
			return !g.IsUnlockedSA;
		}
	}
}
