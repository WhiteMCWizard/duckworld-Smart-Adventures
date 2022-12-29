using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Engine
{
	public class TipParentView : View
	{
		[SerializeField]
		private UIInput emailInput;

		[SerializeField]
		private UIButton sendButton;

		public bool SendButtonEnabled
		{
			get
			{
				return sendButton.isEnabled;
			}
			set
			{
				sendButton.isEnabled = value;
			}
		}

		protected override void Update()
		{
			base.Update();
			SendButtonEnabled = emailInput.value.Length > 0 && isValidEmail(emailInput.value);
		}

		public void OnCloseClicked()
		{
			Close();
		}

		public void OnSendClicked()
		{
			ApiClient.TipAParent(emailInput.value, _003COnSendClicked_003Em__F6);
		}

		private bool isValidEmail(string email)
		{
			Regex regex = new Regex("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$");
			Match match = regex.Match(email);
			return match.Success;
		}

		[CompilerGenerated]
		private void _003COnSendClicked_003Em__F6(bool success)
		{
			if (base.IsOpen)
			{
				Close();
			}
		}
	}
}
