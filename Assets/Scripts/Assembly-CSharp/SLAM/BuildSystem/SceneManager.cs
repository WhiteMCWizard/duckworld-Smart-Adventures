using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SLAM.BuildSystem
{
	public class SceneManager : MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class _003CStart_003Ec__IteratorF8 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal int _0024PC;

			internal object _0024current;

			internal SceneManager _003C_003Ef__this;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return _0024current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return _0024current;
				}
			}

			public bool MoveNext()
			{
				//Discarded unreachable code: IL_0112
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					UnityEngine.Object.DontDestroyOnLoad(_003C_003Ef__this.gameObject);
					_003C_003Ef__this._progressbar.value = 0f;
					_003C_003Ef__this.loadingRequest = null;
					_0024current = null;
					_0024PC = 1;
					break;
				case 1u:
					if (!_003C_003Ef__this._preloading)
					{
						_0024current = _003C_003Ef__this.StartCoroutine(_003C_003Ef__this.loadRequestedScene());
						_0024PC = 2;
						break;
					}
					goto case 2u;
				case 2u:
					if (_003C_003Ef__this._callback != null)
					{
						_003C_003Ef__this._callback(_003C_003Ef__this.loadingRequest);
					}
					if (_003C_003Ef__this._preloading)
					{
						UnityEngine.Object.Destroy(_003C_003Ef__this.gameObject);
					}
					else
					{
						UnityEngine.Object.FindObjectOfType<LoadingScreenManager>().FadeLoadingScreenOut(_003C_003Em__C8);
					}
					_0024PC = -1;
					goto default;
				default:
					return false;
				}
				return true;
			}

			[DebuggerHidden]
			public void Dispose()
			{
				_0024PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			internal void _003C_003Em__C8()
			{
				UnityEngine.Object.Destroy(_003C_003Ef__this.gameObject);
			}
		}

		private const float progressionDelta = 1f;

		private static string scene = string.Empty;

		private static bool preloading;

		private static Action<AsyncOperation> callback;

		private static UIProgressBar progressbar;

		private float targetProgress;

		private AsyncOperation loadingRequest;

		private string _scene;

		private bool _preloading;

		private Action<AsyncOperation> _callback;

		private UIProgressBar _progressbar;

		[CompilerGenerated]
		private static Action _003C_003Ef__am_0024cacheA;

		private void Awake()
		{
			_scene = scene;
			_preloading = preloading;
			_callback = callback;
			_progressbar = progressbar;
		}

		private IEnumerator Start()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			_progressbar.value = 0f;
			loadingRequest = null;
			yield return null;
			if (!_preloading)
			{
				yield return StartCoroutine(loadRequestedScene());
			}
			if (_callback != null)
			{
				_callback(loadingRequest);
			}
			if (_preloading)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				UnityEngine.Object.FindObjectOfType<LoadingScreenManager>().FadeLoadingScreenOut(((_003CStart_003Ec__IteratorF8)(object)this)._003C_003Em__C8);
			}
		}

		private IEnumerator loadRequestedScene()
		{
			loadingRequest = Application.LoadLevelAsync(_scene);
			loadingRequest.allowSceneActivation = false;
			while (!Mathf.Approximately(_progressbar.value, 0.9f))
			{
				targetProgress = Mathf.Lerp(0f, 1f, loadingRequest.progress);
				_progressbar.value = Mathf.MoveTowards(_progressbar.value, targetProgress, 1f * Time.deltaTime);
				yield return null;
			}
			_progressbar.value = 1f;
			loadingRequest.allowSceneActivation = true;
			while (!loadingRequest.isDone)
			{
				yield return null;
			}
		}

		public static Texture2D GetLoadingScreenTextureForLevel(string levelName)
		{
			SceneDataLibrary.LevelAssetVersion versionData = SceneDataLibrary.GetSceneDataLibrary().GetVersionData(levelName);
			string path = ((versionData != null && !(versionData.LoadingScreenName == "none")) ? versionData.LoadingScreenName : "hub_loading_screen");
			return Resources.Load<Texture2D>(path);
		}

		private static void LoadLevel(string scn, Action<AsyncOperation> cb = null)
		{
			LoadingScreenManager loadingScreenManager = UnityEngine.Object.FindObjectOfType<LoadingScreenManager>();
			scene = scn;
			callback = cb;
			progressbar = loadingScreenManager.ProgressBar;
			progressbar.value = 0f;
			if (preloading)
			{
				Application.LoadLevelAdditiveAsync("SceneLoader");
				return;
			}
			loadingScreenManager.SetTexture(GetLoadingScreenTextureForLevel(scene));
			if (_003C_003Ef__am_0024cacheA == null)
			{
				_003C_003Ef__am_0024cacheA = _003CLoadLevel_003Em__C7;
			}
			loadingScreenManager.FadeLoadingScreenIn(_003C_003Ef__am_0024cacheA);
		}

		public static void Load(string scn, Action<AsyncOperation> cb = null)
		{
			preloading = false;
			LoadLevel(scn, cb);
		}

		public static void Preload(string scn, Action<AsyncOperation> cb = null)
		{
		}

		[CompilerGenerated]
		private static void _003CLoadLevel_003Em__C7()
		{
			Application.LoadLevelAsync("SceneLoader");
		}
	}
}
