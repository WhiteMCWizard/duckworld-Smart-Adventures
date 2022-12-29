using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Hub
{
	public class HubHudView : HubMarkerView
	{
		[CompilerGenerated]
		private sealed class _003CgetHighlightLocation_003Ec__AnonStorey172
		{
			internal Location loc;

			internal HubHudView _003C_003Ef__this;

			internal bool _003C_003Em__8C(Game g)
			{
				return _003C_003Ef__this.Controller<HubController>().ProgressionManager.IsUnlocked(g);
			}

			internal bool _003C_003Em__8D(Game g)
			{
				return !_003C_003Ef__this.Controller<HubController>().ProgressionManager.IsUnlocked(g);
			}

			internal bool _003C_003Em__8E(Game g)
			{
				return !g.NextGameId.HasValue || loc.GetGame(g.NextGameId.Value) == null;
			}
		}

		[SerializeField]
		private Camera uiCamera;

		[SerializeField]
		private Material premiumMaterial;

		[SerializeField]
		private Material lockedMaterial;

		[SerializeField]
		private Material unlockedMaterial;

		private HubLocationProvider locationUnderMouse;

		private bool hasFocus = true;

		[CompilerGenerated]
		private static Func<Game, bool> _003C_003Ef__am_0024cache6;

		[CompilerGenerated]
		private static Func<Game, bool> _003C_003Ef__am_0024cache7;

		[CompilerGenerated]
		private static Func<Game, bool> _003C_003Ef__am_0024cache8;

		[CompilerGenerated]
		private static Func<Game, bool> _003C_003Ef__am_0024cache9;

		[CompilerGenerated]
		private static Func<Game, bool> _003C_003Ef__am_0024cacheA;

		[CompilerGenerated]
		private static Func<Game, bool> _003C_003Ef__am_0024cacheB;

		[CompilerGenerated]
		private static Func<Game, bool> _003C_003Ef__am_0024cacheC;

		[CompilerGenerated]
		private static Func<Game, bool> _003C_003Ef__am_0024cacheD;

		private void OnEnable()
		{
			initializeLocationMarkers();
		}

		private void OnApplicationFocus(bool focusStatus)
		{
			hasFocus = focusStatus;
		}

		protected override void Update()
		{
			if (!hasFocus)
			{
				return;
			}
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.HasComponent<HubMarkerButton>())
			{
				HubMarkerButton component = hitInfo.collider.GetComponent<HubMarkerButton>();
				HubLocationProvider hubLocationProvider = component.Data as HubLocationProvider;
				if (hubLocationProvider != locationUnderMouse)
				{
					locationUnderMouse = hubLocationProvider;
					AudioController.Play((component.Data as HubLocationProvider).MouseOverSound.name);
				}
			}
			else
			{
				locationUnderMouse = null;
			}
		}

		public override void Close(Callback callback, bool immediately)
		{
			clearMarkers();
			base.Close(callback, immediately);
		}

		public void OpenSettingsWindow()
		{
			Controller<HubController>().OpenSettingsWindow();
		}

		public void OpenInstructionsWindow()
		{
			Controller<HubController>().OpenInstructionsWindow();
		}

		private void initializeLocationMarkers()
		{
			HubLocationProvider[] array = UnityEngine.Object.FindObjectsOfType<HubLocationProvider>();
			HubLocationProvider highlightLocation = getHighlightLocation(array);
			for (int i = 0; i < array.Length; i++)
			{
				HubLocationProvider hubLocationProvider = array[i];
				Material locationMaterial = getLocationMaterial(hubLocationProvider);
				HubMarkerIcon locationIcon = getLocationIcon(hubLocationProvider);
				spawnMarkerDelayed((float)i * 0.1f, hubLocationProvider.IconLocation.position, hubLocationProvider.IconLocation.rotation, hubLocationProvider.IconLocation.localScale, locationMaterial, locationIcon, hubLocationProvider == highlightLocation, _003CinitializeLocationMarkers_003Em__89, hubLocationProvider);
			}
		}

		private HubLocationProvider getHighlightLocation(HubLocationProvider[] locations)
		{
			for (int i = 0; i < locations.Length; i++)
			{
				_003CgetHighlightLocation_003Ec__AnonStorey172 _003CgetHighlightLocation_003Ec__AnonStorey = new _003CgetHighlightLocation_003Ec__AnonStorey172();
				_003CgetHighlightLocation_003Ec__AnonStorey._003C_003Ef__this = this;
				_003CgetHighlightLocation_003Ec__AnonStorey.loc = Controller<HubController>().GetLocation(locations[i]);
				Game[] games = _003CgetHighlightLocation_003Ec__AnonStorey.loc.Games;
				if (_003C_003Ef__am_0024cache6 == null)
				{
					_003C_003Ef__am_0024cache6 = _003CgetHighlightLocation_003Em__8A;
				}
				IEnumerable<Game> collection = games.Where(_003C_003Ef__am_0024cache6);
				if (UserProfile.Current != null && UserProfile.Current.IsSA)
				{
					Game[] games2 = _003CgetHighlightLocation_003Ec__AnonStorey.loc.Games;
					if (_003C_003Ef__am_0024cache7 == null)
					{
						_003C_003Ef__am_0024cache7 = _003CgetHighlightLocation_003Em__8B;
					}
					collection = games2.Where(_003C_003Ef__am_0024cache7);
				}
				if (collection.Any(_003CgetHighlightLocation_003Ec__AnonStorey._003C_003Em__8C) && collection.Any(_003CgetHighlightLocation_003Ec__AnonStorey._003C_003Em__8D))
				{
					return locations[i];
				}
				Game game = collection.FirstOrDefault(_003CgetHighlightLocation_003Ec__AnonStorey._003C_003Em__8E);
				if (game != null && Controller<HubController>().ProgressionManager.IsUnlocked(game) && (!game.NextGameId.HasValue || !Controller<HubController>().ProgressionManager.IsUnlocked(game.NextGameId.Value)))
				{
					return locations[i];
				}
			}
			if (locations.Length <= 1)
			{
				return locations.First();
			}
			return null;
		}

		private Material getLocationMaterial(HubLocationProvider locProv)
		{
			Location location = Controller<HubController>().GetLocation(locProv);
			Game[] games = location.Games;
			if (_003C_003Ef__am_0024cache8 == null)
			{
				_003C_003Ef__am_0024cache8 = _003CgetLocationMaterial_003Em__8F;
			}
			if (games.All(_003C_003Ef__am_0024cache8))
			{
				return premiumMaterial;
			}
			Game[] games2 = location.Games;
			if (_003C_003Ef__am_0024cache9 == null)
			{
				_003C_003Ef__am_0024cache9 = _003CgetLocationMaterial_003Em__90;
			}
			if (!games2.All(_003C_003Ef__am_0024cache9))
			{
				Game[] games3 = location.Games;
				if (_003C_003Ef__am_0024cacheA == null)
				{
					_003C_003Ef__am_0024cacheA = _003CgetLocationMaterial_003Em__91;
				}
				if (!games3.All(_003C_003Ef__am_0024cacheA) || UserProfile.Current == null || !UserProfile.Current.IsSA)
				{
					return unlockedMaterial;
				}
			}
			return lockedMaterial;
		}

		private HubMarkerIcon getLocationIcon(HubLocationProvider locProv)
		{
			Location location = Controller<HubController>().GetLocation(locProv);
			if (UserProfile.Current.IsFree)
			{
				Game[] games = location.Games;
				if (_003C_003Ef__am_0024cacheB == null)
				{
					_003C_003Ef__am_0024cacheB = _003CgetLocationIcon_003Em__92;
				}
				if (games.All(_003C_003Ef__am_0024cacheB))
				{
					return HubMarkerIcon.Premium;
				}
			}
			Game[] games2 = location.Games;
			if (_003C_003Ef__am_0024cacheC == null)
			{
				_003C_003Ef__am_0024cacheC = _003CgetLocationIcon_003Em__93;
			}
			if (!games2.All(_003C_003Ef__am_0024cacheC))
			{
				Game[] games3 = location.Games;
				if (_003C_003Ef__am_0024cacheD == null)
				{
					_003C_003Ef__am_0024cacheD = _003CgetLocationIcon_003Em__94;
				}
				if (!games3.All(_003C_003Ef__am_0024cacheD) || UserProfile.Current == null || !UserProfile.Current.IsSA)
				{
					return locProv.MarkerIcon;
				}
			}
			return HubMarkerIcon.Locked;
		}

		[CompilerGenerated]
		private void _003CinitializeLocationMarkers_003Em__89(HubMarkerButton button)
		{
			if (!Controller<HubController>().TrialHasEnded)
			{
				AudioController.Play("Interface_buttonClick_primary");
				Controller<HubController>().SelectLocation(button.Data as HubLocationProvider);
			}
		}

		[CompilerGenerated]
		private static bool _003CgetHighlightLocation_003Em__8A(Game g)
		{
			return g.Type == Game.GameType.AdventureGame && g.IsPremiumAvailable && g.IsUnlocked;
		}

		[CompilerGenerated]
		private static bool _003CgetHighlightLocation_003Em__8B(Game g)
		{
			return g.Type == Game.GameType.AdventureGame && g.IsPremiumAvailable && g.IsUnlocked && g.IsUnlockedSA;
		}

		[CompilerGenerated]
		private static bool _003CgetLocationMaterial_003Em__8F(Game g)
		{
			return !g.IsPremiumAvailable;
		}

		[CompilerGenerated]
		private static bool _003CgetLocationMaterial_003Em__90(Game g)
		{
			return !g.IsUnlocked;
		}

		[CompilerGenerated]
		private static bool _003CgetLocationMaterial_003Em__91(Game g)
		{
			return !g.IsUnlockedSA;
		}

		[CompilerGenerated]
		private static bool _003CgetLocationIcon_003Em__92(Game g)
		{
			return g.FreeLevelTo <= 0;
		}

		[CompilerGenerated]
		private static bool _003CgetLocationIcon_003Em__93(Game g)
		{
			return !g.IsUnlocked;
		}

		[CompilerGenerated]
		private static bool _003CgetLocationIcon_003Em__94(Game g)
		{
			return !g.IsUnlockedSA;
		}
	}
}
