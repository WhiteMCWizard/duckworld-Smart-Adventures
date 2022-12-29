using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.ConnectThePipes
{
	public class CTPJunctionPipe : CTPPipe
	{
		[CompilerGenerated]
		private sealed class _003CfillWaterEffect_003Ec__Iterator62 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal GameObject startPipe;

			internal GameObject outPipe1;

			internal GameObject outPipe2;

			internal Material _003CstartMat_003E__0;

			internal Material _003CoutMat1_003E__1;

			internal Material _003CoutMat2_003E__2;

			internal int waterFlowDelay;

			internal Stopwatch _003Csw_003E__3;

			internal int _0024PC;

			internal object _0024current;

			internal GameObject _003C_0024_003EstartPipe;

			internal GameObject _003C_0024_003EoutPipe1;

			internal GameObject _003C_0024_003EoutPipe2;

			internal int _003C_0024_003EwaterFlowDelay;

			internal CTPJunctionPipe _003C_003Ef__this;

			public static Func<Material, bool> _003C_003Ef__am_0024cacheF;

			public static Func<Material, bool> _003C_003Ef__am_0024cache10;

			public static Func<Material, bool> _003C_003Ef__am_0024cache11;

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
				//Discarded unreachable code: IL_0289
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
				{
					startPipe.gameObject.SetActive(true);
					outPipe1.gameObject.SetActive(true);
					outPipe2.gameObject.SetActive(true);
					Material[] materials = startPipe.GetComponent<Renderer>().materials;
					if (_003C_003Ef__am_0024cacheF == null)
					{
						_003C_003Ef__am_0024cacheF = _003C_003Em__42;
					}
					_003CstartMat_003E__0 = materials.FirstOrDefault(_003C_003Ef__am_0024cacheF);
					Material[] materials2 = outPipe1.GetComponent<Renderer>().materials;
					if (_003C_003Ef__am_0024cache10 == null)
					{
						_003C_003Ef__am_0024cache10 = _003C_003Em__43;
					}
					_003CoutMat1_003E__1 = materials2.FirstOrDefault(_003C_003Ef__am_0024cache10);
					Material[] materials3 = outPipe2.GetComponent<Renderer>().materials;
					if (_003C_003Ef__am_0024cache11 == null)
					{
						_003C_003Ef__am_0024cache11 = _003C_003Em__44;
					}
					_003CoutMat2_003E__2 = materials3.FirstOrDefault(_003C_003Ef__am_0024cache11);
					_003CstartMat_003E__0.SetFloat("_Progress", -10f);
					_003CoutMat1_003E__1.SetFloat("_Progress", -10f);
					_003CoutMat2_003E__2.SetFloat("_Progress", -10f);
					_003CstartMat_003E__0.SetFloat("_Scale", -1f);
					_003CoutMat1_003E__1.SetFloat("_Scale", 1f);
					_003CoutMat2_003E__2.SetFloat("_Scale", 1f);
					_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.doOpenTube());
					_0024current = new WaitForSeconds((float)waterFlowDelay * 0.5f);
					_0024PC = 1;
					break;
				}
				case 1u:
					_003Csw_003E__3 = new Stopwatch(0.25f);
					goto IL_0200;
				case 2u:
					_003CstartMat_003E__0.SetFloat("_Progress", _003Csw_003E__3.Progress);
					goto IL_0200;
				case 3u:
					_003CoutMat1_003E__1.SetFloat("_Progress", _003Csw_003E__3.Progress);
					_003CoutMat2_003E__2.SetFloat("_Progress", _003Csw_003E__3.Progress);
					goto IL_026e;
				default:
					{
						return false;
					}
					IL_0200:
					if (!_003Csw_003E__3.Expired)
					{
						_0024current = null;
						_0024PC = 2;
						break;
					}
					_003Csw_003E__3 = new Stopwatch(0.25f);
					goto IL_026e;
					IL_026e:
					if (!_003Csw_003E__3.Expired)
					{
						_0024current = null;
						_0024PC = 3;
						break;
					}
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

			public static bool _003C_003Em__42(Material m)
			{
				return m.HasProperty("_Progress") && m.HasProperty("_Scale");
			}

			public static bool _003C_003Em__43(Material m)
			{
				return m.HasProperty("_Progress") && m.HasProperty("_Scale");
			}

			public static bool _003C_003Em__44(Material m)
			{
				return m.HasProperty("_Progress") && m.HasProperty("_Scale");
			}
		}

		[SerializeField]
		private Vector3 outDirection2;

		[SerializeField]
		private GameObject leftWaterObject;

		[SerializeField]
		private GameObject rightWaterObject;

		[SerializeField]
		private GameObject topWaterObject;

		protected Vector3 waterDirection2;

		protected Vector3 extraPos;

		protected Vector3 extraDir;

		protected override void OnDrawGizmosSelected()
		{
			Vector3 vector = base.transform.TransformDirection(outDirection2);
			Gizmos.color = Color.green;
			GizmosUtils.DrawArrow(base.transform.position + vector * 0.5f, vector * 0.5f);
		}

		public override bool CanFlowWater(Vector3 waterInDir, out Vector3 waterOutDir)
		{
			Vector3 vector = base.transform.TransformDirection(inDirection);
			Vector3 vector2 = base.transform.TransformDirection(outDirection2);
			if (waterInTube)
			{
				waterOutDir = Vector3.one;
				return false;
			}
			if (Mathf.Approximately(Vector3.Dot(-vector2, waterInDir), 1f))
			{
				waterOutDir = -vector;
				Vector3 vector3 = base.transform.TransformDirection(outDirection);
				extraPos = base.transform.localPosition + vector3;
				extraDir = vector3;
				waterInTube = true;
				return true;
			}
			if (base.CanFlowWater(waterInDir, out waterOutDir))
			{
				extraPos = base.transform.localPosition + vector2;
				extraDir = vector2;
				return true;
			}
			waterOutDir = Vector3.zero;
			return false;
		}

		public override void StartDoFillWaterEffect(Vector3 inDir, GameObject waterFlowParticles, int startDelay)
		{
			UnityEngine.Object.FindObjectOfType<CTPWaterFlowManager>().StartWaterFlowFromPosition(extraPos, extraDir, startDelay + 1);
			GameObject startPipe;
			GameObject outPipe;
			GameObject outPipe2;
			if (MathUtilities.AreDirectionsSimilar(inDir, base.transform.right))
			{
				startPipe = rightWaterObject;
				outPipe = leftWaterObject;
				outPipe2 = topWaterObject;
			}
			else if (MathUtilities.AreDirectionsSimilar(inDir, -base.transform.right))
			{
				startPipe = leftWaterObject;
				outPipe = rightWaterObject;
				outPipe2 = topWaterObject;
			}
			else
			{
				if (!MathUtilities.AreDirectionsSimilar(inDir, -base.transform.forward))
				{
					throw new Exception("T junction doesnt know how to handle this direction? " + inDir);
				}
				startPipe = topWaterObject;
				outPipe = leftWaterObject;
				outPipe2 = rightWaterObject;
			}
			StartCoroutine(fillWaterEffect(startPipe, outPipe, outPipe2, startDelay));
		}

		private IEnumerator fillWaterEffect(GameObject startPipe, GameObject outPipe1, GameObject outPipe2, int waterFlowDelay)
		{
			startPipe.gameObject.SetActive(true);
			outPipe1.gameObject.SetActive(true);
			outPipe2.gameObject.SetActive(true);
			Material[] materials = startPipe.GetComponent<Renderer>().materials;
			if (_003CfillWaterEffect_003Ec__Iterator62._003C_003Ef__am_0024cacheF == null)
			{
				_003CfillWaterEffect_003Ec__Iterator62._003C_003Ef__am_0024cacheF = _003CfillWaterEffect_003Ec__Iterator62._003C_003Em__42;
			}
			Material startMat = materials.FirstOrDefault(_003CfillWaterEffect_003Ec__Iterator62._003C_003Ef__am_0024cacheF);
			Material[] materials2 = outPipe1.GetComponent<Renderer>().materials;
			if (_003CfillWaterEffect_003Ec__Iterator62._003C_003Ef__am_0024cache10 == null)
			{
				_003CfillWaterEffect_003Ec__Iterator62._003C_003Ef__am_0024cache10 = _003CfillWaterEffect_003Ec__Iterator62._003C_003Em__43;
			}
			Material outMat1 = materials2.FirstOrDefault(_003CfillWaterEffect_003Ec__Iterator62._003C_003Ef__am_0024cache10);
			Material[] materials3 = outPipe2.GetComponent<Renderer>().materials;
			if (_003CfillWaterEffect_003Ec__Iterator62._003C_003Ef__am_0024cache11 == null)
			{
				_003CfillWaterEffect_003Ec__Iterator62._003C_003Ef__am_0024cache11 = _003CfillWaterEffect_003Ec__Iterator62._003C_003Em__44;
			}
			Material outMat2 = materials3.FirstOrDefault(_003CfillWaterEffect_003Ec__Iterator62._003C_003Ef__am_0024cache11);
			startMat.SetFloat("_Progress", -10f);
			outMat1.SetFloat("_Progress", -10f);
			outMat2.SetFloat("_Progress", -10f);
			startMat.SetFloat("_Scale", -1f);
			outMat1.SetFloat("_Scale", 1f);
			outMat2.SetFloat("_Scale", 1f);
			StartCoroutine(doOpenTube());
			yield return new WaitForSeconds((float)waterFlowDelay * 0.5f);
			Stopwatch sw2 = new Stopwatch(0.25f);
			while (!sw2.Expired)
			{
				yield return null;
				startMat.SetFloat("_Progress", sw2.Progress);
			}
			sw2 = new Stopwatch(0.25f);
			while (!sw2.Expired)
			{
				yield return null;
				outMat1.SetFloat("_Progress", sw2.Progress);
				outMat2.SetFloat("_Progress", sw2.Progress);
			}
		}
	}
}
