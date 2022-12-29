using UnityEngine;

namespace SLAM.KartRacing
{
	public class KRWheelPhysicsMaterialSettings
	{
		private float brakePower;

		private float forwardStiffness;

		private float sidewaysStiffness;

		[SerializeField]
		private KRPhysicsMaterialType materialType;

		public static KRWheelPhysicsMaterialSettings DefaultDirtSettings = new KRWheelPhysicsMaterialSettings
		{
			brakePower = 0f,
			forwardStiffness = 1f,
			sidewaysStiffness = 2.5f,
			materialType = KRPhysicsMaterialType.Dirt
		};

		public static KRWheelPhysicsMaterialSettings DefaultSnowSettings = new KRWheelPhysicsMaterialSettings
		{
			brakePower = 0f,
			forwardStiffness = 0.66f,
			sidewaysStiffness = 1f,
			materialType = KRPhysicsMaterialType.Snow
		};

		public static KRWheelPhysicsMaterialSettings DefaultIceSettings = new KRWheelPhysicsMaterialSettings
		{
			brakePower = 0f,
			forwardStiffness = 0f,
			sidewaysStiffness = 0f,
			materialType = KRPhysicsMaterialType.Ice
		};

		public static KRWheelPhysicsMaterialSettings DefaultMudSettings = new KRWheelPhysicsMaterialSettings
		{
			brakePower = 0.8f,
			forwardStiffness = 0.7f,
			sidewaysStiffness = 0.7f,
			materialType = KRPhysicsMaterialType.Mud
		};

		public static KRWheelPhysicsMaterialSettings DefaultOilSettings = new KRWheelPhysicsMaterialSettings
		{
			brakePower = 0f,
			forwardStiffness = 0f,
			sidewaysStiffness = 0f,
			materialType = KRPhysicsMaterialType.Oil
		};

		public static KRWheelPhysicsMaterialSettings DefaultWoodSettings = new KRWheelPhysicsMaterialSettings
		{
			brakePower = 0f,
			forwardStiffness = 1f,
			sidewaysStiffness = 2.5f,
			materialType = KRPhysicsMaterialType.Wood
		};

		public float BrakePower
		{
			get
			{
				return brakePower;
			}
		}

		public float ForwardStiffness
		{
			get
			{
				return forwardStiffness;
			}
		}

		public float SidewaysStiffness
		{
			get
			{
				return sidewaysStiffness;
			}
		}

		public KRPhysicsMaterialType MaterialType
		{
			get
			{
				return materialType;
			}
		}

		public static KRWheelPhysicsMaterialSettings GetSettingsForMaterial(KRPhysicsMaterialType type)
		{
			switch (type)
			{
			case KRPhysicsMaterialType.Dirt:
				return DefaultDirtSettings;
			case KRPhysicsMaterialType.Ice:
				return DefaultIceSettings;
			case KRPhysicsMaterialType.Mud:
				return DefaultMudSettings;
			case KRPhysicsMaterialType.Oil:
				return DefaultOilSettings;
			case KRPhysicsMaterialType.Snow:
				return DefaultSnowSettings;
			case KRPhysicsMaterialType.Wood:
				return DefaultWoodSettings;
			default:
				return DefaultDirtSettings;
			}
		}
	}
}
