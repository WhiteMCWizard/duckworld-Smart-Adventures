using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using LitJson;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Webservices
{
	public class WebRequest
	{
		public static class MultiPartDataParser
		{
			public class MultiPartData
			{
				public Dictionary<string, string> Headers { get; protected set; }

				public Dictionary<string, string> Data { get; protected set; }

				public MultiPartData(Dictionary<string, string> headers, Dictionary<string, string> data)
				{
					Headers = headers;
					Data = data;
				}
			}

			public static IEnumerable<MultiPartData> ParseMultipartData(WWWForm form)
			{
				string boundary = getBoundaryString(form.headers["Content-Type"].ToString());
				string postData = Encoding.UTF8.GetString(form.data);
				int startIndex = 0;
				while (true)
				{
					int endIndex = postData.IndexOf(boundary, startIndex);
					if (endIndex < 0)
					{
						break;
					}
					string[] data = postData.Substring(startIndex, endIndex - startIndex).Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
					startIndex = endIndex + boundary.Length + 2;
					yield return getMultipartData(data);
				}
			}

			private static MultiPartData getMultipartData(string[] lines)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
				int num = -1;
				while (++num < lines.Length)
				{
					string text = lines[num].Trim();
					if (string.IsNullOrEmpty(text))
					{
						break;
					}
					int num2 = text.IndexOf(':');
					dictionary.Add(text.Substring(0, num2).Trim(), text.Substring(num2 + 1).Trim());
				}
				string key = ((!dictionary.ContainsKey("Content-disposition")) ? "Unkown" : getDispositionName(dictionary["Content-disposition"]));
				Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
				dictionary3.Add(key, string.Join(string.Empty, lines, num, lines.Length - num));
				dictionary2 = dictionary3;
				return new MultiPartData(dictionary, dictionary2);
			}

			private static string getBoundaryString(string input)
			{
				string pattern = "boundary=\"(.+)\"$";
				return "--" + Regex.Matches(input, pattern)[0].Groups[1].Value;
			}

			private static string getDispositionName(string input)
			{
				string pattern = "name=\"(.+?)\"";
				return Regex.Matches(input, pattern)[0].Groups[1].Value;
			}
		}

		private const string SHARED_SECRET = "c4Ix3dvt0wYPunsoU*#Kkb^YJh";

		[CompilerGenerated]
		private static Func<string, string> _003C_003Ef__am_0024cache5;

		[CompilerGenerated]
		private static Func<MultiPartDataParser.MultiPartData, bool> _003C_003Ef__am_0024cache6;

		[CompilerGenerated]
		private static Func<MultiPartDataParser.MultiPartData, IEnumerable<KeyValuePair<string, string>>> _003C_003Ef__am_0024cache7;

		public static float LastRequestTime { get; protected set; }

		public string Url { get; protected set; }

		public string Method { get; protected set; }

		public WebResponse Response { get; protected set; }

		public bool IsDone
		{
			get
			{
				return Response != null && Response.Request.isDone;
			}
		}

		public object Result { get; protected set; }

		public static WebRequest Start<T>(string method, string url, string hash, WWWForm form, Action<T> callback)
		{
			WebRequest webRequest = new WebRequest();
			webRequest.Url = url;
			webRequest.Method = method;
			WebRequest webRequest2 = webRequest;
			StaticCoroutine.Start(executeRequest(webRequest2, method, url, hash, form, callback));
			return webRequest2;
		}

		private static IEnumerator executeRequest<T>(WebRequest request, string method, string url, string hash, WWWForm form, Action<T> callback)
		{
			LastRequestTime = Time.realtimeSinceStartup;
			Dictionary<string, string> headers = new Dictionary<string, string>();
			WWW www;
			if (!url.StartsWith(ApiClient.API_URL) && method == "GET" && form.data.Length <= 0)
			{
				www = new WWW(url);
			}
			else
			{
				form.AddField("hashfield", hash);
				headers.Add("Content-Type", form.headers["Content-Type"].ToString());
				headers.Add("X-HTTP-Method-Override", method);
				headers.Add("Accept", "application/json");
				if (SingletonMonobehaviour<Webservice>.Instance.HasAuthenticationToken())
				{
					headers.Add("Authorization", "Token " + SingletonMonobehaviour<Webservice>.Instance.AuthToken);
				}
				www = new WWW(url, form.data, headers);
			}
			yield return StaticCoroutine.Start(waitForWWWOrTimeout(www, 20f));
			WebResponse webResponse = (request.Response = new WebResponse(www, method));
			if (www.error != null || !www.isDone)
			{
				Debug.LogError(method + "  request to " + www.url + " went wrong! " + webResponse.StatusCode + " Exception: " + www.error + "\n" + getCurlCommand(www, form, headers));
			}
			if (webResponse.StatusCode == 401)
			{
				SingletonMonobehaviour<Webservice>.Instance.ReceiveToken(string.Empty, string.Empty);
			}
			if (webResponse.StatusCode < 200 || webResponse.StatusCode >= 300 || !webResponse.Connected)
			{
				Webservice.WebserviceErrorEvent webserviceErrorEvent = new Webservice.WebserviceErrorEvent();
				webserviceErrorEvent.Response = webResponse;
				GameEvents.Invoke(webserviceErrorEvent);
			}
			T result = default(T);
			if (typeof(T) == typeof(WebResponse))
			{
				result = (T)(object)webResponse;
			}
			else if (www.error == null)
			{
				try
				{
					result = (T)Convert.ChangeType(www.text, typeof(T));
				}
				catch (InvalidCastException)
				{
					try
					{
						result = JsonMapper.ToObject<T>(www.text);
					}
					catch (JsonException ex2)
					{
						JsonException ex = ex2;
						Debug.LogError("Couldnt parse " + method + " request to " + url + " went wrong!\nData" + Encoding.UTF8.GetString(form.data) + "\nResponse: " + www.text);
						throw ex;
					}
				}
			}
			if (callback != null)
			{
				callback(result);
			}
			request.Result = result;
		}

		private static IEnumerator waitForWWWOrTimeout(WWW www, float timeout)
		{
			float timer = 0f;
			while (!www.isDone)
			{
				float num;
				timer = (num = timer + Time.unscaledDeltaTime);
				if (num > timeout)
				{
					break;
				}
				yield return null;
			}
			if (timer >= timeout)
			{
				Debug.LogError("Request to " + www.url + " timed out after " + timeout + " seconds");
			}
		}

		private static string getCurlCommand(WWW www, WWWForm form, Dictionary<string, string> headers)
		{
			string text = "curl -v";
			foreach (KeyValuePair<string, string> header in headers)
			{
				string text2 = text;
				text = text2 + " -H \"" + header.Key + ": " + header.Value + "\"";
			}
			text = text + " --data \"" + Encoding.UTF8.GetString(form.data) + "\"";
			return text + " -ssl1 \"" + www.url + "\"";
		}

		public static string CalculateHash(WWWForm form)
		{
			SortedDictionary<string, string> fields = getFields(form);
			string text = "c4Ix3dvt0wYPunsoU*#Kkb^YJh" + '.';
			string text2 = text;
			SortedDictionary<string, string>.ValueCollection values = fields.Values;
			if (_003C_003Ef__am_0024cache5 == null)
			{
				_003C_003Ef__am_0024cache5 = _003CCalculateHash_003Em__135;
			}
			text = text2 + string.Join(".", values.Select(_003C_003Ef__am_0024cache5).ToArray());
			return MD5(text);
		}

		private static SortedDictionary<string, string> getFields(WWWForm form)
		{
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			string @string = Encoding.UTF8.GetString(form.data);
			if (form.headers.ContainsKey("Content-Type") && form.headers["Content-Type"].ToString().StartsWith("multipart/form-data"))
			{
				IEnumerable<MultiPartDataParser.MultiPartData> collection = MultiPartDataParser.ParseMultipartData(form);
				if (_003C_003Ef__am_0024cache6 == null)
				{
					_003C_003Ef__am_0024cache6 = _003CgetFields_003Em__136;
				}
				IEnumerable<MultiPartDataParser.MultiPartData> source = collection.Where(_003C_003Ef__am_0024cache6);
				if (_003C_003Ef__am_0024cache7 == null)
				{
					_003C_003Ef__am_0024cache7 = _003CgetFields_003Em__137;
				}
				{
					foreach (KeyValuePair<string, string> item in source.SelectMany(_003C_003Ef__am_0024cache7))
					{
						sortedDictionary.Add(item.Key, item.Value);
					}
					return sortedDictionary;
				}
			}
			if (!string.IsNullOrEmpty(@string))
			{
				string[] array = @string.Split('=', '&');
				for (int i = 0; i < array.Length; i += 2)
				{
					sortedDictionary.Add(array[i] + i.ToString("0000"), WWW.UnEscapeURL(array[i + 1]));
				}
			}
			return sortedDictionary;
		}

		public static string MD5(string strToEncrypt)
		{
			UTF8Encoding uTF8Encoding = new UTF8Encoding();
			byte[] bytes = uTF8Encoding.GetBytes(strToEncrypt);
			return MD5(bytes);
		}

		public static string MD5(byte[] bytes)
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes);
			string text = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				text += Convert.ToString(array[i], 16).PadLeft(2, '0');
			}
			return text.PadLeft(32, '0');
		}

		[CompilerGenerated]
		private static string _003CCalculateHash_003Em__135(string t)
		{
			return t.Trim();
		}

		[CompilerGenerated]
		private static bool _003CgetFields_003Em__136(MultiPartDataParser.MultiPartData mp)
		{
			return mp.Headers.ContainsKey("Content-Type") && mp.Headers["Content-Type"].Contains("text/plain");
		}

		[CompilerGenerated]
		private static IEnumerable<KeyValuePair<string, string>> _003CgetFields_003Em__137(MultiPartDataParser.MultiPartData mp)
		{
			return mp.Data;
		}
	}
}
