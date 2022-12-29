using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.AssemblyLine
{
	public class ALTutorialColorMessage : TutorialView
	{
		[CompilerGenerated]
		private sealed class _003CStart_003Ec__Iterator18 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal ALRobotPart _003Cpart_003E__0;

			internal ALRobotPart _003CnewPart_003E__1;

			internal bool _003CmoreDifferentColorSpawned_003E__2;

			internal int _0024PC;

			internal object _0024current;

			internal ALTutorialColorMessage _003C_003Ef__this;

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
				//Discarded unreachable code: IL_011d
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_003C_003Ef__this.messageTooltip.ShowText(Localization.Get(_003C_003Ef__this.welcomeTranslationKey));
					_003Cpart_003E__0 = null;
					_003CnewPart_003E__1 = null;
					_0024current = CoroutineUtils.WaitForGameEvent<AssemblyLineGame.PartSpawnedEvent>(_003C_003Em__20);
					_0024PC = 1;
					break;
				case 1u:
					_0024current = _003Cpart_003E__0.WaitForPartToBeVisible();
					_0024PC = 2;
					break;
				case 2u:
					_003CmoreDifferentColorSpawned_003E__2 = false;
					goto case 3u;
				case 3u:
					if (!_003CmoreDifferentColorSpawned_003E__2)
					{
						_0024current = CoroutineUtils.WaitForGameEvent<AssemblyLineGame.PartSpawnedEvent>(_003C_003Em__21);
						_0024PC = 3;
					}
					else
					{
						_0024current = _003CnewPart_003E__1.WaitForPartToBeVisible();
						_0024PC = 4;
					}
					break;
				case 4u:
					_003C_003Ef__this.messageTooltip.ShowText(Localization.Get(_003C_003Ef__this.secondTranslationKey));
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

			internal void _003C_003Em__20(AssemblyLineGame.PartSpawnedEvent p)
			{
				_003Cpart_003E__0 = p.Part;
			}

			internal void _003C_003Em__21(AssemblyLineGame.PartSpawnedEvent p)
			{
				_003CmoreDifferentColorSpawned_003E__2 = p.Part.Kind != _003Cpart_003E__0.Kind;
				_003CnewPart_003E__1 = p.Part;
			}
		}

		[SerializeField]
		private MessageToolTip messageTooltip;

		[SerializeField]
		private string welcomeTranslationKey = "Je kunt enkel onderdelen van dezelfde kleur in een buis plaatsen.";

		[SerializeField]
		private string secondTranslationKey = "Plaats dit onderdeel in de lege buis.";

		private new IEnumerator Start()
		{
			messageTooltip.ShowText(Localization.Get(welcomeTranslationKey));
			ALRobotPart part = null;
			ALRobotPart newPart = null;
			yield return CoroutineUtils.WaitForGameEvent<AssemblyLineGame.PartSpawnedEvent>(((_003CStart_003Ec__Iterator18)(object)this)._003C_003Em__20);
			yield return part.WaitForPartToBeVisible();
			bool moreDifferentColorSpawned = false;
			while (!moreDifferentColorSpawned)
			{
				yield return CoroutineUtils.WaitForGameEvent<AssemblyLineGame.PartSpawnedEvent>(((_003CStart_003Ec__Iterator18)(object)this)._003C_003Em__21);
			}
			yield return newPart.WaitForPartToBeVisible();
			messageTooltip.ShowText(Localization.Get(secondTranslationKey));
		}
	}
}
