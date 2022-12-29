using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Utils
{
	public class EquationGenerator
	{
		[CompilerGenerated]
		private sealed class _003CGenerateEquation_003Ec__AnonStorey18D
		{
			internal int numberA;

			internal int max;

			internal int min;

			internal bool _003C_003Em__FB(int b)
			{
				return b + numberA < max && b % 10 + numberA % 10 <= 10;
			}

			internal bool _003C_003Em__FC(int b)
			{
				return b + numberA < max;
			}

			internal bool _003C_003Em__FD(int b)
			{
				return numberA - b > min && numberA % 10 - b % 10 >= 0;
			}

			internal bool _003C_003Em__FE(int b)
			{
				return numberA - b > min;
			}
		}

		private List<int> _tables;

		private List<Manipulator> _manipulators;

		private bool _restrictedToTenths;

		public List<int> Tables
		{
			get
			{
				return _tables;
			}
			set
			{
				_tables = value;
			}
		}

		public List<Manipulator> Manipulators
		{
			get
			{
				return _manipulators;
			}
			set
			{
				_manipulators = value;
			}
		}

		public bool RestrictedToTenths
		{
			get
			{
				return _restrictedToTenths;
			}
			set
			{
				_restrictedToTenths = value;
			}
		}

		public EquationGenerator()
		{
			_tables = new List<int>(new int[5] { 2, 3, 4, 5, 10 });
			_manipulators = new List<Manipulator>(new Manipulator[1]);
		}

		public List<Equation> GetEquations(int amount)
		{
			List<Equation> list = new List<Equation>();
			if (_manipulators.Count <= 0)
			{
				Debug.LogWarning("Hey Buddy, cannot generate an equation without a manipulator");
			}
			for (int i = 0; i < amount; i++)
			{
				foreach (Manipulator manipulator in _manipulators)
				{
					Equation item = GenerateEquation(manipulator);
					list.Add(item);
					i++;
				}
			}
			list.Shuffle();
			return list;
		}

		private Equation GenerateEquation(Manipulator m)
		{
			_003CGenerateEquation_003Ec__AnonStorey18D _003CGenerateEquation_003Ec__AnonStorey18D = new _003CGenerateEquation_003Ec__AnonStorey18D();
			_003CGenerateEquation_003Ec__AnonStorey18D.max = _tables.Max();
			_003CGenerateEquation_003Ec__AnonStorey18D.min = _tables.Min();
			_003CGenerateEquation_003Ec__AnonStorey18D.numberA = _tables.GetRandom();
			int num = _tables.GetRandom();
			switch (m)
			{
			case Manipulator.addition:
				num = ((!RestrictedToTenths) ? _tables.Where(_003CGenerateEquation_003Ec__AnonStorey18D._003C_003Em__FC).GetRandom() : _tables.Where(_003CGenerateEquation_003Ec__AnonStorey18D._003C_003Em__FB).GetRandom());
				break;
			case Manipulator.substraction:
				num = ((!RestrictedToTenths) ? _tables.Where(_003CGenerateEquation_003Ec__AnonStorey18D._003C_003Em__FE).GetRandom() : _tables.Where(_003CGenerateEquation_003Ec__AnonStorey18D._003C_003Em__FD).GetRandom());
				break;
			case Manipulator.division:
				_003CGenerateEquation_003Ec__AnonStorey18D.numberA *= num;
				break;
			}
			return new Equation(_003CGenerateEquation_003Ec__AnonStorey18D.numberA, num, m);
		}
	}
}
