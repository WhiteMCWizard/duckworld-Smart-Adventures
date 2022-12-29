using System;
using System.Runtime.CompilerServices;
using SLAM.Slinq;

namespace SLAM.Webservices
{
	public static class DataStorage
	{
		[CompilerGenerated]
		private sealed class _003CGetProgressionData_003Ec__AnonStorey198
		{
			internal Action<UserGameDetails[]> callback;

			internal void _003C_003Em__10F(UserGameDetails[] ugd)
			{
				userProgressionInfo = ugd;
				if (callback != null)
				{
					callback(userProgressionInfo);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetLocationsData_003Ec__AnonStorey199
		{
			internal Action<Location[]> callback;

			internal void _003C_003Em__110(Location[] l)
			{
				locations = l;
				callback(l);
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetGameById_003Ec__AnonStorey19A
		{
			internal int gameId;

			internal Action<Game> callback;

			internal void _003C_003Em__111(Location[] locations)
			{
				Location location = locations.FirstOrDefault(_003C_003Em__114);
				Game game = location.GetGame(gameId);
				if (callback != null)
				{
					callback(game);
				}
			}

			internal bool _003C_003Em__114(Location l)
			{
				return l.Games.Any(_003C_003Em__115);
			}

			internal bool _003C_003Em__115(Game g)
			{
				return g.Id == gameId;
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetFriends_003Ec__AnonStorey19B
		{
			internal Action<UserProfile[]> callback;

			internal void _003C_003Em__112(UserProfile[] frnds)
			{
				friends = frnds;
				if (callback != null)
				{
					callback(friends);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetWebConfiguration_003Ec__AnonStorey19C
		{
			internal Action<WebConfiguration> callback;

			internal void _003C_003Em__113(WebConfiguration cnfg)
			{
				config = cnfg;
				if (callback != null)
				{
					callback(config);
				}
			}
		}

		private static UserGameDetails[] userProgressionInfo;

		private static Location[] locations;

		private static UserProfile[] friends;

		private static WebConfiguration config;

		public static void DeleteAll()
		{
			config = null;
			friends = null;
			locations = null;
			userProgressionInfo = null;
		}

		public static void GetProgressionData(Action<UserGameDetails[]> callback, bool forceRefresh = false)
		{
			_003CGetProgressionData_003Ec__AnonStorey198 _003CGetProgressionData_003Ec__AnonStorey = new _003CGetProgressionData_003Ec__AnonStorey198();
			_003CGetProgressionData_003Ec__AnonStorey.callback = callback;
			if (userProgressionInfo != null && !forceRefresh && _003CGetProgressionData_003Ec__AnonStorey.callback != null)
			{
				_003CGetProgressionData_003Ec__AnonStorey.callback(userProgressionInfo);
			}
			else
			{
				ApiClient.GetUserSpecificDetailsForAllGames(_003CGetProgressionData_003Ec__AnonStorey._003C_003Em__10F);
			}
		}

		public static void GetLocationsData(Action<Location[]> callback)
		{
			_003CGetLocationsData_003Ec__AnonStorey199 _003CGetLocationsData_003Ec__AnonStorey = new _003CGetLocationsData_003Ec__AnonStorey199();
			_003CGetLocationsData_003Ec__AnonStorey.callback = callback;
			if (locations != null)
			{
				_003CGetLocationsData_003Ec__AnonStorey.callback(locations);
			}
			else
			{
				ApiClient.GetLocations(_003CGetLocationsData_003Ec__AnonStorey._003C_003Em__110);
			}
		}

		public static void GetGameById(int gameId, Action<Game> callback)
		{
			_003CGetGameById_003Ec__AnonStorey19A _003CGetGameById_003Ec__AnonStorey19A = new _003CGetGameById_003Ec__AnonStorey19A();
			_003CGetGameById_003Ec__AnonStorey19A.gameId = gameId;
			_003CGetGameById_003Ec__AnonStorey19A.callback = callback;
			GetLocationsData(_003CGetGameById_003Ec__AnonStorey19A._003C_003Em__111);
		}

		public static void GetFriends(Action<UserProfile[]> callback, bool forceRefresh = false)
		{
			_003CGetFriends_003Ec__AnonStorey19B _003CGetFriends_003Ec__AnonStorey19B = new _003CGetFriends_003Ec__AnonStorey19B();
			_003CGetFriends_003Ec__AnonStorey19B.callback = callback;
			if (friends != null && !forceRefresh && _003CGetFriends_003Ec__AnonStorey19B.callback != null)
			{
				_003CGetFriends_003Ec__AnonStorey19B.callback(friends);
			}
			else
			{
				ApiClient.GetFriends(_003CGetFriends_003Ec__AnonStorey19B._003C_003Em__112);
			}
		}

		public static void GetWebConfiguration(Action<WebConfiguration> callback, bool forceRefresh = false)
		{
			_003CGetWebConfiguration_003Ec__AnonStorey19C _003CGetWebConfiguration_003Ec__AnonStorey19C = new _003CGetWebConfiguration_003Ec__AnonStorey19C();
			_003CGetWebConfiguration_003Ec__AnonStorey19C.callback = callback;
			if (config != null && !forceRefresh && _003CGetWebConfiguration_003Ec__AnonStorey19C.callback != null)
			{
				_003CGetWebConfiguration_003Ec__AnonStorey19C.callback(config);
			}
			else
			{
				ApiClient.GetWebConfiguration(_003CGetWebConfiguration_003Ec__AnonStorey19C._003C_003Em__113);
			}
		}
	}
}
