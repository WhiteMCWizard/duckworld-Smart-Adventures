using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Smartphone
{
	public class SmartphoneController : ViewController
	{
		[SerializeField]
		private View[] views;

		[SerializeField]
		private AppController[] apps;

		private AppController currentOpenApp;

		private List<AppController> appsWithNotifications = new List<AppController>();

		private bool isVisible;

		private Animator phoneAnimator;

		[CompilerGenerated]
		private static Comparison<AppController> _003C_003Ef__am_0024cache6;

		public int TotalNotifications
		{
			get
			{
				int num = 0;
				for (int i = 0; i < appsWithNotifications.Count; i++)
				{
					num += appsWithNotifications[i].NotificationCount;
				}
				return num;
			}
		}

		public bool IsVisible
		{
			get
			{
				return isVisible;
			}
			private set
			{
				if (value != isVisible)
				{
					isVisible = value;
					SmartphoneVisibilityChangedEvent smartphoneVisibilityChangedEvent = new SmartphoneVisibilityChangedEvent();
					smartphoneVisibilityChangedEvent.Smartphone = this;
					smartphoneVisibilityChangedEvent.IsVisible = isVisible;
					GameEvents.Invoke(smartphoneVisibilityChangedEvent);
				}
			}
		}

		protected override void Start()
		{
			base.Start();
			phoneAnimator = GetComponent<Animator>();
			AddViews(views);
			for (int i = 0; i < apps.Length; i++)
			{
				apps[i].SetData(this);
			}
			GetView<SpringboardView>().CreateApps(apps);
			OpenView<NotificationCenterView>();
		}

		private void OnEnable()
		{
			GameEvents.Subscribe<AppChangedEvent>(onAppNotification);
			GameEvents.Subscribe<OpenAppRequestEvent>(handleOpenRequest);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<AppChangedEvent>(onAppNotification);
			GameEvents.Unsubscribe<OpenAppRequestEvent>(handleOpenRequest);
		}

		public void Show()
		{
			CloseView<NotificationCenterView>();
			OpenView<SmartphoneView>().SetInfo(string.Empty, GetView<SpringboardView>().Style);
			OpenView<SpringboardView>();
			OpenView<SmartphoneSurroundingView>().SetData(true);
			IsVisible = true;
			AudioController.Play("Interface_phone_open");
			phoneAnimator.SetBool("Visible", true);
		}

		public void Hide()
		{
			if (currentOpenApp != null)
			{
				currentOpenApp.Close();
				currentOpenApp = null;
			}
			if (IsViewOpen<SmartphoneSurroundingView>())
			{
				CloseView<SmartphoneSurroundingView>();
			}
			if (IsViewOpen<SmartphoneView>())
			{
				CloseView<SmartphoneView>(_003CHide_003Em__18D);
			}
			IsVisible = false;
			AudioController.Play("Interface_phone_close");
			phoneAnimator.SetBool("Visible", false);
		}

		public void GoHome()
		{
			if (currentOpenApp == null || (currentOpenApp != null && !currentOpenApp.DisablesPhone))
			{
				if (currentOpenApp != null)
				{
					currentOpenApp.Close();
					currentOpenApp = null;
				}
				else
				{
					Hide();
				}
				if (!IsViewOpen<SpringboardView>())
				{
					OpenView<SpringboardView>();
					GetView<SmartphoneView>().SetInfo(string.Empty, GetView<SpringboardView>().Style);
				}
			}
		}

		public void GoBack()
		{
			if (!(currentOpenApp == null) && (!(currentOpenApp != null) || currentOpenApp.DisablesPhone))
			{
				return;
			}
			if (currentOpenApp != null)
			{
				if (!currentOpenApp.Back())
				{
					GoHome();
				}
			}
			else
			{
				Hide();
			}
		}

		public void ShowNotificationCenter()
		{
			OpenView<NotificationCenterView>();
		}

		public void HideNotificationCenter()
		{
			CloseView<NotificationCenterView>();
		}

		public void ActivatePhoneCall()
		{
			if (IsViewOpen<NotificationCenterView>())
			{
				CloseView<NotificationCenterView>();
			}
			if (!IsViewOpen<SmartphoneView>())
			{
				OpenView<SmartphoneView>().SetInfo(string.Empty, GetView<SpringboardView>().Style);
			}
			if (IsViewOpen<SmartphoneSurroundingView>())
			{
				GetView<SmartphoneSurroundingView>().SetData(false);
			}
			IsVisible = true;
			AppController[] array = apps;
			foreach (AppController appController in array)
			{
				if (appController is PhoneCallApp)
				{
					OpenAppRequestEvent openAppRequestEvent = new OpenAppRequestEvent();
					openAppRequestEvent.App = appController;
					GameEvents.Invoke(openAppRequestEvent);
				}
			}
		}

		public void OpenApp<T>() where T : AppController
		{
			OpenAppRequestEvent openAppRequestEvent = new OpenAppRequestEvent();
			openAppRequestEvent.App = apps.FirstOrDefault(_003COpenApp_00601_003Em__18E<T>);
			GameEvents.Invoke(openAppRequestEvent);
		}

		private void handleOpenRequest(OpenAppRequestEvent evt)
		{
			if (evt.App.IsPremium && UserProfile.Current.IsFree)
			{
				Hide();
				GameEvents.Invoke(new PopupEvent(Localization.Get("UI_SMARTPHONE_POPUP_LOCKED_TITLE"), Localization.Get("UI_SMARTPHONE_POPUP_LOCKED_DESCRIPTION"), Localization.Get("UI_YES"), Localization.Get("UI_NO"), _003ChandleOpenRequest_003Em__18F, _003ChandleOpenRequest_003Em__190));
				return;
			}
			if (currentOpenApp != null)
			{
				currentOpenApp.Close();
			}
			if (IsViewOpen<SpringboardView>())
			{
				CloseView<SpringboardView>();
			}
			evt.App.Open();
			currentOpenApp = evt.App;
			GetView<SmartphoneView>().SetInfo(currentOpenApp);
		}

		private void onAppNotification(AppChangedEvent evt)
		{
			if (evt.App.NotificationCount > 0)
			{
				rememberNotification(evt.App);
			}
			else
			{
				forgetNotification(evt.App);
			}
			GetView<NotificationCenterView>().SetData(TotalNotifications);
		}

		private void rememberNotification(AppController app)
		{
			if (!appsWithNotifications.Contains(app))
			{
				appsWithNotifications.Add(app);
				List<AppController> list = appsWithNotifications;
				if (_003C_003Ef__am_0024cache6 == null)
				{
					_003C_003Ef__am_0024cache6 = _003CrememberNotification_003Em__191;
				}
				list.Sort(_003C_003Ef__am_0024cache6);
			}
		}

		private void forgetNotification(AppController app)
		{
			if (appsWithNotifications.Contains(app))
			{
				appsWithNotifications.Remove(app);
			}
		}

		[CompilerGenerated]
		private void _003CHide_003Em__18D(View v)
		{
			OpenView<NotificationCenterView>();
			if (IsViewOpen<SpringboardView>())
			{
				CloseView<SpringboardView>();
			}
		}

		[CompilerGenerated]
		private static bool _003COpenApp_00601_003Em__18E<T>(AppController a) where T : AppController
		{
			return a is T;
		}

		[CompilerGenerated]
		private void _003ChandleOpenRequest_003Em__18F()
		{
			ApiClient.OpenPropositionPage(0);
			Show();
		}

		[CompilerGenerated]
		private void _003ChandleOpenRequest_003Em__190()
		{
			Show();
		}

		[CompilerGenerated]
		private static int _003CrememberNotification_003Em__191(AppController a, AppController b)
		{
			return b.Priority.CompareTo(a.Priority);
		}
	}
}
