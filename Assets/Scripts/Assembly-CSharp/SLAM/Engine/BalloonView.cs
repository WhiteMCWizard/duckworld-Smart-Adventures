using SLAM.Shared;
using UnityEngine;

namespace SLAM.Engine
{
	public class BalloonView : View
	{
		[SerializeField]
		private GameObject balloonTailLeftBottomPointingLeft;

		[SerializeField]
		private GameObject balloonTailRightBottomPointingRight;

		[SerializeField]
		private GameObject balloonTailLeftBottomPointingRight;

		[SerializeField]
		private GameObject balloonTailRightBottomPointingLeft;

		[SerializeField]
		private GameObject balloonTailLeftSidePointingDown;

		[SerializeField]
		private GameObject balloonTailRightSidePointingDown;

		public virtual SpeechBalloon CreateBalloon(BalloonType type)
		{
			GameObject gameObject = null;
			switch (type)
			{
			default:
				gameObject = balloonTailLeftBottomPointingLeft;
				break;
			case BalloonType.TailRightBottomPointingRight:
				gameObject = balloonTailRightBottomPointingRight;
				break;
			case BalloonType.TailLeftBottomPointingRight:
				gameObject = balloonTailLeftBottomPointingRight;
				break;
			case BalloonType.TailRightBottomPointingLeft:
				gameObject = balloonTailRightBottomPointingLeft;
				break;
			case BalloonType.TailLeftSidePointingDown:
				gameObject = balloonTailLeftSidePointingDown;
				break;
			case BalloonType.TailRightSidePointingDown:
				gameObject = balloonTailRightSidePointingDown;
				break;
			}
			GameObject gameObject2 = NGUITools.AddChild(base.gameObject, gameObject);
			return gameObject2.GetComponent<SpeechBalloon>();
		}
	}
}
