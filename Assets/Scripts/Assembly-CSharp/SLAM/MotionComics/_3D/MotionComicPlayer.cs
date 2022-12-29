using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CinemaDirector;
using SLAM.Analytics;
using SLAM.BuildSystem;
using SLAM.Engine;
using SLAM.Hub;
using SLAM.Shared;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.MotionComics._3D
{
	public class MotionComicPlayer : ViewController
	{
		private static string sceneToLoad = "CraneOperator";

		[SerializeField]
		private bool useSpeechBalloons;

		[SerializeField]
		private Cutscene cutscene;

		private AudioClip clipToPlayAfterInteract;

		private AssetBundleManager.AssetLoadRequest loadRequest;

		[CompilerGenerated]
		private static Action<Location[]> _003C_003Ef__am_0024cache6;

		[CompilerGenerated]
		private static Func<Location, bool> _003C_003Ef__am_0024cache7;

		[CompilerGenerated]
		private static Func<Game, bool> _003C_003Ef__am_0024cache8;

		[CompilerGenerated]
		private static Action<UserScore> _003C_003Ef__am_0024cache9;

		public bool IsLoadingGame { get; protected set; }

		public string SceneToLoad
		{
			get
			{
				return sceneToLoad;
			}
		}

		public Cutscene Cutscene
		{
			get
			{
				return cutscene;
			}
		}

		public static void SetSceneToLoad(string scene)
		{
			sceneToLoad = scene;
		}

		protected override void Start()
		{
			base.Start();
			TrackingEvent trackingEvent = new TrackingEvent();
			trackingEvent.Type = TrackingEvent.TrackingType.MotionComicOpened;
			trackingEvent.Arguments = new Dictionary<string, object> { 
			{
				"Game",
				Application.loadedLevelName
			} };
			GameEvents.Invoke(trackingEvent);
			SceneManager.Preload(SceneToLoad);
			AddViews(base.transform.GetComponentsInChildren<View>(true));
			OpenView<HudView>();
			if (sceneToLoad == "Hub")
			{
				if (_003C_003Ef__am_0024cache6 == null)
				{
					_003C_003Ef__am_0024cache6 = _003CStart_003Em__BC;
				}
				DataStorage.GetLocationsData(_003C_003Ef__am_0024cache6);
			}
		}

		private void OnValidate()
		{
			if (cutscene == null)
			{
				cutscene = UnityEngine.Object.FindObjectOfType<Cutscene>();
			}
		}

		private void OnEnable()
		{
			Cutscene.CutsceneFinished += onCutsceneFinished;
		}

		private void OnDisable()
		{
			Cutscene.CutsceneFinished -= onCutsceneFinished;
		}

		private void onCutsceneFinished(object sender, CutsceneEventArgs e)
		{
			if (IsViewOpen<DialogView>())
			{
				CloseView<DialogView>();
			}
			StartCoroutine(waitForDownloadAndPlayGame());
		}

		public void OpenDialog(string npc, string textKey, NGUIText.Alignment alignment, bool append)
		{
			if (!useSpeechBalloons)
			{
				if (!IsViewOpen<DialogView>())
				{
					OpenView<DialogView>();
				}
				string text = ((UserProfile.Current == null) ? "Avatar" : UserProfile.Current.FirstName);
				string text2 = ((!(npc == "AVATAR_NAME")) ? Localization.Get(npc) : text);
				string text3 = Localization.Get(textKey);
				GetView<DialogView>().SetInfo(text2 + ": ", text3, alignment, append);
				AudioController.Play("Interface_newTextLine");
			}
		}

		public void OpenSpeechBalloon(TimelineTrack track, BalloonType balloonType, GameObject target, string textKey, bool append)
		{
			if (useSpeechBalloons)
			{
				if (!IsViewOpen<DialogView>())
				{
					OpenView<DialogView>();
				}
				string text = Localization.Get(textKey);
				SpeechBalloon speechBalloon = null;
				speechBalloon = ((!append) ? GetView<DialogView>().CreateBalloonOnTrack(track, balloonType) : GetView<DialogView>().GetLastBalloonOnTrack(track));
				speechBalloon.SetInfo(text, target, append);
			}
		}

		public void ShowInteractButton(AudioClip clip)
		{
			clipToPlayAfterInteract = clip;
			cutscene.Pause();
			OpenView<InteractView>();
		}

		public void CloseDialog(TimelineTrack track)
		{
			GetView<DialogView>().DestroyBalloonsOnTrack(track);
		}

		public void PlayGame()
		{
			if (!IsLoadingGame)
			{
				IsLoadingGame = true;
				SceneManager.Load(SceneToLoad);
			}
		}

		public void ResumeCutscene()
		{
			if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist() && clipToPlayAfterInteract != null)
			{
				AudioController.Play(clipToPlayAfterInteract.name);
			}
			cutscene.Play();
			CloseView<InteractView>();
		}

		public void SkipCutscene()
		{
			if (IsViewOpen<DialogView>())
			{
				CloseView<DialogView>();
			}
			cutscene.Pause();
			StartCoroutine(waitForDownloadAndSkip());
		}

		private IEnumerator waitForDownloadAndPlayGame()
		{
			yield return StartCoroutine(waitForDownload());
			PlayGame();
		}

		private IEnumerator waitForDownloadAndSkip()
		{
			yield return StartCoroutine(waitForDownload());
			PlayGame();
		}

		private IEnumerator waitForDownload()
		{
			while (loadRequest != null && !loadRequest.IsDone)
			{
				yield return null;
			}
		}

		[CompilerGenerated]
		private static void _003CStart_003Em__BC(Location[] locations)
		{
			if (_003C_003Ef__am_0024cache7 == null)
			{
				_003C_003Ef__am_0024cache7 = _003CStart_003Em__BD;
			}
			Location location = locations.FirstOrDefault(_003C_003Ef__am_0024cache7);
			if (location == null)
			{
				return;
			}
			Game[] games = location.Games;
			if (_003C_003Ef__am_0024cache8 == null)
			{
				_003C_003Ef__am_0024cache8 = _003CStart_003Em__BE;
			}
			Game game = games.FirstOrDefault(_003C_003Ef__am_0024cache8);
			if (game != null)
			{
				int id = game.Id;
				string difficulty = game.RequiredDifficultyToUnlockNextGame.ToString();
				if (_003C_003Ef__am_0024cache9 == null)
				{
					_003C_003Ef__am_0024cache9 = _003CStart_003Em__BF;
				}
				ApiClient.SubmitScore(id, 1, difficulty, 0, false, _003C_003Ef__am_0024cache9);
				if (game.Name.ToLowerInvariant().Contains("outro"))
				{
					HubController.ZoomOutNextVisit();
				}
			}
		}

		[CompilerGenerated]
		private static bool _003CStart_003Em__BD(Location l)
		{
			return l.Id == HubController.LastSelectedLocationId;
		}

		[CompilerGenerated]
		private static bool _003CStart_003Em__BE(Game g)
		{
			return g.SceneMotionComicName == Application.loadedLevelName;
		}

		[CompilerGenerated]
		private static void _003CStart_003Em__BF(UserScore us)
		{
			DataStorage.GetProgressionData(null);
		}
	}
}
