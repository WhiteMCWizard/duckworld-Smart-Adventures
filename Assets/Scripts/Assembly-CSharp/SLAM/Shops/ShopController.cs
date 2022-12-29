using System.Runtime.CompilerServices;
using SLAM.Avatar;
using SLAM.Engine;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Shops
{
	[RequireComponent(typeof(Shop))]
	public class ShopController : InventoryController
	{
		[CompilerGenerated]
		private sealed class _003CPurchaseShoppingCartContents_003Ec__AnonStorey1AF
		{
			internal int totalAmount;

			internal ShopController _003C_003Ef__this;

			internal void _003C_003Em__16B()
			{
				_003C_003Ef__this.OpenView<LoadingView>();
				_003C_003Ef__this.shop.PurchaseShoppingCartContents(_003C_003Em__16E, _003C_003Ef__this.filter);
			}

			internal void _003C_003Em__16E(Shop.Feedback result)
			{
				if (result.WasSuccesfull)
				{
					ShopView view = _003C_003Ef__this.GetView<ShopView>();
					_003C_003Ef__this.cashInWallet -= totalAmount;
					view.UpdateWallet(_003C_003Ef__this.cashInWallet.ToString());
					view.UpdateShoppingCart(new ShopVariationDefinition[0], 0.ToString());
					view.Load(_003C_003Ef__this.shop.Items);
					ShopVariationDefinition[] shoppingCart = _003C_003Ef__this.shop.ShoppingCart;
					foreach (ShopVariationDefinition shopVariationDefinition in shoppingCart)
					{
						_003C_003Ef__this.originalAvatarConfig.ReplaceItem(shopVariationDefinition.Item.LibraryItem, _003C_003Ef__this.avatarLibrary);
					}
					_003C_003Ef__this.avatarAnimator.SetTrigger("SelectAvatar");
					AudioController.Play("Avatar_clothes_buyItems");
				}
				else
				{
					Debug.LogError("Purchase failed: " + result.Message);
				}
				_003C_003Ef__this.CloseView<LoadingView>();
			}
		}

		private int cashInWallet;

		private AvatarConfigurationData originalAvatarConfig;

		private Shop shop
		{
			get
			{
				return inventory as Shop;
			}
		}

		protected override void Start()
		{
			base.Start();
			originalAvatarConfig = (AvatarConfigurationData)base.AvatarConfig.Clone();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			GameEvents.Subscribe<ShoppingCartItemRemovedEvent>(OnItemRemovedFromCart);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			GameEvents.Unsubscribe<ShoppingCartItemRemovedEvent>(OnItemRemovedFromCart);
		}

		public void PurchaseShoppingCartContents()
		{
			_003CPurchaseShoppingCartContents_003Ec__AnonStorey1AF _003CPurchaseShoppingCartContents_003Ec__AnonStorey1AF = new _003CPurchaseShoppingCartContents_003Ec__AnonStorey1AF();
			_003CPurchaseShoppingCartContents_003Ec__AnonStorey1AF._003C_003Ef__this = this;
			_003CPurchaseShoppingCartContents_003Ec__AnonStorey1AF.totalAmount = shop.ShoppingCartValue;
			if (cashInWallet >= _003CPurchaseShoppingCartContents_003Ec__AnonStorey1AF.totalAmount)
			{
				GameEvents.Invoke(new PopupEvent(Localization.Get("UI_ARE_YOU_SURE"), StringFormatter.GetLocalizationFormatted("WR_POPUP_TOTAL_ITEM_COSTS", _003CPurchaseShoppingCartContents_003Ec__AnonStorey1AF.totalAmount), Localization.Get("UI_YES"), Localization.Get("UI_NO"), _003CPurchaseShoppingCartContents_003Ec__AnonStorey1AF._003C_003Em__16B, null));
			}
			else
			{
				GameEvents.Invoke(new PopupEvent(Localization.Get("WR_POPUP_TITLE_NOT_ENOUGH_COINS"), StringFormatter.GetLocalizationFormatted("WR_POPUP_NOT_ENOUGH_COINS", _003CPurchaseShoppingCartContents_003Ec__AnonStorey1AF.totalAmount, cashInWallet), Localization.Get("UI_OK"), null));
			}
		}

		public override void GoToHub()
		{
			if (shop.ShoppingCart.Length > 0)
			{
				GameEvents.Invoke(new PopupEvent(Localization.Get("UI_ARE_YOU_SURE"), Localization.Get("FS_POPUP_ITEMS_IN_BASKET"), Localization.Get("UI_YES"), Localization.Get("UI_NO"), _003CGoToHub_003Em__16C, null));
			}
			else
			{
				base.GoToHub();
			}
		}

		protected override void OnInventoryRetrieved()
		{
			ApiClient.GetWalletTotal(_003COnInventoryRetrieved_003Em__16D);
		}

		private void OnItemRemovedFromCart(ShoppingCartItemRemovedEvent evt)
		{
			shop.RemoveFromCart(evt.Removeditem);
			AvatarItemLibrary.AvatarItem itemByCategory = originalAvatarConfig.GetItemByCategory(evt.Removeditem.Item.LibraryItem.Category, avatarLibrary);
			base.AvatarConfig.ReplaceItem(itemByCategory, avatarLibrary);
			RefreshAvatar();
			GetView<ShopView>().UpdateShoppingCart(shop.ShoppingCart, shop.ShoppingCartValue.ToString());
			AudioController.Play("Avatar_clothes_removeItem");
		}

		protected override void OnVariationClicked(ShopVariationClickedEvent evt)
		{
			base.OnVariationClicked(evt);
			shop.AddToCart(evt.Data);
			GetView<ShopView>().UpdateShoppingCart(shop.ShoppingCart, shop.ShoppingCartValue.ToString());
		}

		[CompilerGenerated]
		private void _003CGoToHub_003Em__16C()
		{
			ShopCategoryDefinition[] items = shop.Items;
			foreach (ShopCategoryDefinition shopCategoryDefinition in items)
			{
				ShopVariationDefinition[] items2 = shopCategoryDefinition.Items;
				foreach (ShopVariationDefinition shopVariationDefinition in items2)
				{
					string[] items3 = base.AvatarConfig.Items;
					foreach (string text in items3)
					{
						if (shopVariationDefinition.Item.LibraryItem.GUID == text && shopVariationDefinition.HasBeenBoughtByPlayer)
						{
							originalAvatarConfig.ReplaceItem(shopVariationDefinition.Item.LibraryItem, avatarLibrary);
						}
					}
				}
			}
			base.AvatarConfig = (AvatarConfigurationData)originalAvatarConfig.Clone();
			base.GoToHub();
		}

		[CompilerGenerated]
		private void _003COnInventoryRetrieved_003Em__16D(int total)
		{
			base.OnInventoryRetrieved();
			cashInWallet = total;
			ShopView view = GetView<ShopView>();
			view.UpdateShoppingCart(shop.ShoppingCart, 0.ToString());
			view.UpdateWallet(cashInWallet.ToString());
		}
	}
}
