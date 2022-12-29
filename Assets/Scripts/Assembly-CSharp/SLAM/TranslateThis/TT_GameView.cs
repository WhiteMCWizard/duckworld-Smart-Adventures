using System.Collections.Generic;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.TranslateThis
{
	public class TT_GameView : View
	{
		[SerializeField]
		private UITexture startView;

		[SerializeField]
		private UIGrid keyGrid;

		[SerializeField]
		private GameObject buttonPrefab;

		[SerializeField]
		private GameObject spotPrefab;

		[SerializeField]
		private Transform spotContainer;

		[SerializeField]
		private UISprite[] beagleBoys;

		[SerializeField]
		private UISprite sadBeagleBoy;

		[SerializeField]
		private UIToggle[] checkBoxes;

		[SerializeField]
		private UILabel wordsLabel;

		[SerializeField]
		private UILabel categoryLabel;

		[SerializeField]
		private UIToggle safe;

		[SerializeField]
		private UILabel lblWordCount;

		[SerializeField]
		private UILabel lblScore;

		[SerializeField]
		private UIProgressBar timer;

		[SerializeField]
		private UIToggle[] correctAnswersInLevel;

		[SerializeField]
		private GameObject answerPlingPrefab;

		[SerializeField]
		private GameObject timePlingPrefab;

		[SerializeField]
		private GameObject boardPanel;

		[SerializeField]
		private UISprite flagSprite;

		[SerializeField]
		private Color correctTextColor = Color.green;

		[SerializeField]
		private Color incorrectTextColor = Color.red;

		private UILabel[] spots;

		private UIButton[] buttons;

		protected override void Start()
		{
			startView.gameObject.SetActive(true);
		}

		public void InitKeys(TT_GameController game)
		{
			if (buttons != null)
			{
				UIButton[] array = buttons;
				foreach (UIButton uIButton in array)
				{
					keyGrid.RemoveChild(uIButton.transform);
					Object.Destroy(uIButton.gameObject);
				}
			}
			buttons = new UIButton[26];
			int num = 97;
			int num2 = 0;
			while (num <= 122)
			{
				char c = (char)num;
				GameObject gameObject = NGUITools.AddChild(keyGrid.gameObject, buttonPrefab);
				gameObject.GetComponentInChildren<UILabel>().text = new string(new char[1] { char.ToUpper(c) });
				TT_Key tT_Key = gameObject.AddComponent<TT_Key>();
				tT_Key.Init(game, c);
				buttons[num2] = gameObject.GetComponent<UIButton>();
				num++;
				num2++;
			}
			keyGrid.Reposition();
		}

		public void InitSpots(string answer)
		{
			startView.gameObject.SetActive(false);
			if (spots != null)
			{
				UILabel[] array = spots;
				foreach (UILabel uILabel in array)
				{
					Object.Destroy(uILabel.gameObject);
				}
			}
			List<UILabel> list = new List<UILabel>();
			float num = 0f;
			float num2 = (float)answer.Length * -19.7f;
			while (num < (float)answer.Length)
			{
				string text = answer.Substring((int)num, 1);
				if (text != " ")
				{
					GameObject gameObject = Object.Instantiate(spotPrefab);
					gameObject.transform.parent = spotContainer;
					gameObject.transform.localPosition = Vector3.right * num2;
					gameObject.transform.localRotation = Quaternion.identity;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.parent = spotContainer.parent;
					UILabel component = gameObject.GetComponent<UILabel>();
					component.text = string.Empty;
					list.Add(component);
				}
				num += 1f;
				num2 += 39.4f;
			}
			spots = list.ToArray();
		}

		public void UpdateSpots(char[] knownLetters)
		{
			for (int i = 0; i < Mathf.Min(knownLetters.Length, spots.Length); i++)
			{
				spots[i].text = knownLetters[i].ToString();
			}
		}

		public void UpdateTimer(float percent)
		{
			timer.value = percent;
		}

		public void SetBeagleBoy(int bb)
		{
			sadBeagleBoy.gameObject.SetActive(false);
			for (int i = 0; i < beagleBoys.Length; i++)
			{
				if (bb == -1)
				{
					if (beagleBoys[i].color.a == 1f)
					{
						sadBeagleBoy.gameObject.SetActive(true);
						sadBeagleBoy.transform.position = beagleBoys[i].transform.position;
						beagleBoys[i].gameObject.SetActive(false);
					}
				}
				else
				{
					beagleBoys[i].gameObject.SetActive(true);
				}
				beagleBoys[i].color = new Color(1f, 1f, 1f, (i != bb) ? 0.25f : 1f);
			}
			bool flag = bb >= 7;
			safe.value = flag;
			beagleBoys[beagleBoys.Length - 1].color = new Color(1f, 1f, 1f, flag ? 1 : 0);
			if (flag)
			{
				beagleBoys[beagleBoys.Length - 2].color = new Color(1f, 1f, 1f, 0f);
			}
		}

		public void SetButtonState(char c, bool correct)
		{
			if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
			{
				int num = c - 97;
				buttons[num].disabledColor = ((!correct) ? Color.red : Color.green);
				buttons[num].isEnabled = false;
				buttons[num].GetComponent<UIWidget>().depth = 10;
				buttons[num].GetComponentInChildren<UILabel>().depth = 11;
				buttons[num].gameObject.AddComponent<Rigidbody>();
				buttons[num].GetComponent<Rigidbody>().AddTorque(buttons[num].transform.forward * (Random.value - 0.5f) * 50f);
			}
		}

		public void SetLevel(int level, int total)
		{
			lblWordCount.text = "Woord: " + level + "/" + total;
		}

		public void SetScore(int score)
		{
			lblScore.text = Localization.Get("TT_HUD_SCORE_LABEL") + " " + score;
		}

		public void SetCategory(string category)
		{
			categoryLabel.text = category;
		}

		public void VictoriousBeagleBoy(int id)
		{
			checkBoxes[id].value = true;
		}

		public void SetWordFinished(bool correct)
		{
			if (correct)
			{
				SetBeagleBoy(-1);
			}
			UILabel[] array = spots;
			foreach (UILabel uILabel in array)
			{
				uILabel.color = ((!correct) ? incorrectTextColor : correctTextColor);
			}
		}

		public void SetFlag(Texture2D flag)
		{
			flagSprite.spriteName = flag.name;
		}
	}
}
