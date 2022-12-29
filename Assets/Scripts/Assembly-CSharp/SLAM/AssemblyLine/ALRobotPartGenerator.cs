using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.AssemblyLine
{
	public class ALRobotPartGenerator : MonoBehaviour
	{
		private struct PartProbability
		{
			public ALRobotPart RobotPart;

			public int Probability;
		}

		[CompilerGenerated]
		private sealed class _003CGeneratePart_003Ec__AnonStorey153
		{
			internal ALDropZone dropZone;

			internal ALRobotPartGenerator _003C_003Ef__this;

			internal bool _003C_003Em__7(ALRobotPart rb)
			{
				return rb != _003C_003Ef__this.previousPart && rb.Kind == dropZone.DesignatedKind && (dropZone.PlacedTypes & rb.Type) != rb.Type;
			}
		}

		[CompilerGenerated]
		private sealed class _003CSetDifficulty_003Ec__AnonStorey154
		{
			internal AssemblyLineGame.AssemblyLineGameDifficulty diff;

			internal bool _003C_003Em__A(ALRobotPart p)
			{
				return diff.AllowedKinds.Contains(p.Kind);
			}
		}

		[SerializeField]
		private List<ALRobotPart> partPrefabs;

		[SerializeField]
		private ALDropZone[] dropZones;

		private AnimationCurve spawnRandomPartCurve;

		private List<ALRobotPart> filteredRobotPartPrefabs;

		private ALRobotPart previousPart;

		private int partIndex;

		[CompilerGenerated]
		private static Func<PartProbability, int> _003C_003Ef__am_0024cache6;

		[CompilerGenerated]
		private static Func<ALDropZone, float> _003C_003Ef__am_0024cache7;

		public ALRobotPart GeneratePart()
		{
			if (UnityEngine.Random.value > getRandomPartChance())
			{
				List<PartProbability> list = new List<PartProbability>();
				_003CGeneratePart_003Ec__AnonStorey153 _003CGeneratePart_003Ec__AnonStorey = new _003CGeneratePart_003Ec__AnonStorey153();
				_003CGeneratePart_003Ec__AnonStorey._003C_003Ef__this = this;
				ALDropZone[] array = dropZones;
				for (int i = 0; i < array.Length; i++)
				{
					_003CGeneratePart_003Ec__AnonStorey.dropZone = array[i];
					if (_003CGeneratePart_003Ec__AnonStorey.dropZone.DesignatedKind < 0)
					{
						continue;
					}
					IEnumerable<ALRobotPart> enumerable = filteredRobotPartPrefabs.Where(_003CGeneratePart_003Ec__AnonStorey._003C_003Em__7);
					foreach (ALRobotPart item in enumerable)
					{
						list.Add(new PartProbability
						{
							RobotPart = item,
							Probability = _003CGeneratePart_003Ec__AnonStorey.dropZone.DroppedParts.Count
						});
					}
				}
				if (list.Count > 0)
				{
					if (_003C_003Ef__am_0024cache6 == null)
					{
						_003C_003Ef__am_0024cache6 = _003CGeneratePart_003Em__8;
					}
					int num = list.Sum(_003C_003Ef__am_0024cache6);
					int num2 = UnityEngine.Random.Range(0, num + 1);
					int num3 = 0;
					for (int j = 0; j < list.Count; j++)
					{
						num3 += list[j].Probability;
						if (num3 >= num2)
						{
							return previousPart = list[j].RobotPart;
						}
					}
				}
			}
			if (partIndex + 1 > filteredRobotPartPrefabs.Count || filteredRobotPartPrefabs[partIndex] == previousPart)
			{
				partIndex = 0;
				filteredRobotPartPrefabs.Shuffle();
			}
			return filteredRobotPartPrefabs[partIndex++];
		}

		private float getRandomPartChance()
		{
			ALDropZone[] collection = dropZones;
			if (_003C_003Ef__am_0024cache7 == null)
			{
				_003C_003Ef__am_0024cache7 = _003CgetRandomPartChance_003Em__9;
			}
			float num = collection.Select(_003C_003Ef__am_0024cache7).Sum();
			float time = num / ((float)dropZones.Length * (float)Enum.GetValues(typeof(ALRobotPart.RobotPartType)).Length);
			return spawnRandomPartCurve.Evaluate(time);
		}

		public void SetDifficulty(AssemblyLineGame.AssemblyLineGameDifficulty diff)
		{
			_003CSetDifficulty_003Ec__AnonStorey154 _003CSetDifficulty_003Ec__AnonStorey = new _003CSetDifficulty_003Ec__AnonStorey154();
			_003CSetDifficulty_003Ec__AnonStorey.diff = diff;
			filteredRobotPartPrefabs = partPrefabs.Where(_003CSetDifficulty_003Ec__AnonStorey._003C_003Em__A).ToList().Shuffle();
			spawnRandomPartCurve = _003CSetDifficulty_003Ec__AnonStorey.diff.RandomPartChanceCurve;
		}

		[CompilerGenerated]
		private static int _003CGeneratePart_003Em__8(PartProbability pp)
		{
			return pp.Probability;
		}

		[CompilerGenerated]
		private static float _003CgetRandomPartChance_003Em__9(ALDropZone d)
		{
			return d.DroppedParts.Count;
		}
	}
}
