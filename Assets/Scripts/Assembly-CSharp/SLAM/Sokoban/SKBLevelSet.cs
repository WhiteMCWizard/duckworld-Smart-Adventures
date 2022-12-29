using UnityEngine;

namespace SLAM.Sokoban
{
	public class SKBLevelSet : ScriptableObject
	{
		[SerializeField]
		private GameObject[] levelPrefabs;

		[SerializeField]
		private float duration;

		public GameObject[] LevelPrefabs
		{
			get
			{
				return levelPrefabs;
			}
		}

		public float Duration
		{
			get
			{
				return duration;
			}
		}
	}
}
