using System;
using UnityEngine;

[Serializable]
public struct JsonColor
{
	public float r;

	public float g;

	public float b;

	public static implicit operator JsonColor(Color col)
	{
		JsonColor result = default(JsonColor);
		result.r = col.r;
		result.g = col.g;
		result.b = col.b;
		return result;
	}

	public static implicit operator Color(JsonColor jcol)
	{
		return new Color(jcol.r, jcol.g, jcol.b);
	}
}
