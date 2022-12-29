using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Shared;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Hub
{
	public class HubLocationView : HubMarkerView
	{
		[CompilerGenerated]
		private sealed class _003CdoAdventureGameSequence_003Ec__IteratorA7 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal IEnumerable<Game> games;

			internal IEnumerator<Game> _003C_0024s_316_003E__0;

			internal Game _003Cgame_003E__1;

			internal HubLocationProvider hubLocationProvider;

			internal HubLocationProvider.HubGameMarker _003Cmarker_003E__2;

			internal bool _003CshouldBeLocked_003E__3;

			internal bool _003CnextGameUnlocked_003E__4;

			internal Material _003Cmat_003E__5;

			internal float speedMod;

			internal Stopwatch _003Csw_003E__6;

			internal int _0024PC;

			internal object _0024current;

			internal IEnumerable<Game> _003C_0024_003Egames;

			internal HubLocationProvider _003C_0024_003EhubLocationProvider;

			internal float _003C_0024_003EspeedMod;

			internal HubLocationView _003C_003Ef__this;

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
				//Discarded unreachable code: IL_0300
				uint num = (uint)_0024PC;
				_0024PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					games = games.OrderBy(_003C_003Em__99);
					_003C_0024s_316_003E__0 = games.GetEnumerator();
					num = 4294967293u;
					goto case 1u;
				case 1u:
					try
					{
						switch (num)
						{
						case 1u:
							_003Cmat_003E__5.SetFloat("_Progress", _003C_003Ef__this.lineAnimationCurve.Evaluate(_003Csw_003E__6.Progress) * _003Cmarker_003E__2.PathUvLength);
							goto IL_02b7;
						default:
							{
								if (_003C_0024s_316_003E__0.MoveNext())
								{
									_003Cgame_003E__1 = _003C_0024s_316_003E__0.Current;
									_003C_003Ef__this.spawnButtonForGame(_003Cgame_003E__1, 0f);
									_003Cmarker_003E__2 = hubLocationProvider.GameMarkers.FirstOrDefault(_003C_003Em__9A);
									_003CshouldBeLocked_003E__3 = _003Cgame_003E__1.Id != RecentlyUnlockedGameId && _003Cgame_003E__1.IsUnlocked && _003C_003Ef__this.Controller<HubController>().ProgressionManager.IsUnlocked(_003Cgame_003E__1);
									if (_003Cmarker_003E__2.CircleObject != null)
									{
										_003Cmarker_003E__2.CircleObject.SetActive(true);
										_003Cmarker_003E__2.CircleObject.GetComponentInChildren<Renderer>().material.SetFloat("_Blend", (!_003CshouldBeLocked_003E__3) ? 1 : 0);
									}
									if (_003Cmarker_003E__2.PathObject == null)
									{
										goto IL_02fc;
									}
									_003CnextGameUnlocked_003E__4 = !UserProfile.Current.IsFree && _003Cgame_003E__1.NextGameId.HasValue && _003Cgame_003E__1.NextGameId.Value != RecentlyUnlockedGameId && _003C_003Ef__this.Controller<HubController>().ProgressionManager.IsUnlocked(_003C_003Ef__this.locationInfo.GetGame(_003Cgame_003E__1.NextGameId.Value));
									_003Cmat_003E__5 = _003Cmarker_003E__2.PathObject.GetComponent<Renderer>().material;
									_003Cmat_003E__5.SetFloat("_LockedProgress", (!_003CnextGameUnlocked_003E__4) ? 0f : _003Cmarker_003E__2.PathUvLength);
									_003Csw_003E__6 = new Stopwatch(_003Cmarker_003E__2.PathUvLength / (_003C_003Ef__this.pathLineSpeed * speedMod));
									goto IL_02b7;
								}
								break;
							}
							IL_02b7:
							if (!_003Csw_003E__6)
							{
								goto default;
							}
							_0024current = null;
							_0024PC = 1;
							flag = true;
							goto IL_02fe;
						}
					}
					finally
					{
						if (!flag && _003C_0024s_316_003E__0 != null)
						{
							_003C_0024s_316_003E__0.Dispose();
						}
					}
					_0024PC = -1;
					goto IL_02fc;
				default:
					goto IL_02fc;
					IL_02fc:
					return false;
					IL_02fe:
					return true;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
						break;
					}
					finally
					{
						if (_003C_0024s_316_003E__0 != null)
						{
							_003C_0024s_316_003E__0.Dispose();
						}
					}
				case 0u:
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			internal int _003C_003Em__99(Game g)
			{
				return _003C_003Ef__this.getAdventureGameIndex(g);
			}

			internal bool _003C_003Em__9A(HubLocationProvider.HubGameMarker gm)
			{
				return gm.GameId == _003Cgame_003E__1.Id;
			}
		}

		[CompilerGenerated]
		private sealed class _003CdoUnlockSequenceDelayed_003Ec__IteratorA8 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal float delay;

			internal GameObject marker;

			internal HubMarkerButton _003Cbtn_003E__0;

			internal Game _003Cgame_003E__1;

			internal HubLocationProvider.HubGameMarker _003CpreviousMarker_003E__2;

			internal HubLocationProvider.HubGameMarker _003CcurrentMarker_003E__3;

			internal HubMarkerButton[] _003C_0024s_317_003E__4;

			internal int _003C_0024s_318_003E__5;

			internal HubMarkerButton _003CotherBtn_003E__6;

			internal Renderer _003CcircleRenderer_003E__7;

			internal Stopwatch _003Csw_003E__8;

			internal int _0024PC;

			internal object _0024current;

			internal float _003C_0024_003Edelay;

			internal GameObject _003C_0024_003Emarker;

			internal HubLocationView _003C_003Ef__this;

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
				//Discarded unreachable code: IL_0294
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_0024current = new WaitForSeconds(delay);
					_0024PC = 1;
					break;
				case 1u:
					_003Cbtn_003E__0 = marker.GetComponent<HubMarkerButton>();
					_003Cgame_003E__1 = _003Cbtn_003E__0.Data as Game;
					if (_003Cgame_003E__1.PreviousGameId.HasValue)
					{
						_003CpreviousMarker_003E__2 = _003C_003Ef__this.locationProvider.GameMarkers.FirstOrDefault(_003C_003Em__9B);
						if (_003CpreviousMarker_003E__2 != null && _003CpreviousMarker_003E__2.PathObject != null)
						{
							_0024current = _003C_003Ef__this.StartCoroutine(_003C_003Ef__this.animatePathToCompletion(_003CpreviousMarker_003E__2));
							_0024PC = 2;
							break;
						}
					}
					goto case 2u;
				case 2u:
					_003CcurrentMarker_003E__3 = _003C_003Ef__this.locationProvider.GameMarkers.FirstOrDefault(_003C_003Em__9C);
					UnityEngine.Object.Instantiate(_003C_003Ef__this.fireworksPrefab, marker.transform.position, Quaternion.identity);
					_003C_0024s_317_003E__4 = UnityEngine.Object.FindObjectsOfType<HubMarkerButton>();
					for (_003C_0024s_318_003E__5 = 0; _003C_0024s_318_003E__5 < _003C_0024s_317_003E__4.Length; _003C_0024s_318_003E__5++)
					{
						_003CotherBtn_003E__6 = _003C_0024s_317_003E__4[_003C_0024s_318_003E__5];
						_003CotherBtn_003E__6.SetHighlighted(false);
					}
					_003Cbtn_003E__0.SetMaterial(_003C_003Ef__this.unlockedMaterial);
					_003Cbtn_003E__0.SetClickHandler(_003C_003Ef__this.onPlayableGameClicked);
					_003Cbtn_003E__0.SetHighlighted(true);
					RecentlyUnlockedGameId = -1;
					_003Cbtn_003E__0.SetIcon(_003C_003Ef__this.getGameIcon(_003Cgame_003E__1));
					if (_003CcurrentMarker_003E__3.CircleObject != null)
					{
						_003CcircleRenderer_003E__7 = _003CcurrentMarker_003E__3.CircleObject.GetComponentInChildren<Renderer>();
						_003Csw_003E__8 = new Stopwatch(0.2f);
						goto IL_0279;
					}
					goto IL_0289;
				case 3u:
					_003CcircleRenderer_003E__7.material.SetFloat("_Blend", 1f - _003Csw_003E__8.Progress);
					goto IL_0279;
				default:
					{
						return false;
					}
					IL_0289:
					_0024PC = -1;
					goto default;
					IL_0279:
					if ((bool)_003Csw_003E__8)
					{
						_0024current = null;
						_0024PC = 3;
						break;
					}
					goto IL_0289;
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

			internal bool _003C_003Em__9B(HubLocationProvider.HubGameMarker gm)
			{
				return gm.GameId == _003Cgame_003E__1.PreviousGameId;
			}

			internal bool _003C_003Em__9C(HubLocationProvider.HubGameMarker gm)
			{
				return gm.GameId == _003Cgame_003E__1.Id;
			}
		}

		[CompilerGenerated]
		private sealed class _003CspawnButtonForGame_003Ec__AnonStorey173
		{
			internal Game game;

			internal bool _003C_003Em__97(HubLocationProvider.HubGameMarker gm)
			{
				return gm.GameId == game.Id;
			}

			internal bool _003C_003Em__98(UserGameDetails g)
			{
				return g.GameId == game.Id;
			}
		}

		public static int RecentlyUnlockedGameId = -1;

		public static List<int> VisitedLocationIds = new List<int>();

		[SerializeField]
		private UILabel titleLabel;

		[SerializeField]
		private UIPortrait portraitSprite;

		[SerializeField]
		private Material lockedMaterial;

		[SerializeField]
		private Material unlockedMaterial;

		[SerializeField]
		private Material premiumMaterial;

		[SerializeField]
		private GameObject fireworksPrefab;

		[SerializeField]
		private GameObject labelPrefab;

		[SerializeField]
		private GameObject labelRoot;

		[SerializeField]
		private AnimationCurve lineAnimationCurve;

		[SerializeField]
		[Tooltip("uv/s")]
		private float pathLineSpeed = 0.5f;

		[SerializeField]
		private Vector3 nameLabelOffset;

		private Location locationInfo;

		private HubLocationProvider locationProvider;

		private int prevTouchCount;

		private bool cheatsEnabled;

		[CompilerGenerated]
		private static Func<Game, bool> _003C_003Ef__am_0024cache11;

		protected override void Start()
		{
			base.Start();
			DataStorage.GetWebConfiguration(_003CStart_003Em__95);
		}

		protected override void Update()
		{
			if (!cheatsEnabled || (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift)))
			{
				return;
			}
			for (int i = 48; i <= 57; i++)
			{
				if (Input.GetKeyDown((KeyCode)i))
				{
					StartCoroutine(doUnlockSequenceDelayed(0f, markerRoot.transform.GetChild(i - 49).gameObject));
					markerRoot.transform.GetChild(i - 49).GetComponent<HubMarkerButton>().SetClickHandler(onPlayableGameClicked);
				}
			}
		}

		public override void Close(Callback callback, bool immediately)
		{
			clearMarkers();
			labelRoot.transform.DestroyChildren();
			if (locationProvider != null)
			{
				HubLocationProvider.HubGameMarker[] gameMarkers = locationProvider.GameMarkers;
				foreach (HubLocationProvider.HubGameMarker hubGameMarker in gameMarkers)
				{
					if (hubGameMarker.CircleObject != null)
					{
						hubGameMarker.CircleObject.SetActive(false);
					}
					if (hubGameMarker.PathObject != null)
					{
						hubGameMarker.PathObject.GetComponent<Renderer>().material.SetFloat("_Progress", 0f);
					}
				}
			}
			base.Close(callback, immediately);
		}

		public void OnCloseClicked()
		{
			Controller<HubController>().DeselectLocation();
		}

		public virtual void OnLogoutClicked()
		{
			GameEvents.Invoke(new Webservice.LogoutEvent());
		}

		public void UpdateInfo(Location info, HubLocationProvider hubLocationProvider)
		{
			locationInfo = info;
			locationProvider = hubLocationProvider;
			titleLabel.text = Localization.Get(info.Name);
			portraitSprite.SetCharacter(hubLocationProvider.GameCharacter);
			float speedMod = 1f;
			if (VisitedLocationIds.Contains(info.Id))
			{
				speedMod = 2f;
			}
			else
			{
				VisitedLocationIds.Add(info.Id);
			}
			createGameMarkers(info, hubLocationProvider, speedMod);
		}

		private void createGameMarkers(Location info, HubLocationProvider hubLocationProvider, float speedMod = 1f)
		{
			VisitedLocationIds.Add(locationInfo.Id);
			Game[] games = info.Games;
			if (_003C_003Ef__am_0024cache11 == null)
			{
				_003C_003Ef__am_0024cache11 = _003CcreateGameMarkers_003Em__96;
			}
			IEnumerable<Game> enumerable = games.Where(_003C_003Ef__am_0024cache11);
			if (enumerable.Count() > 0)
			{
				StartCoroutine(doAdventureGameSequence(enumerable, hubLocationProvider, speedMod));
			}
			for (int i = 0; i < info.Games.Length; i++)
			{
				if (!enumerable.Contains(info.Games[i]) && info.Games[i].Enabled)
				{
					spawnButtonForGame(info.Games[i], (float)info.Games[i].SortOrder * 0.5f * speedMod);
				}
			}
		}

		private IEnumerator doAdventureGameSequence(IEnumerable<Game> games, HubLocationProvider hubLocationProvider, float speedMod = 1f)
		{
			games = games.OrderBy(((_003CdoAdventureGameSequence_003Ec__IteratorA7)(object)this)._003C_003Em__99);
			foreach (Game game in games)
			{
				spawnButtonForGame(game, 0f);
				HubLocationProvider.HubGameMarker marker = hubLocationProvider.GameMarkers.FirstOrDefault(((_003CdoAdventureGameSequence_003Ec__IteratorA7)(object)this)._003C_003Em__9A);
				bool shouldBeLocked = game.Id != RecentlyUnlockedGameId && game.IsUnlocked && Controller<HubController>().ProgressionManager.IsUnlocked(game);
				if (marker.CircleObject != null)
				{
					marker.CircleObject.SetActive(true);
					marker.CircleObject.GetComponentInChildren<Renderer>().material.SetFloat("_Blend", (!shouldBeLocked) ? 1 : 0);
				}
				if (marker.PathObject == null)
				{
					break;
				}
				bool nextGameUnlocked = !UserProfile.Current.IsFree && game.NextGameId.HasValue && game.NextGameId.Value != RecentlyUnlockedGameId && Controller<HubController>().ProgressionManager.IsUnlocked(locationInfo.GetGame(game.NextGameId.Value));
				Material mat = marker.PathObject.GetComponent<Renderer>().material;
				mat.SetFloat("_LockedProgress", (!nextGameUnlocked) ? 0f : marker.PathUvLength);
				Stopwatch sw = new Stopwatch(marker.PathUvLength / (pathLineSpeed * speedMod));
				while ((bool)sw)
				{
					yield return null;
					mat.SetFloat("_Progress", lineAnimationCurve.Evaluate(sw.Progress) * marker.PathUvLength);
				}
			}
		}

		private IEnumerator doUnlockSequenceDelayed(float delay, GameObject marker)
		{
			yield return new WaitForSeconds(delay);
			HubMarkerButton btn = marker.GetComponent<HubMarkerButton>();
			Game game = btn.Data as Game;
			if (game.PreviousGameId.HasValue)
			{
				HubLocationProvider.HubGameMarker previousMarker = locationProvider.GameMarkers.FirstOrDefault(((_003CdoUnlockSequenceDelayed_003Ec__IteratorA8)(object)this)._003C_003Em__9B);
				if (previousMarker != null && previousMarker.PathObject != null)
				{
					yield return StartCoroutine(animatePathToCompletion(previousMarker));
				}
			}
			HubLocationProvider.HubGameMarker currentMarker = locationProvider.GameMarkers.FirstOrDefault(((_003CdoUnlockSequenceDelayed_003Ec__IteratorA8)(object)this)._003C_003Em__9C);
			UnityEngine.Object.Instantiate(fireworksPrefab, marker.transform.position, Quaternion.identity);
			HubMarkerButton[] array = UnityEngine.Object.FindObjectsOfType<HubMarkerButton>();
			foreach (HubMarkerButton otherBtn in array)
			{
				otherBtn.SetHighlighted(false);
			}
			btn.SetMaterial(unlockedMaterial);
			btn.SetClickHandler(onPlayableGameClicked);
			btn.SetHighlighted(true);
			RecentlyUnlockedGameId = -1;
			btn.SetIcon(getGameIcon(game));
			if (currentMarker.CircleObject != null)
			{
				Renderer circleRenderer = currentMarker.CircleObject.GetComponentInChildren<Renderer>();
				Stopwatch sw = new Stopwatch(0.2f);
				while ((bool)sw)
				{
					yield return null;
					circleRenderer.material.SetFloat("_Blend", 1f - sw.Progress);
				}
			}
		}

		private IEnumerator animatePathToCompletion(HubLocationProvider.HubGameMarker marker)
		{
			Material mat = marker.PathObject.GetComponent<Renderer>().material;
			Stopwatch sw = new Stopwatch(marker.PathUvLength / pathLineSpeed);
			while ((bool)sw)
			{
				yield return null;
				mat.SetFloat("_LockedProgress", lineAnimationCurve.Evaluate(sw.Progress) * marker.PathUvLength);
			}
		}

		private GameObject spawnButtonForGame(Game game, float delay)
		{
			_003CspawnButtonForGame_003Ec__AnonStorey173 _003CspawnButtonForGame_003Ec__AnonStorey = new _003CspawnButtonForGame_003Ec__AnonStorey173();
			_003CspawnButtonForGame_003Ec__AnonStorey.game = game;
			HubLocationProvider.HubGameMarker hubGameMarker = locationProvider.GameMarkers.FirstOrDefault(_003CspawnButtonForGame_003Ec__AnonStorey._003C_003Em__97);
			if (hubGameMarker == null)
			{
				UnityEngine.Debug.LogWarning(string.Concat("Hey buddy, i couldnt find game marker location for game: ", _003CspawnButtonForGame_003Ec__AnonStorey.game.Name, " in provider: ", locationProvider, ".\nPlease add a game maker location in the Locations prefab!"), locationProvider);
				return null;
			}
			Action<HubMarkerButton> clickCallback;
			getGameLocked(_003CspawnButtonForGame_003Ec__AnonStorey.game, out clickCallback);
			HubMarkerIcon gameIcon = getGameIcon(_003CspawnButtonForGame_003Ec__AnonStorey.game);
			Material gameMaterial = getGameMaterial(_003CspawnButtonForGame_003Ec__AnonStorey.game);
			bool gameHighlight = getGameHighlight(_003CspawnButtonForGame_003Ec__AnonStorey.game);
			GameObject gameObject = spawnMarkerDelayed(delay, hubGameMarker.Position, hubGameMarker.Rotation, hubGameMarker.MarkerScale, gameMaterial, gameIcon, gameHighlight, clickCallback, _003CspawnButtonForGame_003Ec__AnonStorey.game);
			gameObject.name = _003CspawnButtonForGame_003Ec__AnonStorey.game.Name;
			UserGameDetails userGameDetails = Controller<HubController>().ProgressionManager.GameDetails.FirstOrDefault(_003CspawnButtonForGame_003Ec__AnonStorey._003C_003Em__98);
			float progress = ((userGameDetails != null) ? ((float)userGameDetails.Progression.Count() / (float)_003CspawnButtonForGame_003Ec__AnonStorey.game.TotalLevels) : ((_003CspawnButtonForGame_003Ec__AnonStorey.game.Type != Game.GameType.Shop) ? 0f : 1f));
			StartCoroutine(showTextAndPlayAudioDelayed(delay + 0.7f, hubGameMarker.Position, Localization.Get(_003CspawnButtonForGame_003Ec__AnonStorey.game.Name), progress));
			if (_003CspawnButtonForGame_003Ec__AnonStorey.game.Id == RecentlyUnlockedGameId && _003CspawnButtonForGame_003Ec__AnonStorey.game.IsPremiumAvailable)
			{
				StartCoroutine(doUnlockSequenceDelayed(delay + 1f, gameObject));
			}
			return gameObject;
		}

		private bool getGameLocked(Game game, out Action<HubMarkerButton> clickCallback)
		{
			if (!game.IsUnlocked)
			{
				clickCallback = onFutureGameClicked;
				return true;
			}
			if (UserProfile.Current.IsFree && game.FreeLevelTo <= 0)
			{
				clickCallback = onPremiumGameClicked;
				return true;
			}
			if (UserProfile.Current != null && UserProfile.Current.IsSA && !game.IsUnlockedSA)
			{
				clickCallback = onSanomaAccountLockedGameClicked;
				return true;
			}
			if (!UserProfile.Current.IsFree && game.Type == Game.GameType.AdventureGame && !Controller<HubController>().ProgressionManager.IsUnlocked(game))
			{
				clickCallback = onLockedAdventureGameClicked;
				return true;
			}
			clickCallback = onPlayableGameClicked;
			return false;
		}

		private bool getGameHighlight(Game game)
		{
			return game.IsUnlocked && game.IsPremiumAvailable && game.Type == Game.GameType.AdventureGame && Controller<HubController>().ProgressionManager.IsUnlocked(game) && game.NextGameId.HasValue && !Controller<HubController>().ProgressionManager.IsUnlocked(game.NextGameId.Value);
		}

		private Material getGameMaterial(Game game)
		{
			if (!game.IsPremiumAvailable)
			{
				return premiumMaterial;
			}
			Action<HubMarkerButton> clickCallback;
			if (getGameLocked(game, out clickCallback) || game.Id == RecentlyUnlockedGameId)
			{
				return lockedMaterial;
			}
			return unlockedMaterial;
		}

		private HubMarkerIcon getGameIcon(Game game)
		{
			if (!game.IsPremiumAvailable)
			{
				return HubMarkerIcon.Premium;
			}
			Action<HubMarkerButton> clickCallback;
			if (getGameLocked(game, out clickCallback))
			{
				return HubMarkerIcon.Locked;
			}
			if (game.Id == 19)
			{
				return HubMarkerIcon.AvatarHouse;
			}
			switch (game.Type)
			{
			case Game.GameType.AdventureGame:
				return (HubMarkerIcon)(6 + getAdventureGameIndexWithoutMotionComics(game));
			case Game.GameType.Shop:
				return HubMarkerIcon.Shop;
			case Game.GameType.Job:
				return HubMarkerIcon.Job;
			case Game.GameType.LocationGame:
				return HubMarkerIcon.Location;
			default:
				throw new Exception("No icon for game type " + game.Type);
			}
		}

		private int getAdventureGameIndexWithoutMotionComics(Game game)
		{
			if (game.Name.StartsWith("HUB_GAME_MOTIONCOMIC"))
			{
				return 10;
			}
			int num = 0;
			while (game != null && game.PreviousGameId.HasValue)
			{
				game = locationInfo.GetGame(game.PreviousGameId.Value);
				if (game != null && !game.Name.StartsWith("HUB_GAME_MOTIONCOMIC"))
				{
					num++;
				}
			}
			return num;
		}

		private int getAdventureGameIndex(Game game)
		{
			int num = 0;
			while (game != null && game.PreviousGameId.HasValue)
			{
				game = locationInfo.GetGame(game.PreviousGameId.Value);
				num++;
			}
			return num;
		}

		private IEnumerator showTextAndPlayAudioDelayed(float delay, Vector3 worldPos, string text, float progress)
		{
			yield return new WaitForSeconds(delay - 0.7f);
			AudioController.Play("hub_balloon_random", worldPos);
			yield return new WaitForSeconds(0.7f);
			Vector3 pos = worldToScreen(worldPos + nameLabelOffset);
			GameObject labelGo = UnityEngine.Object.Instantiate(labelPrefab);
			labelGo.transform.parent = labelRoot.transform;
			labelGo.transform.localScale = Vector3.one;
			labelGo.transform.position = pos;
			labelGo.GetComponent<UILabel>().text = text;
			UIProgressBar pb = labelGo.GetComponentInChildren<UIProgressBar>();
			pb.value = 0f;
			Stopwatch sw = new Stopwatch(0.5f * progress);
			while (!sw.Expired)
			{
				pb.value = Mathf.Lerp(0f, progress, sw.Progress);
				yield return null;
			}
			pb.value = progress;
		}

		private void onPlayableGameClicked(HubMarkerButton button)
		{
			Controller<HubController>().Play(button.Data as Game);
		}

		private void onPremiumGameClicked(HubMarkerButton button)
		{
			Controller<HubController>().OpenPremiumGameLockedWindow(((Game)button.Data).Id);
		}

		private void onSanomaAccountLockedGameClicked(HubMarkerButton button)
		{
			Game game = button.Data as Game;
			GameEvents.Invoke(new PopupEvent(Localization.Get("HUB_POPUP_LOCKEDGAME_SA_TITLE"), StringFormatter.GetLocalizationFormatted("HUB_POPUP_LOCKEDGAME_SA_DESCRIPTION", Localization.Get(game.Name)), Localization.Get("HUB_POPUP_LOCKEDGAME_SA_BUTTON_CONTINUE"), null, null, null));
		}

		private void onLockedAdventureGameClicked(HubMarkerButton button)
		{
			Game game = button.Data as Game;
			GameEvents.Invoke(new PopupEvent(Localization.Get("HUB_POPUP_LOCKEDADVENTURE_TITLE"), StringFormatter.GetLocalizationFormatted("HUB_POPUP_LOCKEDADVENTURE_DESCRIPTION", Localization.Get(game.Name), Localization.Get(locationInfo.GetGame(game.PreviousGameId.Value).Name), locationInfo.GetGame(game.PreviousGameId.Value).RequiredDifficultyToUnlockNextGame), Localization.Get("HUB_POPUP_LOCKEDADVENTURE_BUTTON_CONTINUE"), null, null, null));
		}

		private void onLockedJobClicked(HubMarkerButton button)
		{
			Game game = button.Data as Game;
			GameEvents.Invoke(new PopupEvent(Localization.Get("HUB_POPUP_LOCKEDJOB_TITLE"), StringFormatter.GetLocalizationFormatted("HUB_POPUP_LOCKEDJOB_DESCRIPTION", Localization.Get(game.Name)), Localization.Get("HUB_POPUP_LOCKEDJOB_BUTTON_CONTINUE"), null, null, null));
		}

		private void onFutureGameClicked(HubMarkerButton button)
		{
			Game game = button.Data as Game;
			GameEvents.Invoke(new PopupEvent(Localization.Get("HUB_POPUP_FUTUREGAME_TITLE"), StringFormatter.GetLocalizationFormatted("HUB_POPUP_FUTUREGAME_DESCRIPTION", Localization.Get(game.Name)), Localization.Get("HUB_POPUP_FUTUREGAME_BUTTON_CONTINUE"), null, null, null));
		}

		private Vector3 worldToScreen(Vector3 pos)
		{
			Vector3 result = UICamera.currentCamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(pos));
			result.z = 0f;
			return result;
		}

		[CompilerGenerated]
		private void _003CStart_003Em__95(WebConfiguration config)
		{
			cheatsEnabled = config.CheatsEnabled;
		}

		[CompilerGenerated]
		private static bool _003CcreateGameMarkers_003Em__96(Game g)
		{
			return g.Enabled && g.Type == Game.GameType.AdventureGame;
		}
	}
}
