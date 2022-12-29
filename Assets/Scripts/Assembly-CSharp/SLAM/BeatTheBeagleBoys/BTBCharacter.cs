using UnityEngine;

namespace SLAM.BeatTheBeagleBoys
{
	[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
	public class BTBCharacter : MonoBehaviour
	{
		protected Vector3 destination;

		protected UnityEngine.AI.NavMeshAgent navAgent;

		protected BTBArea currentArea { get; private set; }

		protected BTBDifficultySetting settings { get; private set; }

		public bool PositionReached
		{
			get
			{
				return base.enabled && !navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance;
			}
		}

		public virtual void Initialize(BTBArea area, BTBDifficultySetting difficulty)
		{
			currentArea = area;
			settings = difficulty;
			navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		}

		protected void moveToPosition(Vector3 worldPos)
		{
			destination = worldPos;
			navAgent.SetDestination(destination);
		}
	}
}
