using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LitJson;
using SLAM.Analytics;
using SLAM.Hub;
using SLAM.Invites;
using SLAM.Notifications;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Engine
{
	[RequireComponent(typeof(FiniteStateMachine))]
	public abstract class GameController : ViewController
	{
		[Serializable]
		public class LevelSetting
		{
			public TutorialView Tutorial;

			public int Index { get; private set; }

			public string Name { get; set; }

			public bool IsUnlocked { get; set; }

			[JsonName("is_unlocked_sa")]
			public bool IsUnlockedSA { get; private set; }

			public string Difficulty
			{
				get
				{
					return (Index + 1).ToString();
				}
			}

			public void SetInfo(int index, string name, bool isUnlocked)
			{
				Index = index;
				Name = name;
				IsUnlocked = isUnlocked;
				IsUnlockedSA = IsUnlockedSA;
			}
		}

		[CompilerGenerated]
		private sealed class _003ConProgressionDataLoaded_003Ec__AnonStorey155
		{
			internal int i;

			internal bool _003C_003Em__12(UserGameProgression d)
			{
				return d.LevelIndex == i;
			}
		}

		[CompilerGenerated]
		private sealed class _003ConProgressionDataLoaded_003Ec__AnonStorey156
		{
			internal int levelCount;

			internal bool _003C_003Em__14(LevelSetting l)
			{
				return l.Index + 1 >= levelCount - 5;
			}
		}

		[CompilerGenerated]
		private sealed class _003ConScoresSubmitted_003Ec__AnonStorey157
		{
			internal UserGameDetails[] oldProgressionData;

			internal GameController _003C_003Ef__this;

			private static Action<Game> _003C_003Ef__am_0024cache2;

			internal void _003C_003Em__1A(UserGameDetails[] newProgressionData)
			{
				if (_003C_003Ef__this.currentGameInfo.NextGameId.HasValue && !oldProgressionData.Any(_003C_003Em__1C) && newProgressionData.Any(_003C_003Em__1D))
				{
					int value = _003C_003Ef__this.currentGameInfo.NextGameId.Value;
					if (_003C_003Ef__am_0024cache2 == null)
					{
						_003C_003Ef__am_0024cache2 = _003C_003Em__1E;
					}
					DataStorage.GetGameById(value, _003C_003Ef__am_0024cache2);
				}
			}

			internal bool _003C_003Em__1C(UserGameDetails g)
			{
				return g.GameId == _003C_003Ef__this.currentGameInfo.NextGameId.Value;
			}

			internal bool _003C_003Em__1D(UserGameDetails g)
			{
				return g.GameId == _003C_003Ef__this.currentGameInfo.NextGameId.Value;
			}

			private static void _003C_003Em__1E(Game nextGame)
			{
				if (nextGame.IsPremiumAvailable)
				{
					HubLocationView.RecentlyUnlockedGameId = nextGame.Id;
					GameEvents.Invoke(new NotificationEvent
					{
						Title = Localization.Get("UI_GAME_UNLOCKED_TITLE"),
						Body = StringFormatter.GetLocalizationFormatted("UI_GAME_UNLOCKED_BODY", Localization.Get(nextGame.Name)),
						IconSpriteName = "Achiev_Duckstad1"
					});
				}
			}
		}

		protected const string STATE_LOADING = "Loading";

		protected const string STATE_INTRODUCTION = "Introduction";

		protected const string STATE_READY_TO_BEGIN = "Ready to begin";

		protected const string STATE_RUNNING = "Running";

		protected const string STATE_FINISHED = "Finished";

		[SerializeField]
		[Header("Base Properties")]
		private StartView startView;

		[SerializeField]
		private HUDView hudView;

		[SerializeField]
		private PauseView instructionsView;

		[SerializeField]
		private SuccesBaseView successView;

		[SerializeField]
		private FailedView failedView;

		[SerializeField]
		private ChallengeView challengeView;

		[SerializeField]
		private HighscoresView highscoresView;

		[SerializeField]
		private CinematicView cinematicView;

		[SerializeField]
		private BalloonView balloonView;

		[SerializeField]
		private View[] additionalViews;

		[SerializeField]
		private GameObject introComic;

		private FiniteStateMachine fsm;

		private bool paused;

		private bool wasInvitedForGame;

		private static int queuedLevelIndex = -1;

		public static bool forceSkipIntroduction = false;

		public static List<int> hasRunIntroduction = new List<int>();

		protected Game currentGameInfo;

		protected Location currentLocationInfo;

		private float originalTimeScale = 1f;

		private bool cheatsEnabled;

		public static Message ChallengeAccepted;

		[CompilerGenerated]
		private static Func<KeyValuePair<string, int>, int> _003C_003Ef__am_0024cache19;

		[CompilerGenerated]
		private static Func<LevelSetting, bool> _003C_003Ef__am_0024cache1A;

		[CompilerGenerated]
		private static Func<LevelSetting, bool> _003C_003Ef__am_0024cache1B;

		public abstract LevelSetting[] Levels { get; }

		private LevelSetting selectedLevel { get; set; }

		protected FiniteStateMachine StateMachine
		{
			get
			{
				return fsm;
			}
		}

		public bool Paused
		{
			get
			{
				return paused;
			}
			private set
			{
				paused = value;
				if (paused)
				{
					originalTimeScale = Time.timeScale;
					Time.timeScale = 0f;
				}
				else
				{
					Time.timeScale = originalTimeScale;
				}
			}
		}

		public abstract int GameId { get; }

		public abstract Portrait DuckCharacter { get; }

		public abstract Dictionary<string, int> ScoreCategories { get; }

		public abstract string IntroNPCKey { get; }

		public abstract string IntroTextKey { get; }

		protected string IntroNPC
		{
			get
			{
				return StringFormatter.GetLocalizationFormatted(IntroNPCKey) + " : ";
			}
		}

		protected string IntroText
		{
			get
			{
				return StringFormatter.GetLocalizationFormatted(IntroTextKey, UserProfile.Current.FirstName);
			}
		}

		public virtual int TotalScore
		{
			get
			{
				Dictionary<string, int> scoreCategories = ScoreCategories;
				if (_003C_003Ef__am_0024cache19 == null)
				{
					_003C_003Ef__am_0024cache19 = _003Cget_TotalScore_003Em__E;
				}
				return Mathf.Max(0, scoreCategories.Sum(_003C_003Ef__am_0024cache19));
			}
		}

		public bool HasNextLevel
		{
			get
			{
				return selectedLevel.Index < Levels.Length - 1;
			}
		}

		public bool IsChallengingOtherPlayer { get; protected set; }

		public UserProfile ChallengeUser { get; protected set; }

		public T SelectedLevel<T>() where T : LevelSetting
		{
			return (T)selectedLevel;
		}

		protected virtual void Awake()
		{
			AddViews(startView, instructionsView, hudView, successView, failedView, challengeView, highscoresView, balloonView, cinematicView);
			AddViews(additionalViews);
			fsm = GetComponent<FiniteStateMachine>();
			AddStates();
		}

		protected override void Start()
		{
			base.Start();
			wasInvitedForGame = GameId == InviteSystem.AcceptedInvitationGameId;
			DataStorage.GetLocationsData(_003CStart_003Em__F);
			DataStorage.GetWebConfiguration(_003CStart_003Em__10);
			if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
			{
				if (AudioController.GetCategory("Music") != null && AudioController.GetCategory("Music").AudioItems.Length > 0)
				{
					AudioItem[] audioItems = AudioController.GetCategory("Music").AudioItems;
					foreach (AudioItem audioItem in audioItems)
					{
						AudioController.Play(audioItem.Name);
					}
				}
				else
				{
					Debug.LogWarning("Hey buddy, this game doesn't have music? Make sure there is an AudioController with a category 'Music'!");
				}
				if (AudioController.GetCategory("Ambience") != null)
				{
					AudioItem[] audioItems2 = AudioController.GetCategory("Ambience").AudioItems;
					foreach (AudioItem audioItem2 in audioItems2)
					{
						AudioSubItem[] subItems = audioItem2.subItems;
						foreach (AudioSubItem audioSubItem in subItems)
						{
							audioSubItem.FadeIn = 0.5f;
						}
						AudioController.Play(audioItem2.Name);
					}
				}
				else
				{
					Debug.LogWarning("Hey buddy, this game doesn't have ambience sounds? Make sure there is an AudioController with a category 'Ambience'!");
				}
			}
			AudioController[] array = UnityEngine.Object.FindObjectsOfType<AudioController>();
			AudioController[] array2 = array;
			foreach (AudioController audioController in array2)
			{
				if (audioController.name != "AudioManager")
				{
					StartCoroutine(FadeOutGameAudio(audioController._GetCategory("SFX"), 0f));
				}
			}
		}

		private void onProgressionDataLoaded(UserGameDetails[] games)
		{
			UserGameDetails userGameDetails = games.FirstOrDefault(_003ConProgressionDataLoaded_003Em__11);
			_003ConProgressionDataLoaded_003Ec__AnonStorey155 _003ConProgressionDataLoaded_003Ec__AnonStorey = new _003ConProgressionDataLoaded_003Ec__AnonStorey155();
			_003ConProgressionDataLoaded_003Ec__AnonStorey.i = 0;
			while (_003ConProgressionDataLoaded_003Ec__AnonStorey.i < Levels.Length)
			{
				Levels[_003ConProgressionDataLoaded_003Ec__AnonStorey.i].SetInfo(_003ConProgressionDataLoaded_003Ec__AnonStorey.i, "level_" + _003ConProgressionDataLoaded_003Ec__AnonStorey.i, _003ConProgressionDataLoaded_003Ec__AnonStorey.i == 0 || (userGameDetails != null && userGameDetails.Progression.Any(_003ConProgressionDataLoaded_003Ec__AnonStorey._003C_003Em__12)));
				_003ConProgressionDataLoaded_003Ec__AnonStorey.i++;
			}
			TrackingEvent trackingEvent = new TrackingEvent();
			trackingEvent.Type = TrackingEvent.TrackingType.StartViewOpened;
			trackingEvent.Arguments = new Dictionary<string, object>
			{
				{ "GameId", GameId },
				{ "LocationName", currentLocationInfo.Name },
				{ "GameName", currentGameInfo.Name }
			};
			GameEvents.Invoke(trackingEvent);
			if (wasInvitedForGame)
			{
				_003ConProgressionDataLoaded_003Ec__AnonStorey156 _003ConProgressionDataLoaded_003Ec__AnonStorey2 = new _003ConProgressionDataLoaded_003Ec__AnonStorey156();
				_003ConProgressionDataLoaded_003Ec__AnonStorey2.levelCount = Levels.Count();
				LevelSetting[] levels = Levels;
				if (_003C_003Ef__am_0024cache1A == null)
				{
					_003C_003Ef__am_0024cache1A = _003ConProgressionDataLoaded_003Em__13;
				}
				if (levels.Count(_003C_003Ef__am_0024cache1A) >= _003ConProgressionDataLoaded_003Ec__AnonStorey2.levelCount)
				{
					queuedLevelIndex = Levels.Where(_003ConProgressionDataLoaded_003Ec__AnonStorey2._003C_003Em__14).GetRandom().Index + 1;
				}
				else
				{
					LevelSetting[] levels2 = Levels;
					if (_003C_003Ef__am_0024cache1B == null)
					{
						_003C_003Ef__am_0024cache1B = _003ConProgressionDataLoaded_003Em__15;
					}
					queuedLevelIndex = levels2.Where(_003C_003Ef__am_0024cache1B).Last().Index + 1;
				}
				StateMachine.SwitchTo("Ready to begin", 0.1f);
			}
			else if (ChallengeAccepted != null)
			{
				queuedLevelIndex = ChallengeAccepted.Difficulty;
				StateMachine.SwitchTo("Ready to begin", 0.1f);
			}
			else if (hasRunIntroduction.Contains(GameId) || forceSkipIntroduction)
			{
				forceSkipIntroduction = false;
				StateMachine.SwitchTo("Ready to begin", 0.1f);
			}
			else
			{
				StateMachine.SwitchTo("Introduction");
			}
			if (!IsViewOpen<CinematicView>())
			{
				OpenView<CinematicView>();
			}
		}

		protected virtual void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (IsViewOpen<PauseView>())
				{
					Resume();
				}
				else if (IsViewOpen<HUDView>())
				{
					Pause();
				}
			}
			if (cheatsEnabled && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.F))
			{
				Finish(true);
			}
		}

		protected virtual void OnDestroy()
		{
			ChallengeAccepted = null;
		}

		protected virtual void OnEnterStateLoading()
		{
		}

		protected virtual void WhileStateLoading()
		{
		}

		protected virtual void OnExitStateLoading()
		{
		}

		protected virtual void OnEnterStateReadyToBegin()
		{
			if (introComic != null)
			{
				introComic.SetActive(false);
			}
			if (queuedLevelIndex != -1)
			{
				int num = queuedLevelIndex;
				queuedLevelIndex = -1;
				Play(Levels[num - 1]);
			}
			else if (!IsViewOpen<StartView>())
			{
				OpenView<StartView>().SetInfo(Levels, currentGameInfo);
			}
		}

		protected virtual void WhileStateReadyToBegin()
		{
			if (cheatsEnabled && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
			{
				StartCoroutine(doUnlockLevelSequence());
			}
		}

		protected virtual void OnExitStateReadyToBegin()
		{
		}

		protected virtual void OnEnterStateIntroduction()
		{
			IntroController component = introComic.GetComponent<IntroController>();
			if (!GetView<BalloonView>().IsOpen)
			{
				OpenView<BalloonView>();
			}
			if (component != null)
			{
				component.Enable(GetView<BalloonView>(), IntroText);
			}
			else
			{
				Debug.LogWarning("Hey Buddy, the intro scene has no intro controller", this);
			}
			StartCoroutine(cinematicView.OpenBlindsAndWaitForClick(_003COnEnterStateIntroduction_003Em__16));
		}

		protected virtual void WhileStateIntroduction()
		{
		}

		protected virtual void OnExitStateIntroduction()
		{
			IntroController component = introComic.GetComponent<IntroController>();
			if (GetView<BalloonView>().IsOpen)
			{
				CloseView<BalloonView>();
			}
			if (component != null)
			{
				component.Disable();
			}
			else
			{
				Debug.LogWarning("Hey Buddy, the intro scene has no intro controller", this);
			}
		}

		protected virtual void OnEnterStateRunning()
		{
			TutorialView tutorial = selectedLevel.Tutorial;
			if (tutorial != null)
			{
				AddView(tutorial);
				OpenView(tutorial);
			}
		}

		protected virtual void WhileStateRunning()
		{
		}

		protected virtual void OnExitStateRunning()
		{
		}

		protected virtual void OnEnterStateFinished()
		{
		}

		protected virtual void WhileStateFinished()
		{
		}

		protected virtual void OnExitStateFinished()
		{
		}

		protected virtual void AddStates()
		{
			StateMachine.AddState("Loading", OnEnterStateLoading, WhileStateLoading, OnExitStateLoading);
			StateMachine.AddState("Introduction", OnEnterStateIntroduction, WhileStateIntroduction, OnExitStateIntroduction);
			StateMachine.AddState("Ready to begin", OnEnterStateReadyToBegin, WhileStateReadyToBegin, OnExitStateReadyToBegin);
			StateMachine.AddState("Running", OnEnterStateRunning, WhileStateRunning, OnExitStateRunning);
			StateMachine.AddState("Finished", OnEnterStateFinished, WhileStateFinished, OnExitStateFinished);
			StateMachine.SwitchTo("Loading");
		}

		public virtual void GoToHub()
		{
			if (StateMachine.CurrentState != null && StateMachine.CurrentState.Name != "Finished" && StateMachine.CurrentState.Name != "Ready to begin" && StateMachine.CurrentState.Name != "Loading")
			{
				TrackingEvent trackingEvent = new TrackingEvent();
				trackingEvent.Type = TrackingEvent.TrackingType.GameQuit;
				trackingEvent.Arguments = new Dictionary<string, object>
				{
					{ "GameId", GameId },
					{ "Difficulty", selectedLevel.Difficulty },
					{
						"Time",
						Time.timeSinceLevelLoad
					},
					{ "LocationName", currentLocationInfo.Name },
					{ "GameName", currentGameInfo.Name }
				};
				GameEvents.Invoke(trackingEvent);
			}
			if (wasInvitedForGame)
			{
				InviteSystem.CloseGameInvitation();
			}
			Time.timeScale = 1f;
			Application.LoadLevel("Hub");
		}

		public virtual void Play(LevelSetting selectedLevel)
		{
			this.selectedLevel = selectedLevel;
			TrackingEvent trackingEvent = new TrackingEvent();
			trackingEvent.Type = TrackingEvent.TrackingType.GameStart;
			trackingEvent.Arguments = new Dictionary<string, object>
			{
				{ "GameId", GameId },
				{
					"Difficulty",
					this.selectedLevel.Difficulty
				},
				{ "LocationName", currentLocationInfo.Name },
				{ "GameName", currentGameInfo.Name }
			};
			GameEvents.Invoke(trackingEvent);
			if (IsViewOpen<StartView>())
			{
				CloseView<StartView>();
			}
			OpenView<HUDView>();
			if (IsViewOpen<CinematicView>())
			{
				GetView<CinematicView>().OpenBlindsFull();
			}
			AudioController[] array = UnityEngine.Object.FindObjectsOfType<AudioController>();
			AudioController[] array2 = array;
			foreach (AudioController audioController in array2)
			{
				if (audioController.name != "AudioManager")
				{
					StartCoroutine(FadeInGameAudio(audioController._GetCategory("SFX")));
				}
			}
			StateMachine.SwitchTo("Running");
		}

		public virtual void Pause()
		{
			TrackingEvent trackingEvent = new TrackingEvent();
			trackingEvent.Type = TrackingEvent.TrackingType.GamePause;
			trackingEvent.Arguments = new Dictionary<string, object>
			{
				{ "LocationName", currentLocationInfo.Name },
				{ "GameName", currentGameInfo.Name },
				{ "Difficulty", selectedLevel.Difficulty }
			};
			GameEvents.Invoke(trackingEvent);
			Paused = true;
			AudioController[] array = UnityEngine.Object.FindObjectsOfType<AudioController>();
			AudioController[] array2 = array;
			foreach (AudioController audioController in array2)
			{
				if (audioController.name != "AudioManager")
				{
					StartCoroutine(FadeOutGameAudio(audioController));
				}
			}
			AudioController.Play("Pause_screen_ambience");
			OpenView<PauseView>().SetInfo(selectedLevel, ChallengeAccepted != null, wasInvitedForGame);
		}

		public virtual void Resume()
		{
			TrackingEvent trackingEvent = new TrackingEvent();
			trackingEvent.Type = TrackingEvent.TrackingType.GameResume;
			trackingEvent.Arguments = new Dictionary<string, object>
			{
				{ "LocationName", currentLocationInfo.Name },
				{ "GameName", currentGameInfo.Name },
				{ "Difficulty", selectedLevel.Difficulty }
			};
			GameEvents.Invoke(trackingEvent);
			Paused = false;
			AudioController[] array = UnityEngine.Object.FindObjectsOfType<AudioController>();
			AudioController[] array2 = array;
			foreach (AudioController audioController in array2)
			{
				if (audioController.name != "AudioManager")
				{
					StartCoroutine(FadeInGameAudio(audioController));
				}
			}
			AudioController.Stop("Pause_screen_ambience");
			CloseView<PauseView>();
		}

		public virtual void Restart()
		{
			restartAndPlayImmediately(selectedLevel);
		}

		public virtual void GoToMenu()
		{
			if (wasInvitedForGame)
			{
				forceSkipIntroduction = true;
				InviteSystem.CloseGameInvitation();
			}
			TrackingEvent trackingEvent = new TrackingEvent();
			trackingEvent.Type = TrackingEvent.TrackingType.BackToMenuButton;
			trackingEvent.Arguments = new Dictionary<string, object>
			{
				{ "LocationName", currentLocationInfo.Name },
				{ "GameName", currentGameInfo.Name },
				{ "Difficulty", selectedLevel.Difficulty }
			};
			GameEvents.Invoke(trackingEvent);
			reloadScene();
		}

		public void PlayNextLevel()
		{
			int num = Mathf.Clamp(selectedLevel.Index + 1, 0, Levels.Length - 1);
			restartAndPlayImmediately(Levels[num]);
		}

		public virtual void Finish(bool succes)
		{
			StateMachine.SwitchTo("Finished");
			if (Paused)
			{
				Resume();
			}
			CloseAllViews();
			if (HasView<TutorialView>() && IsViewOpen<TutorialView>())
			{
				CloseView<TutorialView>();
			}
			AudioController[] array = UnityEngine.Object.FindObjectsOfType<AudioController>();
			AudioController[] array2 = array;
			foreach (AudioController audioController in array2)
			{
				if (audioController.name != "AudioManager")
				{
					StartCoroutine(FadeOutGameAudio(audioController));
				}
			}
			if (wasInvitedForGame)
			{
				InviteSystem.CloseGameInvitation();
			}
			if (succes)
			{
				AudioController.Play("Duckstad - score positive");
				int totalScore = TotalScore;
				int coinRewardForThisLevel = getCoinRewardForThisLevel();
				TrackingEvent trackingEvent = new TrackingEvent();
				trackingEvent.Type = TrackingEvent.TrackingType.GameWon;
				trackingEvent.Arguments = new Dictionary<string, object>
				{
					{ "GameId", GameId },
					{ "Difficulty", selectedLevel.Difficulty },
					{
						"Progress",
						(float)(selectedLevel.Index + 1) / (float)Levels.Length
					},
					{
						"Time",
						Time.timeSinceLevelLoad
					},
					{ "Score", totalScore },
					{ "Coins", coinRewardForThisLevel },
					{ "LocationName", currentLocationInfo.Name },
					{ "GameName", currentGameInfo.Name }
				};
				GameEvents.Invoke(trackingEvent);
				bool gameCompleted = !UserProfile.Current.IsFree && selectedLevel.Index >= Levels.Length - 1;
				ApiClient.SubmitScore(GameId, TotalScore, selectedLevel.Difficulty, "default", 0, gameCompleted, onScoresSubmitted);
				if (currentGameInfo.Type == Game.GameType.Job)
				{
					ApiClient.AddToWallet(coinRewardForThisLevel, null);
					trackingEvent = new TrackingEvent();
					trackingEvent.Type = TrackingEvent.TrackingType.DuckcoinsEarned;
					trackingEvent.Arguments = new Dictionary<string, object>
					{
						{ "Amount", coinRewardForThisLevel },
						{ "GameId", currentGameInfo.Id }
					};
					GameEvents.Invoke(trackingEvent);
				}
				OpenView<SuccesBaseView>().SetInfo(currentGameInfo, selectedLevel.Difficulty, !UserProfile.Current.IsFree && currentGameInfo.Type != Game.GameType.Job && ChallengeAccepted == null);
			}
			else
			{
				AudioController.Play("Duckstad - score negative");
				TrackingEvent trackingEvent = new TrackingEvent();
				trackingEvent.Type = TrackingEvent.TrackingType.GameLost;
				trackingEvent.Arguments = new Dictionary<string, object>
				{
					{ "GameId", GameId },
					{ "Difficulty", selectedLevel.Difficulty },
					{
						"Time",
						Time.timeSinceLevelLoad
					},
					{ "LocationName", currentLocationInfo.Name },
					{ "GameName", currentGameInfo.Name }
				};
				GameEvents.Invoke(trackingEvent);
				OpenView<FailedView>().SetInfo(selectedLevel);
			}
		}

		protected int getCoinRewardForThisLevel()
		{
			if (currentGameInfo.Type != Game.GameType.Job)
			{
				return 0;
			}
			int num = ((!wasInvitedForGame) ? 1 : 2);
			if (selectedLevel.Index < 5)
			{
				return 5 * num;
			}
			if (selectedLevel.Index < 10)
			{
				return 10 * num;
			}
			if (selectedLevel.Index < 15)
			{
				return 15 * num;
			}
			throw new Exception("I dont know how many coins to give you for level " + selectedLevel.Index);
		}

		protected void onScoresSubmitted(UserScore score)
		{
			DataStorage.GetProgressionData(_003ConScoresSubmitted_003Em__17);
			if (ChallengeAccepted != null)
			{
				TrackingEvent trackingEvent = new TrackingEvent();
				trackingEvent.Type = TrackingEvent.TrackingType.ChallengeCompleted;
				trackingEvent.Arguments = new Dictionary<string, object>
				{
					{
						"Sender",
						ChallengeAccepted.Sender.Id
					},
					{
						"Recipient",
						ApiClient.UserId
					},
					{ "GameId", GameId },
					{ "Difficulty", selectedLevel.Difficulty }
				};
				GameEvents.Invoke(trackingEvent);
				ApiClient.SendChallengeResult(ChallengeAccepted.Sender.Id, ChallengeAccepted.Game.Id, selectedLevel.Difficulty, TotalScore, ChallengeAccepted.ScoreSender, null);
				ApiClient.UpdateScoreRecipientForChallenge(ChallengeAccepted.Id, TotalScore, null);
			}
		}

		protected IEnumerator doUnlockLevelSequence()
		{
			string levelString = string.Empty;
			do
			{
				for (int i = 0; i < 10; i++)
				{
					if (Input.GetKeyDown(i.ToString()))
					{
						levelString += i;
					}
				}
				yield return null;
			}
			while (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
			int levelID2 = -1;
			if (int.TryParse(levelString, out levelID2))
			{
				levelID2--;
				if (levelID2 > 0 && levelID2 < Levels.Length)
				{
					Levels[levelID2].IsUnlocked = true;
				}
				GetView<StartView>().SetInfo(Levels, currentGameInfo);
			}
		}

		protected IEnumerator FadeOutGameAudio(AudioController controller, float fadeTime = 0.5f)
		{
			if (fadeTime > 0f)
			{
				for (float t = controller.Volume; t > 0f; t -= Time.unscaledDeltaTime / fadeTime)
				{
					controller.Volume = t;
					yield return null;
				}
			}
			controller.Volume = 0f;
		}

		protected IEnumerator FadeOutGameAudio(AudioCategory category, float fadeTime = 0.5f)
		{
			if (fadeTime > 0f)
			{
				for (float t = category.Volume; t > 0f; t -= Time.unscaledDeltaTime / fadeTime)
				{
					category.Volume = t;
					yield return null;
				}
			}
			category.Volume = 0f;
		}

		protected IEnumerator FadeInGameAudio(AudioController controller, float fadeTime = 0.5f)
		{
			if (fadeTime > 0f)
			{
				for (float t = controller.Volume; t < 1f; t += Time.unscaledDeltaTime / fadeTime)
				{
					controller.Volume = t;
					yield return null;
				}
			}
			controller.Volume = 1f;
		}

		protected IEnumerator FadeInGameAudio(AudioCategory category, float fadeTime = 0.5f)
		{
			if (fadeTime > 0f)
			{
				for (float t = category.Volume; t < 1f; t += Time.unscaledDeltaTime / fadeTime)
				{
					category.Volume = t;
					yield return null;
				}
			}
			category.Volume = 1f;
		}

		private void restartAndPlayImmediately(LevelSetting level)
		{
			List<LevelSetting> list = Levels.ToList();
			if (list.Contains(level))
			{
				queuedLevelIndex = list.IndexOf(level) + 1;
			}
			if (!hasRunIntroduction.Contains(GameId))
			{
				hasRunIntroduction.Add(GameId);
			}
			reloadScene();
		}

		private void reloadScene()
		{
			if (StateMachine.CurrentState.Name == "Running")
			{
				TrackingEvent trackingEvent = new TrackingEvent();
				trackingEvent.Type = TrackingEvent.TrackingType.GameQuit;
				trackingEvent.Arguments = new Dictionary<string, object>
				{
					{ "GameId", GameId },
					{ "Difficulty", selectedLevel.Difficulty },
					{
						"Time",
						Time.timeSinceLevelLoad
					},
					{ "LocationName", currentLocationInfo.Name },
					{ "GameName", currentGameInfo.Name }
				};
				GameEvents.Invoke(trackingEvent);
			}
			Time.timeScale = 1f;
			Application.LoadLevel(Application.loadedLevelName);
		}

		public virtual void OpenChallenge()
		{
			DataStorage.GetFriends(_003COpenChallenge_003Em__18);
		}

		public virtual void CloseChallenge()
		{
			CloseView<ChallengeView>();
		}

		public virtual void OpenHighscores()
		{
			OpenView<HighscoresView>().SetInfo(selectedLevel);
			ApiClient.GetHighscores(GameId, selectedLevel.Difficulty, HandleHighscores);
		}

		public virtual void CloseHighscores()
		{
			CloseView<HighscoresView>();
		}

		public virtual void HandleHighscores(HighScore[] highscores)
		{
			GetView<HighscoresView>().SetHighscores(highscores);
		}

		public void StartChallengeOtherPlayer(UserProfile otherPlayer)
		{
			ChallengeUser = otherPlayer;
			IsChallengingOtherPlayer = true;
			TrackingEvent trackingEvent = new TrackingEvent();
			trackingEvent.Type = TrackingEvent.TrackingType.ChallengeRequested;
			trackingEvent.Arguments = new Dictionary<string, object>
			{
				{
					"Sender",
					ApiClient.UserId
				},
				{ "Recipient", ChallengeUser.Id },
				{ "GameId", GameId },
				{ "Difficulty", selectedLevel.Difficulty }
			};
			GameEvents.Invoke(trackingEvent);
			ApiClient.ChallengeFriend(ChallengeUser.Id, GameId, TotalScore, selectedLevel.Difficulty, null);
			if (IsViewOpen<ChallengeView>())
			{
				GetView<ChallengeView>().OnFriendChallenged(otherPlayer);
			}
			GetView<SuccesView>().OnFriendChallenged();
		}

		[CompilerGenerated]
		private static int _003Cget_TotalScore_003Em__E(KeyValuePair<string, int> s)
		{
			return s.Value;
		}

		[CompilerGenerated]
		private void _003CStart_003Em__F(Location[] locs)
		{
			currentLocationInfo = locs.FirstOrDefault(_003CStart_003Em__19);
			currentGameInfo = currentLocationInfo.GetGame(GameId);
			DataStorage.GetProgressionData(onProgressionDataLoaded);
		}

		[CompilerGenerated]
		private void _003CStart_003Em__10(WebConfiguration config)
		{
			cheatsEnabled = config.CheatsEnabled;
		}

		[CompilerGenerated]
		private bool _003ConProgressionDataLoaded_003Em__11(UserGameDetails g)
		{
			return g.GameId == GameId;
		}

		[CompilerGenerated]
		private static bool _003ConProgressionDataLoaded_003Em__13(LevelSetting l)
		{
			return l.IsUnlocked;
		}

		[CompilerGenerated]
		private static bool _003ConProgressionDataLoaded_003Em__15(LevelSetting l)
		{
			return l.IsUnlocked;
		}

		[CompilerGenerated]
		private void _003COnEnterStateIntroduction_003Em__16()
		{
			hasRunIntroduction.Add(GameId);
			StateMachine.SwitchTo("Ready to begin");
		}

		[CompilerGenerated]
		private void _003ConScoresSubmitted_003Em__17(UserGameDetails[] oldProgressionData)
		{
			_003ConScoresSubmitted_003Ec__AnonStorey157 _003ConScoresSubmitted_003Ec__AnonStorey = new _003ConScoresSubmitted_003Ec__AnonStorey157();
			_003ConScoresSubmitted_003Ec__AnonStorey.oldProgressionData = oldProgressionData;
			_003ConScoresSubmitted_003Ec__AnonStorey._003C_003Ef__this = this;
			DataStorage.GetProgressionData(_003ConScoresSubmitted_003Ec__AnonStorey._003C_003Em__1A, true);
		}

		[CompilerGenerated]
		private void _003COpenChallenge_003Em__18(UserProfile[] friends)
		{
			OpenView<ChallengeView>().SetData(selectedLevel.Difficulty);
			GetView<ChallengeView>().SetFriends(friends);
		}

		[CompilerGenerated]
		private bool _003CStart_003Em__19(Location l)
		{
			return l.Games.Any(_003CStart_003Em__1B);
		}

		[CompilerGenerated]
		private bool _003CStart_003Em__1B(Game loc)
		{
			return loc.Id == GameId;
		}
	}
}
