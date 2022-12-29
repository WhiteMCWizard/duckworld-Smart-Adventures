using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Analytics;
using SLAM.BuildSystem;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Smartphone
{
	public class MessagingApp : AppController
	{
		[CompilerGenerated]
		private sealed class _003CCloseFriendRequest_003Ec__AnonStorey1B2
		{
			internal Message message;

			internal void _003C_003Em__178(bool s)
			{
				DataStorage.GetFriends(null, true);
				GameEvents.Invoke(new TrackingEvent
				{
					Type = TrackingEvent.TrackingType.FriendshipAccepted,
					Arguments = new Dictionary<string, object>
					{
						{
							"Sender",
							message.Sender.Id
						},
						{
							"Recipient",
							ApiClient.UserId
						}
					}
				});
			}
		}

		[CompilerGenerated]
		private sealed class _003CcheckForNotifications_003Ec__AnonStorey1B3
		{
			internal Action<AppChangedEvent> eventCallback;

			internal MessagingApp _003C_003Ef__this;

			private static Func<Message, bool> _003C_003Ef__am_0024cache2;

			private static Func<Message, bool> _003C_003Ef__am_0024cache3;

			internal void _003C_003Em__179(Message[] messages)
			{
				MessagingApp messagingApp = _003C_003Ef__this;
				if (_003C_003Ef__am_0024cache2 == null)
				{
					_003C_003Ef__am_0024cache2 = _003C_003Em__180;
				}
				messagingApp.unreadMessages = messages.Where(_003C_003Ef__am_0024cache2).ToList();
				MessagingApp messagingApp2 = _003C_003Ef__this;
				if (_003C_003Ef__am_0024cache3 == null)
				{
					_003C_003Ef__am_0024cache3 = _003C_003Em__181;
				}
				messagingApp2.allMessages = messages.Where(_003C_003Ef__am_0024cache3).ToList();
				if (_003C_003Ef__this.unreadMessages.Count > 0)
				{
					eventCallback(new AppChangedEvent
					{
						App = _003C_003Ef__this
					});
				}
			}

			private static bool _003C_003Em__180(Message m)
			{
				return !m.Archived && m.Type != Message.MessageType.JobNotification;
			}

			private static bool _003C_003Em__181(Message m)
			{
				return m.Type != Message.MessageType.JobNotification;
			}
		}

		[CompilerGenerated]
		private sealed class _003CarchiveMessage_003Ec__AnonStorey1B4
		{
			internal Message message;

			internal bool _003C_003Em__17A(Message m)
			{
				return m.Id == message.Id;
			}
		}

		[CompilerGenerated]
		private sealed class _003CopenChallenge_003Ec__AnonStorey1B5
		{
			internal Message m;

			internal string title;

			internal string body;

			internal MessagingApp _003C_003Ef__this;

			internal void _003C_003Em__17B(UserGameDetails[] ugd)
			{
				if (_003C_003Ef__this.IsViewOpen<InboxItemView>())
				{
					if (ugd.Any(_003C_003Em__182))
					{
						_003C_003Ef__this.GetView<InboxItemView>().SetData(title, body, m, _003C_003Em__183, _003C_003Em__184);
						return;
					}
					body = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_BODY_CHALLENGE_LOCKED", m.Sender.Name, Localization.Get(m.Game.Name), m.Difficulty + 1);
					_003C_003Ef__this.GetView<InboxItemView>().SetData(title, body, m, null, null);
				}
			}

			internal bool _003C_003Em__182(UserGameDetails g)
			{
				return g.GameId == m.Game.Id && g.Progression.Any(_003C_003Em__185);
			}

			internal void _003C_003Em__183()
			{
				_003C_003Ef__this.CloseChallengeRequest(m, true);
			}

			internal void _003C_003Em__184()
			{
				_003C_003Ef__this.CloseChallengeRequest(m, false);
			}

			internal bool _003C_003Em__185(UserGameProgression prg)
			{
				return prg.LevelIndex == m.Difficulty;
			}
		}

		[CompilerGenerated]
		private sealed class _003CopenFriendRequest_003Ec__AnonStorey1B6
		{
			internal Message m;

			internal MessagingApp _003C_003Ef__this;

			internal void _003C_003Em__17C()
			{
				_003C_003Ef__this.CloseFriendRequest(m, true);
			}

			internal void _003C_003Em__17D()
			{
				_003C_003Ef__this.CloseFriendRequest(m, false);
			}
		}

		[CompilerGenerated]
		private sealed class _003CopenUrlMessage_003Ec__AnonStorey1B7
		{
			internal Message message;

			internal void _003C_003Em__17E()
			{
				Application.OpenURL(message.Url);
			}
		}

		private List<Message> allMessages = new List<Message>();

		private List<Message> unreadMessages = new List<Message>();

		[CompilerGenerated]
		private static Func<Message, string> _003C_003Ef__am_0024cache2;

		[CompilerGenerated]
		private static Func<string, bool> _003C_003Ef__am_0024cache3;

		[CompilerGenerated]
		private static Action _003C_003Ef__am_0024cache4;

		public override int NotificationCount
		{
			get
			{
				base.NotificationCount = unreadMessages.Count;
				return base.NotificationCount;
			}
			protected set
			{
				base.NotificationCount = value;
			}
		}

		protected override void Start()
		{
			base.Start();
			InvokeRepeating("refreshNotifications", 0f, 10f);
		}

		private void OnEnable()
		{
			GameEvents.Subscribe<OpenMessageEvent>(onOpenMessageRequest);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<OpenMessageEvent>(onOpenMessageRequest);
		}

		public override void Open()
		{
			OpenTempView<LoadingView>();
			Action callback = onMessagesAndMugshotsRecieved;
			List<Message> collection = allMessages;
			if (_003C_003Ef__am_0024cache2 == null)
			{
				_003C_003Ef__am_0024cache2 = _003COpen_003Em__176;
			}
			IEnumerable<string> collection2 = collection.Select(_003C_003Ef__am_0024cache2);
			if (_003C_003Ef__am_0024cache3 == null)
			{
				_003C_003Ef__am_0024cache3 = _003COpen_003Em__177;
			}
			Webservice.WaitFor(callback, collection2.Where(_003C_003Ef__am_0024cache3));
		}

		private void onMessagesAndMugshotsRecieved()
		{
			if (IsViewOpen<LoadingView>())
			{
				CloseTempView<LoadingView>();
				OpenView<InboxView>().SetData(allMessages);
			}
		}

		public void CloseFriendRequest(Message message, bool accepted)
		{
			_003CCloseFriendRequest_003Ec__AnonStorey1B2 _003CCloseFriendRequest_003Ec__AnonStorey1B = new _003CCloseFriendRequest_003Ec__AnonStorey1B2();
			_003CCloseFriendRequest_003Ec__AnonStorey1B.message = message;
			if (accepted)
			{
				ApiClient.AcceptFriendRequest(_003CCloseFriendRequest_003Ec__AnonStorey1B.message.Id, _003CCloseFriendRequest_003Ec__AnonStorey1B._003C_003Em__178);
			}
			else
			{
				TrackingEvent trackingEvent = new TrackingEvent();
				trackingEvent.Type = TrackingEvent.TrackingType.FriendshipRejected;
				trackingEvent.Arguments = new Dictionary<string, object>
				{
					{
						"Sender",
						_003CCloseFriendRequest_003Ec__AnonStorey1B.message.Sender.Id
					},
					{
						"Recipient",
						ApiClient.UserId
					}
				};
				GameEvents.Invoke(trackingEvent);
			}
			archiveMessage(_003CCloseFriendRequest_003Ec__AnonStorey1B.message);
		}

		public void CloseChallengeRequest(Message message, bool accepted)
		{
			if (accepted)
			{
				GameController.ChallengeAccepted = message;
				SceneManager.Load(message.Game.SceneName);
				Close();
				return;
			}
			TrackingEvent trackingEvent = new TrackingEvent();
			trackingEvent.Type = TrackingEvent.TrackingType.ChallengeRejected;
			trackingEvent.Arguments = new Dictionary<string, object>
			{
				{
					"Sender",
					message.Sender.Id
				},
				{
					"Recipient",
					ApiClient.UserId
				},
				{
					"GameId",
					message.Game.Id
				},
				{ "Difficulty", message.Difficulty }
			};
			GameEvents.Invoke(trackingEvent);
			ApiClient.DeleteMessage(message.Id, null);
		}

		protected override void checkForNotifications(Action<AppChangedEvent> eventCallback)
		{
			_003CcheckForNotifications_003Ec__AnonStorey1B3 _003CcheckForNotifications_003Ec__AnonStorey1B = new _003CcheckForNotifications_003Ec__AnonStorey1B3();
			_003CcheckForNotifications_003Ec__AnonStorey1B.eventCallback = eventCallback;
			_003CcheckForNotifications_003Ec__AnonStorey1B._003C_003Ef__this = this;
			if (!UserProfile.Current.IsFree)
			{
				ApiClient.GetAllMessages(_003CcheckForNotifications_003Ec__AnonStorey1B._003C_003Em__179);
			}
		}

		private void onOpenMessageRequest(OpenMessageEvent evt)
		{
			switch (evt.Message.Type)
			{
			default:
				openGlobalNotification(evt.Message);
				archiveMessage(evt.Message);
				break;
			case Message.MessageType.Notification:
				openNotification(evt.Message);
				archiveMessage(evt.Message);
				break;
			case Message.MessageType.FriendConfirmed:
				openFriendConfirmed(evt.Message);
				archiveMessage(evt.Message);
				break;
			case Message.MessageType.FriendRequest:
				openFriendRequest(evt.Message);
				break;
			case Message.MessageType.Challenge:
				if (evt.Message.ScoreRecipient == 0)
				{
					openChallenge(evt.Message);
				}
				else
				{
					openChallengeResult(evt.Message);
				}
				archiveMessage(evt.Message);
				break;
			case Message.MessageType.ChallengeResult:
				openChallengeResult(evt.Message);
				archiveMessage(evt.Message);
				break;
			case Message.MessageType.UrlMessage:
				openUrlMessage(evt.Message);
				archiveMessage(evt.Message);
				break;
			}
		}

		private void archiveMessage(Message message)
		{
			_003CarchiveMessage_003Ec__AnonStorey1B4 _003CarchiveMessage_003Ec__AnonStorey1B = new _003CarchiveMessage_003Ec__AnonStorey1B4();
			_003CarchiveMessage_003Ec__AnonStorey1B.message = message;
			if (!_003CarchiveMessage_003Ec__AnonStorey1B.message.Archived && _003CarchiveMessage_003Ec__AnonStorey1B.message.Type != Message.MessageType.JobNotification && _003CarchiveMessage_003Ec__AnonStorey1B.message.Type != 0)
			{
				ApiClient.ArchiveMessage(_003CarchiveMessage_003Ec__AnonStorey1B.message.Id, null);
			}
			_003CarchiveMessage_003Ec__AnonStorey1B.message.Archived = true;
			Message message2 = unreadMessages.FirstOrDefault(_003CarchiveMessage_003Ec__AnonStorey1B._003C_003Em__17A);
			if (message2 != null && unreadMessages.Contains(message2))
			{
				unreadMessages.Remove(message2);
				AppChangedEvent appChangedEvent = new AppChangedEvent();
				appChangedEvent.App = this;
				GameEvents.Invoke(appChangedEvent);
			}
		}

		private void openGlobalNotification(Message m)
		{
			string localizationFormatted = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_GLOBALNOTIFICATION", m.Sender.Name);
			string messageBody = m.MessageBody;
			OpenView<InboxItemView>().SetData(localizationFormatted, messageBody, m, null, null);
		}

		private void openNotification(Message m)
		{
			string localizationFormatted = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_NOTIFICATION", m.Sender.Name);
			string messageBody = m.MessageBody;
			OpenView<InboxItemView>().SetData(localizationFormatted, messageBody, m, null, null);
		}

		private void openChallenge(Message m)
		{
			_003CopenChallenge_003Ec__AnonStorey1B5 _003CopenChallenge_003Ec__AnonStorey1B = new _003CopenChallenge_003Ec__AnonStorey1B5();
			_003CopenChallenge_003Ec__AnonStorey1B.m = m;
			_003CopenChallenge_003Ec__AnonStorey1B._003C_003Ef__this = this;
			_003CopenChallenge_003Ec__AnonStorey1B.title = Localization.Get("SF_MESSAGING_TITLE_CHALLENGE");
			_003CopenChallenge_003Ec__AnonStorey1B.body = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_BODY_CHALLENGE", _003CopenChallenge_003Ec__AnonStorey1B.m.Sender.Name, Localization.Get(_003CopenChallenge_003Ec__AnonStorey1B.m.Game.Name), _003CopenChallenge_003Ec__AnonStorey1B.m.ScoreSender);
			OpenView<InboxItemView>().SetData(_003CopenChallenge_003Ec__AnonStorey1B.title, _003CopenChallenge_003Ec__AnonStorey1B.body, _003CopenChallenge_003Ec__AnonStorey1B.m, null, null);
			DataStorage.GetProgressionData(_003CopenChallenge_003Ec__AnonStorey1B._003C_003Em__17B);
		}

		private void openFriendConfirmed(Message m)
		{
			string localizationFormatted = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_FRIENDCONFIRMED", m.Sender.Name);
			string localizationFormatted2 = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_BODY_FRIENDCONFIRMED", m.Sender.Name);
			OpenView<InboxItemView>().SetData(localizationFormatted, localizationFormatted2, m, null, null);
		}

		private void openChallengeResult(Message m)
		{
			string title = Localization.Get("SF_MESSAGING_TITLE_CHALLENGERESULT");
			string localizationFormatted = StringFormatter.GetLocalizationFormatted((!m.HasRecipientWon()) ? "SF_MESSAGING_BODY_CHALLENGERESULT_LOST" : "SF_MESSAGING_BODY_CHALLENGERESULT_VICTORY", m.Sender.Name, Localization.Get(m.Game.Name), m.ScoreRecipient, m.ScoreSender);
			if (m.WasGameTie())
			{
				localizationFormatted = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_BODY_CHALLENGERESULT_TIE", m.Sender.Name, Localization.Get(m.Game.Name), m.ScoreRecipient);
			}
			OpenView<InboxItemView>().SetData(title, localizationFormatted, m, null, null);
		}

		private void openFriendRequest(Message m)
		{
			_003CopenFriendRequest_003Ec__AnonStorey1B6 _003CopenFriendRequest_003Ec__AnonStorey1B = new _003CopenFriendRequest_003Ec__AnonStorey1B6();
			_003CopenFriendRequest_003Ec__AnonStorey1B.m = m;
			_003CopenFriendRequest_003Ec__AnonStorey1B._003C_003Ef__this = this;
			string localizationFormatted = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_FRIENDREQUEST");
			string localizationFormatted2 = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_BODY_FRIENDREQUEST", _003CopenFriendRequest_003Ec__AnonStorey1B.m.Sender.Name, _003CopenFriendRequest_003Ec__AnonStorey1B.m.Sender.Address);
			if (!_003CopenFriendRequest_003Ec__AnonStorey1B.m.Archived)
			{
				OpenView<InboxItemView>().SetData(localizationFormatted, localizationFormatted2, _003CopenFriendRequest_003Ec__AnonStorey1B.m, _003CopenFriendRequest_003Ec__AnonStorey1B._003C_003Em__17C, _003CopenFriendRequest_003Ec__AnonStorey1B._003C_003Em__17D);
			}
			else
			{
				OpenView<InboxItemView>().SetData(localizationFormatted, localizationFormatted2, _003CopenFriendRequest_003Ec__AnonStorey1B.m, null, null);
			}
		}

		private void openUrlMessage(Message message)
		{
			_003CopenUrlMessage_003Ec__AnonStorey1B7 _003CopenUrlMessage_003Ec__AnonStorey1B = new _003CopenUrlMessage_003Ec__AnonStorey1B7();
			_003CopenUrlMessage_003Ec__AnonStorey1B.message = message;
			string localizationFormatted = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_URLMESSAGE", _003CopenUrlMessage_003Ec__AnonStorey1B.message.Sender.Name);
			string messageBody = _003CopenUrlMessage_003Ec__AnonStorey1B.message.MessageBody;
			InboxItemView inboxItemView = OpenView<InboxItemView>();
			Message message2 = _003CopenUrlMessage_003Ec__AnonStorey1B.message;
			Action yesCallback = _003CopenUrlMessage_003Ec__AnonStorey1B._003C_003Em__17E;
			if (_003C_003Ef__am_0024cache4 == null)
			{
				_003C_003Ef__am_0024cache4 = _003CopenUrlMessage_003Em__17F;
			}
			inboxItemView.SetData(localizationFormatted, messageBody, message2, yesCallback, _003C_003Ef__am_0024cache4);
		}

		[CompilerGenerated]
		private static string _003COpen_003Em__176(Message m)
		{
			return m.Sender.MugShotUrl;
		}

		[CompilerGenerated]
		private static bool _003COpen_003Em__177(string mugshoturl)
		{
			return !string.IsNullOrEmpty(mugshoturl);
		}

		[CompilerGenerated]
		private static void _003CopenUrlMessage_003Em__17F()
		{
		}
	}
}
