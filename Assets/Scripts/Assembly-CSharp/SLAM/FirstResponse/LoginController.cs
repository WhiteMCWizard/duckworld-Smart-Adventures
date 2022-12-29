using System.Runtime.CompilerServices;
using SLAM.BuildSystem;
using SLAM.Engine;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.FirstResponse
{
	public class LoginController : ViewController
	{
		[CompilerGenerated]
		private sealed class _003CLogin_003Ec__AnonStorey169
		{
			internal string username;

			internal LoginController _003C_003Ef__this;

			internal void _003C_003Em__5E(bool result)
			{
				if (result)
				{
					UserProfile.GetCurrentProfileData(_003C_003Em__62);
					return;
				}
				_003C_003Ef__this.GetView<LoginView>().LoginButtonEnabled = true;
				_003C_003Ef__this.GetView<LoginView>().ShowFeedback("UI_LOGIN_INVALID_CREDENTIALS");
			}

			internal void _003C_003Em__62(UserProfile profile)
			{
				if (profile != null)
				{
					PlayerPrefs.SetString("login_username", username);
					SceneManager.Load("FirstResponse");
				}
			}
		}

		[SerializeField]
		private View[] views;

		private void Awake()
		{
			AddViews(views);
		}

		protected override void Start()
		{
			base.Start();
			OpenView<LoginView>().DemoButtonEnabled = false;
			UpdateSystem.HasLatestVersion(_003CStart_003Em__5D);
		}

		private void OnEnable()
		{
			GameEvents.Subscribe<Webservice.WebserviceErrorEvent>(onError);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<Webservice.WebserviceErrorEvent>(onError);
		}

		private void onError(Webservice.WebserviceErrorEvent evt)
		{
			WebResponse.WebError error = evt.Response.Error;
			GetView<LoginView>().ShowFeedback("UI_LOGIN_CONNECTION_PROBLEMS");
			if (error != null && error.StatusCode == 401)
			{
				if (error.Detail.Contains("User is banned"))
				{
					GetView<LoginView>().ShowFeedback("UI_LOGIN_BANNED");
				}
				else if (error.Detail.Contains("Invalid or not existing license"))
				{
					GetView<LoginView>().ShowFeedback("UI_LOGIN_LICENSE_EXPIRED");
				}
			}
		}

		public void Login(string username, string password)
		{
			_003CLogin_003Ec__AnonStorey169 _003CLogin_003Ec__AnonStorey = new _003CLogin_003Ec__AnonStorey169();
			_003CLogin_003Ec__AnonStorey.username = username;
			_003CLogin_003Ec__AnonStorey._003C_003Ef__this = this;
			ApiClient.Authenticate(_003CLogin_003Ec__AnonStorey.username, password, _003CLogin_003Ec__AnonStorey._003C_003Em__5E);
		}

		public void FreePlay()
		{
			ApiClient.GetFreeUserToken(_003CFreePlay_003Em__5F);
		}

		public void TipParentClicked()
		{
			if (!GetView<TipParentView>().IsOpen)
			{
				OpenView<TipParentView>();
			}
		}

		[CompilerGenerated]
		private void _003CStart_003Em__5D(bool hasLatest)
		{
			if (!hasLatest)
			{
				UpdateSystem.UpdateToLatestVersion();
			}
			DataStorage.GetWebConfiguration(_003CStart_003Em__60);
		}

		[CompilerGenerated]
		private void _003CFreePlay_003Em__5F(string token)
		{
			UserProfile.GetCurrentProfileData(_003CFreePlay_003Em__61);
		}

		[CompilerGenerated]
		private void _003CStart_003Em__60(WebConfiguration config)
		{
			GetView<LoginView>().DemoButtonEnabled = config.FreeplayerEnabled;
		}

		[CompilerGenerated]
		private void _003CFreePlay_003Em__61(UserProfile profile)
		{
			GetView<LoginView>().DemoButtonEnabled = true;
			SceneManager.Load("FirstResponse");
		}
	}
}
