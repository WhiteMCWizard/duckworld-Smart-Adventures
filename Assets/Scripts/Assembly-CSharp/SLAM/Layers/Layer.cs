using System;
using UnityEngine;

namespace SLAM.Layers
{
	[Serializable]
	public class Layer
	{
		[SerializeField]
		private int m_LayerIndex;

		public int LayerIndex
		{
			get
			{
				return m_LayerIndex;
			}
		}

		public int Mask
		{
			get
			{
				return 1 << m_LayerIndex;
			}
		}

		public void Set(int _layerIndex)
		{
			if (_layerIndex > 0 && _layerIndex < 32)
			{
				m_LayerIndex = _layerIndex;
			}
		}
	}
}
