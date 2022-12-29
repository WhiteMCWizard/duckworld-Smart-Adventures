using System;

namespace SLAM.KartRacing
{
	[Serializable]
	public class KRKartSettings
	{
		private const float minTopSpeed = 24f;

		private const float maxTopSpeed = 31f;

		private const float minTurnSpeed = 3.08f;

		private const float maxTurnSpeed = 5f;

		private const float minStartPush = 30f;

		private const float maxStartPush = 65f;

		private const float minTopSpeedPushBack = 1200f;

		private const float maxTopSpeedPushBack = 2300f;

		private const float minDownForceMagnitude = 25f;

		private const float maxDownForceMagnitude = 90f;

		private const float snowHandlingBoost = 0.35f;

		public float TopSpeed = 0.7f;

		public float Handling = 0.7f;

		public float Acceleration = 0.7f;

		public bool SnowBoost;

		public bool OilBoost;

		public float MinTopSpeed
		{
			get
			{
				return 24f;
			}
		}

		public float MaxTopSpeed
		{
			get
			{
				return 31f;
			}
		}

		public float MinTurnSpeed
		{
			get
			{
				return 3.08f;
			}
		}

		public float MaxTurnSpeed
		{
			get
			{
				return 5f;
			}
		}

		public float MinStartPush
		{
			get
			{
				return 30f;
			}
		}

		public float MaxStartPush
		{
			get
			{
				return 65f;
			}
		}

		public float MinTopSpeedPushBack
		{
			get
			{
				return 1200f;
			}
		}

		public float MaxTopSpeedPushBack
		{
			get
			{
				return 2300f;
			}
		}

		public float MinDownForceMagnitude
		{
			get
			{
				return 25f;
			}
		}

		public float MaxDownForceMagnitude
		{
			get
			{
				return 90f;
			}
		}

		public float GetBoostForMaterial(KRPhysicsMaterialType mat)
		{
			if (mat == KRPhysicsMaterialType.Snow)
			{
				return 0.35f;
			}
			return 0f;
		}
	}
}
