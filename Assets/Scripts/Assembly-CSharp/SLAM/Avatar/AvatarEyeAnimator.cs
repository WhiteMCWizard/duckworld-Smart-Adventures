using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Avatar
{
	public class AvatarEyeAnimator : MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class _003CdoEyeAnimationSequence_003Ec__IteratorF4 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal Renderer _003CeyeRenderer_003E__0;

			internal float _003CspriteWidth_003E__1;

			internal float _003CspriteHeight_003E__2;

			internal float _003CframeWidth_003E__3;

			internal float _003CframeHeight_003E__4;

			internal float _003Cx_003E__5;

			internal float _003Cy_003E__6;

			internal int _0024PC;

			internal object _0024current;

			internal AvatarEyeAnimator _003C_003Ef__this;

			public static Func<Renderer, bool> _003C_003Ef__am_0024cacheA;

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
				//Discarded unreachable code: IL_0374
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_003C_003Ef__this.findCorrectMaterialIndex();
					_0024current = null;
					_0024PC = 1;
					break;
				case 1u:
					_0024current = null;
					_0024PC = 2;
					break;
				case 2u:
					if (_003C_003Ef__this.rendererItem != null && _003C_003Ef__this.materialIndex > -1)
					{
						_003C_003Ef__this.material = _003C_003Ef__this.rendererItem.materials[_003C_003Ef__this.materialIndex];
					}
					if (_003C_003Ef__this.material == null)
					{
						Renderer[] componentsInChildren = _003C_003Ef__this.GetComponentsInChildren<Renderer>();
						if (_003C_003Ef__am_0024cacheA == null)
						{
							_003C_003Ef__am_0024cacheA = _003C_003Em__C2;
						}
						_003CeyeRenderer_003E__0 = componentsInChildren.FirstOrDefault(_003C_003Ef__am_0024cacheA);
						if (_003CeyeRenderer_003E__0 != null)
						{
							_003CeyeRenderer_003E__0.enabled = false;
						}
						UnityEngine.Debug.LogWarning("No material so i quit!", _003C_003Ef__this);
						goto default;
					}
					_003C_003Ef__this.animator = _003C_003Ef__this.GetComponentInParent<Animator>();
					_003CspriteWidth_003E__1 = _003C_003Ef__this.material.mainTexture.width;
					_003CspriteHeight_003E__2 = _003C_003Ef__this.material.mainTexture.height;
					_003CframeWidth_003E__3 = _003CspriteWidth_003E__1 / (float)_003C_003Ef__this.tileXCount;
					_003CframeHeight_003E__4 = _003CspriteHeight_003E__2 / (float)_003C_003Ef__this.tileYCount;
					_003C_003Ef__this.nextBlinkTime = Time.time + 5f;
					_003Cx_003E__5 = 0f;
					_003Cy_003E__6 = 0f;
					goto case 3u;
				case 3u:
					if (_003C_003Ef__this.animationNullObject == null || _003C_003Ef__this.animationNullObject.localPosition.x < 0f || _003C_003Ef__this.animationNullObject.localPosition.y < 0f)
					{
						_003C_003Ef__this.doEyeIdle(out _003Cx_003E__5, out _003Cy_003E__6, _003C_003Ef__this.material);
					}
					else if (!_003C_003Ef__this.isAnimatorTransitioning())
					{
						_003Cx_003E__5 = _003C_003Ef__this.animationNullObject.localPosition.x;
						_003Cy_003E__6 = 0f - _003C_003Ef__this.animationNullObject.localPosition.y;
						_003C_003Ef__this.material.mainTextureScale = new Vector2(_003C_003Ef__this.animationNullObject.localPosition.z, _003C_003Ef__this.material.mainTextureScale.y);
						if (_003C_003Ef__this.animationNullObject.localPosition.z < 0f)
						{
							_003Cx_003E__5 += 1f;
						}
					}
					_003C_003Ef__this.material.mainTextureOffset = new Vector2(_003Cx_003E__5 * _003CframeWidth_003E__3 / _003CspriteWidth_003E__1, _003Cy_003E__6 * _003CframeHeight_003E__4 / _003CspriteHeight_003E__2);
					_0024current = null;
					_0024PC = 3;
					break;
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

			public static bool _003C_003Em__C2(Renderer r)
			{
				return r.name.Contains("Eyes");
			}
		}

		[SerializeField]
		private Transform animationNullObject;

		[SerializeField]
		private Material material;

		[SerializeField]
		private Vector2 uvOffset;

		[SerializeField]
		private int tileXCount = 4;

		[SerializeField]
		private int tileYCount = 4;

		private Animator animator;

		private Renderer rendererItem;

		private int materialIndex = -1;

		private float nextBlinkTime;

		private float blinkTime = 0.2f;

		private float minBlinkInterval = 2f;

		private float maxBlinkInterval = 10f;

		private void OnEnable()
		{
			StartCoroutine(doEyeAnimationSequence());
		}

		private IEnumerator doEyeAnimationSequence()
		{
			findCorrectMaterialIndex();
			yield return null;
			yield return null;
			if (rendererItem != null && materialIndex > -1)
			{
				material = rendererItem.materials[materialIndex];
			}
			if (material == null)
			{
				Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
				if (_003CdoEyeAnimationSequence_003Ec__IteratorF4._003C_003Ef__am_0024cacheA == null)
				{
					_003CdoEyeAnimationSequence_003Ec__IteratorF4._003C_003Ef__am_0024cacheA = _003CdoEyeAnimationSequence_003Ec__IteratorF4._003C_003Em__C2;
				}
				Renderer eyeRenderer = componentsInChildren.FirstOrDefault(_003CdoEyeAnimationSequence_003Ec__IteratorF4._003C_003Ef__am_0024cacheA);
				if (eyeRenderer != null)
				{
					eyeRenderer.enabled = false;
				}
				UnityEngine.Debug.LogWarning("No material so i quit!", this);
				yield break;
			}
			animator = GetComponentInParent<Animator>();
			float spriteWidth = material.mainTexture.width;
			float spriteHeight = material.mainTexture.height;
			float frameWidth = spriteWidth / (float)tileXCount;
			float frameHeight = spriteHeight / (float)tileYCount;
			nextBlinkTime = Time.time + 5f;
			float x = 0f;
			float y = 0f;
			while (true)
			{
				if (animationNullObject == null || animationNullObject.localPosition.x < 0f || animationNullObject.localPosition.y < 0f)
				{
					doEyeIdle(out x, out y, material);
				}
				else if (!isAnimatorTransitioning())
				{
					x = animationNullObject.localPosition.x;
					y = 0f - animationNullObject.localPosition.y;
					material.mainTextureScale = new Vector2(animationNullObject.localPosition.z, material.mainTextureScale.y);
					if (animationNullObject.localPosition.z < 0f)
					{
						x += 1f;
					}
				}
				material.mainTextureOffset = new Vector2(x * frameWidth / spriteWidth, y * frameHeight / spriteHeight);
				yield return null;
			}
		}

		private bool isAnimatorTransitioning()
		{
			for (int i = 0; i < animator.layerCount; i++)
			{
				if (animator.IsInTransition(i))
				{
					return true;
				}
			}
			return false;
		}

		private void findCorrectMaterialIndex()
		{
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				for (int j = 0; j < renderer.sharedMaterials.Length; j++)
				{
					if (renderer.sharedMaterials[j] == material || (material == null && renderer.sharedMaterials[j].name.ToLower().Contains("eye")))
					{
						rendererItem = renderer;
						materialIndex = j;
						return;
					}
				}
			}
			UnityEngine.Debug.LogError("Couldnt find " + animationNullObject.name + " material for " + base.name, this);
		}

		private void doEyeIdle(out float x, out float y, Material mat)
		{
			mat.mainTextureScale = new Vector2(1f, mat.mainTextureScale.y);
			y = 0f;
			if (Time.time > nextBlinkTime)
			{
				float num = (Time.time - nextBlinkTime) / blinkTime;
				x = Mathf.Round((float)tileXCount * num);
				if (num > 1f)
				{
					nextBlinkTime = Time.time + UnityEngine.Random.Range(minBlinkInterval, maxBlinkInterval);
				}
			}
			else
			{
				x = 0f;
			}
			x += uvOffset.x;
			y += uvOffset.y;
		}
	}
}
