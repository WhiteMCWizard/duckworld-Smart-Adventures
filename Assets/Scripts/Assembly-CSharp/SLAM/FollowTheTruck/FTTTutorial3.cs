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
	public class FTTTutorial3 : TutorialView
	{
		[CompilerGenerated]
		private sealed class _003Cstep1_003Ec__Iterator8D : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal FTTSewer _003CclosestBreakable_003E__0;

			internal int _0024PC;

			internal object _0024current;

			internal FTTTutorial3 _003C_003Ef__this;

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
				//Discarded unreachable code: IL_0126
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_003CclosestBreakable_003E__0 = UnityEngine.Object.FindObjectsOfType<FTTSewer>().OrderBy(_003C_003Em__70).First();
					goto case 1u;
				case 1u:
					if (Vector3.Distance(_003CclosestBreakable_003E__0.transform.position, _003C_003Ef__this.avatarObject.transform.position) > 15f)
					{
						_0024current = null;
						_0024PC = 1;
						break;
					}
					_003C_003Ef__this.moveTooltip.Show(_003CclosestBreakable_003E__0.transform);
					_003C_003Ef__this.moveTooltip2.Show(_003CclosestBreakable_003E__0.transform);
					goto case 2u;
				case 2u:
					if (!_003C_003Ef__this.avatarPassedObject(_003CclosestBreakable_003E__0.transform))
					{
						_0024current = null;
						_0024PC = 2;
						break;
					}
					_003C_003Ef__this.moveTooltip.Hide();
					_003C_003Ef__this.moveTooltip2.Hide();
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

			internal float _003C_003Em__70(FTTSewer c)
			{
				return Vector3.Distance(c.transform.position, _003C_003Ef__this.avatarObject.transform.position);
			}
		}

		[SerializeField]
		private ToolTip moveTooltip;

		[SerializeField]
		private ToolTip moveTooltip2;

		[SerializeField]
		private Transform avatarObject;

		protected override void Start()
		{
			base.Start();
			StartCoroutine(step1());
		}

		private IEnumerator step1()
		{
			FTTSewer closestBreakable = UnityEngine.Object.FindObjectsOfType<FTTSewer>().OrderBy(((_003Cstep1_003Ec__Iterator8D)(object)this)._003C_003Em__70).First();
			while (Vector3.Distance(closestBreakable.transform.position, avatarObject.transform.position) > 15f)
			{
				yield return null;
			}
			moveTooltip.Show(closestBreakable.transform);
			moveTooltip2.Show(closestBreakable.transform);
			while (!avatarPassedObject(closestBreakable.transform))
			{
				yield return null;
			}
			moveTooltip.Hide();
			moveTooltip2.Hide();
		}

		private bool avatarPassedObject(Transform obj)
		{
			return Vector3.Dot(avatarObject.transform.forward.normalized, (obj.transform.position - avatarObject.transform.position).normalized) < 0f;
		}
	}
}
