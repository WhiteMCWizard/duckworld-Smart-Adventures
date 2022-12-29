using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Kart
{
	public static class KartSystem
	{
		public enum ItemCategory
		{
			Colors = 0,
			Wheels = 1,
			SteeringWheels = 2,
			Bodies = 3,
			Spoilers = 4,
			Stickers = 5
		}

		public enum ItemStat
		{
			TopSpeed = 0,
			Handling = 1,
			Acceleration = 2
		}

		[CompilerGenerated]
		private sealed class _003CUpdateKart_003Ec__AnonStorey184
		{
			internal KartItemLibrary itemLibrary;

			internal KartItemLibrary.KartItem _003C_003Em__D5(KartConfigurationData.KartConfigItem kartItem)
			{
				return itemLibrary.GetItemByGUID(kartItem.GUID);
			}
		}

		[CompilerGenerated]
		private static Func<KartItemLibrary.KartItem, bool> _003C_003Ef__am_0024cache1;

		[CompilerGenerated]
		private static Func<KartItemLibrary.KartItem, bool> _003C_003Ef__am_0024cache2;

		public static KartConfigurationData PlayerKartConfiguration { get; set; }

		public static GameObject SpawnKart(KartConfigurationData config)
		{
			GameObject gameObject = new GameObject("Kart");
			UpdateKart(gameObject, config);
			return gameObject;
		}

		public static void UpdateKart(GameObject parent, KartConfigurationData config)
		{
			_003CUpdateKart_003Ec__AnonStorey184 _003CUpdateKart_003Ec__AnonStorey = new _003CUpdateKart_003Ec__AnonStorey184();
			parent.transform.DestroyChildren();
			_003CUpdateKart_003Ec__AnonStorey.itemLibrary = KartItemLibrary.GetItemLibrary();
			IEnumerable<KartItemLibrary.KartItem> enumerable = config.Items.Select(_003CUpdateKart_003Ec__AnonStorey._003C_003Em__D5);
			if (_003C_003Ef__am_0024cache1 == null)
			{
				_003C_003Ef__am_0024cache1 = _003CUpdateKart_003Em__D6;
			}
			KartItemLibrary.KartItem kartItem = enumerable.First(_003C_003Ef__am_0024cache1);
			GameObject gameObject = spawnItem(parent, kartItem, config.GetPrimaryColor(kartItem.GUID), config.GetSecondaryColor(kartItem.GUID));
			KartBodyAnchor componentInChildren = gameObject.GetComponentInChildren<KartBodyAnchor>();
			if (_003C_003Ef__am_0024cache2 == null)
			{
				_003C_003Ef__am_0024cache2 = _003CUpdateKart_003Em__D7;
			}
			KartItemLibrary.KartItem kartItem2 = enumerable.First(_003C_003Ef__am_0024cache2);
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				Transform child = gameObject.transform.GetChild(i);
				if (child.name.StartsWith("KS_Decal"))
				{
					if (child.name == kartItem2.Prefab.name)
					{
						child.gameObject.SetActive(true);
						Renderer component = child.GetComponent<Renderer>();
						component.material.SetColor("_RedColor", config.GetPrimaryColor(kartItem2.GUID));
						component.material.SetColor("_GreenColor", config.GetSecondaryColor(kartItem2.GUID));
					}
					else
					{
						child.gameObject.SetActive(false);
					}
				}
			}
			foreach (KartItemLibrary.KartItem item in enumerable)
			{
				foreach (Transform anchor in componentInChildren.GetAnchors(item.Category))
				{
					spawnItem(anchor.gameObject, item, config.GetPrimaryColor(item.GUID), config.GetSecondaryColor(item.GUID));
				}
			}
		}

		private static GameObject spawnItem(GameObject parent, KartItemLibrary.KartItem item, Color primaryColor, Color secondaryColor)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(item.Prefab, parent.transform.position, parent.transform.rotation) as GameObject;
			gameObject.transform.parent = parent.transform;
			gameObject.name = item.GUID;
			Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
			Renderer[] array = componentsInChildren;
			foreach (Renderer renderer in array)
			{
				Material[] materials = renderer.materials;
				foreach (Material material in materials)
				{
					if (material.HasProperty("_RedColor") && material.HasProperty("_GreenColor"))
					{
						material.SetColor("_RedColor", primaryColor);
						material.SetColor("_GreenColor", secondaryColor);
					}
				}
				renderer.materials = materials;
			}
			return gameObject;
		}

		[CompilerGenerated]
		private static bool _003CUpdateKart_003Em__D6(KartItemLibrary.KartItem i)
		{
			return i.Category == ItemCategory.Bodies;
		}

		[CompilerGenerated]
		private static bool _003CUpdateKart_003Em__D7(KartItemLibrary.KartItem i)
		{
			return i.Category == ItemCategory.Stickers;
		}
	}
}
