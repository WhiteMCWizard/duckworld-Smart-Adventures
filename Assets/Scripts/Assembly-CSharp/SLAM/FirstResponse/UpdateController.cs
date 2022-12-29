using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.FirstResponse
{
	public class UpdateController : MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class _003CdoUpdateSequence_003Ec__Iterator83 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal WebConfiguration config;

			internal string _003CinstallerUrl_003E__0;

			internal string _003CinstallerChecksum_003E__1;

			internal WWW _003Cwww_003E__2;

			internal string _003CinstallerFile_003E__3;

			internal string _003CinstallerPath_003E__4;

			internal Exception _003Cer_003E__5;

			internal int _0024PC;

			internal object _0024current;

			internal WebConfiguration _003C_0024_003Econfig;

			internal UpdateController _003C_003Ef__this;

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
				//Discarded unreachable code: IL_027f
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_003CinstallerUrl_003E__0 = config.WindowsInstallerUrl;
					_003CinstallerChecksum_003E__1 = config.WindowsInstallerChecksum;
					_0024current = null;
					_0024PC = 1;
					break;
				case 1u:
					_003C_003Ef__this.setStatus(Localization.Get("UI_INSTALL_DOWNLOADING_FILES"));
					_003Cwww_003E__2 = new WWW(_003CinstallerUrl_003E__0);
					goto case 2u;
				case 2u:
					if (!_003Cwww_003E__2.isDone)
					{
						_003C_003Ef__this.progressbar.value = _003Cwww_003E__2.progress;
						_0024current = null;
						_0024PC = 2;
					}
					else
					{
						_003C_003Ef__this.progressbar.value = _003Cwww_003E__2.progress;
						_003C_003Ef__this.setStatus(Localization.Get("UI_INSTALL_DONE_DOWNLOADING"));
						_0024current = null;
						_0024PC = 3;
					}
					break;
				case 3u:
					_003CinstallerFile_003E__3 = _003CinstallerUrl_003E__0.Split('/').Last();
					_003CinstallerFile_003E__3 = ((!(_003CinstallerFile_003E__3 != string.Empty)) ? "installer.exe" : _003CinstallerFile_003E__3);
					_003CinstallerPath_003E__4 = Path.Combine(Application.temporaryCachePath, _003CinstallerFile_003E__3);
					try
					{
						if (!Directory.Exists(Application.temporaryCachePath))
						{
							Directory.CreateDirectory(Application.temporaryCachePath);
						}
						UnityEngine.Debug.LogFormat("Downloaded {0} to {1}", _003Cwww_003E__2.url, _003CinstallerPath_003E__4);
						File.WriteAllBytes(_003CinstallerPath_003E__4, _003Cwww_003E__2.bytes);
						if (!(_003CinstallerChecksum_003E__1 == WebRequest.MD5(_003Cwww_003E__2.bytes)))
						{
							throw new Exception(Localization.Get("ERROR_DOWNLOAD_CORRUPTED"));
						}
						_003C_003Ef__this.setStatus(Localization.Get("UI_INSTALL_START"));
						Process.Start(_003CinstallerPath_003E__4);
						Application.Quit();
					}
					catch (Exception ex)
					{
						_003Cer_003E__5 = ex;
						GameEvents.Invoke(new PopupEvent(Localization.Get("UI_SOMETHING_WENT_WRONG"), Localization.Get("UI_ERROR_OCCURED") + _003Cer_003E__5.Message, Localization.Get("UI_OK"), _003C_003Em__64));
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

			internal void _003C_003Em__64()
			{
				Application.OpenURL(config.DownloadPageUrl);
				Application.Quit();
			}
		}

		[SerializeField]
		private UILabel statusLabel;

		[SerializeField]
		private UIProgressBar progressbar;

		private void Start()
		{
			DataStorage.GetWebConfiguration(_003CStart_003Em__63);
		}

		private IEnumerator doUpdateSequence(WebConfiguration config)
		{
			string installerUrl = config.WindowsInstallerUrl;
			string installerChecksum = config.WindowsInstallerChecksum;
			yield return null;
			setStatus(Localization.Get("UI_INSTALL_DOWNLOADING_FILES"));
			WWW www = new WWW(installerUrl);
			while (!www.isDone)
			{
				progressbar.value = www.progress;
				yield return null;
			}
			progressbar.value = www.progress;
			setStatus(Localization.Get("UI_INSTALL_DONE_DOWNLOADING"));
			yield return null;
			string installerFile = installerUrl.Split('/').Last();
			string installerPath = Path.Combine(path2: (!(installerFile != string.Empty)) ? "installer.exe" : installerFile, path1: Application.temporaryCachePath);
			try
			{
				if (!Directory.Exists(Application.temporaryCachePath))
				{
					Directory.CreateDirectory(Application.temporaryCachePath);
				}
				UnityEngine.Debug.LogFormat("Downloaded {0} to {1}", www.url, installerPath);
				File.WriteAllBytes(installerPath, www.bytes);
				if (installerChecksum == WebRequest.MD5(www.bytes))
				{
					setStatus(Localization.Get("UI_INSTALL_START"));
					Process.Start(installerPath);
					Application.Quit();
					yield break;
				}
				throw new Exception(Localization.Get("ERROR_DOWNLOAD_CORRUPTED"));
			}
			catch (Exception er)
			{
				GameEvents.Invoke(new PopupEvent(Localization.Get("UI_SOMETHING_WENT_WRONG"), Localization.Get("UI_ERROR_OCCURED") + er.Message, Localization.Get("UI_OK"), ((_003CdoUpdateSequence_003Ec__Iterator83)(object)this)._003C_003Em__64));
			}
		}

		private void setStatus(string status)
		{
			statusLabel.text = status;
		}

		[CompilerGenerated]
		private void _003CStart_003Em__63(WebConfiguration config)
		{
			StartCoroutine(doUpdateSequence(config));
		}
	}
}
