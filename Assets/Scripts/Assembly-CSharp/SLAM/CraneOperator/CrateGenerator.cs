using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.CraneOperator
{
	public class CrateGenerator : MonoBehaviour
	{
		[CompilerGenerated]
		private static Func<Crate[], IEnumerable<Crate>> _003C_003Ef__am_0024cache0;

		public void SpawnCrates(Transform[] pickupZones, List<Crate[]> pickupLists, Crate[] extraCrates, int rows, int columns, int spawnCount = 0)
		{
			if (_003C_003Ef__am_0024cache0 == null)
			{
				_003C_003Ef__am_0024cache0 = _003CSpawnCrates_003Em__4F;
			}
			List<Crate> list = pickupLists.SelectMany(_003C_003Ef__am_0024cache0).ToList();
			List<bool[,]> list2 = new List<bool[,]>();
			for (int i = 0; i < pickupZones.Length; i++)
			{
				list2.Add(new bool[rows, columns]);
			}
			list.AddRange(extraCrates);
			Crate[] array = list.ToArray();
			int num = 0;
			int num2 = 0;
			while (num < array.Length)
			{
				if (num2 >= pickupZones.Length)
				{
					num2 = 0;
				}
				bool[,] grid = list2[num2];
				int posX;
				int posY;
				fitCrateInGrid(list[num], ref grid, out posX, out posY);
				Crate crate = UnityEngine.Object.Instantiate(list[num]);
				crate.transform.parent = pickupZones[num2];
				crate.transform.localPosition = new Vector2(posX, posY);
				crate.name = posX + "," + posY + " - " + crate.name;
				num++;
				num2++;
			}
		}

		private bool fitCrateInGrid(Crate crateToPlace, ref bool[,] grid, out int posX, out int posY)
		{
			int num = grid.GetLength(0) - 1;
			int num2 = grid.GetLength(1) - 1;
			posX = 0;
			posY = 0;
			for (int num3 = num; num3 >= 0; num3--)
			{
				for (int num4 = num2; num4 >= 0; num4--)
				{
					bool flag = true;
					int num5 = num3;
					while (num5 >= 0 && num5 > num3 - crateToPlace.UnitHeight)
					{
						if (grid[num5, num4])
						{
							flag = false;
						}
						num5--;
					}
					int num6 = num4;
					while (num6 >= 0 && num6 > num4 - crateToPlace.UnitWidth)
					{
						if (grid[num3, num6])
						{
							flag = false;
						}
						num6--;
					}
					if (flag)
					{
						int num7 = num3;
						while (num7 >= 0 && num7 > num3 - crateToPlace.UnitHeight)
						{
							grid[num7, num4] = true;
							int num8 = num4;
							while (num8 >= 0 && num8 > num4 - crateToPlace.UnitWidth)
							{
								grid[num7, num8] = true;
								num8--;
							}
							num7--;
						}
						posX = num4 - crateToPlace.UnitWidth + 1;
						posY = num - (num3 - crateToPlace.UnitHeight) - 1;
						return true;
					}
				}
			}
			return false;
		}

		[CompilerGenerated]
		private static IEnumerable<Crate> _003CSpawnCrates_003Em__4F(Crate[] p)
		{
			return p;
		}
	}
}
