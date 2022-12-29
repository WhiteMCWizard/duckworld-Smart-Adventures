using System;
using LitJson;
using SLAM.Shops;
using UnityEngine;

namespace SLAM.Avatar
{
	public class AvatarItemLibrary : ItemLibrary<AvatarItemLibrary.AvatarItem>
	{
		[Serializable]
		public class AvatarItem : Item
		{
			public Material Material;

			public string MeshName;

			public AvatarSystem.ItemCategory Category;

			public Texture2D Icon;

			public override string CategoryName
			{
				get
				{
					return Category.ToString();
				}
			}
		}

		public class AvatarItemGUID : PropertyAttribute
		{
		}

		public class AvatarSkinColor : PropertyAttribute
		{
		}

		[SerializeField]
		[JsonIgnore]
		private Color[] skinColors;

		[SerializeField]
		private AvatarConfigurationData[] defaultConfigurations;

		public Color[] SkinColors
		{
			get
			{
				return skinColors;
			}
		}

		public AvatarConfigurationData[] DefaultConfigurations
		{
			get
			{
				return defaultConfigurations;
			}
		}

		public static AvatarItemLibrary GetItemLibrary(AvatarConfigurationData configData)
		{
			return GetItemLibrary(configData.Race, configData.Gender);
		}

		public static AvatarItemLibrary GetItemLibrary(AvatarSystem.Race race, AvatarSystem.Gender gender)
		{
			return Resources.Load<AvatarItemLibrary>(string.Format("{0}_{1}_Items", gender, race));
		}
	}
}
