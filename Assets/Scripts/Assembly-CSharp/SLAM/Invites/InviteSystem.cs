using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.BuildSystem;
using SLAM.Slinq;
using SLAM.Smartphone;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Invites
{
	public class InviteSystem : SingletonMonobehaviour<InviteSystem>
	{
		public class GameInviteEvent
		{
			public Game Game;
		}

		public class GameInviteResponseEvent
		{
			public Game Game;

			public bool Accepted;
		}

		[CompilerGenerated]
		private sealed class _003CStart_003Ec__Iterator116 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal int _0024PC;

			internal object _0024current;

			internal InviteSystem _003C_003Ef__this;

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
				//Discarded unreachable code: IL_00c3
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
						_003C_003Ef__this.enabled = false;
					}
					DataStorage.GetLocationsData(_003C_003Em__F1);
					DataStorage.GetProgressionData(_003C_003Em__F2);
					_003C_003Ef__this.requiredTimeInHub = UnityEngine.Random.Range(_003C_003Ef__this.phoneNotificationDelayMin, _003C_003Ef__this.phoneNotificationDelayMax);
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

			internal void _003C_003Em__F1(Location[] locs)
			{
				Games = new List<Game>();
				_003C_003Ef__this.GameInvitationsIds = new List<int>();
				for (int i = 0; i < locs.Length; i++)
				{
					Games.AddRange(locs[i].Games.Where(_003C_003Em__F3));
				}
			}

			internal void _003C_003Em__F2(UserGameDetails[] details)
			{
				if (details != null)
				{
					foreach (UserGameDetails userGameDetails in details)
					{
						_003C_003Ef__this.levelsUnlocked += userGameDetails.Progression.Count() + 1;
					}
				}
			}

			internal bool _003C_003Em__F3(Game g)
			{
				return g.IsUnlocked && _003C_003Ef__this.allowedGameTypes.Contains(g.Type) && !_003C_003Ef__this.excludedGames.Contains(g.Id);
			}
		}

		[SerializeField]
		private int inviteIntervalSec = 300;

		[SerializeField]
		private int phoneNotificationDelayMin = 30;

		[SerializeField]
		private int phoneNotificationDelayMax = 90;

		[SerializeField]
		private int requiredLevelsUnlocked;

		[SerializeField]
		private Game.GameType[] allowedGameTypes;

		[SerializeField]
		[GameId]
		private int[] excludedGames;

		private List<int> GameInvitationsIds;

		private int levelsUnlocked;

		private float timeInHub;

		private float requiredTimeInHub = 10f;

		private float nextInvitationTime;

		private bool isSmartphoneOpen;

		[CompilerGenerated]
		private static Predicate<int> _003C_003Ef__am_0024cache10;

		[CompilerGenerated]
		private static Func<Game, bool> _003C_003Ef__am_0024cache11;

		[CompilerGenerated]
		private static Func<Game, bool> _003C_003Ef__am_0024cache12;

		[CompilerGenerated]
		private static Func<Game, bool> _003C_003Ef__am_0024cache13;

		public static int AcceptedInvitationGameId { get; private set; }

		public static int PendingInvitationGameId { get; private set; }

		public static bool HasReceivedInvitation { get; private set; }

		public static bool HasPendingInvitation
		{
			get
			{
				return PendingInvitationGameId > 0;
			}
		}

		public static List<Game> Games { get; private set; }

		private bool SceneIsHub
		{
			get
			{
				return "Hub" == Application.loadedLevelName;
			}
		}

		private void OnEnable()
		{
			GameEvents.Subscribe<GameInviteResponseEvent>(onGameInviteResponse);
			GameEvents.Subscribe<Webservice.LogoutEvent>(onLogout);
			GameEvents.Subscribe<SmartphoneVisibilityChangedEvent>(onSmartphoneVisibilityChanged);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<GameInviteResponseEvent>(onGameInviteResponse);
			GameEvents.Unsubscribe<Webservice.LogoutEvent>(onLogout);
			GameEvents.Unsubscribe<SmartphoneVisibilityChangedEvent>(onSmartphoneVisibilityChanged);
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
				base.enabled = false;
			}
			DataStorage.GetLocationsData(((_003CStart_003Ec__Iterator116)(object)this)._003C_003Em__F1);
			DataStorage.GetProgressionData(((_003CStart_003Ec__Iterator116)(object)this)._003C_003Em__F2);
			requiredTimeInHub = UnityEngine.Random.Range(phoneNotificationDelayMin, phoneNotificationDelayMax);
		}

		private void Update()
		{
			if (Games == null)
			{
				return;
			}
			if (SceneIsHub)
			{
				timeInHub += Time.deltaTime;
				if (!isSmartphoneOpen && !HasReceivedInvitation && Time.time > nextInvitationTime && timeInHub > requiredTimeInHub)
				{
					doInvite();
				}
			}
			else
			{
				timeInHub = 0f;
			}
		}

		private void doInvite()
		{
			Game game = null;
			nextInvitationTime = Time.time + (float)inviteIntervalSec;
			requiredTimeInHub = UnityEngine.Random.Range(phoneNotificationDelayMin, phoneNotificationDelayMax);
			if (levelsUnlocked < requiredLevelsUnlocked)
			{
				levelsUnlocked = 0;
				DataStorage.GetProgressionData(_003CdoInvite_003Em__EA);
				UnityEngine.Debug.Log(string.Format("Hey Buddy, you need to unlock {0} ({1}/{2}) more levels before will receive an invitation.", requiredLevelsUnlocked - levelsUnlocked, levelsUnlocked, requiredLevelsUnlocked));
				return;
			}
			if (GameInvitationsIds.Count >= Games.Count)
			{
				List<int> gameInvitationsIds = GameInvitationsIds;
				if (_003C_003Ef__am_0024cache10 == null)
				{
					_003C_003Ef__am_0024cache10 = _003CdoInvite_003Em__EB;
				}
				gameInvitationsIds.RemoveAll(_003C_003Ef__am_0024cache10);
			}
			if (!HasPendingInvitation)
			{
				game = ((UserProfile.Current == null || !UserProfile.Current.IsSA) ? Games.Where(_003CdoInvite_003Em__EE).GetRandom() : Games.Where(_003CdoInvite_003Em__ED).GetRandom());
			}
			else
			{
				List<Game> games = Games;
				if (_003C_003Ef__am_0024cache11 == null)
				{
					_003C_003Ef__am_0024cache11 = _003CdoInvite_003Em__EC;
				}
				game = games.FirstOrDefault(_003C_003Ef__am_0024cache11);
			}
			if (game != null)
			{
				GameInvitationsIds.Add(game.Id);
				PendingInvitationGameId = game.Id;
				GameInviteEvent gameInviteEvent = new GameInviteEvent();
				gameInviteEvent.Game = game;
				GameEvents.Invoke(gameInviteEvent);
			}
		}

		public static void ReceivedGameInvitation()
		{
			HasReceivedInvitation = true;
		}

		public static void AcceptGameInvitation()
		{
			if (HasPendingInvitation)
			{
				GameInviteResponseEvent gameInviteResponseEvent = new GameInviteResponseEvent();
				List<Game> games = Games;
				if (_003C_003Ef__am_0024cache12 == null)
				{
					_003C_003Ef__am_0024cache12 = _003CAcceptGameInvitation_003Em__EF;
				}
				gameInviteResponseEvent.Game = games.FirstOrDefault(_003C_003Ef__am_0024cache12);
				gameInviteResponseEvent.Accepted = true;
				GameEvents.Invoke(gameInviteResponseEvent);
			}
		}

		public static void DeclineGameInvitation()
		{
			if (HasPendingInvitation)
			{
				GameInviteResponseEvent gameInviteResponseEvent = new GameInviteResponseEvent();
				List<Game> games = Games;
				if (_003C_003Ef__am_0024cache13 == null)
				{
					_003C_003Ef__am_0024cache13 = _003CDeclineGameInvitation_003Em__F0;
				}
				gameInviteResponseEvent.Game = games.FirstOrDefault(_003C_003Ef__am_0024cache13);
				gameInviteResponseEvent.Accepted = false;
				GameEvents.Invoke(gameInviteResponseEvent);
			}
		}

		public static void CloseGameInvitation()
		{
			PendingInvitationGameId = -1;
			AcceptedInvitationGameId = -1;
			HasReceivedInvitation = false;
		}

		private void onGameInviteResponse(GameInviteResponseEvent evt)
		{
			PendingInvitationGameId = -1;
			AcceptedInvitationGameId = ((!evt.Accepted) ? (-1) : evt.Game.Id);
			HasReceivedInvitation = false;
			nextInvitationTime = Time.time + (float)inviteIntervalSec;
			requiredTimeInHub = UnityEngine.Random.Range(phoneNotificationDelayMin, phoneNotificationDelayMax);
			if (evt.Accepted)
			{
				SceneManager.Load(evt.Game.SceneName);
			}
		}

		private void onLogout(Webservice.LogoutEvent evt)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		private void onSmartphoneVisibilityChanged(SmartphoneVisibilityChangedEvent evt)
		{
			isSmartphoneOpen = evt.IsVisible;
		}

		[CompilerGenerated]
		private void _003CdoInvite_003Em__EA(UserGameDetails[] details)
		{
			foreach (UserGameDetails userGameDetails in details)
			{
				levelsUnlocked += userGameDetails.Progression.Count();
			}
		}

		[CompilerGenerated]
		private static bool _003CdoInvite_003Em__EB(int gi)
		{
			return true;
		}

		[CompilerGenerated]
		private static bool _003CdoInvite_003Em__EC(Game g)
		{
			return g.Id == PendingInvitationGameId;
		}

		[CompilerGenerated]
		private bool _003CdoInvite_003Em__ED(Game g)
		{
			return !GameInvitationsIds.Contains(g.Id) && g.IsUnlockedSA;
		}

		[CompilerGenerated]
		private bool _003CdoInvite_003Em__EE(Game g)
		{
			return !GameInvitationsIds.Contains(g.Id);
		}

		[CompilerGenerated]
		private static bool _003CAcceptGameInvitation_003Em__EF(Game g)
		{
			return g.Id == PendingInvitationGameId;
		}

		[CompilerGenerated]
		private static bool _003CDeclineGameInvitation_003Em__F0(Game g)
		{
			return g.Id == PendingInvitationGameId;
		}
	}
}
