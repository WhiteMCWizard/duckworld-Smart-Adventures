using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.TranslateThis
{
	public class TT_TutorialView : TutorialView
	{
		[CompilerGenerated]
		private sealed class _003CpickKey_003Ec__AnonStorey17A
		{
			internal char c;

			internal bool _003C_003Em__C1(TT_Key k)
			{
				return k.c == c;
			}
		}

		[SerializeField]
		private ToolTip mouseClickToolTip;

		private TT_Key[] keys;

		private void OnEnable()
		{
			GameEvents.Subscribe<TT_GameController.WordPickedEvent>(onWordPicked);
			GameEvents.Subscribe<TT_GameController.LetterGuessedEvent>(onLetterGuessed);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<TT_GameController.WordPickedEvent>(onWordPicked);
			GameEvents.Unsubscribe<TT_GameController.LetterGuessedEvent>(onLetterGuessed);
		}

		private void pickKey(char c)
		{
			_003CpickKey_003Ec__AnonStorey17A _003CpickKey_003Ec__AnonStorey17A = new _003CpickKey_003Ec__AnonStorey17A();
			_003CpickKey_003Ec__AnonStorey17A.c = c;
			if (keys == null)
			{
				keys = Object.FindObjectsOfType<TT_Key>();
			}
			mouseClickToolTip.Hide();
			TT_Key tT_Key = keys.FirstOrDefault(_003CpickKey_003Ec__AnonStorey17A._003C_003Em__C1);
			if (tT_Key != null && tT_Key.GetComponent<Collider>().enabled)
			{
				mouseClickToolTip.Show(tT_Key.transform);
			}
		}

		private void onWordPicked(TT_GameController.WordPickedEvent evt)
		{
			keys = null;
		}

		private void onLetterGuessed(TT_GameController.LetterGuessedEvent evt)
		{
			pickKey(evt.letter);
		}
	}
}
