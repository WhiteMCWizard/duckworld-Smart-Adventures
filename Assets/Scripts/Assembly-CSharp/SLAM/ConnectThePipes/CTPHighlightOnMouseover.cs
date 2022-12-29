using System;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.ConnectThePipes
{
	public class CTPHighlightOnMouseover : MonoBehaviour
	{
		[SerializeField]
		private Renderer[] targetRenderers;

		[SerializeField]
		private Color targetEmissionColor;

		private Color[] origEmissionColors;

		[CompilerGenerated]
		private static Func<Renderer, Color> _003C_003Ef__am_0024cache3;

		private void OnMouseEnter()
		{
			if (!UnityEngine.Object.FindObjectOfType<CTPInputManager>().AreControlsLocked)
			{
				Renderer[] collection = targetRenderers;
				if (_003C_003Ef__am_0024cache3 == null)
				{
					_003C_003Ef__am_0024cache3 = _003COnMouseEnter_003Em__3B;
				}
				origEmissionColors = collection.Select(_003C_003Ef__am_0024cache3).ToArray();
				for (int i = 0; i < targetRenderers.Length; i++)
				{
					targetRenderers[i].material.SetColor("_EmissiveColor", targetEmissionColor);
				}
			}
		}

		private void OnMouseExit()
		{
			if (origEmissionColors != null && origEmissionColors.Length > 0)
			{
				for (int i = 0; i < targetRenderers.Length; i++)
				{
					targetRenderers[i].material.SetColor("_EmissiveColor", origEmissionColors[i]);
				}
			}
		}

		[CompilerGenerated]
		private static Color _003COnMouseEnter_003Em__3B(Renderer t)
		{
			return t.material.GetColor("_EmissiveColor");
		}
	}
}
