using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.ZooTransport
{
	public class ZTtutorialView : TutorialView
	{
		[CompilerGenerated]
		private sealed class _003Cstep1_003Ec__Iterator14D : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal int _0024PC;

			internal object _0024current;

			internal ZTtutorialView _003C_003Ef__this;

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
				//Discarded unreachable code: IL_00d9
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
				case 1u:
					if (_003C_003Ef__this.driveColliders.Count(_003C_003Em__195) == 0)
					{
						_0024current = null;
						_0024PC = 1;
						break;
					}
					_003C_003Ef__this.driveToolTip.Show();
					goto case 2u;
				case 2u:
					if (_003C_003Ef__this.driveColliders.Count(_003C_003Em__196) != 0)
					{
						_0024current = null;
						_0024PC = 2;
						break;
					}
					_003C_003Ef__this.driveToolTip.Hide();
					_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.step1());
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

			internal bool _003C_003Em__195(Collider c)
			{
				return c.GetComponent<Collider>().bounds.Contains(_003C_003Ef__this.truck.transform.position);
			}

			internal bool _003C_003Em__196(Collider c)
			{
				return c.GetComponent<Collider>().bounds.Contains(_003C_003Ef__this.truck.transform.position);
			}
		}

		[CompilerGenerated]
		private sealed class _003Cstep2_003Ec__Iterator14E : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal int _0024PC;

			internal object _0024current;

			internal ZTtutorialView _003C_003Ef__this;

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
				//Discarded unreachable code: IL_00d9
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
				case 1u:
					if (_003C_003Ef__this.brakeColliders.Count(_003C_003Em__197) == 0)
					{
						_0024current = null;
						_0024PC = 1;
						break;
					}
					_003C_003Ef__this.brakeToolTip.Show();
					goto case 2u;
				case 2u:
					if (_003C_003Ef__this.brakeColliders.Count(_003C_003Em__198) != 0)
					{
						_0024current = null;
						_0024PC = 2;
						break;
					}
					_003C_003Ef__this.brakeToolTip.Hide();
					_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.step2());
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

			internal bool _003C_003Em__197(Collider c)
			{
				return c.GetComponent<Collider>().bounds.Contains(_003C_003Ef__this.truck.transform.position);
			}

			internal bool _003C_003Em__198(Collider c)
			{
				return c.GetComponent<Collider>().bounds.Contains(_003C_003Ef__this.truck.transform.position);
			}
		}

		[SerializeField]
		private ToolTip driveToolTip;

		[SerializeField]
		private ToolTip brakeToolTip;

		private ZTtruck truck;

		private Collider[] driveColliders;

		private Collider[] brakeColliders;

		protected override void Start()
		{
			base.Start();
			truck = UnityEngine.Object.FindObjectOfType<ZTtruck>();
			driveColliders = driveToolTip.GetComponentsInChildren<Collider>();
			brakeColliders = brakeToolTip.GetComponentsInChildren<Collider>();
			for (int i = 0; i < driveColliders.Length; i++)
			{
				driveColliders[i].enabled = true;
			}
			for (int j = 0; j < brakeColliders.Length; j++)
			{
				brakeColliders[j].enabled = true;
			}
			StartCoroutine(step1());
			StartCoroutine(step2());
		}

		private IEnumerator step1()
		{
			while (driveColliders.Count(((_003Cstep1_003Ec__Iterator14D)(object)this)._003C_003Em__195) == 0)
			{
				yield return null;
			}
			driveToolTip.Show();
			while (driveColliders.Count(((_003Cstep1_003Ec__Iterator14D)(object)this)._003C_003Em__196) != 0)
			{
				yield return null;
			}
			driveToolTip.Hide();
			StartCoroutine(step1());
		}

		private IEnumerator step2()
		{
			while (brakeColliders.Count(((_003Cstep2_003Ec__Iterator14E)(object)this)._003C_003Em__197) == 0)
			{
				yield return null;
			}
			brakeToolTip.Show();
			while (brakeColliders.Count(((_003Cstep2_003Ec__Iterator14E)(object)this)._003C_003Em__198) != 0)
			{
				yield return null;
			}
			brakeToolTip.Hide();
			StartCoroutine(step2());
		}
	}
}
