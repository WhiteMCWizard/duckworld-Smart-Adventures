using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
	[ExecuteInEditMode]
	public abstract class CinemaActorEvent : TimelineItem
	{
		public ActorTrackGroup ActorTrackGroup
		{
			get
			{
				return base.TimelineTrack.TrackGroup as ActorTrackGroup;
			}
		}

		public abstract void Trigger(GameObject Actor);

		public virtual void Reverse(GameObject Actor)
		{
		}

		public virtual void SetTimeTo(float deltaTime)
		{
		}

		public virtual void Pause()
		{
		}

		public virtual void Resume()
		{
		}

		public virtual void Initialize(GameObject Actor)
		{
		}

		public virtual void Stop(GameObject Actor)
		{
		}

		public virtual List<Transform> GetActors()
		{
			IMultiActorTrack multiActorTrack = base.TimelineTrack as IMultiActorTrack;
			if (multiActorTrack != null)
			{
				return multiActorTrack.Actors;
			}
			return null;
		}
	}
}
