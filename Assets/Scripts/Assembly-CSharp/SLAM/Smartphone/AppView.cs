using SLAM.Engine;

namespace SLAM.Smartphone
{
	public class AppView : View
	{
		public virtual void ReturnFromBackground()
		{
			base.gameObject.SetActive(true);
		}

		public virtual void EnterBackground()
		{
			base.gameObject.SetActive(false);
		}

		public virtual void OnBackClicked()
		{
		}
	}
}
