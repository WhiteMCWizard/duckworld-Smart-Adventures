namespace SLAM.Kart
{
	public class AvatarKartSpawner : KartSpawner
	{
		public override KartConfigurationData Config
		{
			get
			{
				return KartSystem.PlayerKartConfiguration;
			}
		}
	}
}
