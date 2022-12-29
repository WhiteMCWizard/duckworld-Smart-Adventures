using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Avatar;
using SLAM.BuildSystem;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.Webservices.ErrorReporting
{
	public class WebErrorReporter : MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class _003ConWebserviceError_003Ec__AnonStorey19D
		{
			internal Webservice.WebserviceErrorEvent evt;

			internal WebErrorReporter _003C_003Ef__this;

			internal void _003C_003Em__117(AsyncOperation callback)
			{
				_003C_003Ef__this.isHandlingError = false;
				_003C_003Ef__this.onWebserviceError(evt);
			}
		}

		private bool isHandlingError;

		private List<int> panicErrorCodes = new List<int> { 401, 408, 418 };

		[CompilerGenerated]
		private static Action<AsyncOperation> _003C_003Ef__am_0024cache2;

		private void OnEnable()
		{
			GameEvents.Subscribe<Webservice.WebserviceErrorEvent>(onWebserviceError);
			GameEvents.Subscribe<Webservice.LogoutEvent>(onLogoutEvent);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<Webservice.WebserviceErrorEvent>(onWebserviceError);
			GameEvents.Unsubscribe<Webservice.LogoutEvent>(onLogoutEvent);
		}

		private void onWebserviceError(Webservice.WebserviceErrorEvent evt)
		{
			_003ConWebserviceError_003Ec__AnonStorey19D _003ConWebserviceError_003Ec__AnonStorey19D = new _003ConWebserviceError_003Ec__AnonStorey19D();
			_003ConWebserviceError_003Ec__AnonStorey19D.evt = evt;
			_003ConWebserviceError_003Ec__AnonStorey19D._003C_003Ef__this = this;
			Debug.LogErrorFormat("Webservice error occurred \n url:{0} \n errorcode:{1} \n error: {2} \n text: {3} \n \n wwwerror: {4} connected: {5}", _003ConWebserviceError_003Ec__AnonStorey19D.evt.Response.Request.url, _003ConWebserviceError_003Ec__AnonStorey19D.evt.Response.StatusCode, _003ConWebserviceError_003Ec__AnonStorey19D.evt.Response.ReasonPhrase, _003ConWebserviceError_003Ec__AnonStorey19D.evt.Response.Request.text, _003ConWebserviceError_003Ec__AnonStorey19D.evt.Response.Request.error, _003ConWebserviceError_003Ec__AnonStorey19D.evt.Response.Connected);
			if ((_003ConWebserviceError_003Ec__AnonStorey19D.evt.Response.StatusCode < 500 && !panicErrorCodes.Contains(_003ConWebserviceError_003Ec__AnonStorey19D.evt.Response.StatusCode) && _003ConWebserviceError_003Ec__AnonStorey19D.evt.Response.StatusCode != -1) || (panicErrorCodes.Contains(_003ConWebserviceError_003Ec__AnonStorey19D.evt.Response.StatusCode) && Application.loadedLevelName == "Login") || isHandlingError)
			{
				return;
			}
			isHandlingError = true;
			if (Application.loadedLevelName != "Hub" && _003ConWebserviceError_003Ec__AnonStorey19D.evt.Response.StatusCode == 418)
			{
				if (_003C_003Ef__am_0024cache2 == null)
				{
					_003C_003Ef__am_0024cache2 = _003ConWebserviceError_003Em__116;
				}
				SceneManager.Load("Hub", _003C_003Ef__am_0024cache2);
			}
			else if (Application.loadedLevelName != "Login")
			{
				Webservice.LogoutEvent logoutEvent = new Webservice.LogoutEvent();
				logoutEvent.LoginLoadedCallback = _003ConWebserviceError_003Ec__AnonStorey19D._003C_003Em__117;
				GameEvents.Invoke(logoutEvent);
			}
			else
			{
				string key = string.Format("ERROR_{0}_TITLE", _003ConWebserviceError_003Ec__AnonStorey19D.evt.Response.StatusCode);
				string key2 = string.Format("ERROR_{0}_TEXT", _003ConWebserviceError_003Ec__AnonStorey19D.evt.Response.StatusCode);
				string title = ((!Localization.Exists(key)) ? Localization.Get("ERROR_GENERIC_TITLE") : Localization.Get(key));
				string message = ((!Localization.Exists(key2)) ? Localization.Get("ERROR_GENERIC_TEXT") : Localization.Get(key2));
				GameEvents.Invoke(new PopupEvent(title, message, Localization.Get("UI_OK"), null));
				isHandlingError = false;
			}
		}

		private void onLogoutEvent(Webservice.LogoutEvent evt)
		{
			ApiClient.Logout();
			UserProfile.UnsetCurrentProfileData();
			AvatarSystem.UnsetPlayerConfiguration();
			DataStorage.DeleteAll();
			SingletonMonobehaviour<Webservice>.Instance.ReceiveToken(null, string.Empty);
			SceneManager.Load("Login", evt.LoginLoadedCallback);
		}

		[CompilerGenerated]
		private static void _003ConWebserviceError_003Em__116(AsyncOperation op)
		{
			GameEvents.Invoke(new Webservice.TrialEndedEvent());
		}
	}
}
