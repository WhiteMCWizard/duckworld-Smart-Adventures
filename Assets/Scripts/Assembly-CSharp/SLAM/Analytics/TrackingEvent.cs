using System.Collections.Generic;
using SLAM.BuildSystem;

namespace SLAM.Analytics
{
	public class TrackingEvent
	{
		public enum TrackingType
		{
			HubOpened = 0,
			PlayerJournalOpened = 1,
			LocationOpened = 2,
			StartViewOpened = 3,
			ViewOpened = 4,
			ViewClosed = 5,
			GameStart = 6,
			GameQuit = 7,
			GameWon = 8,
			GameLost = 9,
			AvatarCreated = 10,
			AvatarSaved = 11,
			DuckcoinsEarned = 12,
			ItemBought = 13,
			FriendshipRequested = 14,
			FriendshipAccepted = 15,
			FriendshipRejected = 16,
			ChallengeRequested = 17,
			ChallengeCompleted = 18,
			ChallengeRejected = 19,
			MotionComicOpened = 20,
			LoadComplete = 21,
			BackToMenuButton = 22,
			GameRestart = 23,
			GamePause = 24,
			GameResume = 25,
			KartBoughtEvent = 26,
			KartCustomizedEvent = 27
		}

		public TrackingType Type;

		private Dictionary<string, object> arguments = new Dictionary<string, object>();

		public Dictionary<string, object> Arguments
		{
			get
			{
				return arguments;
			}
			set
			{
				if (!value.ContainsKey("Version"))
				{
					value.Add("Version", SceneDataLibrary.GetSceneDataLibrary().GameVersion.ToString());
				}
				arguments = value;
			}
		}
	}
}
