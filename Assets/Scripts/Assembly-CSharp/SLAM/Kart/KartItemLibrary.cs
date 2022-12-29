using System;
using SLAM.KartRacing;
using SLAM.Shops;
using UnityEngine;

namespace SLAM.Kart
{
	public class KartItemLibrary : ItemLibrary<KartItemLibrary.KartItem>
	{
		[Serializable]
		public class KartItem : Item
		{
			public GameObject Prefab;

			public Texture2D Icon;

			[Range(0f, 1f)]
			public float TopSpeed;

			[Range(0f, 1f)]
			public float Acceleration;

			[Range(0f, 1f)]
			public float Handling;

			public bool Snow;

			public bool Oil;

			public KartSystem.ItemCategory Category;

			public override string CategoryName
			{
				get
				{
					return Category.ToString();
				}
			}

			public float GetStat(KartSystem.ItemStat stat, KRPhysicsMaterialType materialType = KRPhysicsMaterialType.Dirt)
			{
				float result;
				switch (stat)
				{
				case KartSystem.ItemStat.TopSpeed:
					result = TopSpeed;
					break;
				case KartSystem.ItemStat.Acceleration:
					result = Acceleration;
					break;
				default:
					result = Handling;
					break;
				}
				return result;
			}
		}

		public class KartItemGUID : PropertyAttribute
		{
		}

		[SerializeField]
		private Color[] primaryColorPalette;

		[SerializeField]
		private Color[] secondaryColorPalette;

		[SerializeField]
		private KartConfigurationData[] defaultConfigurations;

		public Color[] PrimaryColorPalette
		{
			get
			{
				return primaryColorPalette;
			}
		}

		public Color[] SecondaryColorPalette
		{
			get
			{
				return secondaryColorPalette;
			}
		}

		public KartConfigurationData[] DefaultConfigurations
		{
			get
			{
				return defaultConfigurations;
			}
		}

		public static KartItemLibrary GetItemLibrary()
		{
			return Resources.Load<KartItemLibrary>("Kart_Items");
		}
	}
}
