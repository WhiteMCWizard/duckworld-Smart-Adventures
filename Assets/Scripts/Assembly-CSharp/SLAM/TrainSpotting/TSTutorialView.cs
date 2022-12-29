using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.TrainSpotting
{
	public class TSTutorialView : View
	{
		[CompilerGenerated]
		private sealed class _003CdoTutorialSequence_003Ec__Iterator145 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal TrainSpottingGame.TrainInfo _003Ctrain_003E__0;

			internal bool _003ChasDeparted_003E__1;

			internal bool _003CwasCorrect_003E__2;

			internal int _0024PC;

			internal object _0024current;

			internal TSTutorialView _003C_003Ef__this;

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
				//Discarded unreachable code: IL_02b5
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_003Ctrain_003E__0 = null;
					_0024current = CoroutineUtils.WaitForGameEvent<TrainSpottingGame.TrainQueuedEvent>(_003C_003Em__192);
					_0024PC = 1;
					break;
				case 1u:
					_003C_003Ef__this.Controller<TrainSpottingGame>().PauseTrains();
					_003C_003Ef__this.doSetText("De trein naar {0} is net aangekomen. We moeten hem eerst op een spoor zetten, klik op de trein in het bord om hem te selecteren.", _003Ctrain_003E__0.Destination);
					_0024current = CoroutineUtils.WaitForGameEvent<TrainSpottingGame.TrainScheduleItemClickedEvent>();
					_0024PC = 2;
					break;
				case 2u:
					_003C_003Ef__this.doSetText("Goed gedaan! Klik nu op het bordje van een spoor om de trein daar heen te laten gaan.");
					_0024current = CoroutineUtils.WaitForGameEvent<TrainSpottingGame.TrainArrivedEvent>();
					_0024PC = 3;
					break;
				case 3u:
					_003C_003Ef__this.doSetText(string.Format("De trein rijd nu naar spoor {0}, en moet om {1} wegrijden. Wacht tot de klok aangeeft dat het {1} is, en klik dan weer snel op het spoor om de trein te laten rijden.\nLet op! De passagiers moeten eerst in de trein zitten.", _003Ctrain_003E__0.Track.TrackName, _003C_003Ef__this.Controller<TrainSpottingGame>().GetFormattedTime(_003Ctrain_003E__0.TargetDepartureTime)));
					_003ChasDeparted_003E__1 = false;
					_003CwasCorrect_003E__2 = false;
					CoroutineUtils.WaitForGameEvent<TrainSpottingGame.TrainDepartedEvent>(_003C_003Em__193);
					goto case 4u;
				case 4u:
					if (_003C_003Ef__this.Controller<TrainSpottingGame>().AbsoluteElapsedTime < _003Ctrain_003E__0.TargetDepartureTime && !_003ChasDeparted_003E__1)
					{
						_0024current = null;
						_0024PC = 4;
						break;
					}
					if (!_003ChasDeparted_003E__1)
					{
						_003C_003Ef__this.doSetText(string.Format("De trein moet nu gaan, klik op spoor {0}!", _003Ctrain_003E__0.Track.TrackName));
						goto case 5u;
					}
					goto IL_01ec;
				case 5u:
					if (!_003ChasDeparted_003E__1)
					{
						_0024current = null;
						_0024PC = 5;
						break;
					}
					goto IL_01ec;
				case 6u:
					_003C_003Ef__this.Controller<TrainSpottingGame>().ResumeTrains();
					_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.doTutorialSequence());
					goto IL_02aa;
				case 7u:
					_003C_003Ef__this.Controller<TrainSpottingGame>().ResumeTrains();
					_0024current = new WaitForSeconds(2f);
					_0024PC = 8;
					break;
				case 8u:
					_003C_003Ef__this.Close();
					goto IL_02aa;
				default:
					{
						return false;
					}
					IL_02aa:
					_0024PC = -1;
					goto default;
					IL_01ec:
					if (!_003CwasCorrect_003E__2)
					{
						_0024current = _003C_003Ef__this.doSetText("Oef, de trein is niet op tijd weggereden. Probeer het opnieuw!");
						_0024PC = 6;
					}
					else
					{
						_0024current = _003C_003Ef__this.doSetText("Goed gedaan! Je bent nu klaar om Trainspotting te spelen.");
						_0024PC = 7;
					}
					break;
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

			internal void _003C_003Em__192(TrainSpottingGame.TrainQueuedEvent tq)
			{
				_003Ctrain_003E__0 = tq.TrainInfo;
			}

			internal void _003C_003Em__193(TrainSpottingGame.TrainDepartedEvent td)
			{
				_003CwasCorrect_003E__2 = td.WasOnTime;
				_003ChasDeparted_003E__1 = true;
			}
		}

		[SerializeField]
		private UILabel lblText;

		[SerializeField]
		private float lettersPerSecond = 0.005f;

		private void OnEnable()
		{
			StartCoroutine(doTutorialSequence());
		}

		private IEnumerator doTutorialSequence()
		{
			TrainSpottingGame.TrainInfo train = null;
			yield return CoroutineUtils.WaitForGameEvent<TrainSpottingGame.TrainQueuedEvent>(((_003CdoTutorialSequence_003Ec__Iterator145)(object)this)._003C_003Em__192);
			Controller<TrainSpottingGame>().PauseTrains();
			doSetText("De trein naar {0} is net aangekomen. We moeten hem eerst op een spoor zetten, klik op de trein in het bord om hem te selecteren.", train.Destination);
			yield return CoroutineUtils.WaitForGameEvent<TrainSpottingGame.TrainScheduleItemClickedEvent>();
			doSetText("Goed gedaan! Klik nu op het bordje van een spoor om de trein daar heen te laten gaan.");
			yield return CoroutineUtils.WaitForGameEvent<TrainSpottingGame.TrainArrivedEvent>();
			doSetText(string.Format("De trein rijd nu naar spoor {0}, en moet om {1} wegrijden. Wacht tot de klok aangeeft dat het {1} is, en klik dan weer snel op het spoor om de trein te laten rijden.\nLet op! De passagiers moeten eerst in de trein zitten.", train.Track.TrackName, Controller<TrainSpottingGame>().GetFormattedTime(train.TargetDepartureTime)));
			bool hasDeparted = false;
			bool wasCorrect = false;
			CoroutineUtils.WaitForGameEvent<TrainSpottingGame.TrainDepartedEvent>(((_003CdoTutorialSequence_003Ec__Iterator145)(object)this)._003C_003Em__193);
			while (Controller<TrainSpottingGame>().AbsoluteElapsedTime < train.TargetDepartureTime && !hasDeparted)
			{
				yield return null;
			}
			if (!hasDeparted)
			{
				doSetText(string.Format("De trein moet nu gaan, klik op spoor {0}!", train.Track.TrackName));
				while (!hasDeparted)
				{
					yield return null;
				}
			}
			if (!wasCorrect)
			{
				yield return doSetText("Oef, de trein is niet op tijd weggereden. Probeer het opnieuw!");
				Controller<TrainSpottingGame>().ResumeTrains();
				StartCoroutine(doTutorialSequence());
			}
			else
			{
				yield return doSetText("Goed gedaan! Je bent nu klaar om Trainspotting te spelen.");
				Controller<TrainSpottingGame>().ResumeTrains();
				yield return new WaitForSeconds(2f);
				Close();
			}
		}

		private Coroutine doSetText(string text, params object[] args)
		{
			StopCoroutine("setText");
			return StartCoroutine("setText", string.Format(text, args));
		}

		private IEnumerator setText(string text)
		{
			lblText.text = string.Empty;
			for (int i = 0; i < text.Length; i++)
			{
				lblText.text += text[i];
				yield return new WaitForSeconds(lettersPerSecond * Time.timeScale);
			}
		}
	}
}
