using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.Sokoban
{
	public class SKBTutorialPushing : TutorialView
	{
		[CompilerGenerated]
		private sealed class _003Cstep2_003Ec__Iterator105 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal SKBCrate _003Ccrate_003E__0;

			internal SKBMarker _003Cmarker_003E__1;

			internal float _003Cangle_003E__2;

			internal int _0024PC;

			internal object _0024current;

			internal SKBTutorialPushing _003C_003Ef__this;

			public static Func<SKBCrate, bool> _003C_003Ef__am_0024cache6;

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
				//Discarded unreachable code: IL_0237
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
				{
					SKBTutorialPushing sKBTutorialPushing = _003C_003Ef__this;
					Vector3 position = _003C_003Ef__this.avatarController.transform.position;
					if (_003C_003Ef__am_0024cache6 == null)
					{
						_003C_003Ef__am_0024cache6 = _003C_003Em__DA;
					}
					_003Ccrate_003E__0 = sKBTutorialPushing.findNearestComponent(position, _003C_003Ef__am_0024cache6);
					if (_003Ccrate_003E__0 == null)
					{
						break;
					}
					_003Cmarker_003E__1 = _003C_003Ef__this.findNearestComponent<SKBMarker>(_003Ccrate_003E__0.transform.position, _003C_003Em__DB);
					if (_003Cmarker_003E__1 == null)
					{
						break;
					}
					_003C_003Ef__this.arrowTooltip.Show(_003Ccrate_003E__0.transform);
					_003Cangle_003E__2 = 0f;
					if (_003Ccrate_003E__0.transform.position.x > _003Cmarker_003E__1.transform.position.x)
					{
						_003Cangle_003E__2 = 180f;
					}
					_003C_003Ef__this.arrowTooltip.GO.transform.rotation = Quaternion.Euler(0f, 0f, _003Cangle_003E__2);
					goto case 1u;
				}
				case 1u:
					if (_003Ccrate_003E__0 != null && Mathf.Abs(_003Ccrate_003E__0.transform.position.x - _003Cmarker_003E__1.transform.position.x) > 0.5f && !_003Ccrate_003E__0.Completed)
					{
						_0024current = null;
						_0024PC = 1;
						return true;
					}
					_003C_003Ef__this.arrowTooltip.Hide();
					if (_003Ccrate_003E__0 != null)
					{
						_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.step3(_003Ccrate_003E__0, _003Cmarker_003E__1));
					}
					else
					{
						_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.step2());
					}
					_0024PC = -1;
					break;
				}
				return false;
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

			public static bool _003C_003Em__DA(SKBCrate c)
			{
				return !c.Completed;
			}

			internal bool _003C_003Em__DB(SKBMarker m)
			{
				return !m.Completed && m.MarkerType == _003Ccrate_003E__0.MarkerType;
			}
		}

		[CompilerGenerated]
		private sealed class _003CfindNearestComponent_003Ec__AnonStorey185<T> where T : Component
		{
			internal Vector3 pos;

			internal float _003C_003Em__D9(T c)
			{
				return (pos - c.transform.position).sqrMagnitude;
			}
		}

		[SerializeField]
		private ToolTip arrowTooltip;

		[SerializeField]
		private ToolTip moveTooltip;

		[SerializeField]
		private SKBAvatarController avatarController;

		[SerializeField]
		private int maxCrateCount = 3;

		private int crateCount;

		private void OnEnable()
		{
			GameEvents.Subscribe<SokobanGameController.MarkerCompletedEvent>(onMarkerCompleted);
			GameEvents.Subscribe<SokobanGameController.MarkerRemovedEvent>(onMarkerRemoved);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<SokobanGameController.MarkerCompletedEvent>(onMarkerCompleted);
			GameEvents.Unsubscribe<SokobanGameController.MarkerRemovedEvent>(onMarkerRemoved);
		}

		private void onMarkerCompleted(SokobanGameController.MarkerCompletedEvent obj)
		{
			if (++crateCount >= maxCrateCount)
			{
				Close();
			}
		}

		private void onMarkerRemoved(SokobanGameController.MarkerRemovedEvent obj)
		{
			crateCount--;
		}

		public override void Open(Callback callback, bool immediately)
		{
			base.Open(callback, immediately);
			StartCoroutine(step1());
		}

		private IEnumerator step1()
		{
			moveTooltip.Show(avatarController.transform);
			while (!avatarController.IsMoving)
			{
				yield return null;
			}
			moveTooltip.Hide();
			StartCoroutine(step2());
		}

		private IEnumerator step2()
		{
			SKBTutorialPushing sKBTutorialPushing = this;
			Vector3 position = avatarController.transform.position;
			if (_003Cstep2_003Ec__Iterator105._003C_003Ef__am_0024cache6 == null)
			{
				_003Cstep2_003Ec__Iterator105._003C_003Ef__am_0024cache6 = _003Cstep2_003Ec__Iterator105._003C_003Em__DA;
			}
			SKBCrate crate = sKBTutorialPushing.findNearestComponent(position, _003Cstep2_003Ec__Iterator105._003C_003Ef__am_0024cache6);
			if (crate == null)
			{
				yield break;
			}
			SKBMarker marker = findNearestComponent<SKBMarker>(crate.transform.position, ((_003Cstep2_003Ec__Iterator105)(object)this)._003C_003Em__DB);
			if (!(marker == null))
			{
				arrowTooltip.Show(crate.transform);
				float angle = 0f;
				if (crate.transform.position.x > marker.transform.position.x)
				{
					angle = 180f;
				}
				arrowTooltip.GO.transform.rotation = Quaternion.Euler(0f, 0f, angle);
				while (crate != null && Mathf.Abs(crate.transform.position.x - marker.transform.position.x) > 0.5f && !crate.Completed)
				{
					yield return null;
				}
				arrowTooltip.Hide();
				if (crate != null)
				{
					StartCoroutine(step3(crate, marker));
				}
				else
				{
					StartCoroutine(step2());
				}
			}
		}

		private IEnumerator step3(SKBCrate crate, SKBMarker marker)
		{
			arrowTooltip.Show(crate.transform);
			float angle = Mathf.Sign(crate.transform.position.z - marker.transform.position.z) * -90f;
			arrowTooltip.GO.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
			while (crate != null && Mathf.Abs(crate.transform.position.z - marker.transform.position.z) > 0.5f && !crate.Completed)
			{
				yield return null;
			}
			arrowTooltip.Hide();
			StartCoroutine(step2());
		}

		private T findNearestComponent<T>(Vector3 pos, Func<T, bool> compare) where T : Component
		{
			_003CfindNearestComponent_003Ec__AnonStorey185<T> _003CfindNearestComponent_003Ec__AnonStorey = new _003CfindNearestComponent_003Ec__AnonStorey185<T>();
			_003CfindNearestComponent_003Ec__AnonStorey.pos = pos;
			return UnityEngine.Object.FindObjectsOfType<T>().Where(compare).Min((Func<T, float>)_003CfindNearestComponent_003Ec__AnonStorey._003C_003Em__D9);
		}
	}
}
