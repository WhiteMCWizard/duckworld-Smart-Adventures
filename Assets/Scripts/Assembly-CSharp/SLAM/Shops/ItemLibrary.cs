using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Shops
{
	public abstract class ItemLibrary<T> : ScriptableObject where T : Item
	{
		[CompilerGenerated]
		private sealed class _003CGetItemByGUID_003Ec__AnonStorey17B
		{
			internal string guid;

			internal bool _003C_003Em__C3(T i)
			{
				return i.GUID == guid;
			}
		}

		[SerializeField]
		protected List<T> items = new List<T>();

		public List<T> Items
		{
			get
			{
				return items;
			}
		}

		public T GetItemByGUID(string guid)
		{
			_003CGetItemByGUID_003Ec__AnonStorey17B _003CGetItemByGUID_003Ec__AnonStorey17B = new _003CGetItemByGUID_003Ec__AnonStorey17B();
			_003CGetItemByGUID_003Ec__AnonStorey17B.guid = guid;
			return items.FirstOrDefault(_003CGetItemByGUID_003Ec__AnonStorey17B._003C_003Em__C3);
		}
	}
}
