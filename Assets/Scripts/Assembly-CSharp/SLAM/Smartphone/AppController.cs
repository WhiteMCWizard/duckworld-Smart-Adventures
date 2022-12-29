using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Smartphone
{
	public abstract class AppController : ViewController
	{
		[SerializeField]
		private AppView[] appViews;

		[SerializeField]
		protected string titleKey = string.Empty;

		[SerializeField]
		protected int priority = -1;

		[SerializeField]
		protected int sortingOrder = -1;

		[SerializeField]
		protected bool hidden;

		[SerializeField]
		protected bool disablesPhone;

		[SerializeField]
		protected string iconSpriteName = "white";

		[SerializeField]
		private bool premiumApp = true;

		[SerializeField]
		private Color style = Color.white;

		private Stack<View> breadcrumbs = new Stack<View>();

		[CompilerGenerated]
		private static Action<AppChangedEvent> _003C_003Ef__am_0024cacheC;

		public int Priority
		{
			get
			{
				return priority;
			}
		}

		public bool Hidden
		{
			get
			{
				return hidden;
			}
		}

		public bool DisablesPhone
		{
			get
			{
				return disablesPhone;
			}
		}

		public string IconSpriteName
		{
			get
			{
				return iconSpriteName;
			}
		}

		public string Title
		{
			get
			{
				return titleKey;
			}
		}

		public int SortingOrder
		{
			get
			{
				return sortingOrder;
			}
		}

		public virtual int NotificationCount { get; protected set; }

		public bool IsPremium
		{
			get
			{
				return premiumApp;
			}
		}

		public Color Style
		{
			get
			{
				return style;
			}
		}

		protected SmartphoneController smartphone { get; private set; }

		protected override void Start()
		{
			base.Start();
			AddViews(appViews);
		}

		protected override void OpenView(View view, View.Callback callback)
		{
			if (breadcrumbs.Count > 0)
			{
				((AppView)breadcrumbs.Peek()).EnterBackground();
			}
			base.OpenView(view, callback);
			breadcrumbs.Push(view);
		}

		protected override void CloseView(View view, View.Callback callback)
		{
			if (!disablesPhone)
			{
				Debug.LogWarning("Hey buddy, please use Back() to close your current view as we need to track the breadcrums.");
			}
		}

		protected T OpenTempView<T>() where T : View
		{
			T view = GetView<T>();
			base.OpenView(view, null);
			return view;
		}

		protected void CloseTempView<T>() where T : View
		{
			base.CloseView(GetView<T>(), null);
		}

		public abstract void Open();

		public virtual void Close()
		{
			while (breadcrumbs.Count > 0)
			{
				base.CloseView(breadcrumbs.Pop(), null);
			}
		}

		public bool Back()
		{
			if (breadcrumbs.Count > 0)
			{
				AppView appView = (AppView)breadcrumbs.Pop();
				appView.OnBackClicked();
				base.CloseView(appView, null);
				if (breadcrumbs.Count > 0)
				{
					((AppView)breadcrumbs.Peek()).ReturnFromBackground();
					return true;
				}
				return false;
			}
			return false;
		}

		public void SetData(SmartphoneController smartphoneController)
		{
			smartphone = smartphoneController;
		}

		public virtual void CreateIcon(UISprite icon)
		{
			icon.spriteName = ((!premiumApp || !UserProfile.Current.IsFree) ? IconSpriteName : (IconSpriteName + "_Premium"));
		}

		protected void refreshNotifications()
		{
			if (_003C_003Ef__am_0024cacheC == null)
			{
				_003C_003Ef__am_0024cacheC = _003CrefreshNotifications_003Em__16F;
			}
			checkForNotifications(_003C_003Ef__am_0024cacheC);
		}

		protected abstract void checkForNotifications(Action<AppChangedEvent> eventCallback);

		[CompilerGenerated]
		private static void _003CrefreshNotifications_003Em__16F(AppChangedEvent e)
		{
			if (e != null)
			{
				GameEvents.Invoke(e);
			}
		}
	}
}
