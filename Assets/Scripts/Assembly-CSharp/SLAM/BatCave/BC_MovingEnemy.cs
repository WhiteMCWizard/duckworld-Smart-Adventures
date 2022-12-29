using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Platformer;
using UnityEngine;

namespace SLAM.BatCave
{
	public class BC_MovingEnemy : ObjectMover
	{
		[CompilerGenerated]
		private sealed class _003COnTriggerEnter_003Ec__Iterator28 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal Collider col;

			internal bool _003CisPlayer_003E__0;

			internal float _003CsavedSpeed_003E__1;

			internal int _0024PC;

			internal object _0024current;

			internal Collider _003C_0024_003Ecol;

			internal BC_MovingEnemy _003C_003Ef__this;

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
				//Discarded unreachable code: IL_0229
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_003CisPlayer_003E__0 = col.GetComponentInParent<CC2DPlayer>() != null;
					if (!_003C_003Ef__this.isAttacking)
					{
						if (_003C_003Ef__this.movingAudioObject != null && _003C_003Ef__this.movingAudioObject.IsPlaying())
						{
							_003C_003Ef__this.movingAudioObject.Stop();
						}
						if (_003C_003Ef__this.attackSound != null)
						{
							AudioController.Play(_003C_003Ef__this.attackSound.name, _003C_003Ef__this.transform);
						}
						_003C_003Ef__this.isAttacking = true;
						_003C_003Ef__this.animator.SetTrigger("Attack");
						_003CsavedSpeed_003E__1 = _003C_003Ef__this.movementSpeed;
						_003C_003Ef__this.movementSpeed = 0f;
						_0024current = new WaitForSeconds(_003C_003Ef__this.timeBetweenAttackAnimAndHitEvent);
						_0024PC = 1;
						break;
					}
					goto IL_020b;
				case 1u:
					if (!_003C_003Ef__this.cooldownActive && _003CisPlayer_003E__0)
					{
						_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.waitTillEndOfFrameAndExecute(_003C_003Em__2B));
						_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.cooldown(_003C_003Ef__this.cooldownDuration));
					}
					_0024current = new WaitForSeconds(0.5f);
					_0024PC = 2;
					break;
				case 2u:
					if (_003C_003Ef__this.movingSound != null)
					{
						_003C_003Ef__this.movingAudioObject = AudioController.Play(_003C_003Ef__this.movingSound.name, _003C_003Ef__this.transform);
					}
					_003C_003Ef__this.movementSpeed = _003CsavedSpeed_003E__1;
					_003C_003Ef__this.TurnAround();
					_003C_003Ef__this.isAttacking = false;
					goto IL_020b;
				case 3u:
					_0024PC = -1;
					goto default;
				default:
					{
						return false;
					}
					IL_020b:
					_0024current = null;
					_0024PC = 3;
					break;
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

			internal void _003C_003Em__2B()
			{
				GameEvents.Invoke(new PlayerHitEvent
				{
					EnemyObject = _003C_003Ef__this.gameObject
				});
			}
		}

		[SerializeField]
		protected Animator animator;

		[SerializeField]
		protected float cooldownDuration = 1f;

		[SerializeField]
		protected AudioClip movingSound;

		[SerializeField]
		protected AudioClip attackSound;

		[SerializeField]
		protected AudioClip squeakSound;

		protected bool cooldownActive;

		protected bool isAttacking;

		protected AudioObject movingAudioObject;

		protected virtual float timeBetweenAttackAnimAndHitEvent
		{
			get
			{
				return 0.15f;
			}
		}

		private void Start()
		{
			if (movingSound != null)
			{
				movingAudioObject = AudioController.Play(movingSound.name, base.transform);
			}
			if (squeakSound != null)
			{
				AudioController.Play(squeakSound.name, base.transform);
			}
		}

		private void Update()
		{
			if (movingSound != null && (movingAudioObject == null || !movingAudioObject.IsPlaying()))
			{
				movingAudioObject = AudioController.Play(movingSound.name, base.transform);
			}
		}

		protected virtual IEnumerator OnTriggerEnter(Collider col)
		{
			bool isPlayer = col.GetComponentInParent<CC2DPlayer>() != null;
			if (!isAttacking)
			{
				if (movingAudioObject != null && movingAudioObject.IsPlaying())
				{
					movingAudioObject.Stop();
				}
				if (attackSound != null)
				{
					AudioController.Play(attackSound.name, base.transform);
				}
				isAttacking = true;
				animator.SetTrigger("Attack");
				float savedSpeed = movementSpeed;
				movementSpeed = 0f;
				yield return new WaitForSeconds(timeBetweenAttackAnimAndHitEvent);
				if (!cooldownActive && isPlayer)
				{
					StartCoroutine(waitTillEndOfFrameAndExecute(((_003COnTriggerEnter_003Ec__Iterator28)(object)this)._003C_003Em__2B));
					StartCoroutine(cooldown(cooldownDuration));
				}
				yield return new WaitForSeconds(0.5f);
				if (movingSound != null)
				{
					movingAudioObject = AudioController.Play(movingSound.name, base.transform);
				}
				movementSpeed = savedSpeed;
				TurnAround();
				isAttacking = false;
			}
			yield return null;
		}

		protected IEnumerator waitTillEndOfFrameAndExecute(System.Action method)
		{
			yield return new WaitForEndOfFrame();
			if (method != null)
			{
				method();
			}
		}

		protected IEnumerator cooldown(float duration)
		{
			cooldownActive = true;
			yield return new WaitForSeconds(duration);
			cooldownActive = false;
		}
	}
}
