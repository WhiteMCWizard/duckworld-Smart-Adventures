using System;
using System.Collections;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Fruityard
{
	public class FYHudView : HUDView
	{
		[Serializable]
		public struct FYHudTreeIcon
		{
			public GameObject Prefab;

			public FruityardGame.FYTreeType TreeType;
		}

		[CompilerGenerated]
		private sealed class _003ConLevelInit_003Ec__AnonStorey16C
		{
			internal FruityardGame.FYTreeType treeType;

			internal bool _003C_003Em__71(FYHudTreeIcon ic)
			{
				return ic.TreeType == treeType;
			}
		}

		[SerializeField]
		private FYHudTreeIcon[] icons;

		[SerializeField]
		private UIGrid gridParent;

		[SerializeField]
		private float fruitShowDelay = 0.5f;

		private void OnEnable()
		{
			GameEvents.Subscribe<FruityardGame.LevelInitEvent>(onLevelInit);
			GameEvents.Subscribe<FruityardGame.TreeCompletedEvent>(onTreeCompleted);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<FruityardGame.LevelInitEvent>(onLevelInit);
			GameEvents.Unsubscribe<FruityardGame.TreeCompletedEvent>(onTreeCompleted);
		}

		public IEnumerator ShowPickupList()
		{
			yield return null;
		}

		private void onLevelInit(FruityardGame.LevelInitEvent evt)
		{
			for (int i = 0; i < gridParent.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(gridParent.transform.GetChild(i).gameObject);
			}
			for (int j = 0; j < evt.PickupList.Count; j++)
			{
				_003ConLevelInit_003Ec__AnonStorey16C _003ConLevelInit_003Ec__AnonStorey16C = new _003ConLevelInit_003Ec__AnonStorey16C();
				_003ConLevelInit_003Ec__AnonStorey16C.treeType = evt.PickupList[j];
				GameObject prefab = icons.First(_003ConLevelInit_003Ec__AnonStorey16C._003C_003Em__71).Prefab;
				Transform transform = UnityEngine.Object.Instantiate(prefab).transform;
				transform.GetComponent<UITweener>().delay = (float)j * fruitShowDelay;
				transform.GetComponent<TweenAlpha>().delay = (float)j * fruitShowDelay;
				if (j + 1 >= evt.PickupList.Count)
				{
					transform.GetComponent<UITweener>().onFinished.Add(new EventDelegate(doneTweening));
				}
				gridParent.AddChild(transform);
				transform.transform.localScale = Vector3.one;
				transform.name = _003ConLevelInit_003Ec__AnonStorey16C.treeType.ToString();
				gridParent.Reposition();
				gridParent.repositionNow = true;
			}
		}

		private void doneTweening()
		{
			GameEvents.Invoke(new FruityardGame.LevelStartedEvent());
		}

		private void onTreeCompleted(FruityardGame.TreeCompletedEvent evt)
		{
			foreach (Transform item in gridParent.transform)
			{
				if (item.name.Equals(evt.Tree.Type.ToString()) && !item.GetChild(0).gameObject.activeSelf)
				{
					item.GetChild(0).gameObject.SetActive(true);
					break;
				}
			}
		}
	}
}
