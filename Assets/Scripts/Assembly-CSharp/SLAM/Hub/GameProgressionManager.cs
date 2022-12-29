using System.Runtime.CompilerServices;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Hub
{
	public class GameProgressionManager : MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class _003CIsUnlocked_003Ec__AnonStorey16E
		{
			internal Game game;

			internal bool _003C_003Em__79(UserGameDetails gd)
			{
				return gd.GameId == game.Id;
			}
		}

		[CompilerGenerated]
		private sealed class _003CIsUnlocked_003Ec__AnonStorey16F
		{
			internal int gameId;

			internal bool _003C_003Em__7A(UserGameDetails gd)
			{
				return gd.GameId == gameId;
			}
		}

		public bool IsLoaded
		{
			get
			{
				return GameDetails != null;
			}
		}

		public UserGameDetails[] GameDetails { get; protected set; }

		public bool IsUnlocked(Game game)
		{
			_003CIsUnlocked_003Ec__AnonStorey16E _003CIsUnlocked_003Ec__AnonStorey16E = new _003CIsUnlocked_003Ec__AnonStorey16E();
			_003CIsUnlocked_003Ec__AnonStorey16E.game = game;
			if (!_003CIsUnlocked_003Ec__AnonStorey16E.game.PreviousGameId.HasValue)
			{
				return true;
			}
			return GameDetails.Any(_003CIsUnlocked_003Ec__AnonStorey16E._003C_003Em__79);
		}

		public bool IsUnlocked(int gameId)
		{
			_003CIsUnlocked_003Ec__AnonStorey16F _003CIsUnlocked_003Ec__AnonStorey16F = new _003CIsUnlocked_003Ec__AnonStorey16F();
			_003CIsUnlocked_003Ec__AnonStorey16F.gameId = gameId;
			return GameDetails.Any(_003CIsUnlocked_003Ec__AnonStorey16F._003C_003Em__7A);
		}

		private void Awake()
		{
			DataStorage.GetProgressionData(_003CAwake_003Em__7B);
		}

		[CompilerGenerated]
		private void _003CAwake_003Em__7B(UserGameDetails[] ugd)
		{
			GameDetails = ugd;
		}
	}
}
