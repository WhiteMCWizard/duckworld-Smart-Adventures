using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.CameraSystem
{
	public class CameraManager : MonoBehaviour
	{
		private struct CameraState
		{
			public float Weight;

			public Vector3 Position;

			public Quaternion Rotation;
		}

		[CompilerGenerated]
		private sealed class _003CdoCrossFade_003Ec__Iterator10B : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal object[] args;

			internal CameraBehaviour _003Cbehaviour_003E__0;

			internal float _003Cduration_003E__1;

			internal AnimationCurve _003CblendCurve_003E__2;

			internal List<CameraBehaviour> _003Cgroup_003E__3;

			internal Stopwatch _003Csw_003E__4;

			internal float[] _003CorigWeights_003E__5;

			internal int _003Ci_003E__6;

			internal float _003Ctarget_003E__7;

			internal int _0024PC;

			internal object _0024current;

			internal object[] _003C_0024_003Eargs;

			internal CameraManager _003C_003Ef__this;

			public static Func<CameraBehaviour, float> _003C_003Ef__am_0024cacheD;

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
				//Discarded unreachable code: IL_019b
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
				{
					_003Cbehaviour_003E__0 = (CameraBehaviour)args[0];
					_003Cduration_003E__1 = (float)args[1];
					_003CblendCurve_003E__2 = (AnimationCurve)args[2];
					_003Cgroup_003E__3 = _003C_003Ef__this.behaviourGroups[_003Cbehaviour_003E__0.Layer];
					_003Csw_003E__4 = new Stopwatch(_003Cduration_003E__1);
					List<CameraBehaviour> collection = _003Cgroup_003E__3;
					if (_003C_003Ef__am_0024cacheD == null)
					{
						_003C_003Ef__am_0024cacheD = _003C_003Em__E8;
					}
					_003CorigWeights_003E__5 = collection.Select(_003C_003Ef__am_0024cacheD).ToArray();
					goto IL_00bf;
				}
				case 1u:
					{
						for (_003Ci_003E__6 = 0; _003Ci_003E__6 < _003Cgroup_003E__3.Count; _003Ci_003E__6++)
						{
							_003Ctarget_003E__7 = ((!(_003Cgroup_003E__3[_003Ci_003E__6] == _003Cbehaviour_003E__0)) ? 0f : 1f);
							_003Cgroup_003E__3[_003Ci_003E__6].Weight = Mathf.Lerp(_003CorigWeights_003E__5[_003Ci_003E__6], _003Ctarget_003E__7, _003CblendCurve_003E__2.Evaluate(_003Csw_003E__4.Progress));
						}
						if (_003Csw_003E__4.Expired)
						{
							_0024PC = -1;
							break;
						}
						goto IL_00bf;
					}
					IL_00bf:
					_0024current = new WaitForEndOfFrame();
					_0024PC = 1;
					return true;
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

			public static float _003C_003Em__E8(CameraBehaviour cb)
			{
				return cb.Weight;
			}
		}

		private Dictionary<int, List<CameraBehaviour>> behaviourGroups = new Dictionary<int, List<CameraBehaviour>>();

		[CompilerGenerated]
		private static Func<CameraBehaviour, float> _003C_003Ef__am_0024cache1;

		private void LateUpdate()
		{
			foreach (KeyValuePair<int, List<CameraBehaviour>> behaviourGroup in behaviourGroups)
			{
				doBehaviourGroup(behaviourGroup.Value);
			}
		}

		private void doBehaviourGroup(List<CameraBehaviour> behaviourGroup)
		{
			float num = 0f;
			List<CameraState> list = new List<CameraState>();
			foreach (CameraBehaviour item in behaviourGroup)
			{
				if (!(item.Weight <= 0f))
				{
					Vector3 position;
					Quaternion rotation;
					item.GetPositionAndRotation(out position, out rotation);
					list.Add(new CameraState
					{
						Weight = item.Weight,
						Position = position,
						Rotation = rotation
					});
					num += item.Weight;
				}
			}
			if (num <= 0f)
			{
				return;
			}
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			Vector3 zero3 = Vector3.zero;
			foreach (CameraState item2 in list)
			{
				float num2 = item2.Weight / num;
				zero += item2.Position * num2;
				zero2 += item2.Rotation * Vector3.forward * num2;
				zero3 += item2.Rotation * Vector3.up * num2;
			}
			base.transform.position = zero;
			base.transform.rotation = Quaternion.LookRotation(zero2, zero3);
		}

		public Coroutine CrossFade(CameraBehaviour behaviour, float duration)
		{
			return CrossFade(behaviour, duration, AnimationCurve.Linear(0f, 0f, 1f, 1f));
		}

		public Coroutine CrossFade(CameraBehaviour behaviour, float duration, AnimationCurve blendCurve)
		{
			if (duration <= 0f)
			{
				List<CameraBehaviour> list = behaviourGroups[behaviour.Layer];
				for (int i = 0; i < list.Count; i++)
				{
					float weight = ((!(list[i] == behaviour)) ? 0f : 1f);
					list[i].Weight = weight;
				}
			}
			StopCoroutine("doCrossFade");
			return StartCoroutine("doCrossFade", new object[3] { behaviour, duration, blendCurve });
		}

		private IEnumerator doCrossFade(object[] args)
		{
			CameraBehaviour behaviour = (CameraBehaviour)args[0];
			float duration = (float)args[1];
			AnimationCurve blendCurve = (AnimationCurve)args[2];
			List<CameraBehaviour> group = behaviourGroups[behaviour.Layer];
			Stopwatch sw = new Stopwatch(duration);
			if (_003CdoCrossFade_003Ec__Iterator10B._003C_003Ef__am_0024cacheD == null)
			{
				_003CdoCrossFade_003Ec__Iterator10B._003C_003Ef__am_0024cacheD = _003CdoCrossFade_003Ec__Iterator10B._003C_003Em__E8;
			}
			float[] origWeights = group.Select(_003CdoCrossFade_003Ec__Iterator10B._003C_003Ef__am_0024cacheD).ToArray();
			do
			{
				yield return new WaitForEndOfFrame();
				for (int i = 0; i < group.Count; i++)
				{
					float target = ((!(group[i] == behaviour)) ? 0f : 1f);
					group[i].Weight = Mathf.Lerp(origWeights[i], target, blendCurve.Evaluate(sw.Progress));
				}
			}
			while (!sw.Expired);
		}

		public void AddBehaviour(CameraBehaviour behaviour)
		{
			if (!behaviourGroups.ContainsKey(behaviour.Layer))
			{
				behaviourGroups.Add(behaviour.Layer, new List<CameraBehaviour> { behaviour });
			}
			else
			{
				behaviourGroups[behaviour.Layer].Add(behaviour);
			}
		}

		public void RemoveBehaviour(CameraBehaviour behaviour)
		{
			behaviourGroups[behaviour.Layer].Remove(behaviour);
		}

		public CameraBehaviour CurrentBehaviour(int layer)
		{
			List<CameraBehaviour> source = behaviourGroups[layer];
			if (_003C_003Ef__am_0024cache1 == null)
			{
				_003C_003Ef__am_0024cache1 = _003CCurrentBehaviour_003Em__E7;
			}
			return source.OrderByDescending(_003C_003Ef__am_0024cache1).First();
		}

		[CompilerGenerated]
		private static float _003CCurrentBehaviour_003Em__E7(CameraBehaviour c)
		{
			return c.Weight;
		}
	}
}
