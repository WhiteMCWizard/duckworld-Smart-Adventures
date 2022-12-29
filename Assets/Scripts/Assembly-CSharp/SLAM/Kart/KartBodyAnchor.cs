using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Kart
{
	public class KartBodyAnchor : MonoBehaviour
	{
		[Serializable]
		private struct KartAnchorPoint
		{
			public KartSystem.ItemCategory Category;

			public Transform Anchor;
		}

		[CompilerGenerated]
		private sealed class _003CGetAnchors_003Ec__AnonStorey17E
		{
			internal KartSystem.ItemCategory category;

			internal bool _003C_003Em__CE(KartAnchorPoint p)
			{
				return p.Category == category;
			}
		}

		[SerializeField]
		private KartAnchorPoint[] anchorPoints;

		[CompilerGenerated]
		private static Func<KartAnchorPoint, Transform> _003C_003Ef__am_0024cache1;

		public IEnumerable<Transform> GetAnchors(KartSystem.ItemCategory category)
		{
			_003CGetAnchors_003Ec__AnonStorey17E _003CGetAnchors_003Ec__AnonStorey17E = new _003CGetAnchors_003Ec__AnonStorey17E();
			_003CGetAnchors_003Ec__AnonStorey17E.category = category;
			IEnumerable<KartAnchorPoint> collection = anchorPoints.Where(_003CGetAnchors_003Ec__AnonStorey17E._003C_003Em__CE);
			if (_003C_003Ef__am_0024cache1 == null)
			{
				_003C_003Ef__am_0024cache1 = _003CGetAnchors_003Em__CF;
			}
			return collection.Select(_003C_003Ef__am_0024cache1);
		}

		[CompilerGenerated]
		private static Transform _003CGetAnchors_003Em__CF(KartAnchorPoint p)
		{
			return p.Anchor;
		}
	}
}
