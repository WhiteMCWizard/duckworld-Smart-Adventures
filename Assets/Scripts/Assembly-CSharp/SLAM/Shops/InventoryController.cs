using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLAM.Analytics;
using SLAM.Avatar;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Shops
{
	[RequireComponent(typeof(Inventory))]
	public class InventoryController : ViewController
	{
		[SerializeField]
		protected AvatarSpawn avatarSpawn;

		[SerializeField]
		protected HUDView hudView;

		[SerializeField]
		protected InventoryView inventoryView;

		[SerializeField]
		protected LoadingView loadingView;

		[SerializeField]
		protected Inventory.Filter filter;

		[SerializeField]
		private int gameId;

		[SerializeField]
		private AudioClip ambienceAudio;

		protected Inventory inventory;

		protected AvatarItemLibrary avatarLibrary;

		protected GameObject avatarGO;

		protected Animator avatarAnimator;

		public AvatarConfigurationData AvatarConfig { get; protected set; }

		protected override void Start()
		{
			base.Start();
			AddView(hudView);
			AddView(inventoryView);
			AddView(loadingView);
			AvatarConfig = AvatarSystem.GetPlayerConfiguration().Clone() as AvatarConfigurationData;
			avatarLibrary = AvatarItemLibrary.GetItemLibrary(AvatarConfig);
			filter.Gender = AvatarConfig.Gender;
			filter.Race = AvatarConfig.Race;
			inventory = GetComponent<Inventory>();
			avatarGO = avatarSpawn.SpawnAvatar();
			OpenView<LoadingView>();
			inventory.RetrieveInventory(filter, OnInventoryRetrieved);
			AudioController.Play(ambienceAudio.name);
			DataStorage.GetLocationsData(_003CStart_003Em__15D);
		}

		private void OnDestroy()
		{
			DataStorage.GetLocationsData(_003COnDestroy_003Em__15E);
		}

		protected virtual void OnEnable()
		{
			GameEvents.Subscribe<ShopVariationClickedEvent>(OnVariationClicked);
		}

		protected virtual void OnDisable()
		{
			GameEvents.Unsubscribe<ShopVariationClickedEvent>(OnVariationClicked);
		}

		public virtual void SaveAvatar()
		{
			GameEvents.Invoke(new PopupEvent(Localization.Get("UI_ARE_YOU_SURE"), Localization.Get("WR_POPUP_SAVE_OUTFIT"), Localization.Get("UI_YES"), Localization.Get("UI_NO"), _003CSaveAvatar_003Em__15F, null));
		}

		public virtual void GoToHub()
		{
			OpenView<LoadingView>();
			SingletonMonobehaviour<PhotoBooth>.Instance.SayCheese(AvatarConfig, _003CGoToHub_003Em__160);
		}

		protected virtual void OnInventoryRetrieved()
		{
			avatarAnimator = avatarSpawn.GetComponent<Animator>();
			CloseView<LoadingView>();
			OpenView<HUDView>();
			InventoryView inventoryView = OpenView<InventoryView>();
			inventoryView.Load(inventory.Items);
			if (inventory.Items.Length == 0)
			{
				Debug.LogWarning("Inventory for shop '" + filter.ShopId + "' is empty, no items will be displayed.");
			}
		}

		protected virtual void OnVariationClicked(ShopVariationClickedEvent evt)
		{
			AvatarConfig.ReplaceItem(evt.Data.Item.LibraryItem, avatarLibrary);
			AvatarSystem.UpdateAvatar(avatarGO, AvatarConfig);
			if (avatarAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
			{
				switch (evt.Data.Item.LibraryItem.Category)
				{
				case AvatarSystem.ItemCategory.Hair:
					avatarAnimator.SetTrigger("NewHair");
					break;
				case AvatarSystem.ItemCategory.Torso:
					avatarAnimator.SetTrigger("NewShirt");
					AudioController.Play("Avatar_clothes_switchShirt");
					break;
				case AvatarSystem.ItemCategory.Legs:
					avatarAnimator.SetTrigger("NewPants");
					AudioController.Play("Avatar_clothes_switchPants");
					break;
				case AvatarSystem.ItemCategory.Feet:
					avatarAnimator.SetTrigger("NewShoes");
					AudioController.Play("Avatar_clothes_switchShoes");
					break;
				case AvatarSystem.ItemCategory.Eyes:
					break;
				}
			}
		}

		public void RotateCharacter()
		{
			avatarAnimator.SetTrigger("TurnAround");
		}

		protected void RefreshAvatar()
		{
			AvatarSystem.UpdateAvatar(avatarGO, AvatarConfig);
		}

		[CompilerGenerated]
		private void _003CStart_003Em__15D(Location[] locs)
		{
			GameEvents.Invoke(new TrackingEvent
			{
				Type = TrackingEvent.TrackingType.GameStart,
				Arguments = new Dictionary<string, object>
				{
					{ "GameId", gameId },
					{ "Difficulty", 0 },
					{
						"LocationName",
						locs.FirstOrDefault(_003CStart_003Em__161).Name
					},
					{
						"GameName",
						locs.FirstOrDefault(_003CStart_003Em__162).GetGame(gameId).Name
					}
				}
			});
		}

		[CompilerGenerated]
		private void _003COnDestroy_003Em__15E(Location[] locs)
		{
			GameEvents.Invoke(new TrackingEvent
			{
				Type = TrackingEvent.TrackingType.GameQuit,
				Arguments = new Dictionary<string, object>
				{
					{ "GameId", gameId },
					{ "Difficulty", 0 },
					{
						"Time",
						Time.timeSinceLevelLoad
					},
					{
						"LocationName",
						locs.FirstOrDefault(_003COnDestroy_003Em__163).Name
					},
					{
						"GameName",
						locs.FirstOrDefault(_003COnDestroy_003Em__164).GetGame(gameId).Name
					}
				}
			});
		}

		[CompilerGenerated]
		private void _003CSaveAvatar_003Em__15F()
		{
			SingletonMonobehaviour<PhotoBooth>.Instance.SayCheese(AvatarConfig, _003CSaveAvatar_003Em__165);
		}

		[CompilerGenerated]
		private void _003CGoToHub_003Em__160(Texture2D mugshot)
		{
			AvatarSystem.SavePlayerConfiguration(AvatarConfig, mugshot);
			GameEvents.Invoke(new TrackingEvent
			{
				Type = TrackingEvent.TrackingType.AvatarSaved
			});
			Time.timeScale = 1f;
			Application.LoadLevel("Hub");
		}

		[CompilerGenerated]
		private bool _003CStart_003Em__161(Location l)
		{
			return l.Games.Any(_003CStart_003Em__166);
		}

		[CompilerGenerated]
		private bool _003CStart_003Em__162(Location l)
		{
			return l.Games.Any(_003CStart_003Em__167);
		}

		[CompilerGenerated]
		private bool _003COnDestroy_003Em__163(Location l)
		{
			return l.Games.Any(_003COnDestroy_003Em__168);
		}

		[CompilerGenerated]
		private bool _003COnDestroy_003Em__164(Location l)
		{
			return l.Games.Any(_003COnDestroy_003Em__169);
		}

		[CompilerGenerated]
		private void _003CSaveAvatar_003Em__165(Texture2D mugshot)
		{
			GameEvents.Invoke(new TrackingEvent
			{
				Type = TrackingEvent.TrackingType.AvatarSaved
			});
			AvatarSystem.SavePlayerConfiguration(AvatarConfig, mugshot);
			avatarAnimator.SetTrigger("SelectAvatar");
		}

		[CompilerGenerated]
		private bool _003CStart_003Em__166(Game loc)
		{
			return loc.Id == gameId;
		}

		[CompilerGenerated]
		private bool _003CStart_003Em__167(Game loc)
		{
			return loc.Id == gameId;
		}

		[CompilerGenerated]
		private bool _003COnDestroy_003Em__168(Game loc)
		{
			return loc.Id == gameId;
		}

		[CompilerGenerated]
		private bool _003COnDestroy_003Em__169(Game loc)
		{
			return loc.Id == gameId;
		}
	}
}
