using UnityEngine;

public class DestroyOnPlatform : MonoBehaviour
{
	public enum Platform
	{
		Web = 0,
		Mobile = 1,
		Standalone = 2,
		Steam = 3
	}

	[SerializeField]
	private Platform platform;

	private void Awake()
	{
		if (platform == Platform.Standalone)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
