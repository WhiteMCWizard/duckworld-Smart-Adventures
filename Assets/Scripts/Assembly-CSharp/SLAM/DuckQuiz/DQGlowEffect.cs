using System;
using System.Collections;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.DuckQuiz
{
	public class DQGlowEffect : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] glowObjects;

		[CompilerGenerated]
		private static Func<GameObject, float> _003C_003Ef__am_0024cache1;

		private void OnValidate()
		{
			GameObject[] source = glowObjects;
			if (_003C_003Ef__am_0024cache1 == null)
			{
				_003C_003Ef__am_0024cache1 = _003COnValidate_003Em__5A;
			}
			glowObjects = source.OrderBy(_003C_003Ef__am_0024cache1).ToArray();
		}

		private IEnumerator Start()
		{
			string[] effects = new string[2] { "doAlternate", "doWave" };
			int i = 0;
			while (true)
			{
				for (int k = 0; k < glowObjects.Length; k++)
				{
					glowObjects[k].SetActive(false);
				}
				yield return StartCoroutine(effects[i % effects.Length]);
				for (int j = 0; j < glowObjects.Length; j++)
				{
					glowObjects[j].SetActive(false);
				}
				yield return new WaitForSeconds(0.5f);
				i++;
			}
		}

		private IEnumerator doWave()
		{
			for (int i = 0; i < 12; i++)
			{
				for (int j = 0; j < glowObjects.Length; j++)
				{
					glowObjects[(i % 4 >= 2) ? (glowObjects.Length - j - 1) : j].SetActive(i % 2 == 0);
					yield return new WaitForSeconds(Mathf.Lerp(0.06f, 0.02f, (float)j / (float)(glowObjects.Length - 1)));
				}
				yield return new WaitForSeconds(0.2f);
			}
		}

		private IEnumerator doAlternate()
		{
			for (int i = 0; i < 20; i++)
			{
				for (int k = 0; k < glowObjects.Length; k++)
				{
					glowObjects[k].SetActive(k % 2 == 0);
				}
				yield return new WaitForSeconds(0.2f);
				for (int j = 0; j < glowObjects.Length; j++)
				{
					glowObjects[j].SetActive(j % 2 != 0);
				}
				yield return new WaitForSeconds(0.2f);
			}
		}

		[CompilerGenerated]
		private static float _003COnValidate_003Em__5A(GameObject g)
		{
			return g.transform.position.x;
		}
	}
}
