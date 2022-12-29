using System;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Kart;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Kartshop
{
	public class KSEditKartView : View
	{
		[CompilerGenerated]
		private sealed class _003CUpdateCategoryParts_003Ec__AnonStorey1A7
		{
			internal KartSystem.ItemCategory selected;

			internal bool _003C_003Em__152(KSShopCategoryDefinition c)
			{
				return c.Category == selected;
			}
		}

		[CompilerGenerated]
		private sealed class _003CselectCategory_003Ec__AnonStorey1A8
		{
			internal KSShopCategoryDefinition categoryDef;

			internal KSEditKartView _003C_003Ef__this;

			internal bool _003C_003Em__153(KSShopItemDefinition itm)
			{
				return itm.Item.LibraryItem.GUID == _003C_003Ef__this.currentKartConfig.GetItem(categoryDef.Category).GUID;
			}
		}

		[SerializeField]
		private UIPagination categoriesPagination;

		[SerializeField]
		private UIPagination itemsPagination;

		private KartConfigurationData currentKartConfig;

		private void OnEnable()
		{
			UIPagination uIPagination = categoriesPagination;
			uIPagination.OnItemCreated = (Action<GameObject, object>)Delegate.Combine(uIPagination.OnItemCreated, new Action<GameObject, object>(onCategoryCreated));
			UIPagination uIPagination2 = itemsPagination;
			uIPagination2.OnItemCreated = (Action<GameObject, object>)Delegate.Combine(uIPagination2.OnItemCreated, new Action<GameObject, object>(onItemCreated));
			GameEvents.Subscribe<KSShopCategoryClickedEvent>(onCategoryClicked);
			GameEvents.Subscribe<KSShopItemClickedEvent>(onShopItemClicked);
		}

		private void OnDisable()
		{
			UIPagination uIPagination = categoriesPagination;
			uIPagination.OnItemCreated = (Action<GameObject, object>)Delegate.Remove(uIPagination.OnItemCreated, new Action<GameObject, object>(onCategoryCreated));
			UIPagination uIPagination2 = itemsPagination;
			uIPagination2.OnItemCreated = (Action<GameObject, object>)Delegate.Remove(uIPagination2.OnItemCreated, new Action<GameObject, object>(onItemCreated));
			GameEvents.Unsubscribe<KSShopCategoryClickedEvent>(onCategoryClicked);
			GameEvents.Unsubscribe<KSShopItemClickedEvent>(onShopItemClicked);
		}

		public void UpdateCategoryParts(KSShopCategoryDefinition[] categories, KartSystem.ItemCategory selected)
		{
			_003CUpdateCategoryParts_003Ec__AnonStorey1A7 _003CUpdateCategoryParts_003Ec__AnonStorey1A = new _003CUpdateCategoryParts_003Ec__AnonStorey1A7();
			_003CUpdateCategoryParts_003Ec__AnonStorey1A.selected = selected;
			if (categories.Length > 0)
			{
				categoriesPagination.UpdateInfo(categories);
				KSShopCategoryDefinition kSShopCategoryDefinition = categories.FirstOrDefault(_003CUpdateCategoryParts_003Ec__AnonStorey1A._003C_003Em__152);
				if (kSShopCategoryDefinition == null)
				{
					kSShopCategoryDefinition = categories[0];
				}
				categoriesPagination.OpenPageContaining(kSShopCategoryDefinition);
				selectCategory(kSShopCategoryDefinition);
			}
		}

		public void UpdateSelection(KartConfigurationData kartConfig)
		{
			currentKartConfig = kartConfig;
		}

		private void onCategoryClicked(KSShopCategoryClickedEvent evt)
		{
			selectCategory(evt.Data);
		}

		private void onShopItemClicked(KSShopItemClickedEvent evt)
		{
			selectItem(evt.Data);
		}

		private void onCategoryCreated(GameObject go, object data)
		{
			go.GetComponent<KSShopCategory>().Initialize((KSShopCategoryDefinition)data);
		}

		private void onItemCreated(GameObject go, object data)
		{
			go.GetComponent<KSShopItem>().Initialize((KSShopItemDefinition)data);
		}

		private void selectCategory(KSShopCategoryDefinition categoryDef)
		{
			_003CselectCategory_003Ec__AnonStorey1A8 _003CselectCategory_003Ec__AnonStorey1A = new _003CselectCategory_003Ec__AnonStorey1A8();
			_003CselectCategory_003Ec__AnonStorey1A.categoryDef = categoryDef;
			_003CselectCategory_003Ec__AnonStorey1A._003C_003Ef__this = this;
			GameObject[] itemsOnPage = categoriesPagination.ItemsOnPage;
			foreach (GameObject gameObject in itemsOnPage)
			{
				KSShopCategory component = gameObject.GetComponent<KSShopCategory>();
				if (component != null && _003CselectCategory_003Ec__AnonStorey1A.categoryDef != null)
				{
					component.SetSelected(_003CselectCategory_003Ec__AnonStorey1A.categoryDef == component.Data);
				}
			}
			itemsPagination.UpdateInfo(_003CselectCategory_003Ec__AnonStorey1A.categoryDef.Items);
			itemsPagination.OpenPageContaining(_003CselectCategory_003Ec__AnonStorey1A.categoryDef);
			KSShopItemDefinition itemDef = _003CselectCategory_003Ec__AnonStorey1A.categoryDef.Items.FirstOrDefault(_003CselectCategory_003Ec__AnonStorey1A._003C_003Em__153);
			selectItem(itemDef);
		}

		private void selectItem(KSShopItemDefinition itemDef)
		{
			itemsPagination.OpenPageContaining(itemDef);
			GameObject[] itemsOnPage = itemsPagination.ItemsOnPage;
			foreach (GameObject gameObject in itemsOnPage)
			{
				KSShopItem component = gameObject.GetComponent<KSShopItem>();
				component.SetSelected(component.Data == itemDef);
			}
			Controller<KSShopController>().OnSelectPart(itemDef);
		}
	}
}
