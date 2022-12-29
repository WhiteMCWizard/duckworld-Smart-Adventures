using UnityEngine;

namespace SLAM.Shared
{
	public class UIAudio : MonoBehaviour
	{
		public enum InterfaceSounds
		{
			Interface_buttonClick_primary = 0,
			Interface_buttonClick_secundary = 1,
			Interface_buttonClick_alt = 2,
			Interface_buttonClick_switch = 3,
			Interface_buttonClick_change = 4,
			Interface_buttonClick_arrow = 5,
			Interface_window_close = 6,
			Interface_window_open = 7,
			Interface_buttonClick_phone = 8
		}

		[SerializeField]
		private InterfaceSounds onClickSound;

		private bool wasOn;

		private void OnClick()
		{
			playSound(onClickSound.ToString());
		}

		private void OnHover(bool hoverOn)
		{
			if (!wasOn && hoverOn && onClickSound == InterfaceSounds.Interface_buttonClick_primary)
			{
				playSound("Interface_button_mouseOver");
			}
			wasOn = hoverOn;
		}

		private void playSound(string id)
		{
			if (!string.IsNullOrEmpty(id) && (bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
			{
				AudioController.Play(id);
			}
		}
	}
}
