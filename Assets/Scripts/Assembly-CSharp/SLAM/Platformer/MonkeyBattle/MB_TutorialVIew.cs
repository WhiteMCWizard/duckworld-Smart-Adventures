using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle
{
	public class MB_TutorialVIew : TutorialView
	{
		[CompilerGenerated]
		private sealed class _003CexplainTurrets_003Ec__IteratorEA : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal MB_Turret _003Cturret_003E__0;

			internal int _0024PC;

			internal object _0024current;

			internal MB_TutorialVIew _003C_003Ef__this;

			public static Func<MB_Turret, bool> _003C_003Ef__am_0024cache4;

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
				//Discarded unreachable code: IL_012b
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
				{
					MB_Turret[] turrets = _003C_003Ef__this.turrets;
					if (_003C_003Ef__am_0024cache4 == null)
					{
						_003C_003Ef__am_0024cache4 = _003C_003Em__BB;
					}
					_003Cturret_003E__0 = turrets.FirstOrDefault(_003C_003Ef__am_0024cache4);
					if (_003Cturret_003E__0 != null)
					{
						_003C_003Ef__this.turretPointerToolTip.Show(_003Cturret_003E__0.transform);
						_003C_003Ef__this.turretToolTip.Show(_003C_003Ef__this.turretPointerToolTip.GO.transform);
						goto case 1u;
					}
					_0024current = null;
					_0024PC = 2;
					break;
				}
				case 1u:
					if (!_003Cturret_003E__0.IsActivated)
					{
						_0024current = null;
						_0024PC = 1;
						break;
					}
					_003C_003Ef__this.turretPointerToolTip.Hide();
					_003C_003Ef__this.turretToolTip.Hide();
					goto IL_0120;
				case 2u:
					_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.explainTurrets());
					goto IL_0120;
				default:
					{
						return false;
					}
					IL_0120:
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

			public static bool _003C_003Em__BB(MB_Turret t)
			{
				return t.enabled;
			}
		}

		[SerializeField]
		private ToolTip moveToolTip;

		[SerializeField]
		private ToolTip movePointerToolTip;

		[SerializeField]
		private ToolTip turretToolTip;

		[SerializeField]
		private ToolTip turretPointerToolTip;

		private MB_PlayerController avatar;

		private MB_Turret[] turrets;

		protected override void Start()
		{
			base.Start();
			avatar = UnityEngine.Object.FindObjectOfType<MB_PlayerController>();
			turrets = UnityEngine.Object.FindObjectsOfType<MB_Turret>();
			StartCoroutine(explainMoving());
		}

		private IEnumerator explainMoving()
		{
			movePointerToolTip.Show(avatar.transform);
			moveToolTip.Show(avatar.transform);
			while (!SLAMInput.Provider.GetButton(SLAMInput.Button.Right) && !SLAMInput.Provider.GetButton(SLAMInput.Button.Left))
			{
				yield return null;
			}
			yield return new WaitForSeconds(1f);
			movePointerToolTip.Hide();
			moveToolTip.Hide();
			yield return new WaitForSeconds(1f);
			StartCoroutine(explainTurrets());
		}

		private IEnumerator explainTurrets()
		{
			MB_Turret[] collection = turrets;
			if (_003CexplainTurrets_003Ec__IteratorEA._003C_003Ef__am_0024cache4 == null)
			{
				_003CexplainTurrets_003Ec__IteratorEA._003C_003Ef__am_0024cache4 = _003CexplainTurrets_003Ec__IteratorEA._003C_003Em__BB;
			}
			MB_Turret turret = collection.FirstOrDefault(_003CexplainTurrets_003Ec__IteratorEA._003C_003Ef__am_0024cache4);
			if (turret != null)
			{
				turretPointerToolTip.Show(turret.transform);
				turretToolTip.Show(turretPointerToolTip.GO.transform);
				while (!turret.IsActivated)
				{
					yield return null;
				}
				turretPointerToolTip.Hide();
				turretToolTip.Hide();
			}
			else
			{
				yield return null;
				StartCoroutine(explainTurrets());
			}
		}
	}
}
