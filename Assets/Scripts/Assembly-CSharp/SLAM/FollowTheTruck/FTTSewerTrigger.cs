using UnityEngine;

namespace SLAM.FollowTheTruck
{
	public class FTTSewerTrigger : FTTInteractable<FTTTruckController>
	{
		[Header("Sound fx")]
		[SerializeField]
		private string loopSound;

		private Animator animator;

		private void Awake()
		{
			animator = GetComponentInChildren<Animator>();
		}

		private void Start()
		{
			animator.gameObject.SetActive(false);
		}

		public override void OnInteract(FTTTruckController truckController)
		{
			animator.gameObject.SetActive(true);
			if (!string.IsNullOrEmpty(loopSound))
			{
				AudioController.Play(loopSound, base.transform);
			}
		}
	}
}
