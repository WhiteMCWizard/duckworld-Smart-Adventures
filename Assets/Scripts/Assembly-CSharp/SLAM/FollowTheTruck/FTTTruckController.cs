using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.FollowTheTruck
{
	public class FTTTruckController : MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class _003CUpdate_003Ec__AnonStorey16B
		{
			internal Vector3 preferedPoint;

			internal float _003C_003Em__6A(Transform o)
			{
				return Vector3.Distance(o.position, preferedPoint);
			}

			internal bool _003C_003Em__6B(Transform w)
			{
				return Vector3.Dot(Vector3.forward, (w.position - preferedPoint).normalized) > 0f;
			}
		}

		private const float INTRO_ANIMATION_LENGHT = 3.083f;

		[SerializeField]
		private FTTAvatarController avatar;

		private float DistanceToPlayer = 10f;

		private List<Transform> waypoints = new List<Transform>();

		private Transform toPoint;

		private Transform fromPoint;

		private Vector3 truckTarget = Vector3.zero;

		private void OnDrawGizmosSelected()
		{
			if (!Application.isPlaying || waypoints == null)
			{
				return;
			}
			Transform transform = null;
			foreach (Transform waypoint in waypoints)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(waypoint.position, 0.2f);
				Gizmos.color = Color.blue;
				if (transform != null)
				{
					Gizmos.DrawLine(transform.position, waypoint.position);
				}
				transform = waypoint;
			}
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(truckTarget, 0.3f);
		}

		private void OnEnable()
		{
			GameEvents.Subscribe<FTTGameStartedEvent>(onGameStart);
			GameEvents.Subscribe<FTTGameEndedEvent>(onGameEnd);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<FTTGameStartedEvent>(onGameStart);
			GameEvents.Unsubscribe<FTTGameEndedEvent>(onGameEnd);
		}

		private void Start()
		{
			avatar = UnityEngine.Object.FindObjectOfType<FTTAvatarController>();
		}

		private void Update()
		{
			_003CUpdate_003Ec__AnonStorey16B _003CUpdate_003Ec__AnonStorey16B = new _003CUpdate_003Ec__AnonStorey16B();
			_003CUpdate_003Ec__AnonStorey16B.preferedPoint = avatar.transform.position + avatar.transform.forward * DistanceToPlayer;
			_003CUpdate_003Ec__AnonStorey16B.preferedPoint = Vector3.Scale(_003CUpdate_003Ec__AnonStorey16B.preferedPoint, new Vector3(0f, 0f, 1f));
			if (toPoint == null || Vector3.Dot(toPoint.forward, (toPoint.position - _003CUpdate_003Ec__AnonStorey16B.preferedPoint).normalized) <= 0f)
			{
				toPoint = waypoints.OrderBy(_003CUpdate_003Ec__AnonStorey16B._003C_003Em__6A).FirstOrDefault(_003CUpdate_003Ec__AnonStorey16B._003C_003Em__6B);
			}
			if (!(toPoint == null))
			{
				fromPoint = ((waypoints.IndexOf(toPoint) <= 0) ? base.transform : waypoints[waypoints.IndexOf(toPoint) - 1]);
				float num = Vector3.Angle(Vector3.forward, (toPoint.position - fromPoint.position).normalized);
				Vector3 b = new Vector3(fromPoint.position.x, fromPoint.position.y, fromPoint.position.z + (_003CUpdate_003Ec__AnonStorey16B.preferedPoint - fromPoint.position).z);
				float num2 = Vector3.Distance(fromPoint.position, b) / Mathf.Cos(num * ((float)Math.PI / 180f));
				float t = num2 / Vector3.Distance(fromPoint.position, toPoint.position);
				base.transform.position = Vector3.Lerp(fromPoint.position, toPoint.position, t);
				float num3 = MathUtilities.AngleSigned(base.transform.forward, (toPoint.position - base.transform.position).normalized, Vector3.up);
				float y = Mathf.LerpAngle(base.transform.eulerAngles.y, base.transform.eulerAngles.y + num3, Time.deltaTime);
				base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, y, base.transform.eulerAngles.z);
			}
		}

		private void onGameStart(FTTGameStartedEvent evt)
		{
			AudioController.Play("CTT_truck_drive_loop", base.transform);
			Transform[] array = evt.Settings.GameObject.transform.FindChildrenRecursively("Waypoints");
			Transform[] array2 = array;
			foreach (Transform transform in array2)
			{
				foreach (Transform item in transform)
				{
					waypoints.Add(item);
				}
			}
			waypoints = waypoints.OrderBy(_003ConGameStart_003Em__6C).ToList();
			DistanceToPlayer = evt.Settings.DistanceToTruck;
		}

		private void onGameEnd(FTTGameEndedEvent evt)
		{
			base.enabled = false;
			GetComponent<Collider>().enabled = false;
			GetComponent<Rigidbody>().isKinematic = true;
		}

		[CompilerGenerated]
		private float _003ConGameStart_003Em__6C(Transform wp)
		{
			return Vector3.Distance(wp.position, base.transform.position);
		}
	}
}
