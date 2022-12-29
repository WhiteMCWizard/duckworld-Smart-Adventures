using UnityEngine;

namespace SLAM.Shops
{
	public class ShopVariationDefinition
	{
		public ShopLibraryItem Item;

		public Texture2D Texture
		{
			get
			{
				return Item.LibraryItem.Icon;
			}
		}

		public int Price
		{
			get
			{
				return Item.ShopItem.Price;
			}
		}

		public bool HasBeenBoughtByPlayer { get; set; }
	}
}
