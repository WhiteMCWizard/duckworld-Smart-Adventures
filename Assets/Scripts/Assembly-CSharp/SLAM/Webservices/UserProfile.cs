using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LitJson;
using UnityEngine;

namespace SLAM.Webservices
{
	[Serializable]
	public class UserProfile
	{
		[CompilerGenerated]
		private sealed class _003CGetCurrentProfileData_003Ec__AnonStorey197
		{
			internal Action<UserProfile> callback;

			internal void _003C_003Em__10C(UserProfile prof)
			{
				if (prof != null)
				{
					Current = prof;
				}
				if (callback != null)
				{
					callback(Current);
				}
			}
		}

		private static Dictionary<string, Texture2D> imageCache = new Dictionary<string, Texture2D>();

		[JsonName("id")]
		public int Id;

		[JsonName("name")]
		public string Name;

		[JsonName("address")]
		public string Address;

		[JsonName("google_analytics")]
		public Dictionary<string, string> CustomDimensions;

		[JsonName("is_free")]
		public bool IsFree;

		[JsonName("is_sa")]
		public bool IsSA;

		private string _mugshotUrl;

		public Texture2D MugShot;

		public static UserProfile Current { get; private set; }

		[JsonName("mugshot")]
		public string MugShotUrl
		{
			get
			{
				return _mugshotUrl;
			}
			set
			{
				_mugshotUrl = value;
				downloadMugshot();
			}
		}

		public string FirstName
		{
			get
			{
				if (Name.IndexOf(' ') > 0)
				{
					return Name.Substring(0, Name.IndexOf(' '));
				}
				return Name;
			}
		}

		public string LastName
		{
			get
			{
				if (Name.Length > 0)
				{
					return Name.Substring(Name.IndexOf(" "));
				}
				return Name;
			}
		}

		public static void GetCurrentProfileData(Action<UserProfile> callback)
		{
			_003CGetCurrentProfileData_003Ec__AnonStorey197 _003CGetCurrentProfileData_003Ec__AnonStorey = new _003CGetCurrentProfileData_003Ec__AnonStorey197();
			_003CGetCurrentProfileData_003Ec__AnonStorey.callback = callback;
			ApiClient.GetUserProfile(_003CGetCurrentProfileData_003Ec__AnonStorey._003C_003Em__10C);
		}

		public static void UnsetCurrentProfileData()
		{
			Current = null;
		}

		private void downloadMugshot()
		{
			if (!string.IsNullOrEmpty(MugShotUrl))
			{
				if (imageCache.ContainsKey(MugShotUrl))
				{
					Webservice.WaitFor(_003CdownloadMugshot_003Em__10D, MugShotUrl);
				}
				else
				{
					imageCache.Add(MugShotUrl, null);
					SingletonMonobehaviour<Webservice>.Instance.DoRequest<WebResponse>("GET", MugShotUrl, _003CdownloadMugshot_003Em__10E);
				}
			}
		}

		public void SetMugShot(Texture2D mugshot)
		{
			MugShot = mugshot;
		}

		[CompilerGenerated]
		private void _003CdownloadMugshot_003Em__10D()
		{
			MugShot = imageCache[MugShotUrl];
		}

		[CompilerGenerated]
		private void _003CdownloadMugshot_003Em__10E(WebResponse response)
		{
			if (response.Request.error == null)
			{
				MugShot = response.Request.textureNonReadable;
				MugShot = new Texture2D(MugShot.width, MugShot.height, TextureFormat.ARGB32, true);
				MugShot.wrapMode = TextureWrapMode.Clamp;
				response.Request.LoadImageIntoTexture(MugShot);
				imageCache[MugShotUrl] = MugShot;
			}
			else
			{
				imageCache[MugShotUrl] = null;
			}
		}
	}
}
