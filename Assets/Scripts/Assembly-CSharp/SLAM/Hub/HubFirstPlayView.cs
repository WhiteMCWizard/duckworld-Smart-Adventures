using System;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Hub
{
	public class HubFirstPlayView : HubMarkerView
	{
		private const int AVATARHOUSE_LOCATION_ID = 5;

		[SerializeField]
		private GameObject logoObject;

		[SerializeField]
		private GameObject labelPrefab;

		[SerializeField]
		private GameObject labelRoot;

		[SerializeField]
		private Material locationMaterial;

		[SerializeField]
		private HubMarkerIcon locationIcon;

		private HubLocationProvider location;

		[CompilerGenerated]
		private static Func<HubLocationProvider, bool> _003C_003Ef__am_0024cache6;

		private void OnEnable()
		{
			HubLocationProvider[] collection = UnityEngine.Object.FindObjectsOfType<HubLocationProvider>();
			if (_003C_003Ef__am_0024cache6 == null)
			{
				_003C_003Ef__am_0024cache6 = _003COnEnable_003Em__88;
			}
			location = collection.FirstOrDefault(_003C_003Ef__am_0024cache6);
			spawnMarker(location.IconLocation.position, location.IconLocation.rotation, location.IconLocation.localScale, locationMaterial, locationIcon, true, onAvatarHouseClicked);
		}

		private void onAvatarHouseClicked(HubMarkerButton btn)
		{
			clearMarkers();
			Controller<HubController>().AnimateToLocation(location, onArriveAtLocation);
			logoObject.SetActive(false);
		}

		private void onArriveAtLocation()
		{
			HubLocationProvider.HubGameMarker hubGameMarker = location.GameMarkers.First();
			spawnMarker(hubGameMarker.Position, hubGameMarker.Rotation, hubGameMarker.MarkerScale, locationMaterial, locationIcon, true, onMarkerClicked);
			showText(hubGameMarker.Position, Localization.Get("HUB_GAME_AVATARCREATOR"));
		}

		private void onMarkerClicked(HubMarkerButton obj)
		{
			Controller<HubController>().Play(new Game(-1, "AvatarCreator", "AvatarCreator"));
		}

		private void showText(Vector3 worldPos, string text)
		{
			Vector3 position = worldToScreen(worldPos);
			GameObject gameObject = UnityEngine.Object.Instantiate(labelPrefab);
			gameObject.transform.parent = labelRoot.transform;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.position = position;
			gameObject.GetComponent<UILabel>().text = text;
		}

		private Vector3 worldToScreen(Vector3 pos)
		{
			Vector3 result = UICamera.currentCamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(pos));
			result.z = 0f;
			return result;
		}

		public void OnOptionsClicked()
		{
			Controller<HubController>().OpenSettingsWindow();
		}

		[CompilerGenerated]
		private static bool _003COnEnable_003Em__88(HubLocationProvider loc)
		{
			return loc.LocationId == 5;
		}
	}
}
