using UnityEngine;

namespace SLAM.Kart
{
	public class ConfigKartSpawner : KartSpawner
	{
		[SerializeField]
		private KartConfigurationData config;

		public override KartConfigurationData Config
		{
			get
			{
				return config;
			}
		}

		public void SetConfiguration(KartConfigurationData conf)
		{
			config = conf;
		}
	}
}
