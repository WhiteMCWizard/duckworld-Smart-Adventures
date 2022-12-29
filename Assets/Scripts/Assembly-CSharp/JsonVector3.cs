using System;
using UnityEngine;

[Serializable]
public struct JsonVector3
{
	public float x;

	public float y;

	public float z;

	public static implicit operator JsonVector3(Vector3 v3)
	{
		JsonVector3 result = default(JsonVector3);
		result.x = v3.x;
		result.y = v3.y;
		result.z = v3.z;
		return result;
	}

	public static implicit operator Vector3(JsonVector3 jv3)
	{
		return new Vector3(jv3.x, jv3.y, jv3.z);
	}
}
