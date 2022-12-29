using UnityEngine;

namespace SLAM.DuckQuiz
{
	public class DQAnswerButton : MonoBehaviour
	{
		[SerializeField]
		private GameObject sprtCorrect;

		[SerializeField]
		private GameObject sprtIncorrect;

		[SerializeField]
		private UILabel lblAnswerText;

		public DQAnswer Answer { get; protected set; }

		public void SetInfo(DQAnswer answer)
		{
			base.gameObject.SetActive(true);
			GetComponent<Collider>().enabled = true;
			Answer = answer;
			lblAnswerText.text = Answer.Text;
			Color color = GetComponent<UISprite>().color;
			color.a = 1f;
			GetComponent<UISprite>().color = color;
		}

		public void SetCorrect()
		{
			sprtCorrect.SetActive(true);
		}

		public void SetIncorrect()
		{
			sprtIncorrect.SetActive(true);
		}

		private void OnDisable()
		{
			UITweener[] componentsInChildren = GetComponentsInChildren<UITweener>(true);
			foreach (UITweener uITweener in componentsInChildren)
			{
				uITweener.ResetToBeginning();
				uITweener.enabled = true;
			}
			sprtCorrect.SetActive(false);
			sprtIncorrect.SetActive(false);
			GetComponent<Collider>().enabled = true;
		}

		public void DisableInteraction()
		{
			GetComponent<Collider>().enabled = false;
		}
	}
}
