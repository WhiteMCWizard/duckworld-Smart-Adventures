using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.FollowTheTruck
{
	public class FTTTutorial1 : TutorialView
	{
		[CompilerGenerated]
		private sealed class _003Cstep1_003Ec__Iterator8B : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal GameObject _003Cobj_003E__0;

			internal FTTPickup _003CclosestFeather_003E__1;

			internal int _0024PC;

			internal object _0024current;

			internal FTTTutorial1 _003C_003Ef__this;

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
				//Discarded unreachable code: IL_0177
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_003C_003Ef__this.keyboardTooltip.Show(_003C_003Ef__this.avatarObject);
					_003Cobj_003E__0 = null;
					_0024current = CoroutineUtils.WaitForGameEvent<FTTCargoThrownEvent>(_003C_003Em__6D);
					_0024PC = 1;
					break;
				case 1u:
					_003C_003Ef__this.keyboardTooltip.Hide();
					_003C_003Ef__this.dodgeObstacleTooltip.Show(_003Cobj_003E__0.transform);
					goto case 2u;
				case 2u:
					if (!_003C_003Ef__this.avatarPassedObject(_003Cobj_003E__0.transform))
					{
						_0024current = null;
						_0024PC = 2;
						break;
					}
					_003C_003Ef__this.dodgeObstacleTooltip.Hide();
					_003CclosestFeather_003E__1 = UnityEngine.Object.FindObjectsOfType<FTTPickup>().OrderBy(_003C_003Em__6E).First();
					_003C_003Ef__this.featherTooltip.Show(_003CclosestFeather_003E__1.transform);
					goto case 3u;
				case 3u:
					if (_003CclosestFeather_003E__1 != null && !_003C_003Ef__this.avatarPassedObject(_003CclosestFeather_003E__1.transform))
					{
						_0024current = null;
						_0024PC = 3;
						break;
					}
					_003C_003Ef__this.featherTooltip.Hide();
					_0024PC = -1;
					goto default;
				default:
					return false;
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

			internal void _003C_003Em__6D(FTTCargoThrownEvent c)
			{
				_003Cobj_003E__0 = c.Object;
			}

			internal float _003C_003Em__6E(FTTPickup c)
			{
				return Vector3.Distance(c.transform.position, _003C_003Ef__this.avatarObject.transform.position);
			}
		}

		[SerializeField]
		private ToolTip keyboardTooltip;

		[SerializeField]
		private ToolTip dodgeObstacleTooltip;

		[SerializeField]
		private ToolTip featherTooltip;

		[SerializeField]
		private Transform avatarObject;

		protected override void Start()
		{
			base.Start();
			StartCoroutine(step1());
		}

		private IEnumerator step1()
		{
			keyboardTooltip.Show(avatarObject);
			GameObject obj = null;
			yield return CoroutineUtils.WaitForGameEvent<FTTCargoThrownEvent>(((_003Cstep1_003Ec__Iterator8B)(object)this)._003C_003Em__6D);
			keyboardTooltip.Hide();
			dodgeObstacleTooltip.Show(obj.transform);
			while (!avatarPassedObject(obj.transform))
			{
				yield return null;
			}
			dodgeObstacleTooltip.Hide();
			FTTPickup closestFeather = UnityEngine.Object.FindObjectsOfType<FTTPickup>().OrderBy(((_003Cstep1_003Ec__Iterator8B)(object)this)._003C_003Em__6E).First();
			featherTooltip.Show(closestFeather.transform);
			while (closestFeather != null && !avatarPassedObject(closestFeather.transform))
			{
				yield return null;
			}
			featherTooltip.Hide();
		}

		private bool avatarPassedObject(Transform obj)
		{
			return Vector3.Dot(avatarObject.transform.forward.normalized, (obj.transform.position - avatarObject.transform.position).normalized) < 0f;
		}
	}
}
