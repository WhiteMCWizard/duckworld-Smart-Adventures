using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Analytics;
using SLAM.Kart;
using SLAM.Shops;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Kartshop
{
	public class KSShop : MonoBehaviour
	{
		[Serializable]
		public class KSFilter
		{
			[Tooltip("Will be ignored when \"OnlyShowBoughtItems\" is checked.")]
			public int ShopId = -1;

			public List<KartSystem.ItemCategory> Categories;

			[Tooltip("Show items that are hidden; this used to show Santa Claus suits in your wardrobe after the Xmax period.")]
			public bool ShowHiddenItems;

			public bool OnlyShowBoughtItems = true;
		}

		[CompilerGenerated]
		private sealed class _003CRetrieveInventory_003Ec__AnonStorey1A0
		{
			internal Action ready;

			internal KSShop _003C_003Ef__this;

			internal void _003C_003Em__13E(ShopData shopData)
			{
				_003C_003Ef__this.rawShopData = shopData;
				_003C_003Ef__this.processShopItems(_003C_003Ef__this.filter, _003C_003Ef__this.rawShopData.Items, ready);
			}
		}

		[CompilerGenerated]
		private sealed class _003CPurchaseShoppingCartContents_003Ec__AnonStorey1A1
		{
			internal Action<Shop.Feedback> callback;

			internal KSShop _003C_003Ef__this;

			internal void _003C_003Em__13F(bool succes)
			{
				if (succes)
				{
					foreach (KSShopItemDefinition item in _003C_003Ef__this.shoppingCart)
					{
						GameEvents.Invoke(new TrackingEvent
						{
							Type = TrackingEvent.TrackingType.ItemBought,
							Arguments = new Dictionary<string, object>
							{
								{
									"ItemGUID",
									item.Item.LibraryItem.GUID
								},
								{
									"Price",
									item.Item.ShopItem.Price
								}
							}
						});
						item.Item.HasBeenBought = true;
					}
					_003C_003Ef__this.shoppingCart.Clear();
					callback(new Shop.Feedback(true, string.Empty));
				}
				else
				{
					callback(new Shop.Feedback(false, StringFormatter.GetLocalizationFormatted("WR_ERROR_PURCHASE_FAILED", _003C_003Ef__this.shoppingCart.Count)));
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetUserKart_003Ec__AnonStorey1A2
		{
			internal Action<KartConfigurationData> callback;

			internal void _003C_003Em__140(KartConfigurationData[] karts)
			{
				if (karts.Length == 0)
				{
					KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
					KartConfigurationData kartConfigurationData = (KartConfigurationData)itemLibrary.DefaultConfigurations.Last().Clone();
					kartConfigurationData.id = -1;
					kartConfigurationData.active = true;
					BuyKart(kartConfigurationData, 0, _003C_003Em__143);
					return;
				}
				KartConfigurationData obj = null;
				for (int i = 0; i < karts.Length; i++)
				{
					if (karts[i].active)
					{
						obj = karts[i];
					}
				}
				callback(obj);
			}

			internal void _003C_003Em__143(KartConfigurationData newKart)
			{
				callback(newKart);
			}
		}

		[CompilerGenerated]
		private sealed class _003CBuyKart_003Ec__AnonStorey1A3
		{
			internal int price;

			internal KartConfigurationData kart;

			internal Action<KartConfigurationData> callback;

			private static Func<ShopItemData, int> _003C_003Ef__am_0024cache3;

			internal void _003C_003Em__141(int total)
			{
				if (total >= price)
				{
					ApiClient.AddToWallet(-price, _003C_003Em__144);
					return;
				}
				Debug.LogError("No money: Want to spend " + price + " but have " + total);
				callback(null);
			}

			internal void _003C_003Em__144(bool transactionSucces)
			{
				ApiClient.GetAllShopItems(_003C_003Em__145);
			}

			internal void _003C_003Em__145(ShopItemData[] allShopItems)
			{
				List<int> list = new List<int>();
				IEnumerable<ShopItemData> collection = allShopItems.Where(_003C_003Em__146);
				if (_003C_003Ef__am_0024cache3 == null)
				{
					_003C_003Ef__am_0024cache3 = _003C_003Em__147;
				}
				list.AddRange(collection.Select(_003C_003Ef__am_0024cache3));
				ApiClient.AddItemsToInventory(list.ToArray(), _003C_003Em__148);
			}

			internal bool _003C_003Em__146(ShopItemData si)
			{
				return kart.HasItem(si.GUID);
			}

			private static int _003C_003Em__147(ShopItemData si)
			{
				return si.Id;
			}

			internal void _003C_003Em__148(bool succes)
			{
				if (succes)
				{
					KartConfigurationData kartConfigurationData = (KartConfigurationData)kart.Clone();
					kartConfigurationData.id = -1;
					kartConfigurationData.active = true;
					ApiClient.SaveKartConfiguration(kartConfigurationData, new Texture2D(4, 4).EncodeToPNG(), _003C_003Em__149);
					AudioController.Play("Avatar_clothes_buyItems");
				}
				else
				{
					Debug.Log("Failed adding items to inventory :(");
					callback(null);
				}
			}

			internal void _003C_003Em__149(KartConfigurationData config)
			{
				callback(config);
			}
		}

		[CompilerGenerated]
		private sealed class _003CprocessShopItems_003Ec__AnonStorey1A4
		{
			private sealed class _003CprocessShopItems_003Ec__AnonStorey1A5
			{
				internal ShopItemData shopItem;

				internal _003CprocessShopItems_003Ec__AnonStorey1A4 _003C_003Ef__ref_0024420;

				internal bool _003C_003Em__14A(PurchasedShopItemData i)
				{
					return i.ShopItemId == shopItem.Id;
				}
			}

			internal ShopItemData[] webserviceItems;

			internal KSFilter filter;

			internal Action callback;

			internal KSShop _003C_003Ef__this;

			private static Func<KSShopLibraryItem, KSShopItemDefinition> _003C_003Ef__am_0024cache4;

			internal void _003C_003Em__142(PurchasedShopItemData[] purchasedItems)
			{
				KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
				Dictionary<KartSystem.ItemCategory, List<KSShopLibraryItem>> dictionary = new Dictionary<KartSystem.ItemCategory, List<KSShopLibraryItem>>();
				List<KSShopCategoryDefinition> list = new List<KSShopCategoryDefinition>();
				_003CprocessShopItems_003Ec__AnonStorey1A5 _003CprocessShopItems_003Ec__AnonStorey1A = new _003CprocessShopItems_003Ec__AnonStorey1A5();
				_003CprocessShopItems_003Ec__AnonStorey1A._003C_003Ef__ref_0024420 = this;
				ShopItemData[] array = webserviceItems;
				for (int i = 0; i < array.Length; i++)
				{
					_003CprocessShopItems_003Ec__AnonStorey1A.shopItem = array[i];
					if (!_003CprocessShopItems_003Ec__AnonStorey1A.shopItem.VisibleInShop && !filter.ShowHiddenItems)
					{
						continue;
					}
					KartItemLibrary.KartItem itemByGUID = itemLibrary.GetItemByGUID(_003CprocessShopItems_003Ec__AnonStorey1A.shopItem.GUID);
					if (itemByGUID == null)
					{
						Debug.Log("Filter out " + _003CprocessShopItems_003Ec__AnonStorey1A.shopItem.GUID);
					}
					else if (filter.Categories.Contains(itemByGUID.Category))
					{
						if (!dictionary.ContainsKey(itemByGUID.Category))
						{
							dictionary.Add(itemByGUID.Category, new List<KSShopLibraryItem>());
						}
						PurchasedShopItemData purchasedShopItemData = purchasedItems.FirstOrDefault(_003CprocessShopItems_003Ec__AnonStorey1A._003C_003Em__14A);
						dictionary[itemByGUID.Category].Add(new KSShopLibraryItem
						{
							LibraryItem = itemByGUID,
							ShopItem = _003CprocessShopItems_003Ec__AnonStorey1A.shopItem,
							HasBeenBought = (purchasedShopItemData != null)
						});
					}
				}
				foreach (KartSystem.ItemCategory category in filter.Categories)
				{
					if (dictionary.ContainsKey(category))
					{
						KSShopCategoryDefinition kSShopCategoryDefinition = new KSShopCategoryDefinition
						{
							Category = category,
							SpriteName = category.ToString()
						};
						List<KSShopLibraryItem> collection = dictionary[category];
						if (_003C_003Ef__am_0024cache4 == null)
						{
							_003C_003Ef__am_0024cache4 = _003C_003Em__14B;
						}
						kSShopCategoryDefinition.Items = collection.Select(_003C_003Ef__am_0024cache4).ToArray();
						KSShopCategoryDefinition kSShopCategoryDefinition2 = kSShopCategoryDefinition;
						if (kSShopCategoryDefinition2.Items.Count() > 0)
						{
							list.Add(kSShopCategoryDefinition2);
						}
					}
				}
				_003C_003Ef__this.categoryDefinitions = list.ToArray();
				callback();
			}

			private static KSShopItemDefinition _003C_003Em__14B(KSShopLibraryItem item)
			{
				return new KSShopItemDefinition
				{
					Item = item
				};
			}
		}

		[SerializeField]
		protected KSFilter filter;

		protected KSShopCategoryDefinition[] categoryDefinitions;

		protected List<KSShopItemDefinition> shoppingCart;

		private ShopData rawShopData;

		public KSShopCategoryDefinition[] Items
		{
			get
			{
				return categoryDefinitions;
			}
		}

		public int ShoppingCartValue
		{
			get
			{
				int num = 0;
				for (int i = 0; i < shoppingCart.Count; i++)
				{
					num += shoppingCart[i].Item.ShopItem.Price;
				}
				return num;
			}
		}

		public KSShopItemDefinition[] ShoppingCart
		{
			get
			{
				return shoppingCart.ToArray();
			}
		}

		public ShopData RawShopData
		{
			get
			{
				return rawShopData;
			}
		}

		protected virtual void Start()
		{
			shoppingCart = new List<KSShopItemDefinition>();
			if (!SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
			{
				return;
			}
			if (AudioController.GetCategory("Music") != null && AudioController.GetCategory("Music").AudioItems.Length > 0)
			{
				AudioItem[] audioItems = AudioController.GetCategory("Music").AudioItems;
				foreach (AudioItem audioItem in audioItems)
				{
					AudioController.Play(audioItem.Name);
				}
			}
			else
			{
				Debug.LogWarning("Hey buddy, this game doesn't have music? Make sure there is an AudioController with a category 'Music'!");
			}
			if (AudioController.GetCategory("Ambience") != null && AudioController.GetCategory("Ambience").AudioItems.Length > 0)
			{
				AudioItem[] audioItems2 = AudioController.GetCategory("Ambience").AudioItems;
				foreach (AudioItem audioItem2 in audioItems2)
				{
					AudioController.Play(audioItem2.Name);
				}
			}
			else
			{
				Debug.LogWarning("Hey buddy, this game doesn't have ambience sounds? Make sure there is an AudioController with a category 'Ambience'!");
			}
		}

		protected virtual void Update()
		{
		}

		public virtual void RetrieveInventory(Action ready)
		{
			_003CRetrieveInventory_003Ec__AnonStorey1A0 _003CRetrieveInventory_003Ec__AnonStorey1A = new _003CRetrieveInventory_003Ec__AnonStorey1A0();
			_003CRetrieveInventory_003Ec__AnonStorey1A.ready = ready;
			_003CRetrieveInventory_003Ec__AnonStorey1A._003C_003Ef__this = this;
			ApiClient.GetShopItems(filter.ShopId, _003CRetrieveInventory_003Ec__AnonStorey1A._003C_003Em__13E);
		}

		public void AddToCart(KSShopItemDefinition item)
		{
			for (int num = shoppingCart.Count - 1; num >= 0; num--)
			{
				if (shoppingCart[num].Item.LibraryItem.Category == item.Item.LibraryItem.Category)
				{
					shoppingCart.RemoveAt(num);
				}
			}
			if (!item.Item.HasBeenBought && !shoppingCart.Contains(item))
			{
				shoppingCart.Add(item);
			}
		}

		public void RemoveFromCart(KSShopItemDefinition item)
		{
			if (shoppingCart.Contains(item))
			{
				shoppingCart.Remove(item);
			}
		}

		public void ClearCart()
		{
			shoppingCart.Clear();
		}

		public void PurchaseShoppingCartContents(Action<Shop.Feedback> callback)
		{
			_003CPurchaseShoppingCartContents_003Ec__AnonStorey1A1 _003CPurchaseShoppingCartContents_003Ec__AnonStorey1A = new _003CPurchaseShoppingCartContents_003Ec__AnonStorey1A1();
			_003CPurchaseShoppingCartContents_003Ec__AnonStorey1A.callback = callback;
			_003CPurchaseShoppingCartContents_003Ec__AnonStorey1A._003C_003Ef__this = this;
			AudioController.Play("Avatar_clothes_buyItems");
			int[] array = new int[shoppingCart.Count];
			for (int i = 0; i < shoppingCart.Count; i++)
			{
				if (!shoppingCart[i].Item.HasBeenBought)
				{
					array[i] = shoppingCart[i].Item.ShopItem.Id;
					continue;
				}
				_003CPurchaseShoppingCartContents_003Ec__AnonStorey1A.callback(new Shop.Feedback(false, Localization.Get("WR_ERROR_ITEM_ALREADY_BOUGHT") + " " + shoppingCart[i].Item.ShopItem.Title));
				return;
			}
			ApiClient.PurchaseItems(array, filter.ShopId, _003CPurchaseShoppingCartContents_003Ec__AnonStorey1A._003C_003Em__13F);
		}

		public static void GetUserKart(Action<KartConfigurationData> callback)
		{
			_003CGetUserKart_003Ec__AnonStorey1A2 _003CGetUserKart_003Ec__AnonStorey1A = new _003CGetUserKart_003Ec__AnonStorey1A2();
			_003CGetUserKart_003Ec__AnonStorey1A.callback = callback;
			ApiClient.GetKartConfigurations(_003CGetUserKart_003Ec__AnonStorey1A._003C_003Em__140);
		}

		public static void BuyKart(KartConfigurationData kart, int price, Action<KartConfigurationData> callback)
		{
			_003CBuyKart_003Ec__AnonStorey1A3 _003CBuyKart_003Ec__AnonStorey1A = new _003CBuyKart_003Ec__AnonStorey1A3();
			_003CBuyKart_003Ec__AnonStorey1A.price = price;
			_003CBuyKart_003Ec__AnonStorey1A.kart = kart;
			_003CBuyKart_003Ec__AnonStorey1A.callback = callback;
			ApiClient.GetWalletTotal(_003CBuyKart_003Ec__AnonStorey1A._003C_003Em__141);
		}

		protected void processShopItems(KSFilter filter, ShopItemData[] webserviceItems, Action callback)
		{
			_003CprocessShopItems_003Ec__AnonStorey1A4 _003CprocessShopItems_003Ec__AnonStorey1A = new _003CprocessShopItems_003Ec__AnonStorey1A4();
			_003CprocessShopItems_003Ec__AnonStorey1A.webserviceItems = webserviceItems;
			_003CprocessShopItems_003Ec__AnonStorey1A.filter = filter;
			_003CprocessShopItems_003Ec__AnonStorey1A.callback = callback;
			_003CprocessShopItems_003Ec__AnonStorey1A._003C_003Ef__this = this;
			ApiClient.GetPlayerPurchasedShopItems(_003CprocessShopItems_003Ec__AnonStorey1A._003C_003Em__142);
		}
	}
}
