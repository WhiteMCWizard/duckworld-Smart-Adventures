using System.Collections;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.InputSystem
{
	public class OnScreenControls : MonoBehaviour
	{
		private IEnumerator Start()
		{
			if (!(SLAMInput.Provider is MobileInputProvider))
			{
				Object.Destroy(base.gameObject);
				yield break;
			}
			GameController controller = Object.FindObjectOfType<GameController>();
			FiniteStateMachine stateMachine = controller.GetComponent<FiniteStateMachine>();
			UIPanel panel = GetComponentInChildren<UIPanel>();
			panel.alpha = 0f;
			while (stateMachine.CurrentState.Name == "Loading" || stateMachine.CurrentState.Name == "Ready to begin")
			{
				yield return null;
			}
			Stopwatch sw2 = new Stopwatch(0.2f);
			while (!sw2.Expired)
			{
				yield return null;
				panel.alpha = sw2.Progress;
			}
			while (stateMachine.CurrentState.Name != "Finished")
			{
				yield return null;
			}
			sw2 = new Stopwatch(0.2f);
			while (!sw2.Expired)
			{
				yield return null;
				panel.alpha = 1f - sw2.Progress;
			}
		}
	}
}
