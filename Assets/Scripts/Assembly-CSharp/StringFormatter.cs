using System;
using UnityEngine;

public static class StringFormatter
{
	public static string GetLocalizationFormatted(string key, params object[] args)
	{
		string result = key;
		try
		{
			result = string.Format(Localization.Get(key), args);
			return result;
		}
		catch
		{
			Debug.LogError("LocalisationKey: " + key + " required more then the supllied " + args.Length + " args!");
			return result;
		}
	}

	public static string GetFormattedDate(DateTime date)
	{
		return string.Format("{0:" + Localization.Get("UI_DATE_FORMAT") + "}", date);
	}

	public static string GetFormattedTime(float elapsedSeconds, bool miliseconds)
	{
		if (miliseconds)
		{
			return string.Format("{0:00}:{1:00}:{2:00}", (int)elapsedSeconds / 60, (int)elapsedSeconds % 60, (elapsedSeconds - (float)(int)elapsedSeconds) * 100f);
		}
		return string.Format("{0:00}:{1:00}", (int)elapsedSeconds / 60, (int)elapsedSeconds % 60);
	}

	public static string GetFormattedTime(float elapsedSeconds)
	{
		return GetFormattedTime(elapsedSeconds, false);
	}

	public static string GetFormattedTimeFromMiliseconds(float elapsedMiliseconds)
	{
		return GetFormattedTime(elapsedMiliseconds / 1000f, true);
	}
}
