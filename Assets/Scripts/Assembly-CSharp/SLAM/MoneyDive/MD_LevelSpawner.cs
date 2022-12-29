using UnityEngine;

namespace SLAM.MoneyDive
{
	public class MD_LevelSpawner : MonoBehaviour
	{
		[SerializeField]
		private GameObject middlePrefab;

		[SerializeField]
		private GameObject bottomGO;

		[SerializeField]
		private float chunkHeight = 32f;

		private GameObject middle1;

		private GameObject middle2;

		private float yPos;

		private bool lastChunkWasMiddle1;

		public float ChunkHeight
		{
			get
			{
				return chunkHeight;
			}
		}

		public GameObject Bottom
		{
			get
			{
				return bottomGO;
			}
		}

		private void Start()
		{
			bottomGO.SetActive(false);
			middle1 = Object.Instantiate(middlePrefab);
			middle1.name = "Middle1";
			yPos -= chunkHeight;
			middle2 = Object.Instantiate(middlePrefab, new Vector3(0f, yPos, 0f), Quaternion.identity) as GameObject;
			middle2.name = "Middle2";
		}

		private void Update()
		{
		}

		public void StartSpawning(float waitFor, float interval)
		{
			InvokeRepeating("spawn", waitFor, interval);
		}

		public void StopSpawning()
		{
			CancelInvoke("spawn");
			yPos -= chunkHeight;
			bottomGO.SetActive(true);
			bottomGO.transform.position = new Vector3(0f, yPos, 0f);
		}

		private void spawn()
		{
			yPos -= chunkHeight;
			if (lastChunkWasMiddle1)
			{
				middle2.transform.position = new Vector3(0f, yPos, 0f);
			}
			else
			{
				middle1.transform.position = new Vector3(0f, yPos, 0f);
			}
			lastChunkWasMiddle1 = !lastChunkWasMiddle1;
		}
	}
}
