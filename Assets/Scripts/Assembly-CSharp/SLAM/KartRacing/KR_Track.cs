using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.KartRacing
{
	public class KR_Track
	{
		[CompilerGenerated]
		private sealed class _003CGetNearestWayPoint_003Ec__AnonStorey174
		{
			internal Transform t;

			internal float _003C_003Em__9F(KR_Waypoint w)
			{
				return Vector3.Distance(t.position, w.Center.position);
			}
		}

		public List<KR_Route> Routes = new List<KR_Route>();

		public KR_Track(Transform trackRoot)
		{
			Dictionary<int, List<Transform>> dictionary = new Dictionary<int, List<Transform>>();
			for (int i = 0; i < trackRoot.childCount; i++)
			{
				int result = 0;
				string name = trackRoot.GetChild(i).name;
				int startIndex = name.IndexOf("p") + 1;
				int length = name.IndexOf("_") - 2;
				if (!int.TryParse(name.Substring(startIndex, length), out result))
				{
					result = -1;
				}
				if (!dictionary.ContainsKey(result))
				{
					dictionary.Add(result, new List<Transform>());
				}
				dictionary[result].Add(trackRoot.GetChild(i));
			}
			foreach (KeyValuePair<int, List<Transform>> item2 in dictionary)
			{
				KR_Route item = new KR_Route(item2.Value, this);
				Routes.Add(item);
			}
			foreach (KR_Route route in Routes)
			{
				foreach (KR_Path path in route.Paths)
				{
					foreach (KR_Waypoint waypoint in path.Waypoints)
					{
						KR_Waypoint previousWayPoint = waypoint.Path.GetPreviousWayPoint(waypoint);
						KR_Waypoint nextWayPoint = waypoint.Path.GetNextWayPoint(waypoint);
						if (previousWayPoint != null)
						{
							waypoint.Neighbours.Add(previousWayPoint);
						}
						else
						{
							foreach (KR_Path path2 in GetPreviousRoute(waypoint.Path.Route).Paths)
							{
								if (path2.Route != waypoint.Path.Route)
								{
									waypoint.Neighbours.Add(path2.Waypoints.Last());
								}
							}
						}
						if (nextWayPoint != null)
						{
							waypoint.Neighbours.Add(nextWayPoint);
							continue;
						}
						foreach (KR_Path path3 in GetNextRoute(waypoint.Path.Route).Paths)
						{
							if (path3.Route != waypoint.Path.Route)
							{
								waypoint.Neighbours.Add(path3.Waypoints.First());
							}
						}
					}
				}
			}
		}

		public float GetProgress(Transform t, KR_Waypoint lastKnowWayPoint = null)
		{
			KR_Waypoint nearestWayPoint = GetNearestWayPoint(t, lastKnowWayPoint);
			KR_Waypoint previousWayPoint = nearestWayPoint.Path.GetPreviousWayPoint(nearestWayPoint);
			KR_Waypoint kR_Waypoint = ((previousWayPoint == null) ? nearestWayPoint : previousWayPoint);
			Vector3 closetPoint = MathUtilities.GetClosetPoint(kR_Waypoint.Right.position, kR_Waypoint.Left.position, t.position, true);
			float num = Vector3.Distance(t.position, closetPoint);
			float num2 = GetDistanceToStart(nearestWayPoint) + num;
			float num3 = GetDistanceToFinish(nearestWayPoint) - num;
			return num2 / (num2 + num3);
		}

		public float GetDistanceToStart(KR_Waypoint wayPoint)
		{
			float num = 0f;
			KR_Waypoint kR_Waypoint = wayPoint;
			KR_Waypoint previousWayPoint = kR_Waypoint.Path.GetPreviousWayPoint(kR_Waypoint);
			KR_Path previousPath = wayPoint.Path.Route.GetPreviousPath(kR_Waypoint.Path);
			while (previousWayPoint != null)
			{
				num += Vector3.Distance(kR_Waypoint.Center.position, previousWayPoint.Center.position);
				kR_Waypoint = previousWayPoint;
				previousWayPoint = previousWayPoint.Path.GetPreviousWayPoint(previousWayPoint);
			}
			while (previousPath != null)
			{
				num += previousPath.Length;
				previousPath = previousPath.Route.GetPreviousPath(previousPath);
			}
			return num;
		}

		public float GetDistanceToFinish(KR_Waypoint wayPoint)
		{
			float num = 0f;
			KR_Waypoint kR_Waypoint = wayPoint;
			KR_Waypoint nextWayPoint = kR_Waypoint.Path.GetNextWayPoint(kR_Waypoint);
			KR_Path nextPath = wayPoint.Path.Route.GetNextPath(kR_Waypoint.Path);
			while (nextWayPoint != null)
			{
				num += Vector3.Distance(kR_Waypoint.Center.position, nextWayPoint.Center.position);
				kR_Waypoint = nextWayPoint;
				nextWayPoint = nextWayPoint.Path.GetNextWayPoint(nextWayPoint);
			}
			while (nextPath != null)
			{
				num += nextPath.Length;
				nextPath = nextPath.Route.GetNextPath(nextPath);
			}
			return num;
		}

		public KR_Route GetNextRoute(KR_Route currentRoute)
		{
			if (Routes.Last() == currentRoute)
			{
				return currentRoute;
			}
			return Routes.ElementAt(Routes.IndexOf(currentRoute) + 1);
		}

		public KR_Route GetPreviousRoute(KR_Route currentRoute)
		{
			if (Routes.First() == currentRoute)
			{
				return currentRoute;
			}
			return Routes.ElementAt(Routes.IndexOf(currentRoute) - 1);
		}

		public KR_Waypoint GetNearestWayPoint(Transform t, KR_Waypoint lastKnownWayPoint = null)
		{
			_003CGetNearestWayPoint_003Ec__AnonStorey174 _003CGetNearestWayPoint_003Ec__AnonStorey = new _003CGetNearestWayPoint_003Ec__AnonStorey174();
			_003CGetNearestWayPoint_003Ec__AnonStorey.t = t;
			if (lastKnownWayPoint != null)
			{
				List<KR_Waypoint> neighbours = lastKnownWayPoint.Neighbours;
				neighbours.Add(lastKnownWayPoint);
				return neighbours.OrderBy(_003CGetNearestWayPoint_003Ec__AnonStorey._003C_003Em__9F).First();
			}
			int index = 0;
			int index2 = 0;
			int index3 = 0;
			float num = float.PositiveInfinity;
			for (int i = 0; i < Routes.Count; i++)
			{
				for (int j = 0; j < Routes[i].Paths.Count; j++)
				{
					for (int k = 0; k < Routes[i].Paths[j].Waypoints.Count; k++)
					{
						float num2 = Vector3.Distance(_003CGetNearestWayPoint_003Ec__AnonStorey.t.position, Routes[i].Paths[j].Waypoints[k].Center.position);
						if (num2 < num)
						{
							num = num2;
							index = i;
							index2 = j;
							index3 = k;
						}
					}
				}
			}
			return Routes[index].Paths[index2].Waypoints[index3];
		}
	}
}
