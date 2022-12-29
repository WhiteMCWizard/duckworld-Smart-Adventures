using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Avatar
{
	public static class AvatarSystem
	{
		public enum Gender
		{
			Boy = 0,
			Girl = 1
		}

		public enum Race
		{
			Dog = 0,
			Cat = 1,
			Duck = 2
		}

		public enum ItemCategory
		{
			Skin = 0,
			Hair = 1,
			Eyes = 2,
			Torso = 3,
			Legs = 4,
			Feet = 5
		}

		public const string defaultConfigurationJson = "{\"Gender\":\"Boy\",\"Race\":\"Dog\",\"SkinColor\":{\"r\":0.945098042488098,\"g\":0.788235306739807,\"b\":0.709803938865662},\"Items\":[\"31ea5849-b3b6-47e9-b6b8-0a606cee53ac\",\"d3167c5e-42f6-43fe-9a1a-6c4045270499\",\"32599b5a-9c8a-4283-8439-f3dd85fd465b\",\"9c675a13-b379-455a-a2a5-e92b87af1f26\",\"d40d736d-e224-4d34-b102-793adee32d95\"]}";

		private static AvatarConfigurationData playerAvatarConfiguration;

		[CompilerGenerated]
		private static Action<PlayerAvatarData> _003C_003Ef__am_0024cache1;

		[CompilerGenerated]
		private static Func<Renderer, IEnumerable<Material>> _003C_003Ef__am_0024cache2;

		public static void SavePlayerConfiguration(AvatarConfigurationData config, Texture2D mugshot)
		{
			playerAvatarConfiguration = config;
			UserProfile.Current.SetMugShot(mugshot);
			ApiClient.SaveAvatarConfiguration(config, mugshot.EncodeToPNG(), null);
		}

		public static AvatarConfigurationData GetPlayerConfiguration()
		{
			return playerAvatarConfiguration;
		}

		public static void UnsetPlayerConfiguration()
		{
			playerAvatarConfiguration = null;
		}

		public static void LoadPlayerConfiguration()
		{
			if (playerAvatarConfiguration == null)
			{
				if (_003C_003Ef__am_0024cache1 == null)
				{
					_003C_003Ef__am_0024cache1 = _003CLoadPlayerConfiguration_003Em__C4;
				}
				ApiClient.GetAvatarConfiguration(_003C_003Ef__am_0024cache1);
			}
		}

		public static GameObject SpawnPlayerAvatar()
		{
			return SpawnAvatar(GetPlayerConfiguration());
		}

		public static GameObject SpawnAvatar(AvatarConfigurationData config)
		{
			GameObject gameObject = loadModel(config.Race, config.Gender);
			UpdateAvatar(gameObject, config);
			return gameObject;
		}

		public static void UpdateAvatar(GameObject avatarRoot, AvatarConfigurationData config)
		{
			AvatarItemLibrary itemLibrary = AvatarItemLibrary.GetItemLibrary(config);
			string[] items = config.Items;
			foreach (string text in items)
			{
				AvatarItemLibrary.AvatarItem itemByGUID = itemLibrary.GetItemByGUID(text);
				if (itemByGUID != null)
				{
					applyItem(avatarRoot.transform.FindChildRecursively(itemByGUID.MeshName), itemByGUID.Material);
				}
				else
				{
					Debug.LogError("Unkown item guid " + text);
				}
			}
			Renderer[] componentsInChildren = avatarRoot.GetComponentsInChildren<Renderer>();
			if (_003C_003Ef__am_0024cache2 == null)
			{
				_003C_003Ef__am_0024cache2 = _003CUpdateAvatar_003Em__C5;
			}
			foreach (Material item in componentsInChildren.SelectMany(_003C_003Ef__am_0024cache2))
			{
				if (item != null && item.HasProperty("_SkinColor"))
				{
					item.SetColor("_SkinColor", config.SkinColor);
				}
			}
		}

		private static void applyItem(Transform itemMesh, Material material)
		{
			foreach (Transform item in itemMesh.parent)
			{
				item.gameObject.SetActive(item == itemMesh);
			}
			Renderer component = itemMesh.GetComponent<Renderer>();
			Material[] materials = component.materials;
			for (int i = 0; i < materials.Length; i++)
			{
				Material material2 = materials[i];
				if (material2 != null && !material2.name.Contains("Skin") && !material2.name.Contains("DontReplace"))
				{
					materials[i] = material;
				}
				if (material2.name.ToLower().Contains("helmet"))
				{
					break;
				}
			}
			component.materials = materials;
		}

		private static GameObject loadModel(Race race, Gender gender)
		{
			return UnityEngine.Object.Instantiate(Resources.Load(string.Format("{0}_{1}_Model_Master", gender, race))) as GameObject;
		}

		[CompilerGenerated]
		private static void _003CLoadPlayerConfiguration_003Em__C4(PlayerAvatarData pad)
		{
			playerAvatarConfiguration = ((pad == null) ? null : pad.Config);
		}

		[CompilerGenerated]
		private static IEnumerable<Material> _003CUpdateAvatar_003Em__C5(Renderer r)
		{
			return r.materials;
		}
	}
}
