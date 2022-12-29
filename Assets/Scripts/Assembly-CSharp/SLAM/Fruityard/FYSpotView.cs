using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Fruityard
{
	public class FYSpotView : View
	{
		[Serializable]
		public struct FYSpotViewIcon
		{
			public GameObject Prefab;

			public FruityardGame.FYTreeAction ActionType;
		}

		[CompilerGenerated]
		private sealed class _003CdoSpotUI_003Ec__Iterator99 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal FYSpot spot;

			internal Vector3 _003CnewPos_003E__0;

			internal int _003Caction_003E__1;

			internal GameObject _003Cobj_003E__2;

			internal UISprite _003CtimerWidget_003E__3;

			internal float _003CwaitingTime_003E__4;

			internal int _0024PC;

			internal object _0024current;

			internal FYSpot _003C_0024_003Espot;

			internal FYSpotView _003C_003Ef__this;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return _0024current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return _0024current;
				}
			}

			public bool MoveNext()
			{
				//Discarded unreachable code: IL_029d
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_0024current = null;
					_0024PC = 1;
					break;
				case 1u:
					if (spot.gameObject.activeInHierarchy)
					{
						_003CnewPos_003E__0 = UICamera.currentCamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(spot.transform.position + _003C_003Ef__this.iconOffset[spot.GetGrowPhase()]));
						_003CnewPos_003E__0.z = 0f;
						_003Caction_003E__1 = spot.RequiredActionIndex;
						_003Cobj_003E__2 = UnityEngine.Object.Instantiate(_003C_003Ef__this.icons.First(_003C_003Em__72).Prefab);
						_003Cobj_003E__2.transform.parent = _003C_003Ef__this.transform;
						_003Cobj_003E__2.transform.position = _003CnewPos_003E__0;
						_003Cobj_003E__2.transform.localScale = Vector3.one;
						_003Cobj_003E__2.GetComponentInChildren<FYIconListener>().spot = spot;
						_003CtimerWidget_003E__3 = UnityEngine.Object.Instantiate(_003C_003Ef__this.timerPrefab).GetComponent<UISprite>();
						_003CtimerWidget_003E__3.transform.parent = _003Cobj_003E__2.transform;
						_003CtimerWidget_003E__3.transform.localPosition = Vector3.down * 90f;
						_003CtimerWidget_003E__3.transform.localScale = Vector3.one;
						goto case 2u;
					}
					goto IL_0292;
				case 2u:
					if (spot.RequiredActionIndex == _003Caction_003E__1)
					{
						_003CtimerWidget_003E__3.fillAmount = 1f - spot.CurrentActionTimer.Progress;
						_0024current = null;
						_0024PC = 2;
						break;
					}
					UnityEngine.Object.Destroy(_003Cobj_003E__2);
					if (spot.RequiredActionIndex < spot.RequiredActions.Length)
					{
						_003CwaitingTime_003E__4 = ((spot.RequiredActionIndex <= _003Caction_003E__1) ? 0.3f : 3f);
						_0024current = new WaitForSeconds(_003CwaitingTime_003E__4);
						_0024PC = 3;
						break;
					}
					goto IL_0292;
				case 3u:
					_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.doSpotUI(spot));
					goto IL_0292;
				default:
					{
						return false;
					}
					IL_0292:
					_0024PC = -1;
					goto default;
				}
				return true;
			}

			[DebuggerHidden]
			public void Dispose()
			{
				_0024PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			internal bool _003C_003Em__72(FYSpotViewIcon i)
			{
				return i.ActionType == spot.CurrentRequiredAction;
			}
		}

		[SerializeField]
		private FYSpotViewIcon[] icons;

		[SerializeField]
		private Vector3[] iconOffset;

		[SerializeField]
		private GameObject timerPrefab;

		private void OnEnable()
		{
			GameEvents.Subscribe<FruityardGame.LevelCompletedEvent>(levelCompleted);
			FYSpot[] array = UnityEngine.Object.FindObjectsOfType<FYSpot>();
			foreach (FYSpot fYSpot in array)
			{
				updateSpotUI(fYSpot);
			}
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<FruityardGame.LevelCompletedEvent>(levelCompleted);
		}

		private void levelCompleted(FruityardGame.LevelCompletedEvent obj)
		{
			FYSpot[] array = UnityEngine.Object.FindObjectsOfType<FYSpot>();
			foreach (FYSpot fYSpot in array)
			{
				updateSpotUI(fYSpot);
			}
		}

		private void updateSpotUI(FYSpot fYSpot)
		{
			if (fYSpot.gameObject.activeInHierarchy)
			{
				StartCoroutine(doSpotUI(fYSpot));
			}
		}

		private IEnumerator doSpotUI(FYSpot spot)
		{
			yield return null;
			if (spot.gameObject.activeInHierarchy)
			{
				Vector3 newPos = UICamera.currentCamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(spot.transform.position + iconOffset[spot.GetGrowPhase()]));
				newPos.z = 0f;
				int action = spot.RequiredActionIndex;
				GameObject obj = UnityEngine.Object.Instantiate(icons.First(((_003CdoSpotUI_003Ec__Iterator99)(object)this)._003C_003Em__72).Prefab);
				obj.transform.parent = base.transform;
				obj.transform.position = newPos;
				obj.transform.localScale = Vector3.one;
				obj.GetComponentInChildren<FYIconListener>().spot = spot;
				UISprite timerWidget = UnityEngine.Object.Instantiate(timerPrefab).GetComponent<UISprite>();
				timerWidget.transform.parent = obj.transform;
				timerWidget.transform.localPosition = Vector3.down * 90f;
				timerWidget.transform.localScale = Vector3.one;
				while (spot.RequiredActionIndex == action)
				{
					timerWidget.fillAmount = 1f - spot.CurrentActionTimer.Progress;
					yield return null;
				}
				UnityEngine.Object.Destroy(obj);
				if (spot.RequiredActionIndex < spot.RequiredActions.Length)
				{
					float waitingTime = ((spot.RequiredActionIndex <= action) ? 0.3f : 3f);
					yield return new WaitForSeconds(waitingTime);
					StartCoroutine(doSpotUI(spot));
				}
			}
		}
	}
}
