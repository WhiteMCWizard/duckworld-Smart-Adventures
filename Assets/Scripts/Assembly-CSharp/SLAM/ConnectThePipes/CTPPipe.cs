using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.ConnectThePipes
{
	[SelectionBase]
	public class CTPPipe : MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class _003CdoFillWaterEffect_003Ec__Iterator5A : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal Material _003Cmat_003E__0;

			internal int waterFlowDelay;

			internal Vector3 _003CworldInDir_003E__1;

			internal Vector3 waterInDirection;

			internal float _003Cdot_003E__2;

			internal float _003Cscale_003E__3;

			internal Stopwatch _003Csw_003E__4;

			internal float _003Cprog_003E__5;

			internal Vector3 _003CnewPos_003E__6;

			internal GameObject particleEffect;

			internal int _0024PC;

			internal object _0024current;

			internal int _003C_0024_003EwaterFlowDelay;

			internal Vector3 _003C_0024_003EwaterInDirection;

			internal GameObject _003C_0024_003EparticleEffect;

			internal CTPPipe _003C_003Ef__this;

			public static Func<Material, bool> _003C_003Ef__am_0024cache10;

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
				//Discarded unreachable code: IL_0241
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
				{
					Material[] materials = _003C_003Ef__this.openObject.GetComponent<Renderer>().materials;
					if (_003C_003Ef__am_0024cache10 == null)
					{
						_003C_003Ef__am_0024cache10 = _003C_003Em__40;
					}
					_003Cmat_003E__0 = materials.FirstOrDefault(_003C_003Ef__am_0024cache10);
					_003Cmat_003E__0.SetFloat("_Progress", -10f);
					_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.doOpenTube());
					_0024current = new WaitForSeconds((float)waterFlowDelay * 0.5f);
					_0024PC = 1;
					break;
				}
				case 1u:
					if (_003C_003Ef__this.closedObject == null || _003C_003Ef__this.openObject == null)
					{
						goto default;
					}
					_003CworldInDir_003E__1 = _003C_003Ef__this.transform.TransformDirection(_003C_003Ef__this.inDirection);
					_003Cdot_003E__2 = Vector3.Dot(waterInDirection, _003CworldInDir_003E__1);
					_003Cscale_003E__3 = ((!(_003Cdot_003E__2 > 0.1f)) ? 1 : (-1));
					_003Cmat_003E__0.SetFloat("_Scale", _003Cscale_003E__3);
					_003Csw_003E__4 = new Stopwatch(0.5f);
					goto IL_0226;
				case 2u:
					_003Cmat_003E__0.SetFloat("_Progress", _003Csw_003E__4.Progress);
					_003Cprog_003E__5 = ((!(_003Cdot_003E__2 > 0.01f)) ? (1f - _003Csw_003E__4.Progress) : _003Csw_003E__4.Progress);
					_003CnewPos_003E__6 = _003C_003Ef__this.getPosOnTube(_003Cprog_003E__5);
					particleEffect.transform.rotation = Quaternion.LookRotation(_003CnewPos_003E__6 - particleEffect.transform.position);
					particleEffect.transform.position = _003CnewPos_003E__6;
					goto IL_0226;
				default:
					{
						return false;
					}
					IL_0226:
					if (!_003Csw_003E__4.Expired)
					{
						_0024current = null;
						_0024PC = 2;
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

			public static bool _003C_003Em__40(Material m)
			{
				return m.HasProperty("_Progress") && m.HasProperty("_Scale");
			}
		}

		public const float WATER_FLOW_SPEED = 0.5f;

		[SerializeField]
		protected Vector3 inDirection;

		[SerializeField]
		protected Vector3 outDirection;

		[SerializeField]
		protected bool canRotate = true;

		[SerializeField]
		private float rotationSpeed;

		[SerializeField]
		private AnimationCurve rotationCurve;

		[SerializeField]
		protected GameObject openObject;

		[SerializeField]
		protected GameObject closedObject;

		protected bool waterInTube;

		protected virtual void OnDrawGizmosSelected()
		{
			Vector3 vector = base.transform.TransformDirection(inDirection);
			Vector3 vector2 = base.transform.TransformDirection(outDirection);
			Gizmos.color = Color.red;
			GizmosUtils.DrawArrow(base.transform.position - vector * 0.5f, vector * 0.5f);
			Gizmos.color = Color.blue;
			GizmosUtils.DrawArrow(base.transform.position + vector2 * 0.5f, vector2 * 0.5f);
		}

		public virtual bool CanFlowWater(Vector3 waterInDir, out Vector3 waterOutDir)
		{
			Vector3 vector = base.transform.TransformDirection(inDirection);
			Vector3 vector2 = base.transform.TransformDirection(outDirection);
			waterOutDir = Vector3.zero;
			if (waterInTube)
			{
				if (Mathf.Approximately(Vector3.Dot(vector, waterInDir), 1f) || Mathf.Approximately(Vector3.Dot(-vector2, waterInDir), 1f))
				{
					waterOutDir = Vector3.one;
				}
				return false;
			}
			if (Mathf.Approximately(Vector3.Dot(vector, waterInDir), 1f))
			{
				waterOutDir = vector2;
				waterInTube = true;
				return true;
			}
			if (Mathf.Approximately(Vector3.Dot(-vector2, waterInDir), 1f))
			{
				waterOutDir = -vector;
				waterInTube = true;
				return true;
			}
			return false;
		}

		public IEnumerator doFillWaterEffect(Vector3 waterInDirection, GameObject particleEffect, int waterFlowDelay)
		{
			Material[] materials = openObject.GetComponent<Renderer>().materials;
			if (_003CdoFillWaterEffect_003Ec__Iterator5A._003C_003Ef__am_0024cache10 == null)
			{
				_003CdoFillWaterEffect_003Ec__Iterator5A._003C_003Ef__am_0024cache10 = _003CdoFillWaterEffect_003Ec__Iterator5A._003C_003Em__40;
			}
			Material mat = materials.FirstOrDefault(_003CdoFillWaterEffect_003Ec__Iterator5A._003C_003Ef__am_0024cache10);
			mat.SetFloat("_Progress", -10f);
			StartCoroutine(doOpenTube());
			yield return new WaitForSeconds((float)waterFlowDelay * 0.5f);
			if (!(closedObject == null) && !(openObject == null))
			{
				Vector3 worldInDir = base.transform.TransformDirection(inDirection);
				float dot = Vector3.Dot(waterInDirection, worldInDir);
				float scale = ((!(dot > 0.1f)) ? 1 : (-1));
				mat.SetFloat("_Scale", scale);
				Stopwatch sw = new Stopwatch(0.5f);
				while (!sw.Expired)
				{
					yield return null;
					mat.SetFloat("_Progress", sw.Progress);
					float prog = ((!(dot > 0.01f)) ? (1f - sw.Progress) : sw.Progress);
					Vector3 newPos = getPosOnTube(prog);
					particleEffect.transform.rotation = Quaternion.LookRotation(newPos - particleEffect.transform.position);
					particleEffect.transform.position = newPos;
				}
			}
		}

		protected IEnumerator doOpenTube()
		{
			Material mat = closedObject.GetComponent<Renderer>().material;
			openObject.SetActive(true);
			Stopwatch sw = new Stopwatch(1f);
			while ((bool)sw)
			{
				yield return null;
				mat.SetFloat("_AlphaMod", 1f - sw.Progress);
			}
		}

		protected IEnumerator doCloseTube()
		{
			Material mat = closedObject.GetComponent<Renderer>().material;
			Stopwatch sw = new Stopwatch(1f);
			while ((bool)sw)
			{
				yield return null;
				mat.SetFloat("_AlphaMod", sw.Progress);
			}
			openObject.SetActive(false);
		}

		public virtual void ResetWater()
		{
			waterInTube = false;
			StartCoroutine(doCloseTube());
		}

		public virtual bool HasWaterInTube()
		{
			return waterInTube;
		}

		protected virtual Vector3 getPosOnTube(float prog)
		{
			if (openObject == null)
			{
				return Vector3.zero;
			}
			Renderer component = openObject.GetComponent<Renderer>();
			Vector3 vector = base.transform.TransformDirection(inDirection);
			Vector3 vector2 = base.transform.TransformDirection(outDirection);
			Vector3 vector3 = component.bounds.center / 2f + GetComponent<Collider>().bounds.center / 2f;
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

		public virtual void OnClick(int direction = 1)
		{
			if (canRotate)
			{
				ConnectThePipesGame.PipeClickedEvent pipeClickedEvent = new ConnectThePipesGame.PipeClickedEvent();
				pipeClickedEvent.pipe = this;
				GameEvents.Invoke(pipeClickedEvent);
				AudioController.Play("pipe_rotate_01", base.transform);
				StartCoroutine(rotatePipe(direction));
			}
		}

		protected IEnumerator rotatePipe(int direction = 1)
		{
			canRotate = false;
			Stopwatch sw = new Stopwatch(rotationSpeed);
			Quaternion startRot = base.transform.rotation;
			Quaternion endRot = base.transform.rotation * Quaternion.AngleAxis(90f * (float)direction, Vector3.up);
			while (!sw.Expired)
			{
				yield return null;
				base.transform.rotation = Quaternion.Lerp(startRot, endRot, rotationCurve.Evaluate(sw.Progress));
			}
			canRotate = true;
		}

		public virtual void StartDoFillWaterEffect(Vector3 inDir, GameObject waterFlowParticles, int startDelay)
		{
			StartCoroutine(doFillWaterEffect(inDir, waterFlowParticles, startDelay));
		}
	}
}
