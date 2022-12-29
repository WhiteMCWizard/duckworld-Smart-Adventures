using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using LitJson;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Webservices
{
	public class Webservice : SingletonMonobehaviour<Webservice>
	{
		public class WebserviceErrorEvent
		{
			public WebResponse Response;
		}

		public class LogoutEvent
		{
			public Action<AsyncOperation> LoginLoadedCallback;
		}

		public class TrialEndedEvent
		{
		}

		[CompilerGenerated]
		private sealed class _003CdoWaitFor_003Ec__Iterator131 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal IEnumerable<string> methods;

			internal string[] _003Cmethds_003E__0;

			internal int _003Cindex_003E__1;

			internal IEnumerable<string> urls;

			internal IEnumerator<string> _003C_0024s_502_003E__2;

			internal string _003Curl_003E__3;

			internal WebRequest _003ClastRequestWithThisUrl_003E__4;

			internal Action callback;

			internal int _0024PC;

			internal object _0024current;

			internal IEnumerable<string> _003C_0024_003Emethods;

			internal IEnumerable<string> _003C_0024_003Eurls;

			internal Action _003C_0024_003Ecallback;

			internal Webservice _003C_003Ef__this;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return _0024current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return _0024current;
				}
			}

			public bool MoveNext()
			{
				//Discarded unreachable code: IL_015e
				uint num = (uint)_0024PC;
				_0024PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					_003Cmethds_003E__0 = ((methods == null) ? null : methods.ToArray());
					_003Cindex_003E__1 = 0;
					_003C_0024s_502_003E__2 = urls.GetEnumerator();
					num = 4294967293u;
					goto case 1u;
				case 1u:
					try
					{
						switch (num)
						{
						case 1u:
							if (!_003ClastRequestWithThisUrl_003E__4.IsDone)
							{
								_0024current = null;
								_0024PC = 1;
								flag = true;
								goto IL_015c;
							}
							_003Cindex_003E__1++;
							goto default;
						default:
							while (_003C_0024s_502_003E__2.MoveNext())
							{
								_003Curl_003E__3 = _003C_0024s_502_003E__2.Current;
								_003ClastRequestWithThisUrl_003E__4 = _003C_003Ef__this.requestHistory.LastOrDefault(_003C_003Em__13D);
								if (_003ClastRequestWithThisUrl_003E__4 == null)
								{
									_003Cindex_003E__1++;
									UnityEngine.Debug.LogWarning("Couldnt find a request to wait for with url: " + _003Curl_003E__3);
									continue;
								}
								goto case 1u;
							}
							break;
						}
					}
					finally
					{
						if (!flag && _003C_0024s_502_003E__2 != null)
						{
							_003C_0024s_502_003E__2.Dispose();
						}
					}
					if (callback != null)
					{
						callback();
					}
					_0024PC = -1;
					goto default;
				default:
					{
						return false;
					}
					IL_015c:
					return true;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
						break;
					}
					finally
					{
						if (_003C_0024s_502_003E__2 != null)
						{
							_003C_0024s_502_003E__2.Dispose();
						}
					}
				case 0u:
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			internal bool _003C_003Em__13D(WebRequest req)
			{
				return req.Url == _003Curl_003E__3 && (_003Cmethds_003E__0 == null || _003Cmethds_003E__0[_003Cindex_003E__1] == req.Method);
			}
		}

		private int keepaliveInterval = 180;

		private List<WebRequest> requestHistory = new List<WebRequest>();

		[CompilerGenerated]
		private static Func<WebRequest, string> _003C_003Ef__am_0024cache4;

		[CompilerGenerated]
		private static Func<WebRequest, string> _003C_003Ef__am_0024cache5;

		[CompilerGenerated]
		private static Func<WebRequest, string> _003C_003Ef__am_0024cache6;

		[CompilerGenerated]
		private static Func<WebRequest, string> _003C_003Ef__am_0024cache7;

		public string AuthToken { get; protected set; }

		public string SessionID { get; protected set; }

		protected override void Awake()
		{
			base.Awake();
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		private void Start()
		{
			authenticate();
		}

		private void Update()
		{
			if (UserProfile.Current != null && WebRequest.LastRequestTime + (float)keepaliveInterval < Time.realtimeSinceStartup)
			{
				ApiClient.GetUserId(null);
			}
		}

		protected void authenticate()
		{
			AuthToken = PlayerPrefs.GetString("auth_token", string.Empty);
		}

		public void ReceiveToken(string token, string sessionId = "", bool persistent = true)
		{
			AuthToken = token;
			SessionID = sessionId;
			if (!string.IsNullOrEmpty(token))
			{
				DataStorage.GetWebConfiguration(_003CReceiveToken_003Em__138);
			}
			if (persistent)
			{
				PlayerPrefs.SetString("auth_token", token);
				PlayerPrefs.SetString("session_id", SessionID);
			}
			else
			{
				PlayerPrefs.DeleteKey("auth_token");
				PlayerPrefs.DeleteKey("session_id");
			}
			if (persistent)
			{
				PlayerPrefs.SetString("auth_token", token);
			}
			else
			{
				PlayerPrefs.DeleteKey("auth_token");
			}
			PlayerPrefs.Save();
		}

		public bool HasAuthenticationToken()
		{
			return !string.IsNullOrEmpty(AuthToken);
		}

		public WebRequest DoRequest<T>(string method, string url, Action<T> callback)
		{
			return DoRequest(method, url, new WWWForm(), callback);
		}

		public WebRequest DoRequest<T>(string method, string url, Dictionary<string, object> data, Action<T> callback)
		{
			WWWForm wWWForm = new WWWForm();
			if (data != null)
			{
				foreach (KeyValuePair<string, object> datum in data)
				{
					if (datum.Value.GetType().IsArray)
					{
						foreach (object item in (Array)datum.Value)
						{
							wWWForm.AddField(datum.Key, item.ToString());
						}
					}
					else if (datum.Value is int)
					{
						wWWForm.AddField(datum.Key, (int)datum.Value);
					}
					else if (datum.Value is string)
					{
						wWWForm.AddField(datum.Key, (string)datum.Value);
					}
					else
					{
						wWWForm.AddField(datum.Key, JsonMapper.ToJson(datum.Value));
					}
				}
			}
			return DoRequest(method, url, wWWForm, callback);
		}

		public WebRequest DoRequest<T>(string method, string url, WWWForm form, Action<T> callback)
		{
			return DoRequest(method, url, WebRequest.CalculateHash(form), form, callback);
		}

		public WebRequest DoRequest<T>(string method, string url, string hash, WWWForm form, Action<T> callback)
		{
			WebRequest webRequest = WebRequest.Start(method, url, hash, form, callback);
			requestHistory.Add(webRequest);
			return webRequest;
		}

		public static void WaitFor(Action callback, params string[] requests)
		{
			WaitFor(callback, new List<string>(requests));
		}

		public static void WaitFor(Action callback, IEnumerable<WebRequest> requests)
		{
			if (_003C_003Ef__am_0024cache4 == null)
			{
				_003C_003Ef__am_0024cache4 = _003CWaitFor_003Em__139;
			}
			IEnumerable<string> urls = requests.Select(_003C_003Ef__am_0024cache4);
			if (_003C_003Ef__am_0024cache5 == null)
			{
				_003C_003Ef__am_0024cache5 = _003CWaitFor_003Em__13A;
			}
			WaitFor(callback, urls, requests.Select(_003C_003Ef__am_0024cache5));
		}

		public static void WaitFor(Action callback, params WebRequest[] requests)
		{
			if (_003C_003Ef__am_0024cache6 == null)
			{
				_003C_003Ef__am_0024cache6 = _003CWaitFor_003Em__13B;
			}
			IEnumerable<string> urls = requests.Select(_003C_003Ef__am_0024cache6);
			if (_003C_003Ef__am_0024cache7 == null)
			{
				_003C_003Ef__am_0024cache7 = _003CWaitFor_003Em__13C;
			}
			WaitFor(callback, urls, requests.Select(_003C_003Ef__am_0024cache7));
		}

		public static void WaitFor(Action callback, IEnumerable<string> urls, IEnumerable<string> methods = null)
		{
			SingletonMonobehaviour<Webservice>.Instance.waitFor(callback, urls, methods);
		}

		private void waitFor(Action callback, IEnumerable<string> urls, IEnumerable<string> methods)
		{
			StartCoroutine(doWaitFor(callback, urls, methods));
		}

		private IEnumerator doWaitFor(Action callback, IEnumerable<string> urls, IEnumerable<string> methods)
		{
			string[] methds = ((methods == null) ? null : methods.ToArray());
			int index = 0;
			foreach (string url in urls)
			{
				WebRequest lastRequestWithThisUrl = requestHistory.LastOrDefault(((_003CdoWaitFor_003Ec__Iterator131)(object)this)._003C_003Em__13D);
				if (lastRequestWithThisUrl == null)
				{
					index++;
					UnityEngine.Debug.LogWarning("Couldnt find a request to wait for with url: " + url);
					continue;
				}
				while (!lastRequestWithThisUrl.IsDone)
				{
					yield return null;
				}
				index++;
			}
			if (callback != null)
			{
				callback();
			}
		}

		[CompilerGenerated]
		private void _003CReceiveToken_003Em__138(WebConfiguration c)
		{
			keepaliveInterval = c.KeepaliveInterval;
		}

		[CompilerGenerated]
		private static string _003CWaitFor_003Em__139(WebRequest r)
		{
			return r.Url;
		}

		[CompilerGenerated]
		private static string _003CWaitFor_003Em__13A(WebRequest r)
		{
			return r.Method;
		}

		[CompilerGenerated]
		private static string _003CWaitFor_003Em__13B(WebRequest r)
		{
			return r.Url;
		}

		[CompilerGenerated]
		private static string _003CWaitFor_003Em__13C(WebRequest r)
		{
			return r.Method;
		}
	}
}
