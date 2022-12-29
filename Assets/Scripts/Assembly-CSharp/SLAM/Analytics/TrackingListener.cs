using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.BuildSystem;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Analytics
{
	public class TrackingListener : MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class _003ConTrackingEvent_003Ec__AnonStorey18A
		{
			internal TrackingEvent evt;

			internal TrackingListener _003C_003Ef__this;

			internal void _003C_003Em__E6(WebConfiguration config)
			{
				_003C_003Ef__this.ga = _003C_003Ef__this.gameObject.AddComponent<GoogleAnalyticsV3>();
				_003C_003Ef__this.ga.InitializeWithTrackingCode(config.GoogleAnalyticsTrackingCode, "Duckworld", "com.sanoma.duckworld", SceneDataLibrary.GetSceneDataLibrary().GameVersion.ToString());
				_003C_003Ef__this.onTrackingEvent(evt);
			}
		}

		private GoogleAnalyticsV3 ga;

		private void Awake()
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}

		private void OnEnable()
		{
			GameEvents.Subscribe<TrackingEvent>(onTrackingEvent);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<TrackingEvent>(onTrackingEvent);
		}

		private void onLogMessagesReceived(string condition, string stackTrace, LogType type)
		{
			if (type == LogType.Error || type == LogType.Exception || type == LogType.Warning)
			{
				Application.ExternalCall("TRACKING.GAME.TrackException", condition, stackTrace, type.ToString());
			}
		}

		private void onTrackingEvent(TrackingEvent evt)
		{
			_003ConTrackingEvent_003Ec__AnonStorey18A _003ConTrackingEvent_003Ec__AnonStorey18A = new _003ConTrackingEvent_003Ec__AnonStorey18A();
			_003ConTrackingEvent_003Ec__AnonStorey18A.evt = evt;
			_003ConTrackingEvent_003Ec__AnonStorey18A._003C_003Ef__this = this;
			if (ga == null)
			{
				DataStorage.GetWebConfiguration(_003ConTrackingEvent_003Ec__AnonStorey18A._003C_003Em__E6);
				return;
			}
			trackSat(_003ConTrackingEvent_003Ec__AnonStorey18A.evt);
			switch (_003ConTrackingEvent_003Ec__AnonStorey18A.evt.Type)
			{
			case TrackingEvent.TrackingType.HubOpened:
				DoTrackPageview(ga, "/game/hub");
				break;
			case TrackingEvent.TrackingType.LocationOpened:
				DoTrackPageview(ga, "/game/" + _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["LocationName"]);
				break;
			case TrackingEvent.TrackingType.StartViewOpened:
				DoTrackPageview(ga, GetPageViewUrl(_003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments, string.Empty), _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments);
				break;
			case TrackingEvent.TrackingType.GameStart:
				DoTrackPageview(ga, GetPageViewUrl(_003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments, "game"), _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments);
				DoTrackEvent(ga, _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["LocationName"].ToString(), string.Concat(_003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["GameName"], " start"), string.Concat(_003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["GameName"], " ", _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["Difficulty"]), 0L, _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments);
				break;
			case TrackingEvent.TrackingType.GameQuit:
				DoTrackPageview(ga, GetPageViewUrl(_003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments, "game"), _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments);
				DoTrackEvent(ga, _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["LocationName"].ToString(), string.Concat(_003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["GameName"], " quit"), string.Concat(_003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["GameName"], " ", _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["Difficulty"]), 0L, _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments);
				break;
			case TrackingEvent.TrackingType.GameWon:
				DoTrackPageview(ga, GetPageViewUrl(_003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments, "game"), _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments);
				DoTrackEvent(ga, _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["LocationName"].ToString(), string.Concat(_003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["GameName"], " won"), string.Concat(_003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["GameName"], " ", _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["Difficulty"]), 0L, _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments);
				break;
			case TrackingEvent.TrackingType.GameLost:
				DoTrackPageview(ga, GetPageViewUrl(_003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments, "game"), _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments);
				DoTrackEvent(ga, _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["LocationName"].ToString(), string.Concat(_003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["GameName"], " lost"), string.Concat(_003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["GameName"], " ", _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["Difficulty"]), 0L, _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments);
				break;
			case TrackingEvent.TrackingType.ItemBought:
				DoTrackEvent(ga, "Game", "Store", "buy-" + _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["ItemGUID"], (int)_003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["Price"], _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments);
				break;
			case TrackingEvent.TrackingType.FriendshipRequested:
				DoTrackEvent(ga, "game", "profile", "friend request send");
				break;
			case TrackingEvent.TrackingType.FriendshipAccepted:
				DoTrackEvent(ga, "game", "friends", string.Concat("accept-", _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["Sender"], "-", _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["Recipient"]));
				break;
			case TrackingEvent.TrackingType.FriendshipRejected:
				DoTrackEvent(ga, "game", "friends", string.Concat("decline-", _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["Sender"], "-", _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["Recipient"]));
				break;
			case TrackingEvent.TrackingType.LoadComplete:
				DoTrackEvent(ga, "duckworld", "Application Loaded", _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["Version"].ToString(), 0L, _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments);
				break;
			case TrackingEvent.TrackingType.BackToMenuButton:
			case TrackingEvent.TrackingType.GameRestart:
			case TrackingEvent.TrackingType.GamePause:
			case TrackingEvent.TrackingType.GameResume:
				DoTrackEvent(ga, _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["LocationName"].ToString(), string.Concat(_003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["GameName"], " ", _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Type), string.Concat(_003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["GameName"], " ", _003ConTrackingEvent_003Ec__AnonStorey18A.evt.Arguments["Difficulty"]));
				break;
			case TrackingEvent.TrackingType.PlayerJournalOpened:
			case TrackingEvent.TrackingType.ViewOpened:
			case TrackingEvent.TrackingType.ViewClosed:
			case TrackingEvent.TrackingType.AvatarCreated:
			case TrackingEvent.TrackingType.AvatarSaved:
			case TrackingEvent.TrackingType.DuckcoinsEarned:
			case TrackingEvent.TrackingType.ChallengeRequested:
			case TrackingEvent.TrackingType.ChallengeCompleted:
			case TrackingEvent.TrackingType.ChallengeRejected:
			case TrackingEvent.TrackingType.MotionComicOpened:
				break;
			}
		}

		private void trackSat(TrackingEvent evt)
		{
		}

		private void DoTrackEvent(GoogleAnalyticsV3 ga, string category, string action, string label, long value = 0, Dictionary<string, object> arguments = null)
		{
			ga.LogEvent(buildHit<EventHitBuilder>(arguments).SetEventCategory(category).SetEventAction(action).SetEventLabel(label)
				.SetEventValue(value));
		}

		private void DoTrackPageview(GoogleAnalyticsV3 ga, string url, Dictionary<string, object> arguments = null)
		{
			ga.LogScreen(buildHit<AppViewHitBuilder>(arguments).SetScreenName(url));
		}

		private string GetPageViewUrl(Dictionary<string, object> arguments, string suffix = "")
		{
			return string.Format("/game/{0}/{1}/{2}", arguments["LocationName"], arguments["GameName"], suffix);
		}

		private T buildHit<T>(Dictionary<string, object> arguments) where T : HitBuilder<T>, new()
		{
			T result = new T();
			if (UserProfile.Current.CustomDimensions != null)
			{
				foreach (KeyValuePair<string, string> customDimension in UserProfile.Current.CustomDimensions)
				{
					if (customDimension.Key.Contains("dimension"))
					{
						int result2 = 0;
						if (int.TryParse(customDimension.Key.Replace("dimension", string.Empty), out result2) && !result.GetCustomDimensions().ContainsKey(result2))
						{
							result.SetCustomDimension(result2, customDimension.Value);
						}
					}
				}
			}
			if (arguments != null)
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				dictionary.Add("GameId", 13);
				dictionary.Add("Difficulty", 16);
				Dictionary<string, int> dictionary2 = dictionary;
				dictionary = new Dictionary<string, int>();
				dictionary.Add("Score", 3);
				dictionary.Add("Coins", 6);
				dictionary.Add("LoadingTime", 1);
				dictionary.Add("UserLoggedIn", 9);
				Dictionary<string, int> dictionary3 = dictionary;
				{
					foreach (KeyValuePair<string, object> argument in arguments)
					{
						if (dictionary2.ContainsKey(argument.Key))
						{
							if (!result.GetCustomDimensions().ContainsKey(dictionary2[argument.Key]))
							{
								result.SetCustomDimension(dictionary2[argument.Key], argument.Value.ToString());
							}
						}
						else if (dictionary3.ContainsKey(argument.Key))
						{
							result.SetCustomMetric(dictionary3[argument.Key], argument.Value.ToString());
						}
					}
					return result;
				}
			}
			return result;
		}
	}
}
