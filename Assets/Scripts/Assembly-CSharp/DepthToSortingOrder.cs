using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

public class DepthToSortingOrder : MonoBehaviour
{
	[SerializeField]
	private string objectPrefix = "Sprites_";

	private void Awake()
	{
		foreach (Transform item in GetComponentsInChildren<Transform>(true).Where(_003CAwake_003Em__194))
		{
			item.GetComponent<Renderer>().sortingOrder = (int)(0f - item.position.z);
		}
	}

	[CompilerGenerated]
	private bool _003CAwake_003Em__194(Transform t)
	{
		return t.name.StartsWith(objectPrefix);
	}
}
