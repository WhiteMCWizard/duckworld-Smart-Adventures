using System;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Kart;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Kartshop
{
	public class KSSelectColorsView : View
	{
		[CompilerGenerated]
		private sealed class _003CSetInfo_003Ec__AnonStorey1A9
		{
			internal KartSystem.ItemCategory selected;

			internal bool _003C_003Em__154(KSShopCategoryDefinition c)
			{
				return c.Category == selected;
			}
		}

		[CompilerGenerated]
		private sealed class _003CselectCategory_003Ec__AnonStorey1AA
		{
			internal Color primaryColor;

			internal Color secondaryColor;

			internal bool _003C_003Em__157(ColorData c)
			{
				return c.Color == primaryColor;
			}

			internal bool _003C_003Em__158(ColorData c)
			{
				return c.Color == secondaryColor;
			}
		}

		[SerializeField]
		private UIPagination categoriesPagination;

		[SerializeField]
		private UIPagination primaryColorsPagination;

		[SerializeField]
		private UIPagination secondaryColorsPagination;

		private KartConfigurationData currentKartConfig;

		private ColorData[] primaryColors;

		private ColorData[] secondaryColors;

		[CompilerGenerated]
		private static Func<Color, ColorData> _003C_003Ef__am_0024cache6;

		[CompilerGenerated]
		private static Func<Color, ColorData> _003C_003Ef__am_0024cache7;

		private void OnEnable()
		{
			UIPagination uIPagination = categoriesPagination;
			uIPagination.OnItemCreated = (Action<GameObject, object>)Delegate.Combine(uIPagination.OnItemCreated, new Action<GameObject, object>(onCategoryCreated));
			UIPagination uIPagination2 = primaryColorsPagination;
			uIPagination2.OnItemCreated = (Action<GameObject, object>)Delegate.Combine(uIPagination2.OnItemCreated, new Action<GameObject, object>(onPrimaryColorPaginationCreated));
			UIPagination uIPagination3 = secondaryColorsPagination;
			uIPagination3.OnItemCreated = (Action<GameObject, object>)Delegate.Combine(uIPagination3.OnItemCreated, new Action<GameObject, object>(onSecondaryColorPaginationCreated));
			GameEvents.Subscribe<KSShopCategoryClickedEvent>(onCategoryClicked);
			GameEvents.Subscribe<KSShopColorItemClickedEvent>(onColorClicked);
		}

		private void OnDisable()
		{
			UIPagination uIPagination = categoriesPagination;
			uIPagination.OnItemCreated = (Action<GameObject, object>)Delegate.Remove(uIPagination.OnItemCreated, new Action<GameObject, object>(onCategoryCreated));
			UIPagination uIPagination2 = primaryColorsPagination;
			uIPagination2.OnItemCreated = (Action<GameObject, object>)Delegate.Remove(uIPagination2.OnItemCreated, new Action<GameObject, object>(onPrimaryColorPaginationCreated));
			UIPagination uIPagination3 = secondaryColorsPagination;
			uIPagination3.OnItemCreated = (Action<GameObject, object>)Delegate.Remove(uIPagination3.OnItemCreated, new Action<GameObject, object>(onSecondaryColorPaginationCreated));
			GameEvents.Unsubscribe<KSShopCategoryClickedEvent>(onCategoryClicked);
			GameEvents.Unsubscribe<KSShopColorItemClickedEvent>(onColorClicked);
		}

		public void SetInfo(Color[] primaryColors, Color[] secondaryColors, KSShopCategoryDefinition[] categories, KartConfigurationData kartConfig, KartSystem.ItemCategory selected)
		{
			_003CSetInfo_003Ec__AnonStorey1A9 _003CSetInfo_003Ec__AnonStorey1A = new _003CSetInfo_003Ec__AnonStorey1A9();
			_003CSetInfo_003Ec__AnonStorey1A.selected = selected;
			currentKartConfig = kartConfig;
			categoriesPagination.UpdateInfo(categories);
			KSShopCategoryDefinition kSShopCategoryDefinition = categories.FirstOrDefault(_003CSetInfo_003Ec__AnonStorey1A._003C_003Em__154);
			if (kSShopCategoryDefinition == null)
			{
				kSShopCategoryDefinition = categories[0];
			}
			categoriesPagination.OpenPageContaining(kSShopCategoryDefinition);
			if (_003C_003Ef__am_0024cache6 == null)
			{
				_003C_003Ef__am_0024cache6 = _003CSetInfo_003Em__155;
			}
			this.primaryColors = primaryColors.Select(_003C_003Ef__am_0024cache6).ToArray();
			if (_003C_003Ef__am_0024cache7 == null)
			{
				_003C_003Ef__am_0024cache7 = _003CSetInfo_003Em__156;
			}
			this.secondaryColors = secondaryColors.Select(_003C_003Ef__am_0024cache7).ToArray();
			primaryColorsPagination.UpdateInfo(this.primaryColors);
			secondaryColorsPagination.UpdateInfo(this.secondaryColors);
			selectCategory(kSShopCategoryDefinition);
		}

		private void onCategoryClicked(KSShopCategoryClickedEvent evt)
		{
			selectCategory(evt.Data);
		}

		private void onColorClicked(KSShopColorItemClickedEvent evt)
		{
			ColorData data = evt.Item.Data;
			Controller<KSShopController>().SetColor(data.IsPrimary, data.Color);
			selectColor(data, (!data.IsPrimary) ? secondaryColorsPagination : primaryColorsPagination);
		}

		private void onPrimaryColorPaginationCreated(GameObject go, object data)
		{
			go.GetComponent<KSShopColorItem>().Initialize((ColorData)data);
		}

		private void onSecondaryColorPaginationCreated(GameObject go, object data)
		{
			ColorData colorData = data as ColorData;
			colorData.IsPrimary = false;
			go.GetComponent<KSShopColorItem>().Initialize(colorData);
		}

		private void onCategoryCreated(GameObject go, object data)
		{
			KSShopCategory component = go.GetComponent<KSShopCategory>();
			component.Initialize((KSShopCategoryDefinition)data);
		}

		private void selectCategory(KSShopCategoryDefinition catDef)
		{
			_003CselectCategory_003Ec__AnonStorey1AA _003CselectCategory_003Ec__AnonStorey1AA = new _003CselectCategory_003Ec__AnonStorey1AA();
			GameObject[] itemsOnPage = categoriesPagination.ItemsOnPage;
			foreach (GameObject gameObject in itemsOnPage)
			{
				KSShopCategory component = gameObject.GetComponent<KSShopCategory>();
				if (component != null)
				{
					component.SetSelected(catDef == component.Data);
				}
			}
			_003CselectCategory_003Ec__AnonStorey1AA.primaryColor = currentKartConfig.GetPrimaryColor(currentKartConfig.GetItem(catDef.Category).GUID);
			_003CselectCategory_003Ec__AnonStorey1AA.secondaryColor = currentKartConfig.GetSecondaryColor(currentKartConfig.GetItem(catDef.Category).GUID);
			ColorData data = primaryColors.FirstOrDefault(_003CselectCategory_003Ec__AnonStorey1AA._003C_003Em__157);
			ColorData data2 = secondaryColors.FirstOrDefault(_003CselectCategory_003Ec__AnonStorey1AA._003C_003Em__158);
			selectColor(data, primaryColorsPagination);
			selectColor(data2, secondaryColorsPagination);
		}

		private void selectColor(ColorData data, UIPagination pag)
		{
			pag.OpenPageContaining(data);
			GameObject[] itemsOnPage = pag.ItemsOnPage;
			foreach (GameObject gameObject in itemsOnPage)
			{
				KSShopColorItem component = gameObject.GetComponent<KSShopColorItem>();
				if (component != null)
				{
					component.SetSelected(data == component.Data);
				}
			}
		}

		[CompilerGenerated]
		private static ColorData _003CSetInfo_003Em__155(Color s)
		{
			return new ColorData
			{
				Color = s,
				IsPrimary = true
			};
		}

		[CompilerGenerated]
		private static ColorData _003CSetInfo_003Em__156(Color s)
		{
			return new ColorData
			{
				Color = s,
				IsPrimary = false
			};
		}
	}
}
