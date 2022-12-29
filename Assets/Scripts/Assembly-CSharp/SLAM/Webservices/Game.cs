using LitJson;
using SLAM.Engine;
using SLAM.Slinq;

namespace SLAM.Webservices
{
	public class Game
	{
		public enum GameType
		{
			AdventureGame = 0,
			Shop = 1,
			Job = 2,
			LocationGame = 3,
			Event = 4
		}

		public enum GameCharacter
		{
			NPC_AVATAR_NAME = -1,
			None = 0,
			Major = 1,
			NPC_NAME_GRANDMA_DUCK = 2,
			NPC_NAME_GYRO_GEARLOOSE = 3,
			NPC_NAME_SCROOGE = 4,
			NPC_NAME_HUEY = 5,
			NPC_NAME_MAY = 6,
			NPC_NAME_CHRISQUIZ = 7,
			NPC_NAME_WARBOL = 8,
			NPC_NAME_GRANDMOGUL = 9,
			NPC_NAME_DONALD_DUCK = 10
		}

		[JsonName("free_to_play_levels")]
		public int[] FreeLevels;

		[JsonName("required_level_to_unlock")]
		public int RequiredDifficultyToUnlockNextGame;

		[JsonName("total_number_of_levels")]
		public int TotalLevels;

		[JsonName("location")]
		public int Location { get; private set; }

		[JsonName("id")]
		public int Id { get; private set; }

		[JsonName("name")]
		public string Name { get; private set; }

		[JsonName("scene")]
		public string SceneName { get; private set; }

		[JsonName("type")]
		public GameType Type { get; private set; }

		[JsonName("enabled")]
		public bool Enabled { get; private set; }

		[JsonName("is_unlocked")]
		public bool IsUnlocked { get; private set; }

		[JsonName("is_unlocked_sa")]
		public bool IsUnlockedSA { get; private set; }

		[JsonName("display_order")]
		public int SortOrder { get; set; }

		[JsonName("next_game")]
		public int? NextGameId { get; set; }

		[JsonName("previous_game")]
		public int? PreviousGameId { get; set; }

		[JsonName("scene_motioncomic")]
		public string SceneMotionComicName { get; set; }

		[JsonName("special_character")]
		public int _specialCharacterID { get; private set; }

		public int FreeLevelTo
		{
			get
			{
				return FreeLevels.Max();
			}
		}

		public GameCharacter SpecialCharacter
		{
			get
			{
				return (GameCharacter)_specialCharacterID;
			}
		}

		public bool IsPremiumAvailable
		{
			get
			{
				return true;
			}
		}

		public Game()
		{
		}

		public Game(int id, string name)
			: this()
		{
			Id = id;
			Name = name;
		}

		public Game(int id, string name, string sceneName)
			: this(id, name)
		{
			SceneName = sceneName;
		}

		public bool CanPlayLevel(GameController.LevelSetting level)
		{
			return FreeLevels.Contains(level.Index + 1);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
