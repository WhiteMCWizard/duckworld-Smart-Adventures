using System;
using System.Collections;
using UnityEngine;

public class CutscenePlayer : MonoBehaviour
{
	[Serializable]
	private struct CutsceneItem
	{
		[SerializeField]
		private Animation animation;

		[SerializeField]
		private AnimationClip clip;

		[SerializeField]
		private float startDelay;

		public Animation Animation
		{
			get
			{
				return animation;
			}
		}

		public AnimationClip Clip
		{
			get
			{
				return clip;
			}
		}

		public float StartDelay
		{
			get
			{
				return startDelay;
			}
		}
	}

	[SerializeField]
	private CutsceneItem[] items;

	private bool done;

	public Coroutine PlayCutscene(int id)
	{
		done = false;
		return StartCoroutine(doPlayCutscene(id));
	}

	private IEnumerator doPlayCutscene(int id)
	{
		StartCoroutine(playCutsceneItem(items[id]));
		while (!done)
		{
			yield return null;
		}
	}

	private IEnumerator playCutsceneItem(CutsceneItem item)
	{
		yield return new WaitForSeconds(item.StartDelay);
		item.Animation.clip = item.Clip;
		item.Animation.Play();
		yield return new WaitForSeconds(item.Clip.length);
		done = true;
	}
}
