using UnityEngine;

namespace SLAM.MoneyDive
{
	public class MD_AnimationEventCatcher : MonoBehaviour
	{
		[SerializeField]
		private GameObject teacup;

		[SerializeField]
		private GameObject saucer;

		[SerializeField]
		private GameObject player;

		private void Start()
		{
			teacup.transform.parent = player.transform.FindChildRecursively("Object_A_Loc");
			teacup.transform.localPosition = Vector3.zero;
			saucer.transform.parent = player.transform.FindChildRecursively("Object_B_Loc");
			saucer.transform.localPosition = Vector3.zero;
		}

		private void OnShowTeaCup()
		{
			teacup.SetActive(true);
			saucer.SetActive(true);
		}

		private void OnHideTeaCup()
		{
			teacup.SetActive(false);
			saucer.SetActive(false);
		}
	}
}
