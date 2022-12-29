using System;
using UnityEngine;

public class LocalisedAsset : MonoBehaviour
{
	[Serializable]
	private class LocalisableAsset
	{
		public AssetLanguage language;

		public UnityEngine.Object localisedAsset;

		public Component targetComponent;
	}

	private enum AssetLanguage
	{
		English = 0,
		Dutch = 1
	}

	[SerializeField]
	private LocalisableAsset asset;

	private void Start()
	{
		if (asset.language.ToString() == Localization.language && asset.targetComponent is MeshRenderer && asset.localisedAsset is Material)
		{
			((MeshRenderer)asset.targetComponent).material = (Material)asset.localisedAsset;
		}
	}
}
