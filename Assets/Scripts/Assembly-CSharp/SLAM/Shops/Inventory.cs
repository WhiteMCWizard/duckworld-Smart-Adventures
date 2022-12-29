using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Avatar;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Shops
{
	public class Inventory : MonoBehaviour
	{
		[Serializable]
		public class Filter
		{
			[HideInInspector]
			public AvatarSystem.Gender Gender;

			[HideInInspector]
			public AvatarSystem.Race Race;

			[Tooltip("Will be ignored when \"OnlyShowBoughtItems\" is checked.")]
			public int ShopId = -1;

			public List<AvatarSystem.ItemCategory> Categories;

			[Tooltip("Show items that are hidden; this used to show Santa Claus suits in your wardrobe after the Xmax period.")]
			public bool ShowHiddenItems;

			public bool OnlyShowBoughtItems = true;
		}

		[CompilerGenerated]
		private sealed class _003CRetrieveInventory_003Ec__AnonStorey1AB
		{
			internal Filter filter;

			internal Action ready;

			internal Inventory _003C_003Ef__this;

			internal void _003C_003Em__159(ShopItemData[] allItems)
			{
				_003C_003Ef__this.processShopItems(filter, allItems, ready);
			}

			internal void _003C_003Em__15A(ShopData shopData)
			{
				_003C_003Ef__this.processShopItems(filter, shopData.Items, ready);
			}
		}

		[CompilerGenerated]
		private sealed class _003CprocessShopItems_003Ec__AnonStorey1AC
		{
			private sealed class _003CprocessShopItems_003Ec__AnonStorey1AD
			{
				internal ShopLibraryItem vari;

				internal _003CprocessShopItems_003Ec__AnonStorey1AC _003C_003Ef__ref_0024428;

				internal bool _003C_003Em__15C(PurchasedShopItemData a)
				{
					return a.ShopItemId == vari.ShopItem.Id;
				}
			}

			internal Filter filter;

			internal ShopItemData[] items;

			internal Action callback;

			internal Inventory _003C_003Ef__this;

			internal void _003C_003Em__15B(PurchasedShopItemData[] purchasedItems)
			{
				AvatarItemLibrary itemLibrary = AvatarItemLibrary.GetItemLibrary(filter.Race, filter.Gender);
				Dictionary<AvatarSystem.ItemCategory, Dictionary<string, List<ShopLibraryItem>>> dictionary = new Dictionary<AvatarSystem.ItemCategory, Dictionary<string, List<ShopLibraryItem>>>();
				ShopItemData[] array = items;
				foreach (ShopItemData shopItemData in array)
				{
					if (!shopItemData.VisibleInShop && !filter.ShowHiddenItems)
					{
						continue;
					}
					AvatarItemLibrary.AvatarItem itemByGUID = itemLibrary.GetItemByGUID(shopItemData.GUID);
					if (itemByGUID != null && filter.Categories.Contains(itemByGUID.Category))
					{
						if (!dictionary.ContainsKey(itemByGUID.Category))
						{
							dictionary.Add(itemByGUID.Category, new Dictionary<string, List<ShopLibraryItem>>());
						}
						if (!dictionary[itemByGUID.Category].ContainsKey(itemByGUID.MeshName))
						{
							dictionary[itemByGUID.Category].Add(itemByGUID.MeshName, new List<ShopLibraryItem>());
						}
						dictionary[itemByGUID.Category][itemByGUID.MeshName].Add(new ShopLibraryItem
						{
							ShopItem = shopItemData,
							LibraryItem = itemByGUID
						});
					}
				}
				List<ShopCategoryDefinition> list = new List<ShopCategoryDefinition>();
				foreach (AvatarSystem.ItemCategory category in filter.Categories)
				{
					if (!dictionary.ContainsKey(category))
					{
						continue;
					}
					string empty = string.Empty;
					switch (category)
					{
					default:
						empty = "Shop_FS_icon_top";
						break;
					case AvatarSystem.ItemCategory.Legs:
						empty = "Shop_FS_icon_bottom";
						break;
					case AvatarSystem.ItemCategory.Feet:
						empty = "Shop_FS_icon_shoe";
						break;
					}
					ShopCategoryDefinition shopCategoryDefinition = new ShopCategoryDefinition
					{
						SpriteName = empty
					};
					List<ShopVariationDefinition> list2 = new List<ShopVariationDefinition>();
					foreach (KeyValuePair<string, List<ShopLibraryItem>> item in dictionary[category])
					{
						_003CprocessShopItems_003Ec__AnonStorey1AD _003CprocessShopItems_003Ec__AnonStorey1AD = new _003CprocessShopItems_003Ec__AnonStorey1AD();
						_003CprocessShopItems_003Ec__AnonStorey1AD._003C_003Ef__ref_0024428 = this;
						foreach (ShopLibraryItem item2 in item.Value)
						{
							_003CprocessShopItems_003Ec__AnonStorey1AD.vari = item2;
							bool flag = purchasedItems.FirstOrDefault(_003CprocessShopItems_003Ec__AnonStorey1AD._003C_003Em__15C) != null;
							if (filter.OnlyShowBoughtItems == flag)
							{
								list2.Add(new ShopVariationDefinition
								{
									Item = _003CprocessShopItems_003Ec__AnonStorey1AD.vari,
									HasBeenBoughtByPlayer = flag
								});
							}
						}
					}
					shopCategoryDefinition.Items = list2.ToArray();
					if (list2.Count > 0)
					{
						list.Add(shopCategoryDefinition);
					}
				}
				_003C_003Ef__this.categoryDefinitions = list.ToArray();
				callback();
			}
		}

		protected ShopCategoryDefinition[] categoryDefinitions;

		public ShopCategoryDefinition[] Items
		{
			get
			{
				return categoryDefinitions;
			}
		}

		protected virtual void Start()
		{
		}

		protected virtual void Update()
		{
		}

		public virtual void RetrieveInventory(Filter filter, Action ready)
		{
			_003CRetrieveInventory_003Ec__AnonStorey1AB _003CRetrieveInventory_003Ec__AnonStorey1AB = new _003CRetrieveInventory_003Ec__AnonStorey1AB();
			_003CRetrieveInventory_003Ec__AnonStorey1AB.filter = filter;
			_003CRetrieveInventory_003Ec__AnonStorey1AB.ready = ready;
			_003CRetrieveInventory_003Ec__AnonStorey1AB._003C_003Ef__this = this;
			if (_003CRetrieveInventory_003Ec__AnonStorey1AB.filter.OnlyShowBoughtItems)
			{
				ApiClient.GetAllShopItems(_003CRetrieveInventory_003Ec__AnonStorey1AB._003C_003Em__159);
			}
			else
			{
				ApiClient.GetShopItems(_003CRetrieveInventory_003Ec__AnonStorey1AB.filter.ShopId, _003CRetrieveInventory_003Ec__AnonStorey1AB._003C_003Em__15A);
			}
		}

		protected void processShopItems(Filter filter, ShopItemData[] items, Action callback)
		{
			_003CprocessShopItems_003Ec__AnonStorey1AC _003CprocessShopItems_003Ec__AnonStorey1AC = new _003CprocessShopItems_003Ec__AnonStorey1AC();
			_003CprocessShopItems_003Ec__AnonStorey1AC.filter = filter;
			_003CprocessShopItems_003Ec__AnonStorey1AC.items = items;
			_003CprocessShopItems_003Ec__AnonStorey1AC.callback = callback;
			_003CprocessShopItems_003Ec__AnonStorey1AC._003C_003Ef__this = this;
			ApiClient.GetPlayerPurchasedShopItems(_003CprocessShopItems_003Ec__AnonStorey1AC._003C_003Em__15B);
		}
	}
}
