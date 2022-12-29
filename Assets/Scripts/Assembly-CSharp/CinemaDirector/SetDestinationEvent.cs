using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Navigation", "Set Destination", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
	public class SetDestinationEvent : CinemaActorEvent
	{
		public Vector3 target;

		public override void Trigger(GameObject actor)
		{
			UnityEngine.AI.NavMeshAgent component = actor.GetComponent<UnityEngine.AI.NavMeshAgent>();
			if (component != null)
			{
				component.SetDestination(target);
			}
		}
	}
}
