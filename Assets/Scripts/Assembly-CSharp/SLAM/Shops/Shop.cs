using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Analytics;
using SLAM.Webservices;

namespace SLAM.Shops
{
	public class Shop : Inventory
	{
		public class Feedback
		{
			public bool WasSuccesfull { get; private set; }

			public string Message { get; private set; }

			public Feedback(bool succes, string message)
			{
				WasSuccesfull = succes;
				Message = message;
			}
		}

		[CompilerGenerated]
		private sealed class _003CPurchaseShoppingCartContents_003Ec__AnonStorey1AE
		{
			internal Filter filter;

			internal Action<Feedback> callback;

			internal Shop _003C_003Ef__this;

			internal void _003C_003Em__16A(bool succes)
			{
				if (succes)
				{
					foreach (ShopVariationDefinition item in _003C_003Ef__this.shoppingCart)
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
								{ "Price", item.Price }
							}
						});
						item.HasBeenBoughtByPlayer = true;
					}
					if (!filter.OnlyShowBoughtItems)
					{
						ShopCategoryDefinition[] categoryDefinitions = _003C_003Ef__this.categoryDefinitions;
						foreach (ShopCategoryDefinition shopCategoryDefinition in categoryDefinitions)
						{
							List<ShopVariationDefinition> list = new List<ShopVariationDefinition>(shopCategoryDefinition.Items);
							bool flag = false;
							foreach (ShopVariationDefinition item2 in _003C_003Ef__this.shoppingCart)
							{
								if (list.Contains(item2))
								{
									list.Remove(item2);
									flag = true;
								}
							}
							if (flag)
							{
								shopCategoryDefinition.Items = list.ToArray();
							}
						}
					}
					callback(new Feedback(true, string.Empty));
					_003C_003Ef__this.shoppingCart.Clear();
				}
				else
				{
					callback(new Feedback(true, StringFormatter.GetLocalizationFormatted("WR_ERROR_PURCHASE_FAILED", _003C_003Ef__this.shoppingCart.Count)));
				}
			}
		}

		protected List<ShopVariationDefinition> shoppingCart;

		public int ShoppingCartValue
		{
			get
			{
				int num = 0;
				for (int i = 0; i < shoppingCart.Count; i++)
				{
					num += shoppingCart[i].Price;
				}
				return num;
			}
		}

		public ShopVariationDefinition[] ShoppingCart
		{
			get
			{
				return shoppingCart.ToArray();
			}
		}

		protected override void Start()
		{
			base.Start();
			shoppingCart = new List<ShopVariationDefinition>();
		}

		public void AddToCart(ShopVariationDefinition item)
		{
			for (int num = shoppingCart.Count - 1; num >= 0; num--)
			{
				if (shoppingCart[num].Item.LibraryItem.Category == item.Item.LibraryItem.Category)
				{
					shoppingCart.RemoveAt(num);
				}
			}
			if (!item.HasBeenBoughtByPlayer && !shoppingCart.Contains(item))
			{
				shoppingCart.Add(item);
			}
		}

		public void RemoveFromCart(ShopVariationDefinition item)
		{
			if (shoppingCart.Contains(item))
			{
				shoppingCart.Remove(item);
			}
		}

		public void PurchaseShoppingCartContents(Action<Feedback> callback, Filter filter)
		{
			_003CPurchaseShoppingCartContents_003Ec__AnonStorey1AE _003CPurchaseShoppingCartContents_003Ec__AnonStorey1AE = new _003CPurchaseShoppingCartContents_003Ec__AnonStorey1AE();
			_003CPurchaseShoppingCartContents_003Ec__AnonStorey1AE.filter = filter;
			_003CPurchaseShoppingCartContents_003Ec__AnonStorey1AE.callback = callback;
			_003CPurchaseShoppingCartContents_003Ec__AnonStorey1AE._003C_003Ef__this = this;
			int[] array = new int[shoppingCart.Count];
			for (int i = 0; i < shoppingCart.Count; i++)
			{
				if (!shoppingCart[i].HasBeenBoughtByPlayer)
				{
					array[i] = shoppingCart[i].Item.ShopItem.Id;
					continue;
				}
				_003CPurchaseShoppingCartContents_003Ec__AnonStorey1AE.callback(new Feedback(false, Localization.Get("WR_ERROR_ITEM_ALREADY_BOUGHT") + " " + shoppingCart[i].Texture.name));
				return;
			}
			ApiClient.PurchaseItems(array, 1, _003CPurchaseShoppingCartContents_003Ec__AnonStorey1AE._003C_003Em__16A);
		}
	}
}
