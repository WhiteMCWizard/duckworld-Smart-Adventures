using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.CameraSystem;
using UnityEngine;

namespace SLAM.MoneyDive
{
	public class MD_Cinematics : MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class _003CDoShortIntro_003Ec__IteratorD0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal Action callback;

			internal int _0024PC;

			internal object _0024current;

			internal Action _003C_0024_003Ecallback;

			internal MD_Cinematics _003C_003Ef__this;

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
				//Discarded unreachable code: IL_012f
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_003C_003Ef__this.avatarController.AreControlsLocked = true;
					AudioController.Play("MD_dive_anim", 1f, 0f, _003C_003Ef__this.intro1Duration + _003C_003Ef__this.intro2Duration);
					_003C_003Ef__this.avatarAnimator.SetTrigger("Intro");
					_003C_003Ef__this.scroogeAnimator.SetTrigger("Intro");
					_003C_003Ef__this.plankAnimator.SetTrigger("Intro");
					_003C_003Ef__this.gameController.FadeInAndOut(_003C_003Em__B4, null);
					_0024current = new WaitForSeconds(_003C_003Ef__this.intro3Duration + 0.56f);
					_0024PC = 1;
					return true;
				case 1u:
					AudioController.Play("MD_diving_wind_loop");
					_003C_003Ef__this.avatarController.StartFalling();
					_003C_003Ef__this.avatarController.AreControlsLocked = false;
					if (callback != null)
					{
						callback();
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

			internal void _003C_003Em__B4()
			{
				_003C_003Ef__this.scroogeAnimator.transform.parent = _003C_003Ef__this.avatarOutroAnimationLocation;
				_003C_003Ef__this.scroogeAnimator.transform.localPosition = Vector3.zero;
				_003C_003Ef__this.plankAnimator.SetTrigger("Intro");
				_003C_003Ef__this.avatarAnimator.SetTrigger("Intro");
				_003C_003Ef__this.camera3Animator.SetTrigger("Intro");
				_003C_003Ef__this.cameraManager.CrossFade(_003C_003Ef__this.intro3, 0f);
			}
		}

		[CompilerGenerated]
		private sealed class _003CDoIntro_003Ec__IteratorD1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal Action callback;

			internal int _0024PC;

			internal object _0024current;

			internal Action _003C_0024_003Ecallback;

			internal MD_Cinematics _003C_003Ef__this;

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
				//Discarded unreachable code: IL_01b4
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_003C_003Ef__this.IsDoingIntroduction = true;
					_003C_003Ef__this.avatarController.AreControlsLocked = true;
					_003C_003Ef__this.cameraManager.CrossFade(_003C_003Ef__this.intro1, 0f);
					AudioController.Play("MD_dive_anim");
					_003C_003Ef__this.avatarAnimator.SetTrigger("Intro");
					_003C_003Ef__this.scroogeAnimator.SetTrigger("Intro");
					_003C_003Ef__this.plankAnimator.SetTrigger("Intro");
					_0024current = new WaitForSeconds(_003C_003Ef__this.intro1Duration);
					_0024PC = 1;
					break;
				case 1u:
					_003C_003Ef__this.cameraManager.CrossFade(_003C_003Ef__this.intro2, 0f);
					_0024current = new WaitForSeconds(_003C_003Ef__this.intro2Duration);
					_0024PC = 2;
					break;
				case 2u:
					_003C_003Ef__this.gameController.FadeInAndOut(_003C_003Em__B5, null);
					_0024current = new WaitForSeconds(_003C_003Ef__this.intro3Duration + 0.56f);
					_0024PC = 3;
					break;
				case 3u:
					AudioController.Play("MD_diving_wind_loop");
					_003C_003Ef__this.avatarController.StartFalling();
					_003C_003Ef__this.avatarController.AreControlsLocked = false;
					_003C_003Ef__this.IsDoingIntroduction = false;
					if (callback != null)
					{
						callback();
					}
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

			internal void _003C_003Em__B5()
			{
				_003C_003Ef__this.scroogeAnimator.transform.parent = _003C_003Ef__this.avatarOutroAnimationLocation;
				_003C_003Ef__this.scroogeAnimator.transform.localPosition = Vector3.zero;
				_003C_003Ef__this.plankAnimator.SetTrigger("Intro");
				_003C_003Ef__this.avatarAnimator.SetTrigger("Intro");
				_003C_003Ef__this.camera3Animator.SetTrigger("Intro");
				_003C_003Ef__this.cameraManager.CrossFade(_003C_003Ef__this.intro3, 0f);
			}
		}

		[CompilerGenerated]
		private sealed class _003CSkipIntro_003Ec__AnonStorey177
		{
			internal Action callback;

			internal MD_Cinematics _003C_003Ef__this;

			internal void _003C_003Em__B3()
			{
				_003C_003Ef__this.scroogeAnimator.transform.parent = _003C_003Ef__this.avatarOutroAnimationLocation;
				_003C_003Ef__this.scroogeAnimator.transform.localPosition = Vector3.zero;
				_003C_003Ef__this.cameraManager.CrossFade(_003C_003Ef__this.intro3, 0f);
				_003C_003Ef__this.avatarAnimator.SetTrigger("Idle");
				_003C_003Ef__this.avatarController.StartFalling();
				_003C_003Ef__this.avatarController.AreControlsLocked = false;
				if (callback != null)
				{
					callback();
				}
			}
		}

		[SerializeField]
		private Animator avatarAnimator;

		[SerializeField]
		private Animator scroogeAnimator;

		[SerializeField]
		private Animator plankAnimator;

		[SerializeField]
		private Animator camera3Animator;

		[SerializeField]
		private MD_AvatarController avatarController;

		[SerializeField]
		private Transform avatarOutroAnimationLocation;

		[SerializeField]
		private MD_LevelSpawner levelSpawner;

		[SerializeField]
		private CameraManager cameraManager;

		[SerializeField]
		private MD_Controller gameController;

		[SerializeField]
		private CameraBehaviour intro1;

		[SerializeField]
		private CameraBehaviour intro2;

		[SerializeField]
		private CameraBehaviour intro3;

		[SerializeField]
		private CameraBehaviour outro1;

		[SerializeField]
		private float outro1Duration;

		[SerializeField]
		private float outro1CrossFade = 2f;

		[SerializeField]
		private AnimationCurve outro1Curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private PrefabSpawner coinSplashParticleSpawner;

		[SerializeField]
		private Transform avatarBottomReachedLocation;

		private float intro1Duration = 1.25f;

		private float intro2Duration = 6.208f;

		private float intro3Duration = 2.917f;

		private float time;

		public bool IsDoingIntroduction { get; private set; }

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void PlayShortIntro(Action callback)
		{
			StartCoroutine(DoShortIntro(callback));
		}

		public void PlayIntro(Action callback)
		{
			StartCoroutine(DoIntro(callback));
		}

		public void SkipIntro(Action callback)
		{
			_003CSkipIntro_003Ec__AnonStorey177 _003CSkipIntro_003Ec__AnonStorey = new _003CSkipIntro_003Ec__AnonStorey177();
			_003CSkipIntro_003Ec__AnonStorey.callback = callback;
			_003CSkipIntro_003Ec__AnonStorey._003C_003Ef__this = this;
			StopAllCoroutines();
			AudioController.StopCategory("SFX", 0f);
			AudioController.Play("MD_diving_wind_loop");
			gameController.FadeInAndOut(_003CSkipIntro_003Ec__AnonStorey._003C_003Em__B3, null);
		}

		public void PlayOutro(bool hasWon, Action callback)
		{
			StartCoroutine(DoOutro(hasWon, callback));
		}

		private IEnumerator DoShortIntro(Action callback)
		{
			avatarController.AreControlsLocked = true;
			AudioController.Play("MD_dive_anim", 1f, 0f, intro1Duration + intro2Duration);
			avatarAnimator.SetTrigger("Intro");
			scroogeAnimator.SetTrigger("Intro");
			plankAnimator.SetTrigger("Intro");
			gameController.FadeInAndOut(((_003CDoShortIntro_003Ec__IteratorD0)(object)this)._003C_003Em__B4, null);
			yield return new WaitForSeconds(intro3Duration + 0.56f);
			AudioController.Play("MD_diving_wind_loop");
			avatarController.StartFalling();
			avatarController.AreControlsLocked = false;
			if (callback != null)
			{
				callback();
			}
		}

		private IEnumerator DoIntro(Action callback)
		{
			IsDoingIntroduction = true;
			avatarController.AreControlsLocked = true;
			cameraManager.CrossFade(intro1, 0f);
			AudioController.Play("MD_dive_anim");
			avatarAnimator.SetTrigger("Intro");
			scroogeAnimator.SetTrigger("Intro");
			plankAnimator.SetTrigger("Intro");
			yield return new WaitForSeconds(intro1Duration);
			cameraManager.CrossFade(intro2, 0f);
			yield return new WaitForSeconds(intro2Duration);
			gameController.FadeInAndOut(((_003CDoIntro_003Ec__IteratorD1)(object)this)._003C_003Em__B5, null);
			yield return new WaitForSeconds(intro3Duration + 0.56f);
			AudioController.Play("MD_diving_wind_loop");
			avatarController.StartFalling();
			avatarController.AreControlsLocked = false;
			IsDoingIntroduction = false;
			if (callback != null)
			{
				callback();
			}
		}

		private IEnumerator DoOutro(bool victory, Action callback)
		{
			StartCoroutine(doStopwatch());
			avatarController.AreControlsLocked = true;
			levelSpawner.StopSpawning();
			cameraManager.CrossFade(outro1, outro1CrossFade, outro1Curve);
			scroogeAnimator.SetTrigger("Idle");
			if (victory)
			{
				AudioController.Play("MD_landing_succes");
			}
			else
			{
				AudioController.Play("MD_landing_fail");
			}
			while (avatarController.transform.position.y > avatarBottomReachedLocation.position.y)
			{
				yield return null;
			}
			avatarController.StopFalling();
			avatarController.transform.position = avatarOutroAnimationLocation.position;
			coinSplashParticleSpawner.Spawn();
			avatarAnimator.SetTrigger((!victory) ? "Failure" : "Succes");
			scroogeAnimator.SetTrigger((!victory) ? "Failure" : "Succes");
			AudioController.Stop("MD_diving_wind_loop");
			yield return new WaitForSeconds(outro1Duration);
			if (callback != null)
			{
				callback();
			}
		}

		private IEnumerator doStopwatch()
		{
			time = 0f;
			bool run = true;
			while (run)
			{
				time += Time.deltaTime;
				yield return null;
			}
		}
	}
}
