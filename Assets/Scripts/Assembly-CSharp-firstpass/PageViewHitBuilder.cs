using UnityEngine;

public class PageViewHitBuilder : HitBuilder<PageViewHitBuilder>
{
	private string screenName = string.Empty;

	public string GetScreenName()
	{
		return screenName;
	}

	public PageViewHitBuilder SetScreenName(string screenName)
	{
		if (screenName != null)
		{
			this.screenName = screenName;
		}
		return this;
	}

	public override PageViewHitBuilder GetThis()
	{
		return this;
	}

	public override PageViewHitBuilder Validate()
	{
		if (string.IsNullOrEmpty(screenName))
		{
			Debug.Log("No screen name provided - Page View hit cannot be sent.");
			return null;
		}
		return this;
	}
}
