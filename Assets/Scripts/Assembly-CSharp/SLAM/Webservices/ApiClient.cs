using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using LitJson;
using SLAM.Achievements;
using SLAM.Avatar;
using SLAM.Kart;
using SLAM.KartRacing;
using UnityEngine;

namespace SLAM.Webservices
{
	public static class ApiClient
	{
		[CompilerGenerated]
		private sealed class _003CGetUserProfile_003Ec__AnonStorey18E
		{
			internal Action<UserProfile> callback;

			internal void _003C_003Em__FF(int id)
			{
				if (id < 0)
				{
					callback(null);
				}
				else
				{
					SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(USER_PROFILE_URL, id), callback);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetUserId_003Ec__AnonStorey18F
		{
			internal Action<int> callback;

			internal void _003C_003Em__100(UserProfile sillyProfile)
			{
				if (callback != null)
				{
					if (sillyProfile != null)
					{
						callback(sillyProfile.Id);
					}
					else
					{
						callback(-1);
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CAuthenticate_003Ec__AnonStorey190
		{
			internal Action<bool> callback;

			internal void _003C_003Em__101(WebResponse response)
			{
				if (response.StatusCode == 200)
				{
					SingletonMonobehaviour<Webservice>.Instance.ReceiveToken(JsonMapper.ToObject<AuthToken>(response.Request.text).Token, JsonMapper.ToObject<AuthToken>(response.Request.text).SessionID);
					callback(true);
				}
				else
				{
					callback(false);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetFreeUserToken_003Ec__AnonStorey191
		{
			internal Action<string> callback;

			internal void _003C_003Em__103(AuthToken token)
			{
				SingletonMonobehaviour<Webservice>.Instance.ReceiveToken(token.Token, string.Empty, false);
				if (callback != null)
				{
					callback(token.Token);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CSaveKartConfiguration_003Ec__AnonStorey192
		{
			internal KartConfigurationData kartConfig;

			internal Action<KartConfigurationData> callback;

			internal void _003C_003Em__104(KartConfigurationData newConfig)
			{
				kartConfig.id = newConfig.id;
				if (callback != null)
				{
					callback(newConfig);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadGhostRecording_003Ec__AnonStorey193
		{
			internal Action<GhostRecordingData> callback;

			internal void _003C_003Em__105(WebResponse response)
			{
				if (callback == null)
				{
					return;
				}
				GhostRecordingData obj = default(GhostRecordingData);
				if (response.StatusCode == 200)
				{
					try
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						MemoryStream serializationStream = new MemoryStream(response.Request.bytes);
						obj = (GhostRecordingData)binaryFormatter.Deserialize(serializationStream);
					}
					catch (Exception ex)
					{
						Debug.LogError("LoadGhostRecording failed!" + Environment.NewLine + ex);
						obj = default(GhostRecordingData);
					}
				}
				callback(obj);
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadMugshot_003Ec__AnonStorey194
		{
			internal Action<Texture2D> callback;

			internal void _003C_003Em__106(WebResponse result)
			{
				callback(result.Request.textureNonReadable);
			}
		}

		[CompilerGenerated]
		private sealed class _003COpenPropositionPage_003Ec__AnonStorey195
		{
			internal int gameId;

			internal void _003C_003Em__108(WebConfiguration config)
			{
				string url = string.Format("{0}#demo=levelID-{1}", config.PropositionUrl, gameId);
				Application.OpenURL(url);
			}
		}

		[CompilerGenerated]
		private sealed class _003CvalidateResponseStatus_003Ec__AnonStorey196
		{
			internal Action<bool> callback;

			internal int requiredStatus;

			internal void _003C_003Em__10B(WebResponse response)
			{
				if (callback != null)
				{
					callback(response.StatusCode == requiredStatus);
				}
			}
		}

		[CompilerGenerated]
		private static Action<WebResponse> _003C_003Ef__am_0024cache1;

		[CompilerGenerated]
		private static Action<WebConfiguration> _003C_003Ef__am_0024cache2;

		[CompilerGenerated]
		private static Action<WebConfiguration> _003C_003Ef__am_0024cache3;

		[CompilerGenerated]
		private static Action<WebConfiguration> _003C_003Ef__am_0024cache4;

		public static int UserId
		{
			get
			{
				return UserProfile.Current.Id;
			}
		}

		public static string API_URL { get; private set; }

		private static string AUTHENTICATE_URL
		{
			get
			{
				return API_URL + "/authenticate/";
			}
		}

		private static string LOGOUT_URL
		{
			get
			{
				return API_URL + "/sanoma-account/logout/";
			}
		}

		private static string GET_FREE_TOKEN_URL
		{
			get
			{
				return API_URL + "/freeplay-auth/";
			}
		}

		private static string USER_ID_URL
		{
			get
			{
				return API_URL + "/users/id/";
			}
		}

		private static string USER_FIND_URL
		{
			get
			{
				return API_URL + "/users/?find={0}";
			}
		}

		private static string USER_PROFILE_URL
		{
			get
			{
				return API_URL + "/users/{0}/";
			}
		}

		private static string USER_FRIENDS_URL
		{
			get
			{
				return API_URL + "/users/{0}/friends/";
			}
		}

		private static string LOCATIONS_URL
		{
			get
			{
				return API_URL + "/locations/";
			}
		}

		private static string USER_GAME_URL
		{
			get
			{
				return API_URL + "/users/{0}/games/{1}/";
			}
		}

		private static string USER_GAME_SCENE_URL
		{
			get
			{
				return API_URL + "/users/{0}/games/?scene={1}";
			}
		}

		private static string USER_GAMES_URL
		{
			get
			{
				return API_URL + "/users/{0}/games/";
			}
		}

		private static string USER_JOBS_URL
		{
			get
			{
				return API_URL + "/users/{0}/jobs/";
			}
		}

		private static string SCORES_URL
		{
			get
			{
				return API_URL + "/users/{0}/scores/?complete={1}";
			}
		}

		private static string HIGHSCORES_URL
		{
			get
			{
				return API_URL + "/users/{0}/games/{1}/highscores/?level={2}&difficulty={3}";
			}
		}

		private static string GHOST_URL
		{
			get
			{
				return API_URL + "/users/{0}/ghosts/?game={1}&level={2}";
			}
		}

		private static string GHOST_URL2
		{
			get
			{
				return API_URL + "/users/{0}/ghosts/";
			}
		}

		private static string AVATAR_URL
		{
			get
			{
				return API_URL + "/users/{0}/avatar/";
			}
		}

		private static string WALLET_URL
		{
			get
			{
				return API_URL + "/users/{0}/wallet/";
			}
		}

		private static string SHOP_URL
		{
			get
			{
				return API_URL + "/shops/";
			}
		}

		private static string SHOPITEM_URL
		{
			get
			{
				return API_URL + "/shopitems/";
			}
		}

		private static string INVENTORY_URL
		{
			get
			{
				return API_URL + "/users/{0}/inventory/";
			}
		}

		private static string INVENTORY_BUY_URL
		{
			get
			{
				return API_URL + "/users/{0}/inventory/buy/";
			}
		}

		private static string MESSAGES_URL
		{
			get
			{
				return API_URL + "/users/{0}/messages/";
			}
		}

		private static string MESSAGES_DETAIL_URL
		{
			get
			{
				return API_URL + "/users/{0}/messages/{1}/";
			}
		}

		private static string MESSAGES_ADDFRIEND_URL
		{
			get
			{
				return API_URL + "/users/{0}/messages/addfriend/";
			}
		}

		private static string MESSAGES_ACCEPTFRIEND_URL
		{
			get
			{
				return API_URL + "/users/{0}/messages/acceptfriend/{1}/";
			}
		}

		private static string MESSAGES_CHALLENGEFRIEND_URL
		{
			get
			{
				return API_URL + "/users/{0}/messages/challengefriend/";
			}
		}

		private static string MESSAGES_CHALLENGERESPONSE_URL
		{
			get
			{
				return API_URL + "/users/{0}/messages/challengeresponse/";
			}
		}

		private static string ACHIEVEMENTS_URL
		{
			get
			{
				return API_URL + "/achievements/";
			}
		}

		private static string USER_ACHIEVEMENT_URL
		{
			get
			{
				return API_URL + "/users/{0}/achievements/{1}/";
			}
		}

		private static string USER_ACHIEVEMENTS_URL
		{
			get
			{
				return API_URL + "/users/{0}/achievements/";
			}
		}

		private static string USER_KART_URL
		{
			get
			{
				return API_URL + "/users/{0}/kart/";
			}
		}

		private static string USER_KART_DETAIL_URL
		{
			get
			{
				return API_URL + "/users/{0}/kart/{1}/";
			}
		}

		private static string CONFIGURATION_URL
		{
			get
			{
				return API_URL + "/configuration/";
			}
		}

		private static string TIP_A_PARENT_URL
		{
			get
			{
				return API_URL + "/send-mail/";
			}
		}

		static ApiClient()
		{
			API_URL = ((!Debug.isDebugBuild) ? "https://www.duckworld.com/api/1.0" : "http://127.0.0.1:8000/api/1.0");
			Dictionary<string, string> dictionary = new Dictionary<string, string>
			{
				{ "--localhost", "http://127.0.0.1:8000/api/1.0" },
				{ "--test", "https://test.duckworld.com/api/1.0" },
				{ "--staging", "https://staging-new.duckworld.com/api/1.0" },
				{ "--production", "https://www.duckworld.com/api/1.0" }
			};
			IEnumerator enumerator = Environment.GetCommandLineArgs().GetEnumerator();
			while (enumerator.MoveNext())
			{
				string text = (string)enumerator.Current;
				string value;
				if (dictionary.TryGetValue(text, out value))
				{
					API_URL = value;
				}
				if (text == "--host" && enumerator.MoveNext())
				{
					API_URL = (string)enumerator.Current;
				}
			}
		}

		public static WebRequest GetWebConfiguration(Action<WebConfiguration> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", CONFIGURATION_URL, callback);
		}

		public static WebRequest GetUserProfile(Action<UserProfile> callback)
		{
			_003CGetUserProfile_003Ec__AnonStorey18E _003CGetUserProfile_003Ec__AnonStorey18E = new _003CGetUserProfile_003Ec__AnonStorey18E();
			_003CGetUserProfile_003Ec__AnonStorey18E.callback = callback;
			if (UserProfile.Current == null)
			{
				return GetUserId(_003CGetUserProfile_003Ec__AnonStorey18E._003C_003Em__FF);
			}
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(USER_PROFILE_URL, UserId), _003CGetUserProfile_003Ec__AnonStorey18E.callback);
		}

		public static WebRequest GetUserId(Action<int> callback)
		{
			_003CGetUserId_003Ec__AnonStorey18F _003CGetUserId_003Ec__AnonStorey18F = new _003CGetUserId_003Ec__AnonStorey18F();
			_003CGetUserId_003Ec__AnonStorey18F.callback = callback;
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest<UserProfile>("GET", USER_ID_URL, _003CGetUserId_003Ec__AnonStorey18F._003C_003Em__100);
		}

		public static WebRequest SavePlayerName(string newName, string newAddress, Action<bool> callback)
		{
			UserProfile.Current.Name = newName;
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("PATCH", string.Format(USER_PROFILE_URL, UserId.ToString()), new Dictionary<string, object>
			{
				{ "name", newName },
				{ "address", newAddress }
			}, validateResponseStatus(200, callback));
		}

		public static WebRequest GetFriends(Action<UserProfile[]> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(USER_FRIENDS_URL, UserId), callback);
		}

		public static WebRequest Authenticate(string username, string passwrd, Action<bool> callback)
		{
			_003CAuthenticate_003Ec__AnonStorey190 _003CAuthenticate_003Ec__AnonStorey = new _003CAuthenticate_003Ec__AnonStorey190();
			_003CAuthenticate_003Ec__AnonStorey.callback = callback;
			SingletonMonobehaviour<Webservice>.Instance.ReceiveToken(null, string.Empty);
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest<WebResponse>("POST", string.Format(AUTHENTICATE_URL), new Dictionary<string, object>
			{
				{ "username", username },
				{ "password", passwrd }
			}, _003CAuthenticate_003Ec__AnonStorey._003C_003Em__101);
		}

		public static WebRequest Logout()
		{
			Webservice instance = SingletonMonobehaviour<Webservice>.Instance;
			string url = string.Format(LOGOUT_URL);
			Dictionary<string, object> data = new Dictionary<string, object> { 
			{
				"session_id",
				PlayerPrefs.GetString("session_id")
			} };
			if (_003C_003Ef__am_0024cache1 == null)
			{
				_003C_003Ef__am_0024cache1 = _003CLogout_003Em__102;
			}
			return instance.DoRequest("POST", url, data, _003C_003Ef__am_0024cache1);
		}

		public static WebRequest GetFreeUserToken(Action<string> callback)
		{
			_003CGetFreeUserToken_003Ec__AnonStorey191 _003CGetFreeUserToken_003Ec__AnonStorey = new _003CGetFreeUserToken_003Ec__AnonStorey191();
			_003CGetFreeUserToken_003Ec__AnonStorey.callback = callback;
			SingletonMonobehaviour<Webservice>.Instance.ReceiveToken(null, string.Empty, false);
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest<AuthToken>("POST", GET_FREE_TOKEN_URL, _003CGetFreeUserToken_003Ec__AnonStorey._003C_003Em__103);
		}

		public static WebRequest GetLocations(Action<Location[]> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", LOCATIONS_URL, callback);
		}

		public static WebRequest GetUserSpecificDetailsForGame(int gameId, Action<UserGameDetails> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(USER_GAME_URL, UserId, gameId), callback);
		}

		public static WebRequest GetUserSpecificDetailsForGame(string sceneName, Action<UserGameDetails[]> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(USER_GAME_SCENE_URL, UserId, sceneName), callback);
		}

		public static WebRequest GetUserSpecificDetailsForAllGames(Action<UserGameDetails[]> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(USER_GAMES_URL, UserId, UserId), callback);
		}

		public static WebRequest GetUserSpecificDetailsForAllJobs(Action<Job[]> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("Get", string.Format(USER_JOBS_URL, UserId), callback);
		}

		public static WebRequest SubmitScore(int gameId, int score, string difficulty, int elapsedMilliseconds, bool gameCompleted, Action<UserScore> callback)
		{
			return SubmitScore(gameId, score, difficulty, "default", elapsedMilliseconds, gameCompleted, callback);
		}

		public static WebRequest SubmitScore(int gameId, int score, string difficulty, string levelName, int elapsedMilliseconds, bool gameCompleted, Action<UserScore> callback)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("game", gameId.ToString());
			dictionary.Add("score", score.ToString());
			dictionary.Add("difficulty", difficulty);
			dictionary.Add("time", elapsedMilliseconds.ToString());
			dictionary.Add("gameuser", UserId.ToString());
			dictionary.Add("level", levelName);
			Dictionary<string, object> data = dictionary;
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", string.Format(SCORES_URL, UserId, (!gameCompleted) ? "0" : "1"), data, callback);
		}

		public static WebRequest GetHighscores(int gameId, string difficulty, Action<HighScore[]> callback, string levelname = "default")
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(HIGHSCORES_URL, UserId.ToString(), gameId.ToString(), levelname, difficulty), callback);
		}

		public static WebRequest GetAllShopItems(Action<ShopItemData[]> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", SHOPITEM_URL, callback);
		}

		public static WebRequest GetAllShops(Action<ShopData[]> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", SHOP_URL, callback);
		}

		public static WebRequest GetShopItems(int shopid, Action<ShopData> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", SHOP_URL + shopid + "/", callback);
		}

		public static WebRequest GetWalletTotal(Action<int> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(WALLET_URL + "total/", UserId), callback);
		}

		public static WebRequest AddToWallet(int amount, Action<bool> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", string.Format(WALLET_URL, UserId), new Dictionary<string, object>
			{
				{
					"amount",
					amount.ToString()
				},
				{
					"gameuser",
					UserId.ToString()
				}
			}, validateResponseStatus(201, callback));
		}

		public static WebRequest PurchaseItem(int shopItemId, int shopId = 1, Action<bool> callback = null)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", string.Format(INVENTORY_BUY_URL, UserId), new Dictionary<string, object>
			{
				{
					"shop",
					shopId.ToString()
				},
				{
					"shopitem",
					shopItemId.ToString()
				},
				{
					"gameuser",
					UserId.ToString()
				}
			}, validateResponseStatus(201, callback));
		}

		public static WebRequest PurchaseItems(int[] shopItemIds, int shopId, Action<bool> callback = null)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", string.Format(INVENTORY_BUY_URL, UserId), new Dictionary<string, object> { { "shopitems", shopItemIds } }, validateResponseStatus(201, callback));
		}

		public static WebRequest AddItemToInventory(int shopItemId, Action<bool> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", string.Format(INVENTORY_URL, UserId), new Dictionary<string, object>
			{
				{
					"shopitem",
					shopItemId.ToString()
				},
				{
					"gameuser",
					UserId.ToString()
				}
			}, validateResponseStatus(201, callback));
		}

		public static WebRequest AddItemsToInventory(int[] shopItemIds, Action<bool> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", string.Format(INVENTORY_URL, UserId), new Dictionary<string, object> { { "shopitems", shopItemIds } }, validateResponseStatus(201, callback));
		}

		public static WebRequest GetPlayerPurchasedShopItems(Action<PurchasedShopItemData[]> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(INVENTORY_URL, UserId), callback);
		}

		public static WebRequest SaveKartConfiguration(KartConfigurationData kartConfig, byte[] image, Action<KartConfigurationData> callback)
		{
			_003CSaveKartConfiguration_003Ec__AnonStorey192 _003CSaveKartConfiguration_003Ec__AnonStorey = new _003CSaveKartConfiguration_003Ec__AnonStorey192();
			_003CSaveKartConfiguration_003Ec__AnonStorey.kartConfig = kartConfig;
			_003CSaveKartConfiguration_003Ec__AnonStorey.callback = callback;
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("config", JsonMapper.ToJson(_003CSaveKartConfiguration_003Ec__AnonStorey.kartConfig.Items));
			wWWForm.AddField("active", _003CSaveKartConfiguration_003Ec__AnonStorey.kartConfig.active.ToString());
			string hash = WebRequest.CalculateHash(wWWForm);
			wWWForm.AddBinaryData("image", image);
			if (_003CSaveKartConfiguration_003Ec__AnonStorey.kartConfig.id == -1)
			{
				return SingletonMonobehaviour<Webservice>.Instance.DoRequest<KartConfigurationData>("POST", string.Format(USER_KART_URL, UserId), hash, wWWForm, _003CSaveKartConfiguration_003Ec__AnonStorey._003C_003Em__104);
			}
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("PUT", string.Format(USER_KART_DETAIL_URL, UserId, _003CSaveKartConfiguration_003Ec__AnonStorey.kartConfig.id), hash, wWWForm, _003CSaveKartConfiguration_003Ec__AnonStorey.callback);
		}

		public static WebRequest DeleteKartConfiguration(KartConfigurationData kartConfig, Action<bool> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("DELETE", string.Format(USER_KART_DETAIL_URL, UserId, kartConfig.id), validateResponseStatus(204, callback));
		}

		public static WebRequest GetKartConfigurations(Action<KartConfigurationData[]> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(USER_KART_URL, UserId), callback);
		}

		public static WebRequest GetTimeTrialConfiguration(int gameId, string difficulty, Action<GhostRecording[]> callback)
		{
			return GetTimeTrialConfiguration(UserId, gameId, difficulty, callback);
		}

		public static WebRequest GetTimeTrialConfiguration(int userId, int gameId, string difficulty, Action<GhostRecording[]> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(GHOST_URL, userId, gameId, difficulty), callback);
		}

		public static WebRequest LoadGhostRecording(GhostRecording recording, Action<GhostRecordingData> callback)
		{
			_003CLoadGhostRecording_003Ec__AnonStorey193 _003CLoadGhostRecording_003Ec__AnonStorey = new _003CLoadGhostRecording_003Ec__AnonStorey193();
			_003CLoadGhostRecording_003Ec__AnonStorey.callback = callback;
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest<WebResponse>("GET", recording.RecordingUrl, _003CLoadGhostRecording_003Ec__AnonStorey._003C_003Em__105);
		}

		public static WebRequest SubmitGhostRecording(int gameId, int difficulty, int elapsedMilliseconds, GhostRecordingData recording, Action<string> callback)
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("game", gameId);
			wWWForm.AddField("level", difficulty);
			wWWForm.AddField("gameuser", UserId.ToString());
			wWWForm.AddField("kart_config", JsonMapper.ToJson(recording.Kart));
			wWWForm.AddField("avatar_config", JsonMapper.ToJson(AvatarSystem.GetPlayerConfiguration()));
			wWWForm.AddField("time", elapsedMilliseconds);
			string hash = WebRequest.CalculateHash(wWWForm);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream memoryStream = new MemoryStream();
			binaryFormatter.Serialize(memoryStream, recording);
			wWWForm.AddBinaryData("coords", memoryStream.GetBuffer());
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", string.Format(GHOST_URL2, UserId), hash, wWWForm, callback);
		}

		public static float FixOutOfBoundsFloat(float inp)
		{
			if ((double)inp > 1.0)
			{
				float num = 1f;
				while (inp > num)
				{
					num *= 10f;
				}
				return inp / num;
			}
			return inp;
		}

		public static WebRequest SaveAvatarConfiguration(AvatarConfigurationData avatarConfig, byte[] mugshot, Action<string> callback)
		{
			WWWForm wWWForm = new WWWForm();
			avatarConfig.SkinColor.r = FixOutOfBoundsFloat(avatarConfig.SkinColor.r);
			avatarConfig.SkinColor.g = FixOutOfBoundsFloat(avatarConfig.SkinColor.g);
			avatarConfig.SkinColor.b = FixOutOfBoundsFloat(avatarConfig.SkinColor.b);
			wWWForm.AddField("config", JsonMapper.ToJson(avatarConfig));
			wWWForm.AddField("gender", avatarConfig.Gender.ToString());
			wWWForm.AddField("race", avatarConfig.Race.ToString());
			wWWForm.AddField("gameuser", UserId.ToString());
			string hash = WebRequest.CalculateHash(wWWForm);
			wWWForm.AddBinaryData("mugshot", mugshot);
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("PUT", string.Format(AVATAR_URL, UserId), hash, wWWForm, callback);
		}

		public static WebRequest GetAvatarConfiguration(Action<PlayerAvatarData> callback)
		{
			return GetAvatarConfiguration(UserId, callback);
		}

		public static WebRequest GetAvatarConfiguration(int uid, Action<PlayerAvatarData> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(AVATAR_URL, uid), callback);
		}

		public static WebRequest LoadMugshot(string url, Action<Texture2D> callback)
		{
			_003CLoadMugshot_003Ec__AnonStorey194 _003CLoadMugshot_003Ec__AnonStorey = new _003CLoadMugshot_003Ec__AnonStorey194();
			_003CLoadMugshot_003Ec__AnonStorey.callback = callback;
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest<WebResponse>("GET", url, _003CLoadMugshot_003Ec__AnonStorey._003C_003Em__106);
		}

		public static WebRequest GetAllMessages(Action<Message[]> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(MESSAGES_URL, UserId), callback);
		}

		public static WebRequest GetUnarchivedMessages(Action<Message[]> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(MESSAGES_URL, UserId) + "?archived=false", callback);
		}

		public static WebRequest SendFriendRequest(int recipientId, Action<bool> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", string.Format(MESSAGES_ADDFRIEND_URL, UserId), new Dictionary<string, object> { 
			{
				"recipient",
				recipientId.ToString()
			} }, validateResponseStatus(201, callback));
		}

		public static WebRequest AcceptFriendRequest(int messageId, Action<bool> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(MESSAGES_ACCEPTFRIEND_URL, UserId, messageId), validateResponseStatus(200, callback));
		}

		public static WebRequest ChallengeFriend(int recipientId, int gameId, int score, string difficulty, Action<bool> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", string.Format(MESSAGES_CHALLENGEFRIEND_URL, UserId), new Dictionary<string, object>
			{
				{
					"recipient",
					recipientId.ToString()
				},
				{
					"game",
					gameId.ToString()
				},
				{
					"score_sender",
					score.ToString()
				},
				{ "difficulty", difficulty }
			}, validateResponseStatus(201, callback));
		}

		public static WebRequest SendChallengeResult(int recipientId, int game, string difficulty, int score_sender, int score_recipient, Action<bool> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", string.Format(MESSAGES_CHALLENGERESPONSE_URL, UserId), new Dictionary<string, object>
			{
				{
					"recipient",
					recipientId.ToString()
				},
				{
					"game",
					game.ToString()
				},
				{
					"difficulty",
					difficulty.ToString()
				},
				{
					"score_sender",
					score_sender.ToString()
				},
				{
					"score_recipient",
					score_recipient.ToString()
				}
			}, validateResponseStatus(201, callback));
		}

		public static WebRequest UpdateScoreRecipientForChallenge(int messageId, int score_recipient, Action<bool> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("PATCH", string.Format(MESSAGES_DETAIL_URL, UserId, messageId), new Dictionary<string, object> { 
			{
				"score_recipient",
				score_recipient.ToString()
			} }, validateResponseStatus(201, callback));
		}

		public static WebRequest ArchiveMessage(int messageId, Action<bool> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("PATCH", string.Format(MESSAGES_DETAIL_URL, UserId, messageId), new Dictionary<string, object> { 
			{
				"archived",
				bool.TrueString
			} }, validateResponseStatus(200, callback));
		}

		public static WebRequest DeleteMessage(int messageId, Action<bool> callback)
		{
			Debug.LogWarning("There is no endpoint for deleting messages yet!");
			if (callback != null)
			{
				callback(true);
			}
			return null;
		}

		public static WebRequest SearchPlayerByName(string name, Action<UserProfile[]> callback)
		{
			name = Uri.EscapeDataString(name);
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(USER_FIND_URL, name), callback);
		}

		public static WebRequest SetAchievementProgress(UserAchievement achievement, Action<UserAchievement> callback)
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("game_user", UserId);
			wWWForm.AddField("achievement", achievement.Info.Id);
			wWWForm.AddField("progress", achievement.Progress.ToString());
			wWWForm.AddField("completed", achievement.Completed.ToString());
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("PUT", string.Format(USER_ACHIEVEMENT_URL, UserId, achievement.Info.Id), wWWForm, callback);
		}

		public static WebRequest GetAchievements(Action<UserAchievement[]> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(USER_ACHIEVEMENTS_URL, UserId), callback);
		}

		public static void OpenRegisterPage()
		{
			if (_003C_003Ef__am_0024cache2 == null)
			{
				_003C_003Ef__am_0024cache2 = _003COpenRegisterPage_003Em__107;
			}
			DataStorage.GetWebConfiguration(_003C_003Ef__am_0024cache2);
		}

		public static void OpenPropositionPage(int gameId)
		{
			_003COpenPropositionPage_003Ec__AnonStorey195 _003COpenPropositionPage_003Ec__AnonStorey = new _003COpenPropositionPage_003Ec__AnonStorey195();
			_003COpenPropositionPage_003Ec__AnonStorey.gameId = gameId;
			DataStorage.GetWebConfiguration(_003COpenPropositionPage_003Ec__AnonStorey._003C_003Em__108);
		}

		public static void OpenForgotPasswordPage()
		{
			if (_003C_003Ef__am_0024cache3 == null)
			{
				_003C_003Ef__am_0024cache3 = _003COpenForgotPasswordPage_003Em__109;
			}
			DataStorage.GetWebConfiguration(_003C_003Ef__am_0024cache3);
		}

		public static void OpenHelpPage()
		{
			if (_003C_003Ef__am_0024cache4 == null)
			{
				_003C_003Ef__am_0024cache4 = _003COpenHelpPage_003Em__10A;
			}
			DataStorage.GetWebConfiguration(_003C_003Ef__am_0024cache4);
		}

		public static WebRequest TipAParent(string recipient, Action<bool> callback)
		{
			return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", TIP_A_PARENT_URL, new Dictionary<string, object>
			{
				{ "toname", recipient },
				{ "subject", "Tip DuckWorld.com" }
			}, validateResponseStatus(200, callback));
		}

		private static Action<WebResponse> validateResponseStatus(int requiredStatus, Action<bool> callback)
		{
			_003CvalidateResponseStatus_003Ec__AnonStorey196 _003CvalidateResponseStatus_003Ec__AnonStorey = new _003CvalidateResponseStatus_003Ec__AnonStorey196();
			_003CvalidateResponseStatus_003Ec__AnonStorey.callback = callback;
			_003CvalidateResponseStatus_003Ec__AnonStorey.requiredStatus = requiredStatus;
			return _003CvalidateResponseStatus_003Ec__AnonStorey._003C_003Em__10B;
		}

		[CompilerGenerated]
		private static void _003CLogout_003Em__102(WebResponse response)
		{
		}

		[CompilerGenerated]
		private static void _003COpenRegisterPage_003Em__107(WebConfiguration config)
		{
			Application.OpenURL(config.RegisterUrl);
		}

		[CompilerGenerated]
		private static void _003COpenForgotPasswordPage_003Em__109(WebConfiguration config)
		{
			Application.OpenURL(config.PasswordForgetUrl);
		}

		[CompilerGenerated]
		private static void _003COpenHelpPage_003Em__10A(WebConfiguration config)
		{
			Application.OpenURL(config.HelpUrl);
		}
	}
}
