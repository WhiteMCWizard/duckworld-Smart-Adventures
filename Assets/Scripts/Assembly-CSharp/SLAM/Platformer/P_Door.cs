using System.Collections;
using UnityEngine;

namespace SLAM.Platformer
{
	public class P_Door : P_PressUpTrigger
	{
		[SerializeField]
		private P_Door otherDoor;

		[SerializeField]
		public Transform myExit;

		[SerializeField]
		protected float lockAvatarDurations = 2.61f;

		[SerializeField]
		private float walkThroughDoorTime = 1f;

		[SerializeField]
		private float fadeToBlackTime = 0.6f;

		private SidescrollerBehaviour sidescrollerCamera;

		private bool warpCamera;

		protected override void Start()
		{
			base.Start();
			sidescrollerCamera = Object.FindObjectOfType<SidescrollerBehaviour>();
		}

		public void TeleportObject(Transform obj)
		{
			obj.position = myExit.position;
			if (warpCamera)
			{
				sidescrollerCamera.WarpTo(myExit.position);
			}
		}

		protected override UpAction DoAction()
		{
			base.DoAction();
			EnterDoor(this, otherDoor);
			return new UpAction(Action.EnterDoor, lockAvatarDurations, this);
		}

		private void EnterDoor(P_Door entrance, P_Door exit)
		{
			StartCoroutine(DoDoorRoutine(entrance, exit));
		}

		private IEnumerator DoDoorRoutine(P_Door entrance, P_Door exit)
		{
			Vector3 from2 = entrance.myExit.transform.position;
			Vector3 to2 = entrance.myExit.transform.position + Vector3.forward * 1.5f;
			base.Player.SwitchTo("Walking");
			base.Player.GetComponent<Animator>().SetBool("Walking", true);
			base.Player.transform.rotation = Quaternion.AngleAxis(0f, Vector3.up);
			base.Player.Pause();
			float time2 = 0f;
			while (time2 < walkThroughDoorTime)
			{
				time2 += Time.deltaTime;
				base.Player.transform.position = Vector3.Lerp(from2, to2, time2 / walkThroughDoorTime);
				yield return null;
			}
			DoorEnterEvent doorEnterEvent = new DoorEnterEvent();
			doorEnterEvent.fadeToBlackTime = fadeToBlackTime;
			GameEvents.Invoke(doorEnterEvent);
			exit.TeleportObject(base.Player.transform);
			from2 = exit.myExit.position + Vector3.forward * 1.5f;
			to2 = exit.myExit.position;
			to2.z = 0f;
			base.Player.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
			time2 = 0f;
			while (time2 < walkThroughDoorTime)
			{
				time2 += Time.deltaTime;
				base.Player.transform.position = Vector3.Lerp(from2, to2, time2 / walkThroughDoorTime);
				base.Player.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
				yield return null;
			}
			base.Player.GetComponent<Animator>().SetBool("Walking", false);
			base.Player.transform.rotation = Quaternion.AngleAxis(90f, Vector3.up);
			base.Player.Resume();
		}
	}
}
