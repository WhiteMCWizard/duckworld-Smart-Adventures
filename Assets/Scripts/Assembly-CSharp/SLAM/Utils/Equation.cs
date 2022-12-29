using System;
using System.Collections.Generic;
using SLAM.Slinq;

namespace SLAM.Utils
{
	[Serializable]
	public class Equation
	{
		private int numberA;

		private int numberB;

		private Manipulator sign;

		private List<int> offsets = new List<int> { 2, 3 };

		public string EquationString
		{
			get
			{
				return string.Format("{0} {1} {2} = ?", numberA, SignString, numberB);
			}
		}

		public int WrongAnswer
		{
			get
			{
				int num = offsets.ElementAt(offsets.Count - 1) + offsets.ElementAt(offsets.Count - 2);
				offsets.Add(num);
				return (offsets.Count % 2 != 0 && CorrectAnswer - num > 0) ? (CorrectAnswer - num) : (CorrectAnswer + num);
			}
		}

		public int CorrectAnswer
		{
			get
			{
				switch (sign)
				{
				case Manipulator.addition:
					return numberA + numberB;
				case Manipulator.substraction:
					return numberA - numberB;
				case Manipulator.multiplication:
					return numberA * numberB;
				case Manipulator.division:
					return numberA / numberB;
				default:
					return numberA + numberB;
				}
			}
		}

		public string SignString
		{
			get
			{
				switch (sign)
				{
				case Manipulator.addition:
					return "+";
				case Manipulator.substraction:
					return "-";
				case Manipulator.multiplication:
					return "x";
				case Manipulator.division:
					return ":";
				default:
					return "+";
				}
			}
		}

		public string NumberA
		{
			get
			{
				return numberA.ToString();
			}
		}

		public string NumberB
		{
			get
			{
				return numberB.ToString();
			}
		}

		public Equation(int n1, int n2, Manipulator sign)
		{
			this.sign = sign;
			numberA = n1;
			numberB = n2;
		}
	}
}
