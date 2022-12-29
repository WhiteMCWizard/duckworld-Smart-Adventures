using SLAM.Engine;
using UnityEngine;

namespace SLAM.DuckQuiz
{
	public class DQCorrectAnswerView : View
	{
		[SerializeField]
		private UILabel lblScoreCount;

		[SerializeField]
		private UILabel lblScoreCountShadow;

		public void SetInfo(int scoreCount)
		{
			UILabel uILabel = lblScoreCount;
			string text = string.Format("{0} punten", scoreCount);
			lblScoreCountShadow.text = text;
			uILabel.text = text;
		}

		private void OnDisable()
		{
			UITweener[] componentsInChildren = GetComponentsInChildren<UITweener>(true);
			foreach (UITweener uITweener in componentsInChildren)
			{
				uITweener.ResetToBeginning();
				uITweener.enabled = true;
			}
		}
	}
}
