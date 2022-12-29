using System;
using System.Runtime.CompilerServices;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Engine
{
	public class ChallengeView : View
	{
		[SerializeField]
		private UIPagination pagination;

		[SerializeField]
		private UILabel titleLabel;

		[SerializeField]
		private UILabel lblChallengeResult;

		[CompilerGenerated]
		private static Action<GameObject, object> _003C_003Ef__am_0024cache3;

		public void OnCloseClicked()
		{
			Controller<GameController>().CloseChallenge();
		}

		public void SetData(string level)
		{
			titleLabel.text = StringFormatter.GetLocalizationFormatted("UI_WINDOW_TITLE", level);
		}

		public void SetFriends(UserProfile[] friends)
		{
			UIPagination uIPagination = pagination;
			if (_003C_003Ef__am_0024cache3 == null)
			{
				_003C_003Ef__am_0024cache3 = _003CSetFriends_003Em__F4;
			}
			uIPagination.OnItemCreated = _003C_003Ef__am_0024cache3;
			pagination.UpdateInfo(friends);
		}

		public void OnFriendChallenged(UserProfile profile)
		{
			string key = "SF_CHALLENGE_SEND";
			lblChallengeResult.text = string.Format(Localization.Get(key), profile.Name, Controller<GameController>().TotalScore);
			ChallengeFriendRow[] componentsInChildren = GetComponentsInChildren<ChallengeFriendRow>();
			ChallengeFriendRow[] array = componentsInChildren;
			foreach (ChallengeFriendRow challengeFriendRow in array)
			{
				challengeFriendRow.GetComponentInChildren<UIButton>().gameObject.SetActive(false);
			}
		}

		[CompilerGenerated]
		private static void _003CSetFriends_003Em__F4(GameObject go, object obj)
		{
			if (go.activeInHierarchy)
			{
				go.GetComponentInChildren<ChallengeFriendRow>().SetInfo(obj as UserProfile);
			}
		}
	}
}
