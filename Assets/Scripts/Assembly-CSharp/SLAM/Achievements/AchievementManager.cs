using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Analytics;
using SLAM.Avatar;
using SLAM.Notifications;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Achievements
{
	public class AchievementManager : SingletonMonobehaviour<AchievementManager>
	{
		public enum AchievementId
		{
			COMPLETE_MONEYDIVE = 14,
			COMPLETE_HIGHERTHAN = 15,
			COMPLETE_HANGMAN = 16,
			COMPLETE_TRANSLATETHIS = 17,
			COMPLETE_KARTRACING = 18,
			COMPLETE_KARTRACING_TIMETRIAL = 64,
			COMPLETE_ASSEMBLYLINE = 34,
			COMPLETE_FRUITYARD = 35,
			COMPLETE_TRAINSPOTTING = 36,
			COMPLETE_CRATEMESS = 37,
			COMPLETE_CRANEOPERATOR = 41,
			COMPLETE_CHASETHEBOAT = 42,
			COMPLETE_CHASETHETRUCK = 45,
			COMPLETE_PUSHTHECRATE = 48,
			COMPLETE_ZOOTRANSPORT = 49,
			COMPLETE_ADVENTURE_1 = 51,
			COMPLETE_JOB_EASY = 32,
			COMPLETE_JOB_MEDIUM = 33,
			EARN_COINS_EASY = 38,
			EARN_COINS_MEDIUM = 39,
			EARN_COINS_HARD = 40,
			PLAYDUCKWORLD_EASY = 12,
			PLAYDUCKWORLD_MEDIUM = 13,
			CUSTOMIZE_AVATAR = 23,
			BUY_CLOTHES_EASY = 24,
			BUY_CLOTHES_MEDIUM = 25,
			GET_FRIENDS_EASY = 26,
			GET_FRIENDS_MEDIUM = 27,
			CUSTOMIZE_KART = 21,
			COLLECT_KART_EASY = 22,
			MEETCHARACTERS_EASY = 10,
			MEETCHARACTERS_MEDIUM = 11,
			FINISH_KARTRACING_EASY = 19,
			FINISH_KARTRACING_MEDIUM = 20,
			ACCEPT_CHALLENGES_EASY = 28,
			ACCEPT_CHALLENGES_MEDIUM = 29,
			WIN_CHALLENGES_EASY = 30,
			WIN_CHALLENGES_MEDIUM = 31,
			CHASETHEBOAT_COMPLETE_NOHEARTLOST = 43,
			CHASETHEBOAT_COLLECT_FEATHERS = 44,
			CHASETHETRUCK_COMPLETE_NOHEARTLOST = 46,
			CHASETHETRUCK_COLLECT_FEATHERS = 47,
			ZOOTRANSPORT_COMPLETE_NOHEARTLOST = 50,
			COMPLETE_CONNECT_THE_PIPES = 58,
			COMPLETE_BEAT_THE_BEAGLEBOYS = 59,
			COMPLETE_BATCAVE = 60,
			COMPLETE_JUMP_THE_CROC = 61,
			COMPLETE_MONKEY_BATTLE = 62,
			COMPLETE_ADVENTURE_2 = 63,
			COMPLETE_DUCKQUIZ = 65,
			COMPLETE_ALL_ADVENTURES = 66
		}

		[CompilerGenerated]
		private sealed class _003CStart_003Ec__Iterator109 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal int _0024PC;

			internal object _0024current;

			internal AchievementManager _003C_003Ef__this;

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
				//Discarded unreachable code: IL_00b4
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					UnityEngine.Object.DontDestroyOnLoad(_003C_003Ef__this.gameObject);
					goto case 1u;
				case 1u:
					if (UserProfile.Current == null)
					{
						_0024current = null;
						_0024PC = 1;
						return true;
					}
					if (UserProfile.Current.IsFree)
					{
						UnityEngine.Object.Destroy(_003C_003Ef__this.gameObject);
						break;
					}
					_003C_003Ef__this.achievementRequest = ApiClient.GetAchievements(_003C_003Em__E3);
					GameEvents.Subscribe<TrackingEvent>(_003C_003Ef__this.onTrackingEvent);
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

			internal void _003C_003Em__E3(UserAchievement[] result)
			{
				_003C_003Ef__this.Achievements = new List<UserAchievement>(result);
				foreach (UserAchievement achievement in _003C_003Ef__this.Achievements)
				{
					achievement.Info.Target = ((!targets.ContainsKey((AchievementId)achievement.Info.Id)) ? 1 : targets[(AchievementId)achievement.Info.Id]);
				}
				_003C_003Ef__this.checkStartDuckworldAchievement();
			}
		}

		[CompilerGenerated]
		private sealed class _003ConTrackingEvent_003Ec__AnonStorey187
		{
			internal TrackingEvent evt;

			internal AchievementManager _003C_003Ef__this;

			internal void _003C_003Em__DF()
			{
				if (evt.Type == TrackingEvent.TrackingType.GameWon)
				{
					int gameId = (int)evt.Arguments["GameId"];
					float progress = (float)evt.Arguments["Progress"];
					_003C_003Ef__this.checkGameCompleteAchievement(gameId, progress);
					_003C_003Ef__this.checkJobCompleteAchievement(gameId);
					_003C_003Ef__this.checkAllAdventureGamesComplete();
				}
				else if (evt.Type == TrackingEvent.TrackingType.DuckcoinsEarned)
				{
					int num = (int)evt.Arguments["Amount"];
					_003C_003Ef__this.increaseProgress(AchievementId.EARN_COINS_EASY, num);
					_003C_003Ef__this.increaseProgress(AchievementId.EARN_COINS_MEDIUM, num);
					_003C_003Ef__this.increaseProgress(AchievementId.EARN_COINS_HARD, num);
				}
				else if (evt.Type == TrackingEvent.TrackingType.ItemBought)
				{
					string guid = (string)evt.Arguments["ItemGUID"];
					_003C_003Ef__this.checkBuyClothesAchievement(guid);
				}
				else if (evt.Type == TrackingEvent.TrackingType.FriendshipAccepted)
				{
					_003C_003Ef__this.increaseProgressByOne(AchievementId.GET_FRIENDS_EASY);
					_003C_003Ef__this.increaseProgressByOne(AchievementId.GET_FRIENDS_MEDIUM);
				}
				else if (evt.Type == TrackingEvent.TrackingType.AvatarSaved)
				{
					_003C_003Ef__this.complete(AchievementId.CUSTOMIZE_AVATAR);
				}
				else if (evt.Type == TrackingEvent.TrackingType.KartBoughtEvent)
				{
					_003C_003Ef__this.increaseProgressByOne(AchievementId.CUSTOMIZE_KART);
				}
				else if (evt.Type == TrackingEvent.TrackingType.KartCustomizedEvent)
				{
					_003C_003Ef__this.complete(AchievementId.CUSTOMIZE_KART);
				}
				else if (evt.Type == TrackingEvent.TrackingType.MotionComicOpened)
				{
					_003C_003Ef__this.checkAdventureComplete((string)evt.Arguments["Game"]);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CcheckJobCompleteAchievement_003Ec__AnonStorey188
		{
			internal int gameId;

			internal AchievementManager _003C_003Ef__this;

			internal void _003C_003Em__E1(Location[] locs)
			{
				Location location = locs.FirstOrDefault(_003C_003Em__E4);
				Game game = location.GetGame(gameId);
				if (game.Type == Game.GameType.Job)
				{
					_003C_003Ef__this.increaseProgressByOne(AchievementId.COMPLETE_JOB_EASY);
					_003C_003Ef__this.increaseProgressByOne(AchievementId.COMPLETE_JOB_MEDIUM);
				}
			}

			internal bool _003C_003Em__E4(Location l)
			{
				return l.Games.Any(_003C_003Em__E5);
			}

			internal bool _003C_003Em__E5(Game loc)
			{
				return loc.Id == gameId;
			}
		}

		[CompilerGenerated]
		private sealed class _003CgetAchievement_003Ec__AnonStorey189
		{
			internal AchievementId id;

			internal bool _003C_003Em__E2(UserAchievement a)
			{
				return a.Info.Type == id;
			}
		}

		private static Dictionary<AchievementId, int> targets = new Dictionary<AchievementId, int>
		{
			{
				AchievementId.COMPLETE_CONNECT_THE_PIPES,
				15
			},
			{
				AchievementId.COMPLETE_BEAT_THE_BEAGLEBOYS,
				5
			},
			{
				AchievementId.COMPLETE_BATCAVE,
				5
			},
			{
				AchievementId.COMPLETE_JUMP_THE_CROC,
				15
			},
			{
				AchievementId.COMPLETE_MONKEY_BATTLE,
				5
			},
			{
				AchievementId.COMPLETE_MONEYDIVE,
				15
			},
			{
				AchievementId.COMPLETE_HIGHERTHAN,
				15
			},
			{
				AchievementId.COMPLETE_HANGMAN,
				15
			},
			{
				AchievementId.COMPLETE_TRANSLATETHIS,
				15
			},
			{
				AchievementId.COMPLETE_KARTRACING,
				10
			},
			{
				AchievementId.COMPLETE_KARTRACING_TIMETRIAL,
				6
			},
			{
				AchievementId.COMPLETE_ASSEMBLYLINE,
				15
			},
			{
				AchievementId.COMPLETE_FRUITYARD,
				15
			},
			{
				AchievementId.COMPLETE_TRAINSPOTTING,
				15
			},
			{
				AchievementId.COMPLETE_CRATEMESS,
				15
			},
			{
				AchievementId.COMPLETE_CRANEOPERATOR,
				15
			},
			{
				AchievementId.COMPLETE_CHASETHEBOAT,
				5
			},
			{
				AchievementId.COMPLETE_CHASETHETRUCK,
				3
			},
			{
				AchievementId.COMPLETE_PUSHTHECRATE,
				3
			},
			{
				AchievementId.COMPLETE_ZOOTRANSPORT,
				15
			},
			{
				AchievementId.COMPLETE_DUCKQUIZ,
				15
			},
			{
				AchievementId.MEETCHARACTERS_EASY,
				3
			},
			{
				AchievementId.MEETCHARACTERS_MEDIUM,
				10
			},
			{
				AchievementId.PLAYDUCKWORLD_EASY,
				5
			},
			{
				AchievementId.PLAYDUCKWORLD_MEDIUM,
				25
			},
			{
				AchievementId.FINISH_KARTRACING_EASY,
				25
			},
			{
				AchievementId.FINISH_KARTRACING_MEDIUM,
				100
			},
			{
				AchievementId.CUSTOMIZE_KART,
				1
			},
			{
				AchievementId.COLLECT_KART_EASY,
				5
			},
			{
				AchievementId.CUSTOMIZE_AVATAR,
				1
			},
			{
				AchievementId.BUY_CLOTHES_EASY,
				10
			},
			{
				AchievementId.BUY_CLOTHES_MEDIUM,
				25
			},
			{
				AchievementId.GET_FRIENDS_EASY,
				5
			},
			{
				AchievementId.GET_FRIENDS_MEDIUM,
				25
			},
			{
				AchievementId.ACCEPT_CHALLENGES_EASY,
				10
			},
			{
				AchievementId.ACCEPT_CHALLENGES_MEDIUM,
				25
			},
			{
				AchievementId.WIN_CHALLENGES_EASY,
				10
			},
			{
				AchievementId.WIN_CHALLENGES_MEDIUM,
				50
			},
			{
				AchievementId.COMPLETE_JOB_EASY,
				1
			},
			{
				AchievementId.COMPLETE_JOB_MEDIUM,
				10
			},
			{
				AchievementId.EARN_COINS_EASY,
				100
			},
			{
				AchievementId.EARN_COINS_MEDIUM,
				500
			},
			{
				AchievementId.EARN_COINS_HARD,
				1000
			},
			{
				AchievementId.CHASETHEBOAT_COMPLETE_NOHEARTLOST,
				3
			},
			{
				AchievementId.CHASETHEBOAT_COLLECT_FEATHERS,
				3
			},
			{
				AchievementId.CHASETHETRUCK_COMPLETE_NOHEARTLOST,
				3
			},
			{
				AchievementId.CHASETHETRUCK_COLLECT_FEATHERS,
				3
			},
			{
				AchievementId.ZOOTRANSPORT_COMPLETE_NOHEARTLOST,
				15
			},
			{
				AchievementId.COMPLETE_ALL_ADVENTURES,
				10
			}
		};

		private WebRequest achievementRequest;

		public List<UserAchievement> Achievements { get; protected set; }

		private void OnEnable()
		{
			GameEvents.Subscribe<Webservice.LogoutEvent>(onLogout);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<Webservice.LogoutEvent>(onLogout);
		}

		private IEnumerator Start()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			while (UserProfile.Current == null)
			{
				yield return null;
			}
			if (UserProfile.Current.IsFree)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				yield break;
			}
			achievementRequest = ApiClient.GetAchievements(((_003CStart_003Ec__Iterator109)(object)this)._003C_003Em__E3);
			GameEvents.Subscribe<TrackingEvent>(onTrackingEvent);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (UserProfile.Current != null && !UserProfile.Current.IsFree)
			{
				GameEvents.Unsubscribe<TrackingEvent>(onTrackingEvent);
			}
		}

		private void onLogout(Webservice.LogoutEvent evt)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		private void onTrackingEvent(TrackingEvent evt)
		{
			_003ConTrackingEvent_003Ec__AnonStorey187 _003ConTrackingEvent_003Ec__AnonStorey = new _003ConTrackingEvent_003Ec__AnonStorey187();
			_003ConTrackingEvent_003Ec__AnonStorey.evt = evt;
			_003ConTrackingEvent_003Ec__AnonStorey._003C_003Ef__this = this;
			Webservice.WaitFor(_003ConTrackingEvent_003Ec__AnonStorey._003C_003Em__DF, achievementRequest);
		}

		private void checkAdventureComplete(string gameName)
		{
			if (gameName == "MC_ADV01_07_Outro")
			{
				complete(AchievementId.COMPLETE_ADVENTURE_1);
			}
			else if (gameName == "MC_ADV02_07_Outro")
			{
				complete(AchievementId.COMPLETE_ADVENTURE_2);
			}
		}

		private void checkAllAdventureGamesComplete()
		{
			AchievementId[] array = new AchievementId[10]
			{
				AchievementId.COMPLETE_CONNECT_THE_PIPES,
				AchievementId.COMPLETE_BEAT_THE_BEAGLEBOYS,
				AchievementId.COMPLETE_BATCAVE,
				AchievementId.COMPLETE_JUMP_THE_CROC,
				AchievementId.COMPLETE_MONKEY_BATTLE,
				AchievementId.COMPLETE_CRANEOPERATOR,
				AchievementId.COMPLETE_CHASETHEBOAT,
				AchievementId.COMPLETE_CHASETHETRUCK,
				AchievementId.COMPLETE_PUSHTHECRATE,
				AchievementId.COMPLETE_ZOOTRANSPORT
			};
			float progress = (float)array.Count(_003CcheckAllAdventureGamesComplete_003Em__E0) / (float)array.Length;
			setProgress(AchievementId.COMPLETE_ALL_ADVENTURES, progress);
		}

		private void checkStartDuckworldAchievement()
		{
			increaseProgressByOne(AchievementId.PLAYDUCKWORLD_EASY);
			increaseProgressByOne(AchievementId.PLAYDUCKWORLD_MEDIUM);
		}

		private void checkBuyClothesAchievement(string guid)
		{
			if (AvatarItemLibrary.GetItemLibrary(AvatarSystem.GetPlayerConfiguration()).GetItemByGUID(guid) != null)
			{
				increaseProgressByOne(AchievementId.BUY_CLOTHES_EASY);
				increaseProgressByOne(AchievementId.BUY_CLOTHES_MEDIUM);
			}
		}

		private void checkGameCompleteAchievement(int gameId, float progress)
		{
			AchievementId achievementId = AchievementId.COMPLETE_ASSEMBLYLINE;
			switch (gameId)
			{
			case 2:
				achievementId = AchievementId.COMPLETE_MONEYDIVE;
				break;
			case 23:
				achievementId = AchievementId.COMPLETE_HIGHERTHAN;
				break;
			case 24:
				achievementId = AchievementId.COMPLETE_HANGMAN;
				break;
			case 33:
				achievementId = AchievementId.COMPLETE_TRANSLATETHIS;
				break;
			case 11:
				achievementId = AchievementId.COMPLETE_KARTRACING;
				break;
			case 39:
				achievementId = AchievementId.COMPLETE_KARTRACING_TIMETRIAL;
				break;
			case 12:
				achievementId = AchievementId.COMPLETE_ASSEMBLYLINE;
				break;
			case 10:
				achievementId = AchievementId.COMPLETE_FRUITYARD;
				break;
			case 30:
				achievementId = AchievementId.COMPLETE_TRAINSPOTTING;
				break;
			case 26:
				achievementId = AchievementId.COMPLETE_CRATEMESS;
				break;
			case 5:
				achievementId = AchievementId.COMPLETE_CRANEOPERATOR;
				break;
			case 6:
				achievementId = AchievementId.COMPLETE_CHASETHEBOAT;
				break;
			case 7:
				achievementId = AchievementId.COMPLETE_CHASETHETRUCK;
				break;
			case 8:
				achievementId = AchievementId.COMPLETE_PUSHTHECRATE;
				break;
			case 9:
				achievementId = AchievementId.COMPLETE_ZOOTRANSPORT;
				break;
			case 4:
				achievementId = AchievementId.COMPLETE_CONNECT_THE_PIPES;
				break;
			case 16:
				achievementId = AchievementId.COMPLETE_BEAT_THE_BEAGLEBOYS;
				break;
			case 27:
				achievementId = AchievementId.COMPLETE_BATCAVE;
				break;
			case 1:
				achievementId = AchievementId.COMPLETE_JUMP_THE_CROC;
				break;
			case 28:
				achievementId = AchievementId.COMPLETE_MONKEY_BATTLE;
				break;
			case 36:
				achievementId = AchievementId.COMPLETE_DUCKQUIZ;
				break;
			default:
				UnityEngine.Debug.LogWarning("Hey buddy, this game doesnt have an Complete achievement?");
				break;
			}
			if (getAchievement(achievementId).Progress < progress)
			{
				setProgress(achievementId, progress);
			}
		}

		private void checkJobCompleteAchievement(int gameId)
		{
			_003CcheckJobCompleteAchievement_003Ec__AnonStorey188 _003CcheckJobCompleteAchievement_003Ec__AnonStorey = new _003CcheckJobCompleteAchievement_003Ec__AnonStorey188();
			_003CcheckJobCompleteAchievement_003Ec__AnonStorey.gameId = gameId;
			_003CcheckJobCompleteAchievement_003Ec__AnonStorey._003C_003Ef__this = this;
			DataStorage.GetLocationsData(_003CcheckJobCompleteAchievement_003Ec__AnonStorey._003C_003Em__E1);
		}

		private void increaseProgress(AchievementId aid, float amount)
		{
			float progressNormalized = getProgressNormalized(aid);
			setProgressNormalized(aid, progressNormalized + amount);
		}

		private void increaseProgressByOne(AchievementId achievementId)
		{
			UserAchievement achievement = getAchievement(achievementId);
			int num = Mathf.CeilToInt(achievement.Progress * (float)achievement.Info.Target);
			num++;
			setProgress(achievementId, Mathf.Clamp01((float)num / (float)achievement.Info.Target));
		}

		private void setProgressNormalized(AchievementId achievementId, float progress)
		{
			setProgress(achievementId, progress / (float)getAchievement(achievementId).Info.Target);
		}

		private float getProgressNormalized(AchievementId achievementId)
		{
			return (float)getAchievement(achievementId).Info.Target * getAchievement(achievementId).Progress;
		}

		private void complete(AchievementId achievementId)
		{
			setProgress(achievementId, 1f);
		}

		private void setProgress(AchievementId achievementId, float progress)
		{
			UserAchievement achievement = getAchievement(achievementId);
			if (achievement == null)
			{
				UnityEngine.Debug.Log("Failed to set progress " + achievementId);
				return;
			}
			bool completed = achievement.Completed;
			if (achievement.Progress < 1f && progress >= 1f)
			{
				achievement.Completed = true;
			}
			if (!Mathf.Approximately(progress, achievement.Progress))
			{
				achievement.Progress = progress;
				ApiClient.SetAchievementProgress(achievement, null);
			}
			if (completed != achievement.Completed)
			{
				fire(achievement);
			}
		}

		private void fire(UserAchievement achievement)
		{
			AchievementCompletedEvent achievementCompletedEvent = new AchievementCompletedEvent();
			achievementCompletedEvent.Achievement = achievement;
			GameEvents.Invoke(achievementCompletedEvent);
			NotificationEvent notificationEvent = new NotificationEvent();
			notificationEvent.Title = Localization.Get(achievement.Info.Name);
			notificationEvent.Body = StringFormatter.GetLocalizationFormatted(achievement.Info.Description, achievement.Info.Target);
			notificationEvent.IconSpriteName = AchievementToIcon(achievement);
			GameEvents.Invoke(notificationEvent);
		}

		private UserAchievement getAchievement(AchievementId id)
		{
			_003CgetAchievement_003Ec__AnonStorey189 _003CgetAchievement_003Ec__AnonStorey = new _003CgetAchievement_003Ec__AnonStorey189();
			_003CgetAchievement_003Ec__AnonStorey.id = id;
			if (Achievements == null)
			{
				return null;
			}
			UserAchievement userAchievement = Achievements.FirstOrDefault(_003CgetAchievement_003Ec__AnonStorey._003C_003Em__E2);
			if (userAchievement == null)
			{
				UnityEngine.Debug.Log(string.Concat("Couldnt find achievement: ", _003CgetAchievement_003Ec__AnonStorey.id, " with id: ", (int)_003CgetAchievement_003Ec__AnonStorey.id));
			}
			return userAchievement;
		}

		public static string AchievementToIcon(UserAchievement achievement)
		{
			string empty = string.Empty;
			switch (achievement.Info.Id)
			{
			case 12:
			case 13:
			case 51:
				empty = "Achiev_Duckstad1";
				break;
			case 32:
			case 33:
				empty = "Achiev_Job1";
				break;
			case 38:
				empty = "Achiev_Duiten1";
				break;
			case 39:
			case 40:
				empty = "Achiev_Duiten2";
				break;
			case 21:
			case 22:
				empty = "Achiev_Zeepkist1";
				break;
			case 23:
			case 24:
				empty = "Achiev_Fashion1";
				break;
			case 25:
				empty = "Achiev_Fashion2";
				break;
			case 26:
			case 27:
				empty = "Achiev_Buur1";
				break;
			default:
				empty = "Achiev_Default_HighestLevel";
				break;
			}
			if (!achievement.Completed)
			{
				empty += "_Grey";
			}
			return empty;
		}

		[CompilerGenerated]
		private bool _003CcheckAllAdventureGamesComplete_003Em__E0(AchievementId ga)
		{
			return getAchievement(ga).Completed;
		}
	}
}
