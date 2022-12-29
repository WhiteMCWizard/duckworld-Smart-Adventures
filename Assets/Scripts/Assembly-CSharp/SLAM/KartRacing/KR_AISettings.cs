using SLAM.Avatar;
using SLAM.Kart;
using UnityEngine;

namespace SLAM.KartRacing
{
	public class KR_AISettings : MonoBehaviour
	{
		[SerializeField]
		private float rubberBanding;

		[SerializeField]
		private float baseDifficulty;

		[SerializeField]
		private KartConfigurationData config = new KartConfigurationData();

		[SerializeField]
		private AvatarConfigurationData avatar = new AvatarConfigurationData();

		[SerializeField]
		[Tooltip("A prefab takes precedence over the avatar settings")]
		private GameObject avatarPrefab;

		[SerializeField]
		private string localizationName;

		public KartConfigurationData Config
		{
			get
			{
				return config;
			}
		}

		public AvatarConfigurationData Avatar
		{
			get
			{
				return avatar;
			}
		}

		public GameObject AvatarPrefab
		{
			get
			{
				return avatarPrefab;
			}
		}

		public float RubberBanding
		{
			get
			{
				return rubberBanding;
			}
		}

		public float BaseDifficulty
		{
			get
			{
				return baseDifficulty;
			}
		}

		public string LocalizationName
		{
			get
			{
				return localizationName;
			}
		}
	}
}
