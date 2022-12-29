using System.Collections;
using UnityEngine;

namespace SLAM.Engine
{
	public class HeartIcon : MonoBehaviour
	{
		[SerializeField]
		private Animation animations;

		[SerializeField]
		private UITweener pickupTween;

		[SerializeField]
		private UITweener arrivedTween;

		[SerializeField]
		private AnimationCurve moveXCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private AnimationCurve moveYCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private float moveDuration = 0.7f;

		[SerializeField]
		private UISprite heartSprite;

		[SerializeField]
		private Transform heartDestination;

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void PlayShowAnimation(Vector3 atWorldPosition)
		{
			Vector3 position = Camera.main.WorldToScreenPoint(atWorldPosition);
			position.z = 0f;
			Vector3 from = base.gameObject.GetComponentInParent<UICamera>().cachedCamera.ScreenToWorldPoint(position);
			from.z = 0f;
			Vector3 position2 = heartDestination.position;
			StartCoroutine(showRoutine(from, position2));
		}

		public void PlayLoseAnimation()
		{
			heartSprite.gameObject.SetActive(false);
			animations.Rewind();
			animations.Play("HeartLost");
			AudioController.Play("Avatar_heart_break");
			AudioController.Play("Avatar_lose_heart");
		}

		private IEnumerator showRoutine(Vector3 from, Vector3 to)
		{
			heartSprite.cachedTransform.position = from;
			heartSprite.gameObject.SetActive(true);
			pickupTween.ResetToBeginning();
			pickupTween.PlayForward();
			yield return new WaitForSeconds(UIExtentions.GetAnimationDuration(pickupTween));
			float time = 0f;
			while (time < moveDuration)
			{
				time += Time.deltaTime;
				float progress = time / moveDuration;
				float x = Mathf.Lerp(from.x, to.x, moveXCurve.Evaluate(progress));
				float y = Mathf.Lerp(from.y, to.y, moveYCurve.Evaluate(progress));
				heartSprite.cachedTransform.position = new Vector3(x, y, to.z);
				yield return null;
			}
			AudioController.Play("Avatar_pickup_heart_followup");
			arrivedTween.ResetToBeginning();
			arrivedTween.PlayForward();
		}
	}
}
