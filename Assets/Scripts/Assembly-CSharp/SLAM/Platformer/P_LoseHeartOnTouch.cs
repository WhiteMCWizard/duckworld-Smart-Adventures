using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SLAM.Platformer
{
	public class P_LoseHeartOnTouch : P_Trigger
	{
		[SerializeField]
		private float cooldownDuration = 1f;

		[SerializeField]
		private AudioClip audioClip;

		private bool cooldownActive;

		private void OnTriggerStay(Collider col)
		{
			if (!cooldownActive && col.transform.root.GetComponentInChildren<CC2DPlayer>() != null)
			{
				StartCoroutine(waitTillEndOfFrameAndExecute(_003COnTriggerStay_003Em__D8));
				StartCoroutine(cooldown(cooldownDuration));
			}
		}

		protected IEnumerator waitTillEndOfFrameAndExecute(System.Action method)
		{
			yield return new WaitForEndOfFrame();
			if (audioClip != null)
			{
				AudioController.Play(audioClip.name, base.transform);
			}
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

		[CompilerGenerated]
		private void _003COnTriggerStay_003Em__D8()
		{
			GameEvents.Invoke(new PlayerHitEvent
			{
				EnemyObject = base.gameObject
			});
		}
	}
}
