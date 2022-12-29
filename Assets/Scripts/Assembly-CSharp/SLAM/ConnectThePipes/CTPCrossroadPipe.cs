using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.ConnectThePipes
{
	public class CTPCrossroadPipe : CTPPipe
	{
		[CompilerGenerated]
		private sealed class _003CdoFillWaterEffect_003Ec__Iterator5F : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal Vector3 _003CworldIn_003E__0;

			internal Vector3 _003CworldOut_003E__1;

			internal Vector3 otherInDir;

			internal Material _003Cmat_003E__2;

			internal int waterFlowDelay;

			internal float _003Cdot_003E__3;

			internal float _003Cscale_003E__4;

			internal Stopwatch _003Csw_003E__5;

			internal float _003Cprog_003E__6;

			internal Vector3 _003CnewPos_003E__7;

			internal GameObject particleEffect;

			internal int _0024PC;

			internal object _0024current;

			internal Vector3 _003C_0024_003EotherInDir;

			internal int _003C_0024_003EwaterFlowDelay;

			internal GameObject _003C_0024_003EparticleEffect;

			internal CTPCrossroadPipe _003C_003Ef__this;

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
				//Discarded unreachable code: IL_02f8
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_003CworldIn_003E__0 = _003C_003Ef__this.transform.TransformDirection(_003C_003Ef__this.inDirection2);
					_003CworldOut_003E__1 = _003C_003Ef__this.transform.TransformDirection(_003C_003Ef__this.outDirection2);
					if (Mathf.Approximately(Vector3.Dot(-_003CworldOut_003E__1, otherInDir), 1f) || Mathf.Approximately(Vector3.Dot(_003CworldIn_003E__0, otherInDir), 1f))
					{
						Material[] materials = _003C_003Ef__this.openObject2.GetComponent<Renderer>().materials;
						if (_003C_003Ef__am_0024cache11 == null)
						{
							_003C_003Ef__am_0024cache11 = _003C_003Em__41;
						}
						_003Cmat_003E__2 = materials.FirstOrDefault(_003C_003Ef__am_0024cache11);
						_003Cmat_003E__2.SetFloat("_Progress", 0f);
						_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.doOpenTube2());
						_0024current = new WaitForSeconds((float)waterFlowDelay * 0.5f);
						_0024PC = 1;
					}
					else
					{
						_0024current = _003C_003Ef__this.StartCoroutine(((CTPPipe)_003C_003Ef__this).doFillWaterEffect(otherInDir, particleEffect, waterFlowDelay));
						_0024PC = 3;
					}
					break;
				case 1u:
					_003C_003Ef__this.closedObject2.SetActive(false);
					_003C_003Ef__this.openObject2.SetActive(true);
					_003Cdot_003E__3 = Vector3.Dot(otherInDir, _003CworldIn_003E__0);
					_003Cscale_003E__4 = 1f;
					if (_003Cdot_003E__3 > 0.1f)
					{
						_003Cscale_003E__4 = -1f;
					}
					_003Cmat_003E__2.SetFloat("_Scale", _003Cscale_003E__4);
					_003Cmat_003E__2.SetFloat("_Progress", 0f);
					_003Csw_003E__5 = new Stopwatch(0.5f);
					goto IL_029e;
				case 2u:
					_003Cmat_003E__2.SetFloat("_Progress", _003Csw_003E__5.Progress);
					_003Cprog_003E__6 = ((!(_003Cdot_003E__3 > 0.01f)) ? (1f - _003Csw_003E__5.Progress) : _003Csw_003E__5.Progress);
					_003CnewPos_003E__7 = _003C_003Ef__this.getPosOnTube2(_003Cprog_003E__6);
					particleEffect.transform.rotation = Quaternion.LookRotation(particleEffect.transform.position - _003CnewPos_003E__7);
					particleEffect.transform.position = _003CnewPos_003E__7;
					goto IL_029e;
				case 3u:
					_0024PC = -1;
					goto default;
				default:
					{
						return false;
					}
					IL_029e:
					if (!_003Csw_003E__5.Expired)
					{
						_0024current = null;
						_0024PC = 2;
						break;
					}
					goto case 3u;
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

			public static bool _003C_003Em__41(Material m)
			{
				return m.HasProperty("_Progress") && m.HasProperty("_Scale");
			}
		}

		[SerializeField]
		protected Vector3 inDirection2;

		[SerializeField]
		protected Vector3 outDirection2;

		[SerializeField]
		private GameObject openObject2;

		[SerializeField]
		private GameObject closedObject2;

		protected bool hasWaterInTube2;

		public override bool HasWaterInTube()
		{
			return hasWaterInTube2 || base.HasWaterInTube();
		}

		public override void ResetWater()
		{
			base.ResetWater();
			if (hasWaterInTube2)
			{
				StartCoroutine(doCloseTube2());
			}
			hasWaterInTube2 = false;
		}

		protected override void OnDrawGizmosSelected()
		{
			base.OnDrawGizmosSelected();
			Vector3 vector = base.transform.TransformDirection(inDirection2);
			Vector3 vector2 = base.transform.TransformDirection(outDirection2);
			Gizmos.color = Color.green;
			GizmosUtils.DrawArrow(base.transform.position - vector * 0.5f, vector * 0.5f);
			Gizmos.color = Color.magenta;
			GizmosUtils.DrawArrow(base.transform.position + vector2 * 0.5f, vector2 * 0.5f);
		}

		public override bool CanFlowWater(Vector3 otherInDir, out Vector3 otherOutDir)
		{
			Vector3 vector = base.transform.TransformDirection(inDirection2);
			Vector3 vector2 = base.transform.TransformDirection(outDirection2);
			if (Mathf.Approximately(Vector3.Dot(vector, otherInDir), 1f))
			{
				if (hasWaterInTube2)
				{
					otherOutDir = Vector3.one;
					return false;
				}
				hasWaterInTube2 = true;
				otherOutDir = vector2;
				return true;
			}
			if (Mathf.Approximately(Vector3.Dot(-vector2, otherInDir), 1f))
			{
				if (hasWaterInTube2)
				{
					otherOutDir = Vector3.one;
					return false;
				}
				hasWaterInTube2 = true;
				otherOutDir = -vector;
				return true;
			}
			return base.CanFlowWater(otherInDir, out otherOutDir);
		}

		private new IEnumerator doFillWaterEffect(Vector3 otherInDir, GameObject particleEffect, int waterFlowDelay)
		{
			Vector3 worldIn = base.transform.TransformDirection(inDirection2);
			Vector3 worldOut = base.transform.TransformDirection(outDirection2);
			if (Mathf.Approximately(Vector3.Dot(-worldOut, otherInDir), 1f) || Mathf.Approximately(Vector3.Dot(worldIn, otherInDir), 1f))
			{
				Material[] materials = openObject2.GetComponent<Renderer>().materials;
				if (_003CdoFillWaterEffect_003Ec__Iterator5F._003C_003Ef__am_0024cache11 == null)
				{
					_003CdoFillWaterEffect_003Ec__Iterator5F._003C_003Ef__am_0024cache11 = _003CdoFillWaterEffect_003Ec__Iterator5F._003C_003Em__41;
				}
				Material mat = materials.FirstOrDefault(_003CdoFillWaterEffect_003Ec__Iterator5F._003C_003Ef__am_0024cache11);
				mat.SetFloat("_Progress", 0f);
				StartCoroutine(doOpenTube2());
				yield return new WaitForSeconds((float)waterFlowDelay * 0.5f);
				closedObject2.SetActive(false);
				openObject2.SetActive(true);
				float dot = Vector3.Dot(otherInDir, worldIn);
				float scale = 1f;
				if (dot > 0.1f)
				{
					scale = -1f;
				}
				mat.SetFloat("_Scale", scale);
				mat.SetFloat("_Progress", 0f);
				Stopwatch sw = new Stopwatch(0.5f);
				while (!sw.Expired)
				{
					yield return null;
					mat.SetFloat("_Progress", sw.Progress);
					float prog = ((!(dot > 0.01f)) ? (1f - sw.Progress) : sw.Progress);
					Vector3 newPos = getPosOnTube2(prog);
					particleEffect.transform.rotation = Quaternion.LookRotation(particleEffect.transform.position - newPos);
					particleEffect.transform.position = newPos;
				}
			}
			else
			{
				yield return StartCoroutine(base.doFillWaterEffect(otherInDir, particleEffect, waterFlowDelay));
			}
		}

		public override void StartDoFillWaterEffect(Vector3 inDir, GameObject waterFlowParticles, int p)
		{
			StartCoroutine(doFillWaterEffect(inDir, waterFlowParticles, p));
		}

		protected override Vector3 getPosOnTube(float prog)
		{
			if (openObject == null)
			{
				return Vector3.zero;
			}
			Renderer component = openObject.GetComponent<Renderer>();
			Vector3 vector = base.transform.TransformDirection(inDirection);
			Vector3 vector2 = base.transform.TransformDirection(outDirection);
			Vector3 vector3 = component.bounds.center / 2f + GetComponent<Collider>().bounds.center / 2f;
			vector3.y -= component.bounds.extents.y;
			if (prog < 0.5f)
			{
				return Vector3.Lerp(base.transform.position - vector / 2f, vector3, prog / 0.5f);
			}
			if (prog > 0.5f)
			{
				return Vector3.Lerp(vector3, base.transform.position + vector2 / 2f, (prog - 0.5f) / 0.5f);
			}
			return vector3;
		}

		protected Vector3 getPosOnTube2(float prog)
		{
			if (openObject == null)
			{
				return Vector3.zero;
			}
			Renderer component = openObject2.GetComponent<Renderer>();
			Vector3 vector = base.transform.TransformDirection(inDirection2);
			Vector3 vector2 = base.transform.TransformDirection(outDirection2);
			Vector3 vector3 = component.bounds.center / 2f + GetComponent<Collider>().bounds.center / 2f;
			vector3.y += component.bounds.extents.y;
			if (prog < 0.5f)
			{
				return Vector3.Lerp(base.transform.position - vector / 2f, vector3, prog / 0.5f);
			}
			if (prog > 0.5f)
			{
				return Vector3.Lerp(vector3, base.transform.position + vector2 / 2f, (prog - 0.5f) / 0.5f);
			}
			return vector3;
		}

		protected IEnumerator doOpenTube2()
		{
			Material mat = closedObject2.GetComponent<Renderer>().material;
			openObject2.SetActive(true);
			Stopwatch sw = new Stopwatch(1f);
			while ((bool)sw)
			{
				yield return null;
				mat.SetFloat("_AlphaMod", 1f - sw.Progress);
			}
		}

		protected IEnumerator doCloseTube2()
		{
			Material mat = closedObject2.GetComponent<Renderer>().material;
			closedObject2.SetActive(true);
			Stopwatch sw = new Stopwatch(1f);
			while ((bool)sw)
			{
				yield return null;
				mat.SetFloat("_AlphaMod", sw.Progress);
			}
			openObject2.SetActive(false);
		}
	}
}
