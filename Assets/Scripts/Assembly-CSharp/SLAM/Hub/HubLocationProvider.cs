using System;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Hub
{
	public class HubLocationProvider : MonoBehaviour
	{
		[Serializable]
		public class HubGameMarker
		{
			[SerializeField]
			private Transform gameMarkerObject;

			[SerializeField]
			private Vector3 buttonOffset;

			[GameId]
			public int GameId;

			[SerializeField]
			private GameObject circleObject;

			[SerializeField]
			private GameObject pathObject;

			[SerializeField]
			private float pathUVLength;

			public Vector3 Position
			{
				get
				{
					return (!(gameMarkerObject != null)) ? Vector3.zero : gameMarkerObject.position;
				}
			}

			public Quaternion Rotation
			{
				get
				{
					return (!(gameMarkerObject != null)) ? Quaternion.identity : gameMarkerObject.rotation;
				}
			}

			public Vector3 ButtonOffset
			{
				get
				{
					return buttonOffset;
				}
			}

			public Vector3 MarkerScale
			{
				get
				{
					return gameMarkerObject.localScale;
				}
			}

			public GameObject CircleObject
			{
				get
				{
					return circleObject;
				}
			}

			public GameObject PathObject
			{
				get
				{
					return pathObject;
				}
			}

			public float PathUvLength
			{
				get
				{
					return pathUVLength;
				}
			}
		}

		[SerializeField]
		private int locationId;

		[SerializeField]
		private AnimationClip flyToAnimation;

		[SerializeField]
		private Transform zoomInLocation;

		[SerializeField]
		private AudioClip mouseOverSound;

		[SerializeField]
		private AudioClip ambientLoop;

		[SerializeField]
		private Transform iconLocation;

		[SerializeField]
		private HubMarkerView.HubMarkerIcon icon = HubMarkerView.HubMarkerIcon.Location;

		[SerializeField]
		private string iconSpriteName;

		[SerializeField]
		private HubGameMarker[] gameMarkers;

		[SerializeField]
		private Game.GameCharacter gameCharacter;

		public Game.GameCharacter GameCharacter
		{
			get
			{
				return gameCharacter;
			}
		}

		public string IconSpriteName
		{
			get
			{
				return iconSpriteName;
			}
		}

		public int LocationId
		{
			get
			{
				return locationId;
			}
		}

		public HubGameMarker[] GameMarkers
		{
			get
			{
				return gameMarkers;
			}
		}

		public AnimationClip FlyToAnimation
		{
			get
			{
				return flyToAnimation;
			}
		}

		public Transform ZoomInLocation
		{
			get
			{
				return zoomInLocation;
			}
		}

		public AudioClip MouseOverSound
		{
			get
			{
				return mouseOverSound;
			}
		}

		public AudioClip AmbientLoop
		{
			get
			{
				return ambientLoop;
			}
		}

		public Transform IconLocation
		{
			get
			{
				return iconLocation;
			}
		}

		public HubMarkerView.HubMarkerIcon MarkerIcon
		{
			get
			{
				return icon;
			}
		}

		private void OnDrawGizmos()
		{
			if (gameMarkers != null && gameMarkers.Length > 0)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(gameMarkers[0].Position, 0.1f);
				for (int i = 1; i < gameMarkers.Length; i++)
				{
					Vector3 direction = gameMarkers[i].Position - gameMarkers[i - 1].Position;
					GizmosUtils.DrawArrow(gameMarkers[i - 1].Position, direction);
					Gizmos.DrawSphere(gameMarkers[i].Position, 0.1f);
				}
			}
		}
	}
}
