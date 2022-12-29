using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.AssemblyLine
{
	public class ALRobotPart : MonoBehaviour
	{
		[Flags]
		public enum RobotPartType
		{
			Head = 1,
			ArmRight = 2,
			ArmLeft = 4,
			Body = 8,
			Feet = 0x10
		}

		[CompilerGenerated]
		private sealed class _003CsetAmbientThreshold_003Ec__IteratorD : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal Material[] _003Cmats_003E__0;

			internal float _003CstartThreshold_003E__1;

			internal Stopwatch _003Csw_003E__2;

			internal Material[] _003C_0024s_219_003E__3;

			internal int _003C_0024s_220_003E__4;

			internal Material _003Cmat_003E__5;

			internal float threshold;

			internal int _0024PC;

			internal object _0024current;

			internal float _003C_0024_003Ethreshold;

			internal ALRobotPart _003C_003Ef__this;

			public static Func<Material, bool> _003C_003Ef__am_0024cacheB;

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
				//Discarded unreachable code: IL_0130
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
				{
					_003Cmats_003E__0 = _003C_003Ef__this.GetComponentInChildren<Renderer>().materials;
					Material[] collection = _003Cmats_003E__0;
					if (_003C_003Ef__am_0024cacheB == null)
					{
						_003C_003Ef__am_0024cacheB = _003C_003Em__6;
					}
					_003CstartThreshold_003E__1 = collection.FirstOrDefault(_003C_003Ef__am_0024cacheB).GetFloat("_AmbientThreshold");
					_003Csw_003E__2 = new Stopwatch(_003C_003Ef__this.fadeTime);
					goto IL_0115;
				}
				case 1u:
					{
						_003C_0024s_219_003E__3 = _003Cmats_003E__0;
						for (_003C_0024s_220_003E__4 = 0; _003C_0024s_220_003E__4 < _003C_0024s_219_003E__3.Length; _003C_0024s_220_003E__4++)
						{
							_003Cmat_003E__5 = _003C_0024s_219_003E__3[_003C_0024s_220_003E__4];
							_003Cmat_003E__5.SetFloat("_AmbientThreshold", Mathf.Lerp(_003CstartThreshold_003E__1, threshold, _003Csw_003E__2.Progress));
						}
						goto IL_0115;
					}
					IL_0115:
					if (!_003Csw_003E__2.Expired)
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

			public static bool _003C_003Em__6(Material m)
			{
				return m.HasProperty("_AmbientThreshold");
			}
		}

		[SerializeField]
		private RobotPartType type;

		[SerializeField]
		private int kind;

		[SerializeField]
		private Color beamColor;

		private bool _isDragging;

		private bool _canDrag = true;

		private float defaultAmbientThreshold = 0.4f;

		private float mouseoverAmbientThreshold = 1f;

		private float fadeTime = 0.2f;

		public Stopwatch Timer { get; set; }

		public bool IsDragging
		{
			get
			{
				return _isDragging;
			}
			set
			{
				if (!value)
				{
					StopAllCoroutines();
					StartCoroutine(setAmbientThreshold(defaultAmbientThreshold));
				}
				_isDragging = value;
			}
		}

		public bool CanDrag
		{
			get
			{
				return _canDrag;
			}
			set
			{
				StopAllCoroutines();
				StartCoroutine(setAmbientThreshold(defaultAmbientThreshold));
				_canDrag = value;
			}
		}

		public int Kind
		{
			get
			{
				return kind;
			}
		}

		public RobotPartType Type
		{
			get
			{
				return type;
			}
		}

		public Color BeamColor
		{
			get
			{
				return beamColor;
			}
		}

		private void OnMouseEnter()
		{
			setAmbientThresholdDirect(mouseoverAmbientThreshold);
		}

		private void OnMouseExit()
		{
			if (!IsDragging)
			{
				setAmbientThresholdDirect(defaultAmbientThreshold);
			}
		}

		private void setAmbientThresholdDirect(float threshold)
		{
			Material[] materials = GetComponentInChildren<Renderer>().materials;
			Material[] array = materials;
			foreach (Material material in array)
			{
				material.SetFloat("_AmbientThreshold", threshold);
			}
		}

		private IEnumerator setAmbientThreshold(float threshold)
		{
			Material[] mats = GetComponentInChildren<Renderer>().materials;
			if (_003CsetAmbientThreshold_003Ec__IteratorD._003C_003Ef__am_0024cacheB == null)
			{
				_003CsetAmbientThreshold_003Ec__IteratorD._003C_003Ef__am_0024cacheB = _003CsetAmbientThreshold_003Ec__IteratorD._003C_003Em__6;
			}
			float startThreshold = mats.FirstOrDefault(_003CsetAmbientThreshold_003Ec__IteratorD._003C_003Ef__am_0024cacheB).GetFloat("_AmbientThreshold");
			Stopwatch sw = new Stopwatch(fadeTime);
			while (!sw.Expired)
			{
				yield return null;
				Material[] array = mats;
				foreach (Material mat in array)
				{
					mat.SetFloat("_AmbientThreshold", Mathf.Lerp(startThreshold, threshold, sw.Progress));
				}
			}
		}

		private IEnumerator waitForPartToBeVisible()
		{
			Renderer renderer = GetComponentInChildren<Renderer>();
			ALDragAndDropManager dragdropmanager = UnityEngine.Object.FindObjectOfType<ALDragAndDropManager>();
			Collider draggableArea = dragdropmanager.GetComponent<Collider>();
			while (!draggableArea.bounds.Contains(renderer.bounds.center))
			{
				yield return null;
			}
		}

		public Coroutine WaitForPartToBeVisible()
		{
			return StartCoroutine(waitForPartToBeVisible());
		}
	}
}
