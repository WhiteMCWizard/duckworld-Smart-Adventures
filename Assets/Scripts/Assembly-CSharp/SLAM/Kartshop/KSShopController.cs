using System;
using System.Runtime.CompilerServices;
using SLAM.Analytics;
using SLAM.BuildSystem;
using SLAM.Engine;
using SLAM.Kart;
using SLAM.Shops;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Kartshop
{
	[RequireComponent(typeof(KSShop))]
	public class KSShopController : ViewController
	{
		public class KartChangedEvent
		{
			public KartConfigurationData Configuration;
		}

		[CompilerGenerated]
		private sealed class _003CbuyShoppingbasket_003Ec__AnonStorey1A6
		{
			internal int costs;

			internal KSShopController _003C_003Ef__this;

			internal void _003C_003Em__151(Shop.Feedback feedback)
			{
				if (costs <= _003C_003Ef__this.cashInWallet)
				{
					if (feedback.WasSuccesfull)
					{
						GameEvents.Invoke(new TrackingEvent
						{
							Type = TrackingEvent.TrackingType.KartCustomizedEvent
						});
						_003C_003Ef__this.selectedKart = _003C_003Ef__this.previewKart.Clone() as KartConfigurationData;
						_003C_003Ef__this.cashInWallet -= costs;
						_003C_003Ef__this.GetView<SLAM.Shops.HUDView>().SetInfo(_003C_003Ef__this.cashInWallet);
						_003C_003Ef__this.syncShop();
						_003C_003Ef__this.syncShoppingBasket();
					}
					else
					{
						GameEvents.Invoke(new PopupEvent("Error", feedback.Message, Localization.Get("UI_OK"), null, null, null));
					}
				}
				else
				{
					GameEvents.Invoke(new PopupEvent(Localization.Get("WR_POPUP_TITLE_NOT_ENOUGH_COINS"), Localization.Get("KS_POPUP_NOT_ENOUGH_COINS"), Localization.Get("UI_OK"), null, null, null));
				}
				_003C_003Ef__this.CloseView<LoadingView>();
			}
		}

		[SerializeField]
		private View[] views;

		[SerializeField]
		private Transform turnTable;

		protected KSShop inventory;

		private int cashInWallet;

		private KartConfigurationData selectedKart;

		private KartConfigurationData previewKart;

		private GameObject currentKartGO;

		private KartSystem.ItemCategory selectedCategory = KartSystem.ItemCategory.Bodies;

		[CompilerGenerated]
		private static Action<KartConfigurationData> _003C_003Ef__am_0024cache8;

		private KSShop shop
		{
			get
			{
				return inventory;
			}
		}

		protected override void Start()
		{
			base.Start();
			AddViews(views);
			inventory = GetComponent<KSShop>();
			OpenView<LoadingView>();
			KSShop.GetUserKart(_003CStart_003Em__14C);
		}

		private void OnEnable()
		{
			GameEvents.Subscribe<KSShopCategoryClickedEvent>(onCategoryClicked);
			GameEvents.Subscribe<ShoppingbasketItemRemoveEvent>(onShoppingCartItemRemoved);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<KSShopCategoryClickedEvent>(onCategoryClicked);
			GameEvents.Unsubscribe<ShoppingbasketItemRemoveEvent>(onShoppingCartItemRemoved);
		}

		public void OnSelectPart(KSShopItemDefinition item)
		{
			if (item.Item.HasBeenBought)
			{
				selectedKart.ReplaceItem(item.Item.LibraryItem);
			}
			inventory.AddToCart(item);
			syncShoppingBasket();
			previewKart.ReplaceItem(item.Item.LibraryItem);
			KartSystem.UpdateKart(currentKartGO, previewKart);
			KartChangedEvent kartChangedEvent = new KartChangedEvent();
			kartChangedEvent.Configuration = previewKart;
			GameEvents.Invoke(kartChangedEvent);
		}

		public void GoToHub()
		{
			if (shop.ShoppingCart.Length > 0)
			{
				ShowQuitEditingPopup();
			}
			else
			{
				doCloseEdit(true);
			}
		}

		public void ShowBuyPartsPopup()
		{
			int shoppingCartValue = inventory.ShoppingCartValue;
			if (shoppingCartValue > cashInWallet)
			{
				GameEvents.Invoke(new PopupEvent(Localization.Get("WR_POPUP_TITLE_NOT_ENOUGH_COINS"), Localization.Get("KS_POPUP_NOT_ENOUGH_COINS"), Localization.Get("UI_OK"), null, null, null));
				return;
			}
			GameEvents.Invoke(new PopupEvent(Localization.Get("UI_ARE_YOU_SURE"), StringFormatter.GetLocalizationFormatted("KS_CONFIRM_PURCHASE_PART", shoppingCartValue), Localization.Get("UI_YES"), Localization.Get("UI_NO"), _003CShowBuyPartsPopup_003Em__14D, null));
		}

		public void ShowQuitEditingPopup()
		{
			GameEvents.Invoke(new PopupEvent(Localization.Get("UI_ARE_YOU_SURE"), Localization.Get("KS_POPUP_ITEMS_IN_BASKET"), Localization.Get("UI_YES"), Localization.Get("UI_NO"), _003CShowQuitEditingPopup_003Em__14E, null));
		}

		public void SetColor(bool isPrimary, Color color)
		{
			KartItemLibrary.KartItem item = previewKart.GetItem(selectedCategory);
			if (isPrimary)
			{
				previewKart.SetPrimaryColor(item.GUID, color);
			}
			else
			{
				previewKart.SetSecondaryColor(item.GUID, color);
			}
			KartSystem.UpdateKart(currentKartGO, previewKart);
		}

		private void doCloseEdit(bool saveKartConf)
		{
			if (saveKartConf)
			{
				saveKart(previewKart);
				selectedKart = previewKart.Clone() as KartConfigurationData;
			}
			SceneManager.Load("Hub");
		}

		public void SwitchToColorMode()
		{
			CloseView<KSEditKartView>();
			KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
			OpenView<KSSelectColorsView>().SetInfo(itemLibrary.PrimaryColorPalette, itemLibrary.SecondaryColorPalette, inventory.Items, previewKart, selectedCategory);
		}

		public void SwitchToEditMode()
		{
			CloseView<KSSelectColorsView>();
			OpenView<KSEditKartView>().UpdateCategoryParts(inventory.Items.ToArray(), selectedCategory);
		}

		private void onInventoryRetrieved()
		{
			ApiClient.GetWalletTotal(_003ConInventoryRetrieved_003Em__14F);
		}

		private void onCategoryClicked(KSShopCategoryClickedEvent evt)
		{
			selectedCategory = evt.Data.Category;
		}

		private void onShoppingCartItemRemoved(ShoppingbasketItemRemoveEvent item)
		{
			previewKart.ReplaceItem(selectedKart.GetItem(item.RemovedItem.Item.LibraryItem.Category));
			spawnConfiguration(previewKart);
			GetView<KSEditKartView>().UpdateSelection(previewKart);
			syncShop();
			inventory.RemoveFromCart(item.RemovedItem);
			syncShoppingBasket();
			KartChangedEvent kartChangedEvent = new KartChangedEvent();
			kartChangedEvent.Configuration = previewKart;
			GameEvents.Invoke(kartChangedEvent);
		}

		private void saveKart(KartConfigurationData kart)
		{
			kart.active = true;
			byte[] image = new Texture2D(4, 4).EncodeToPNG();
			if (_003C_003Ef__am_0024cache8 == null)
			{
				_003C_003Ef__am_0024cache8 = _003CsaveKart_003Em__150;
			}
			ApiClient.SaveKartConfiguration(kart, image, _003C_003Ef__am_0024cache8);
		}

		private void spawnConfiguration(KartConfigurationData config)
		{
			if (currentKartGO != null)
			{
				UnityEngine.Object.Destroy(currentKartGO);
			}
			GameObject gameObject = KartSystem.SpawnKart(config);
			gameObject.transform.parent = turnTable;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			currentKartGO = gameObject.gameObject;
		}

		private void syncShop()
		{
			GetView<KSEditKartView>().UpdateCategoryParts(inventory.Items.ToArray(), selectedCategory);
		}

		private void syncShoppingBasket()
		{
			GetView<KSShopView>().UpdateShoppingbasket(inventory.ShoppingCart);
		}

		private void buyShoppingbasket()
		{
			_003CbuyShoppingbasket_003Ec__AnonStorey1A6 _003CbuyShoppingbasket_003Ec__AnonStorey1A = new _003CbuyShoppingbasket_003Ec__AnonStorey1A6();
			_003CbuyShoppingbasket_003Ec__AnonStorey1A._003C_003Ef__this = this;
			OpenView<LoadingView>();
			_003CbuyShoppingbasket_003Ec__AnonStorey1A.costs = inventory.ShoppingCartValue;
			inventory.PurchaseShoppingCartContents(_003CbuyShoppingbasket_003Ec__AnonStorey1A._003C_003Em__151);
		}

		[CompilerGenerated]
		private void _003CStart_003Em__14C(KartConfigurationData kart)
		{
			selectedKart = kart;
			inventory.RetrieveInventory(onInventoryRetrieved);
		}

		[CompilerGenerated]
		private void _003CShowBuyPartsPopup_003Em__14D()
		{
			buyShoppingbasket();
		}

		[CompilerGenerated]
		private void _003CShowQuitEditingPopup_003Em__14E()
		{
			doCloseEdit(false);
		}

		[CompilerGenerated]
		private void _003ConInventoryRetrieved_003Em__14F(int total)
		{
			cashInWallet = total;
			CloseView<LoadingView>();
			previewKart = selectedKart.Clone() as KartConfigurationData;
			spawnConfiguration(previewKart);
			OpenView<KSShopView>();
			OpenView<KSEditKartView>().UpdateSelection(previewKart);
			OpenView<SLAM.Shops.HUDView>().SetInfo(cashInWallet);
			OpenView<KSStatsView>().SetInfo(selectedKart);
			inventory.ClearCart();
			syncShop();
			syncShoppingBasket();
		}

		[CompilerGenerated]
		private static void _003CsaveKart_003Em__150(KartConfigurationData result)
		{
			KartSystem.PlayerKartConfiguration = result;
		}
	}
}
