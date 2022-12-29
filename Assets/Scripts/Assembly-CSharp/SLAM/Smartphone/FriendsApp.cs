using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using SLAM.Webservices;

namespace SLAM.Smartphone
{
	public class FriendsApp : AppController
	{
		[CompilerGenerated]
		private sealed class _003CSearch_003Ec__AnonStorey1B1
		{
			private sealed class _003CSearch_003Ec__AnonStorey1B0
			{
				internal List<int> friendsPlusMe;

				internal IEnumerable<UserProfile> notFriends;

				internal _003CSearch_003Ec__AnonStorey1B1 _003C_003Ef__ref_0024433;

				internal bool _003C_003Em__173(UserProfile profile)
				{
					return !friendsPlusMe.Contains(profile.Id);
				}

				internal void _003C_003Em__174()
				{
					_003C_003Ef__ref_0024433.callback(notFriends.ToArray());
				}
			}

			internal Action<UserProfile[]> callback;

			internal FriendsApp _003C_003Ef__this;

			private static Func<UserProfile, int> _003C_003Ef__am_0024cache2;

			private static Func<UserProfile, string> _003C_003Ef__am_0024cache3;

			internal void _003C_003Em__171(UserProfile[] results)
			{
				_003CSearch_003Ec__AnonStorey1B0 _003CSearch_003Ec__AnonStorey1B = new _003CSearch_003Ec__AnonStorey1B0();
				_003CSearch_003Ec__AnonStorey1B._003C_003Ef__ref_0024433 = this;
				UserProfile[] friends = _003C_003Ef__this.friends;
				if (_003C_003Ef__am_0024cache2 == null)
				{
					_003C_003Ef__am_0024cache2 = _003C_003Em__172;
				}
				IEnumerable<int> collection = friends.Select(_003C_003Ef__am_0024cache2);
				_003CSearch_003Ec__AnonStorey1B.friendsPlusMe = new List<int>(collection);
				_003CSearch_003Ec__AnonStorey1B.friendsPlusMe.Add(UserProfile.Current.Id);
				_003CSearch_003Ec__AnonStorey1B.notFriends = results.Where(_003CSearch_003Ec__AnonStorey1B._003C_003Em__173);
				Action action = _003CSearch_003Ec__AnonStorey1B._003C_003Em__174;
				IEnumerable<UserProfile> notFriends = _003CSearch_003Ec__AnonStorey1B.notFriends;
				if (_003C_003Ef__am_0024cache3 == null)
				{
					_003C_003Ef__am_0024cache3 = _003C_003Em__175;
				}
				Webservice.WaitFor(action, notFriends.Select(_003C_003Ef__am_0024cache3));
			}

			private static int _003C_003Em__172(UserProfile f)
			{
				return f.Id;
			}

			private static string _003C_003Em__175(UserProfile p)
			{
				return p.MugShotUrl;
			}
		}

		private UserProfile[] friends;

		protected override void Start()
		{
			base.Start();
			refreshNotifications();
		}

		private void OnEnable()
		{
			GameEvents.Subscribe<InviteUserEvent>(onInviteUser);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<InviteUserEvent>(onInviteUser);
		}

		public override void Open()
		{
			OpenTempView<LoadingView>();
			DataStorage.GetFriends(_003COpen_003Em__170);
		}

		public void Search(string name, Action<UserProfile[]> callback)
		{
			_003CSearch_003Ec__AnonStorey1B1 _003CSearch_003Ec__AnonStorey1B = new _003CSearch_003Ec__AnonStorey1B1();
			_003CSearch_003Ec__AnonStorey1B.callback = callback;
			_003CSearch_003Ec__AnonStorey1B._003C_003Ef__this = this;
			ApiClient.SearchPlayerByName(name, _003CSearch_003Ec__AnonStorey1B._003C_003Em__171);
		}

		private void onInviteUser(InviteUserEvent evt)
		{
			ApiClient.SendFriendRequest(evt.User.Id, null);
		}

		protected override void checkForNotifications(Action<AppChangedEvent> eventCallback)
		{
		}

		internal void ShowProfile(UserProfile userProfile)
		{
			OpenView<ProfileView>().SetData(userProfile);
		}

		[CompilerGenerated]
		private void _003COpen_003Em__170(UserProfile[] friends)
		{
			this.friends = friends;
			CloseTempView<LoadingView>();
			OpenView<FriendsView>().SetData(this.friends);
		}
	}
}
