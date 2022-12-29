using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LitJson;
using SLAM.KartRacing;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Kart
{
	[Serializable]
	public class KartConfigurationData : ICloneable
	{
		[Serializable]
		public struct KartConfigItem
		{
			[KartItemLibrary.KartItemGUID]
			public string GUID;

			public int PrimaryColorPaletteIndex;

			public int SecondaryColorPaletteIndex;
		}

		[CompilerGenerated]
		private sealed class _003CGetPrimaryColor_003Ec__AnonStorey17F
		{
			internal string guid;

			internal bool _003C_003Em__D0(KartConfigItem kc)
			{
				return kc.GUID == guid;
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetSecondaryColor_003Ec__AnonStorey180
		{
			internal string guid;

			internal bool _003C_003Em__D1(KartConfigItem kc)
			{
				return kc.GUID == guid;
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetStat_003Ec__AnonStorey181
		{
			internal KartItemLibrary itemLibrary;

			internal KartSystem.ItemStat stat;

			internal KRPhysicsMaterialType materialType;

			internal float _003C_003Em__D2(KartConfigItem kartItem)
			{
				return itemLibrary.GetItemByGUID(kartItem.GUID).GetStat(stat, materialType);
			}
		}

		[CompilerGenerated]
		private sealed class _003CHasSnowItem_003Ec__AnonStorey182
		{
			internal KartItemLibrary lib;

			internal bool _003C_003Em__D3(KartConfigItem it)
			{
				return lib.GetItemByGUID(it.GUID).Snow;
			}
		}

		[CompilerGenerated]
		private sealed class _003CHasOilItem_003Ec__AnonStorey183
		{
			internal KartItemLibrary lib;

			internal bool _003C_003Em__D4(KartConfigItem it)
			{
				return lib.GetItemByGUID(it.GUID).Oil;
			}
		}

		[JsonName("id")]
		[HideInInspector]
		public int id;

		[JsonName("active")]
		public bool active;

		[JsonName("config")]
		public KartConfigItem[] Items;

		public Color GetPrimaryColor(string guid)
		{
			_003CGetPrimaryColor_003Ec__AnonStorey17F _003CGetPrimaryColor_003Ec__AnonStorey17F = new _003CGetPrimaryColor_003Ec__AnonStorey17F();
			_003CGetPrimaryColor_003Ec__AnonStorey17F.guid = guid;
			KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
			return itemLibrary.PrimaryColorPalette[Items.First(_003CGetPrimaryColor_003Ec__AnonStorey17F._003C_003Em__D0).PrimaryColorPaletteIndex];
		}

		public Color GetSecondaryColor(string guid)
		{
			_003CGetSecondaryColor_003Ec__AnonStorey180 _003CGetSecondaryColor_003Ec__AnonStorey = new _003CGetSecondaryColor_003Ec__AnonStorey180();
			_003CGetSecondaryColor_003Ec__AnonStorey.guid = guid;
			KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
			return itemLibrary.SecondaryColorPalette[Items.First(_003CGetSecondaryColor_003Ec__AnonStorey._003C_003Em__D1).SecondaryColorPaletteIndex];
		}

		public void SetPrimaryColor(string guid, Color color)
		{
			KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
			for (int i = 0; i < Items.Length; i++)
			{
				if (Items[i].GUID == guid)
				{
					Items[i].PrimaryColorPaletteIndex = Array.IndexOf(itemLibrary.PrimaryColorPalette, color);
				}
			}
		}

		public void SetSecondaryColor(string guid, Color color)
		{
			KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
			for (int i = 0; i < Items.Length; i++)
			{
				if (Items[i].GUID == guid)
				{
					Items[i].SecondaryColorPaletteIndex = Array.IndexOf(itemLibrary.SecondaryColorPalette, color);
				}
			}
		}

		public float GetStat(KartSystem.ItemStat stat, KRPhysicsMaterialType materialType = KRPhysicsMaterialType.Dirt)
		{
			_003CGetStat_003Ec__AnonStorey181 _003CGetStat_003Ec__AnonStorey = new _003CGetStat_003Ec__AnonStorey181();
			_003CGetStat_003Ec__AnonStorey.stat = stat;
			_003CGetStat_003Ec__AnonStorey.materialType = materialType;
			_003CGetStat_003Ec__AnonStorey.itemLibrary = KartItemLibrary.GetItemLibrary();
			return Items.Select(_003CGetStat_003Ec__AnonStorey._003C_003Em__D2).Sum();
		}

		public bool HasSnowItem()
		{
			_003CHasSnowItem_003Ec__AnonStorey182 _003CHasSnowItem_003Ec__AnonStorey = new _003CHasSnowItem_003Ec__AnonStorey182();
			_003CHasSnowItem_003Ec__AnonStorey.lib = KartItemLibrary.GetItemLibrary();
			return Items.Any(_003CHasSnowItem_003Ec__AnonStorey._003C_003Em__D3);
		}

		public bool HasOilItem()
		{
			_003CHasOilItem_003Ec__AnonStorey183 _003CHasOilItem_003Ec__AnonStorey = new _003CHasOilItem_003Ec__AnonStorey183();
			_003CHasOilItem_003Ec__AnonStorey.lib = KartItemLibrary.GetItemLibrary();
			return Items.Any(_003CHasOilItem_003Ec__AnonStorey._003C_003Em__D4);
		}

		public bool ReplaceItem(KartItemLibrary.KartItem newItem)
		{
			KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
			for (int i = 0; i < Items.Length; i++)
			{
				KartItemLibrary.KartItem itemByGUID = itemLibrary.GetItemByGUID(Items[i].GUID);
				if (itemByGUID != null && itemByGUID.Category == newItem.Category)
				{
					bool result = Items[i].GUID != newItem.GUID;
					Items[i].GUID = newItem.GUID;
					return result;
				}
			}
			List<KartConfigItem> list = new List<KartConfigItem>(Items);
			list.Add(new KartConfigItem
			{
				GUID = newItem.GUID,
				PrimaryColorPaletteIndex = 0,
				SecondaryColorPaletteIndex = 0
			});
			Items = list.ToArray();
			return true;
		}

		public bool HasItem(string guid)
		{
			for (int i = 0; i < Items.Length; i++)
			{
				if (Items[i].GUID == guid)
				{
					return true;
				}
			}
			return false;
		}

		public KartItemLibrary.KartItem GetItem(KartSystem.ItemCategory category)
		{
			KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
			KartConfigItem[] items = Items;
			for (int i = 0; i < items.Length; i++)
			{
				KartConfigItem kartConfigItem = items[i];
				KartItemLibrary.KartItem itemByGUID = itemLibrary.GetItemByGUID(kartConfigItem.GUID);
				if (itemByGUID.Category == category)
				{
					return itemByGUID;
				}
			}
			return null;
		}

		public object Clone()
		{
			KartConfigurationData kartConfigurationData = new KartConfigurationData();
			kartConfigurationData.id = id;
			kartConfigurationData.active = active;
			kartConfigurationData.Items = new KartConfigItem[Items.Length];
			Array.Copy(Items, kartConfigurationData.Items, Items.Length);
			return kartConfigurationData;
		}

		public override string ToString()
		{
			return string.Format("|{0}|{1}|{2}|", "id=" + id, "active?" + active, "itemcount=" + Items.Length);
		}
	}
}
