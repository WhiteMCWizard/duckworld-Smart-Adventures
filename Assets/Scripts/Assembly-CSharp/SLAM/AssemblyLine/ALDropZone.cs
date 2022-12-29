using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.AssemblyLine
{
	public class ALDropZone : MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class _003CrobotCompleted_003Ec__IteratorA : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal GameObject _003CdropRoot_003E__0;

			internal Stopwatch _003Csw_003E__1;

			internal int _0024PC;

			internal object _0024current;

			internal ALDropZone _003C_003Ef__this;

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
				//Discarded unreachable code: IL_01d2
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_003CdropRoot_003E__0 = new GameObject("DropAnimRoot " + _003C_003Ef__this.name);
					GameEvents.Invoke(new AssemblyLineGame.RobotCompletedEvent
					{
						DropZone = _003C_003Ef__this,
						Robot = _003CdropRoot_003E__0
					});
					_0024current = new WaitForSeconds(0.5f);
					_0024PC = 1;
					break;
				case 1u:
					_003CdropRoot_003E__0.transform.position = _003C_003Ef__this.transform.position;
					_003C_003Ef__this.DroppedParts.ForEach(_003C_003Em__5);
					_003C_003Ef__this.DroppedParts.Clear();
					_003C_003Ef__this.PlacedTypes = (ALRobotPart.RobotPartType)0;
					_003C_003Ef__this.DesignatedKind = -1;
					_003Csw_003E__1 = new Stopwatch(_003C_003Ef__this.robotCompleteAnimLength);
					goto IL_0177;
				case 2u:
					_003CdropRoot_003E__0.transform.position = Vector3.Lerp(_003C_003Ef__this.transform.position, _003C_003Ef__this.transform.position + _003C_003Ef__this.robotCompletePosition, _003C_003Ef__this.robotCompleteAnimCurve.Evaluate(_003Csw_003E__1.Progress));
					goto IL_0177;
				default:
					{
						return false;
					}
					IL_0177:
					if (!_003Csw_003E__1.Expired)
					{
						_0024current = null;
						_0024PC = 2;
						break;
					}
					if (_003C_003Ef__this.DroppedParts.Count <= 0)
					{
						_003C_003Ef__this.beam.material.SetColor("_Color", Color.white);
					}
					UnityEngine.Object.Destroy(_003CdropRoot_003E__0);
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

			internal void _003C_003Em__5(ALRobotPart d)
			{
				d.transform.parent = _003CdropRoot_003E__0.transform;
				_003C_003Ef__this.blendToAnim(d.GetComponentInChildren<Animation>(), "Outro");
			}
		}

		[SerializeField]
		private float partMovementSpeed;

		[SerializeField]
		private AnimationCurve partAnimationCurve;

		[SerializeField]
		private MeshRenderer beam;

		[SerializeField]
		private AnimationCurve robotCompleteAnimCurve;

		[SerializeField]
		private Vector3 robotCompletePosition;

		[SerializeField]
		private float robotCompleteAnimLength;

		[SerializeField]
		private GameObject[] lightOffObject;

		public List<ALRobotPart> DroppedParts { get; protected set; }

		public int DesignatedKind { get; private set; }

		public ALRobotPart.RobotPartType PlacedTypes { get; private set; }

		private void Start()
		{
			DroppedParts = new List<ALRobotPart>();
			DesignatedKind = -1;
			beam.material.SetTextureOffset("_MainTex", Vector2.one * UnityEngine.Random.value);
			beam.material.SetTextureOffset("_SecondTex", Vector2.one * UnityEngine.Random.value);
			beam.material.SetTextureOffset("_ThirdTex", Vector2.one * UnityEngine.Random.value);
			beam.material.SetFloat("_MainTexSpeed", UnityEngine.Random.value);
			beam.material.SetFloat("_SecondTexSpeed", UnityEngine.Random.value);
			beam.material.SetFloat("_ThirdTexSpeed", UnityEngine.Random.value);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawLine(base.transform.position, base.transform.position + robotCompletePosition);
		}

		private void OnEnable()
		{
			if (beam != null)
			{
				beam.gameObject.SetActive(true);
			}
			for (int i = 0; i < lightOffObject.Length; i++)
			{
				if (lightOffObject[i] != null)
				{
					lightOffObject[i].SetActive(false);
				}
			}
		}

		private void OnDisable()
		{
			if (beam != null)
			{
				beam.gameObject.SetActive(false);
			}
			for (int i = 0; i < lightOffObject.Length; i++)
			{
				if (lightOffObject[i] != null)
				{
					lightOffObject[i].SetActive(true);
				}
			}
		}

		public bool CanDropPart(ALRobotPart robotPart, Vector3 worldMousePos)
		{
			Collider component = GetComponent<Collider>();
			worldMousePos.z = component.bounds.center.z;
			bool flag = component.bounds.Contains(worldMousePos) && (DesignatedKind == -1 || DesignatedKind == robotPart.Kind) && (PlacedTypes & robotPart.Type) != robotPart.Type;
			float num = ((!flag) ? 0.75f : 1f);
			if (beam.material.GetFloat("_AlphaMod") != num)
			{
				beam.material.SetFloat("_AlphaMod", num);
				if (flag)
				{
					GameEvents.Invoke(new AssemblyLineGame.PartHoverEvent());
				}
			}
			return flag;
		}

		public void DropPart(ALRobotPart robotPart)
		{
			DesignatedKind = robotPart.Kind;
			PlacedTypes |= robotPart.Type;
			DroppedParts.Add(robotPart);
			robotPart.CanDrag = false;
			robotPart.transform.parent = base.transform;
			StartCoroutine(animatePartToTargetPosition(robotPart, base.transform.position));
			beam.material.SetColor("_Color", robotPart.BeamColor);
			beam.material.SetFloat("_AlphaMod", 0.75f);
		}

		private IEnumerator animatePartToTargetPosition(ALRobotPart robotPart, Vector3 endPosition)
		{
			Vector3 startPosition = robotPart.transform.position;
			Stopwatch sw = new Stopwatch(Vector3.Distance(startPosition, endPosition) / partMovementSpeed);
			blendToAnim(robotPart.GetComponentInChildren<Animation>(), "Idle", sw.Duration, true);
			AssemblyLineGame.PartDroppedEvent partDroppedEvent = new AssemblyLineGame.PartDroppedEvent();
			partDroppedEvent.Part = robotPart;
			partDroppedEvent.DropZone = this;
			GameEvents.Invoke(partDroppedEvent);
			while (!sw.Expired)
			{
				yield return null;
				if (robotPart == null)
				{
					yield break;
				}
				robotPart.transform.position = Vector3.Lerp(startPosition, endPosition, partAnimationCurve.Evaluate(sw.Progress));
			}
			if (PlacedTypes == (ALRobotPart.RobotPartType.Head | ALRobotPart.RobotPartType.ArmRight | ALRobotPart.RobotPartType.ArmLeft | ALRobotPart.RobotPartType.Body | ALRobotPart.RobotPartType.Feet))
			{
				StartCoroutine(robotCompleted());
			}
		}

		private IEnumerator robotCompleted()
		{
			GameObject dropRoot = new GameObject("DropAnimRoot " + base.name);
			AssemblyLineGame.RobotCompletedEvent robotCompletedEvent = new AssemblyLineGame.RobotCompletedEvent();
			robotCompletedEvent.DropZone = this;
			robotCompletedEvent.Robot = dropRoot;
			GameEvents.Invoke(robotCompletedEvent);
			yield return new WaitForSeconds(0.5f);
			dropRoot.transform.position = base.transform.position;
			DroppedParts.ForEach(((_003CrobotCompleted_003Ec__IteratorA)(object)this)._003C_003Em__5);
			DroppedParts.Clear();
			PlacedTypes = (ALRobotPart.RobotPartType)0;
			DesignatedKind = -1;
			Stopwatch sw = new Stopwatch(robotCompleteAnimLength);
			while (!sw.Expired)
			{
				yield return null;
				dropRoot.transform.position = Vector3.Lerp(base.transform.position, base.transform.position + robotCompletePosition, robotCompleteAnimCurve.Evaluate(sw.Progress));
			}
			if (DroppedParts.Count <= 0)
			{
				beam.material.SetColor("_Color", Color.white);
			}
			UnityEngine.Object.Destroy(dropRoot);
		}

		private void blendToAnim(Animation anim, string animName, float fadeLength = 0.3f, bool syncTime = false)
		{
			foreach (AnimationState item in anim)
			{
				if (item.name.EndsWith(animName))
				{
					anim.clip = item.clip;
					if (syncTime)
					{
						Animation componentInChildren = DroppedParts.First().GetComponentInChildren<Animation>();
						float normalizedTime = componentInChildren[componentInChildren.clip.name].normalizedTime;
						item.normalizedTime = normalizedTime;
					}
					anim.CrossFade(item.clip.name, fadeLength);
					return;
				}
			}
			UnityEngine.Debug.Log("Couldnt find anim that ends with " + animName + " in " + anim.gameObject.name, anim.gameObject);
		}
	}
}
