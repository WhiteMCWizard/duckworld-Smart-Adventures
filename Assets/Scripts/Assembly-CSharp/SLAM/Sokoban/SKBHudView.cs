using SLAM.Engine;
using UnityEngine;

namespace SLAM.Sokoban
{
	public class SKBHudView : HUDView
	{
		[SerializeField]
		private UILabel lblProgression;

		protected override void Update()
		{
			lblProgression.text = string.Format("{0}/{1}", Controller<SokobanGameController>().CurrentLevelIndex + 1, Controller<SokobanGameController>().TotalLevelCount);
		}
	}
}
