using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Avatar;
using SLAM.BuildSystem;
using SLAM.Engine;
using SLAM.MotionComics._3D;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.AvatarCreator
{
	public class AvatarCreatorController : ViewController
	{
		[CompilerGenerated]
		private sealed class _003CAskUserToSave_003Ec__AnonStorey158
		{
			internal string playername;

			internal AvatarCreatorController _003C_003Ef__this;

			internal void _003C_003Em__22()
			{
				_003C_003Ef__this.saveAndContinue(playername);
			}
		}

		[CompilerGenerated]
		private sealed class _003CUpdateAvatar_003Ec__AnonStorey159
		{
			internal AvatarSystem.Race race;

			internal AvatarSystem.Gender gender;

			internal bool _003C_003Em__23(AvatarConfigurationData c)
			{
				return c.Race == race && c.Gender == gender;
			}
		}

		[CompilerGenerated]
		private sealed class _003CsaveAndContinue_003Ec__AnonStorey15A
		{
			private sealed class _003CsaveAndContinue_003Ec__AnonStorey15B
			{
				private sealed class _003CsaveAndContinue_003Ec__AnonStorey15C
				{
					internal AvatarConfigurationData defaultItems;

					internal _003CsaveAndContinue_003Ec__AnonStorey15A _003C_003Ef__ref_0024346;

					internal bool _003C_003Em__28(ShopItemData si)
					{
						return _003C_003Ef__ref_0024346.playerConfig.Items.Contains(si.GUID) || defaultItems.Items.Contains(si.GUID);
					}
				}

				internal Texture2D mugshot;

				internal _003CsaveAndContinue_003Ec__AnonStorey15A _003C_003Ef__ref_0024346;

				private static Func<ShopItemData, int> _003C_003Ef__am_0024cache2;

				private static Action<bool> _003C_003Ef__am_0024cache3;

				internal void _003C_003Em__26(bool succes)
				{
					AvatarSystem.SavePlayerConfiguration(_003C_003Ef__ref_0024346.playerConfig, mugshot);
					ApiClient.GetAllShopItems(_003C_003Em__27);
				}

				internal void _003C_003Em__27(ShopItemData[] allShopItems)
				{
					_003CsaveAndContinue_003Ec__AnonStorey15C _003CsaveAndContinue_003Ec__AnonStorey15C = new _003CsaveAndContinue_003Ec__AnonStorey15C();
					_003CsaveAndContinue_003Ec__AnonStorey15C._003C_003Ef__ref_0024346 = _003C_003Ef__ref_0024346;
					_003CsaveAndContinue_003Ec__AnonStorey15C.defaultItems = (AvatarConfigurationData)AvatarItemLibrary.GetItemLibrary(_003C_003Ef__ref_0024346.playerConfig).DefaultConfigurations.First().Clone();
					IEnumerable<ShopItemData> collection = allShopItems.Where(_003CsaveAndContinue_003Ec__AnonStorey15C._003C_003Em__28);
					if (_003C_003Ef__am_0024cache2 == null)
					{
						_003C_003Ef__am_0024cache2 = _003C_003Em__29;
					}
					int[] shopItemIds = collection.Select(_003C_003Ef__am_0024cache2).ToArray();
					if (_003C_003Ef__am_0024cache3 == null)
					{
						_003C_003Ef__am_0024cache3 = _003C_003Em__2A;
					}
					ApiClient.AddItemsToInventory(shopItemIds, _003C_003Ef__am_0024cache3);
				}

				private static int _003C_003Em__29(ShopItemData si)
				{
					return si.Id;
				}

				private static void _003C_003Em__2A(bool success)
				{
					MotionComicPlayer.SetSceneToLoad("Hub");
					SceneManager.Load("MC_ADV00_01_Intro");
				}
			}

			internal string playername;

			internal string playerAddress;

			internal AvatarConfigurationData playerConfig;

			internal void _003C_003Em__25(Texture2D mugshot)
			{
				_003CsaveAndContinue_003Ec__AnonStorey15B _003CsaveAndContinue_003Ec__AnonStorey15B = new _003CsaveAndContinue_003Ec__AnonStorey15B();
				_003CsaveAndContinue_003Ec__AnonStorey15B._003C_003Ef__ref_0024346 = this;
				_003CsaveAndContinue_003Ec__AnonStorey15B.mugshot = mugshot;
				ApiClient.SavePlayerName(playername, playerAddress, _003CsaveAndContinue_003Ec__AnonStorey15B._003C_003Em__26);
			}
		}

		private const int MAX_HAIR_COUNT = 6;

		private const int MAX_SKIN_COUNT = 6;

		[SerializeField]
		private RuntimeAnimatorController boyController;

		[SerializeField]
		private RuntimeAnimatorController girlController;

		[SerializeField]
		private Transform avatarRoot;

		[SerializeField]
		private AC_CustomiseAvatarView customiseView;

		[SerializeField]
		private AC_NamePickerView nameView;

		[SerializeField]
		private LoadingView loadingView;

		[SerializeField]
		private AvatarConfigurationData[] configurations;

		private GameObject spawnedAvatar;

		private AvatarConfigurationData configData;

		private AvatarConfigurationData avatarConfig;

		private AvatarItemLibrary avatarItemLibrary;

		[CompilerGenerated]
		private static Func<AvatarItemLibrary.AvatarItem, bool> _003C_003Ef__am_0024cacheB;

		public Animator AvatarAnimator
		{
			get
			{
				return avatarRoot.GetComponentInChildren<Animator>();
			}
		}

		protected void Awake()
		{
			AddView(customiseView);
			AddView(nameView);
			AddView(loadingView);
			avatarConfig = configurations.FirstOrDefault();
			avatarItemLibrary = AvatarItemLibrary.GetItemLibrary(avatarConfig);
			updateAvatarModel(avatarConfig);
			updateCustomiseOptions();
		}

		protected override void Start()
		{
			OpenCustomiseAvatarView();
			if (!SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
			{
				return;
			}
			if (AudioController.GetCategory("Music") != null && AudioController.GetCategory("Music").AudioItems.Length > 0)
			{
				AudioItem[] audioItems = AudioController.GetCategory("Music").AudioItems;
				foreach (AudioItem audioItem in audioItems)
				{
					AudioController.Play(audioItem.Name);
				}
			}
			else
			{
				Debug.LogWarning("Hey buddy, this game doesn't have music? Make sure there is an AudioController with a category 'Music'!");
			}
			if (AudioController.GetCategory("Ambience") != null && AudioController.GetCategory("Ambience").AudioItems.Length > 0)
			{
				AudioItem[] audioItems2 = AudioController.GetCategory("Ambience").AudioItems;
				foreach (AudioItem audioItem2 in audioItems2)
				{
					AudioController.Play(audioItem2.Name);
				}
			}
			else
			{
				Debug.LogWarning("Hey buddy, this game doesn't have ambience sounds? Make sure there is an AudioController with a category 'Ambience'!");
			}
		}

		private void OnEnable()
		{
			GameEvents.Subscribe<HairClickedEvent>(onHairClicked);
			GameEvents.Subscribe<SkinClickedEvent>(onSkinClicked);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<HairClickedEvent>(onHairClicked);
			GameEvents.Unsubscribe<SkinClickedEvent>(onSkinClicked);
		}

		public void Rotate()
		{
			AvatarAnimator.SetTrigger("TurnAround");
		}

		public void OpenCustomiseAvatarView()
		{
			CloseAllViews();
			OpenView<AC_CustomiseAvatarView>();
		}

		public void OpenNamePickerView()
		{
			CloseAllViews();
			OpenView<AC_NamePickerView>().SetInfo(configData.Gender);
		}

		public void AskUserToSave(string playername)
		{
			_003CAskUserToSave_003Ec__AnonStorey158 _003CAskUserToSave_003Ec__AnonStorey = new _003CAskUserToSave_003Ec__AnonStorey158();
			_003CAskUserToSave_003Ec__AnonStorey.playername = playername;
			_003CAskUserToSave_003Ec__AnonStorey._003C_003Ef__this = this;
			GameEvents.Invoke(new PopupEvent(Localization.Get("UI_ARE_YOU_SURE"), Localization.Get("AC_LABEL_AREYOUSURE"), Localization.Get("UI_YES"), Localization.Get("UI_NO"), _003CAskUserToSave_003Ec__AnonStorey._003C_003Em__22, null));
		}

		public void UpdateAvatar(AvatarSystem.Race race, AvatarSystem.Gender gender)
		{
			_003CUpdateAvatar_003Ec__AnonStorey159 _003CUpdateAvatar_003Ec__AnonStorey = new _003CUpdateAvatar_003Ec__AnonStorey159();
			_003CUpdateAvatar_003Ec__AnonStorey.race = race;
			_003CUpdateAvatar_003Ec__AnonStorey.gender = gender;
			avatarConfig = configurations.FirstOrDefault(_003CUpdateAvatar_003Ec__AnonStorey._003C_003Em__23);
			avatarItemLibrary = AvatarItemLibrary.GetItemLibrary(avatarConfig);
			updateAvatarModel(avatarConfig);
			updateCustomiseOptions();
		}

		private void updateAvatarModel(AvatarConfigurationData configData)
		{
			if (this.configData != null && configData.Gender == this.configData.Gender && configData.Race == this.configData.Race)
			{
				this.configData = configData;
				AvatarSystem.UpdateAvatar(spawnedAvatar, this.configData);
			}
			else
			{
				this.configData = configData;
				spawnModel();
			}
			if (configData.Gender == AvatarSystem.Gender.Girl)
			{
				AvatarAnimator.runtimeAnimatorController = girlController;
			}
			else
			{
				AvatarAnimator.runtimeAnimatorController = boyController;
			}
		}

		private void updateCustomiseOptions()
		{
			List<AvatarItemLibrary.AvatarItem> items = avatarItemLibrary.Items;
			if (_003C_003Ef__am_0024cacheB == null)
			{
				_003C_003Ef__am_0024cacheB = _003CupdateCustomiseOptions_003Em__24;
			}
			AvatarItemLibrary.AvatarItem[] hairs = items.Where(_003C_003Ef__am_0024cacheB).Take(6).ToArray();
			Color[] skinColors = avatarItemLibrary.SkinColors.Take(6).ToArray();
			GetView<AC_CustomiseAvatarView>().SetInfo(hairs, skinColors);
		}

		private void saveAndContinue(string playername)
		{
			_003CsaveAndContinue_003Ec__AnonStorey15A _003CsaveAndContinue_003Ec__AnonStorey15A = new _003CsaveAndContinue_003Ec__AnonStorey15A();
			_003CsaveAndContinue_003Ec__AnonStorey15A.playername = playername;
			CloseAllViews();
			OpenView<LoadingView>();
			_003CsaveAndContinue_003Ec__AnonStorey15A.playerConfig = configData;
			_003CsaveAndContinue_003Ec__AnonStorey15A.playerAddress = Localization.Get("AC_STREETNAMES").Split(',').GetRandom();
			SingletonMonobehaviour<PhotoBooth>.Instance.SayCheese(_003CsaveAndContinue_003Ec__AnonStorey15A.playerConfig, _003CsaveAndContinue_003Ec__AnonStorey15A._003C_003Em__25);
		}

		private void spawnModel()
		{
			if (spawnedAvatar != null)
			{
				UnityEngine.Object.Destroy(spawnedAvatar);
			}
			spawnedAvatar = AvatarSystem.SpawnAvatar(configData);
			spawnedAvatar.transform.parent = avatarRoot;
			spawnedAvatar.transform.localPosition = Vector3.zero;
			spawnedAvatar.transform.localRotation = Quaternion.identity;
			spawnedAvatar.SetLayerRecursively(LayerMask.NameToLayer("Avatar"));
			Invoke("rebind", 0f);
		}

		private void rebind()
		{
			avatarRoot.GetComponent<Animator>().Rebind();
		}

		private void onSkinClicked(SkinClickedEvent evt)
		{
			avatarConfig.SkinColor = evt.Color;
			updateAvatarModel(avatarConfig);
		}

		private void onHairClicked(HairClickedEvent evt)
		{
			avatarConfig.ReplaceItem(evt.Item, avatarItemLibrary);
			updateAvatarModel(avatarConfig);
		}

		[CompilerGenerated]
		private static bool _003CupdateCustomiseOptions_003Em__24(AvatarItemLibrary.AvatarItem i)
		{
			return i.Category == AvatarSystem.ItemCategory.Hair && !i.Material.name.ToLower().Contains("helmet");
		}
	}
}
