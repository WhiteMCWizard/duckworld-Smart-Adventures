using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM;
using SLAM.Slinq;
using UnityEngine;

public class FYTreeBlossom : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CfadeBlossom_003Ec__Iterator9A : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal Material[] _003Cmats_003E__0;

		internal SLAM.Stopwatch _003Csw_003E__1;

		internal float startBlend;

		internal float endBlend;

		internal float _003Cblend_003E__2;

		internal int _003Ci_003E__3;

		internal int _0024PC;

		internal object _0024current;

		internal float _003C_0024_003EstartBlend;

		internal float _003C_0024_003EendBlend;

		internal FYTreeBlossom _003C_003Ef__this;

		public static Func<Renderer, IEnumerable<Material>> _003C_003Ef__am_0024cacheB;

		public static Func<Material, bool> _003C_003Ef__am_0024cacheC;

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
			//Discarded unreachable code: IL_0140
			uint num = (uint)_0024PC;
			_0024PC = -1;
			switch (num)
			{
			case 0u:
			{
				Renderer[] componentsInChildren = _003C_003Ef__this.GetComponentsInChildren<Renderer>();
				if (_003C_003Ef__am_0024cacheB == null)
				{
					_003C_003Ef__am_0024cacheB = _003C_003Em__73;
				}
				IEnumerable<Material> collection = componentsInChildren.SelectMany(_003C_003Ef__am_0024cacheB);
				if (_003C_003Ef__am_0024cacheC == null)
				{
					_003C_003Ef__am_0024cacheC = _003C_003Em__74;
				}
				_003Cmats_003E__0 = collection.Where(_003C_003Ef__am_0024cacheC).ToArray();
				_003Csw_003E__1 = new SLAM.Stopwatch(_003C_003Ef__this.blossomDuration);
				goto IL_0125;
			}
			case 1u:
				{
					_003Cblend_003E__2 = Mathf.Lerp(startBlend, endBlend, _003C_003Ef__this.blossomCurve.Evaluate(_003Csw_003E__1.Progress));
					for (_003Ci_003E__3 = 0; _003Ci_003E__3 < _003Cmats_003E__0.Length; _003Ci_003E__3++)
					{
						_003Cmats_003E__0[_003Ci_003E__3].SetFloat("_Blend", _003Cblend_003E__2);
					}
					goto IL_0125;
				}
				IL_0125:
				if (!_003Csw_003E__1.Expired)
				{
					_0024current = null;
					_0024PC = 1;
					return true;
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

		public static IEnumerable<Material> _003C_003Em__73(Renderer r)
		{
			return r.materials;
		}

		public static bool _003C_003Em__74(Material m)
		{
			return m.HasProperty("_Blend");
		}
	}

	[Tooltip("How many seconds it takes to show the blossom")]
	[SerializeField]
	private float blossomDuration;

	[SerializeField]
	private AnimationCurve blossomCurve;

	private bool isToggled;

	[ContextMenu("Toggle blossom")]
	private void ToggleBlossom()
	{
		if (isToggled)
		{
			StartCoroutine(fadeBlossom(1f, 0f));
		}
		else
		{
			StartCoroutine(fadeBlossom(0f, 1f));
		}
		isToggled = !isToggled;
	}

	private IEnumerator fadeBlossom(float startBlend, float endBlend)
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		if (_003CfadeBlossom_003Ec__Iterator9A._003C_003Ef__am_0024cacheB == null)
		{
			_003CfadeBlossom_003Ec__Iterator9A._003C_003Ef__am_0024cacheB = _003CfadeBlossom_003Ec__Iterator9A._003C_003Em__73;
		}
		IEnumerable<Material> collection = componentsInChildren.SelectMany(_003CfadeBlossom_003Ec__Iterator9A._003C_003Ef__am_0024cacheB);
		if (_003CfadeBlossom_003Ec__Iterator9A._003C_003Ef__am_0024cacheC == null)
		{
			_003CfadeBlossom_003Ec__Iterator9A._003C_003Ef__am_0024cacheC = _003CfadeBlossom_003Ec__Iterator9A._003C_003Em__74;
		}
		Material[] mats = collection.Where(_003CfadeBlossom_003Ec__Iterator9A._003C_003Ef__am_0024cacheC).ToArray();
		SLAM.Stopwatch sw = new SLAM.Stopwatch(blossomDuration);
		while (!sw.Expired)
		{
			yield return null;
			float blend = Mathf.Lerp(startBlend, endBlend, blossomCurve.Evaluate(sw.Progress));
			for (int i = 0; i < mats.Length; i++)
			{
				mats[i].SetFloat("_Blend", blend);
			}
		}
	}
}
