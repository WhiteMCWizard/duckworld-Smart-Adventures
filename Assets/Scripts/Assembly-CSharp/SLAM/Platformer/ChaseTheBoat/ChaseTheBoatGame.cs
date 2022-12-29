using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.Platformer.ChaseTheBoat
{
	public class ChaseTheBoatGame : PlatformerGame
	{
		[Serializable]
		public class ChaseTheBoatDifficultySettings : LevelSetting
		{
			public int Level = 1;

			public float CompletionTime = 300f;

			public GameObject LevelRoot;
		}

		[CompilerGenerated]
		private sealed class _003CDoFinishRoutine_003Ec__Iterator45 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal P_Finish finish;

			internal Vector3 _003Cfrom_003E__0;

			internal Vector3 _003Cto_003E__1;

			internal float _003Ctime_003E__2;

			internal bool _003Cfaded_003E__3;

			internal int _0024PC;

			internal object _0024current;

			internal P_Finish _003C_0024_003Efinish;

			internal ChaseTheBoatGame _003C_003Ef__this;

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
				//Discarded unreachable code: IL_01da
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_003C_003Ef__this.player.Pause();
					_003C_003Ef__this.timer.Pause();
					_003Cfrom_003E__0 = finish.entrance.transform.position;
					_003Cto_003E__1 = finish.transform.position + Vector3.forward * 1f;
					_003C_003Ef__this.player.GetComponent<Animator>().SetBool("Outro", true);
					_003C_003Ef__this.player.transform.rotation = Quaternion.AngleAxis(0f, Vector3.up);
					_003Ctime_003E__2 = 0f;
					goto case 1u;
				case 1u:
					if (_003Ctime_003E__2 < 1f)
					{
						_003Ctime_003E__2 += Time.deltaTime;
						_003C_003Ef__this.player.transform.position = Vector3.Lerp(_003Cfrom_003E__0, _003Cto_003E__1, _003Ctime_003E__2 / 1f);
						_0024current = null;
						_0024PC = 1;
						break;
					}
					_003Cfaded_003E__3 = false;
					_003C_003Ef__this.OpenView<FadeView>(_003C_003Em__3A);
					goto case 2u;
				case 2u:
					if (!_003Cfaded_003E__3)
					{
						_003C_003Ef__this.player.transform.position = _003Cto_003E__1;
						_0024current = null;
						_0024PC = 2;
						break;
					}
					_003C_003Ef__this.player.GetComponent<Animator>().SetBool("Walking", false);
					_003C_003Ef__this.CloseView<FadeView>();
					_003C_003Ef__this.Finish(true);
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

			internal void _003C_003Em__3A(View v)
			{
				_003Cfaded_003E__3 = true;
			}
		}

		private const int POINTS_PER_HEART = 100;

		private const int POINTS_PER_FEATHER = 10;

		private const int POINTS_PER_SECOND_LEFT = 10;

		[SerializeField]
		[Header("Chase the Boat Properties")]
		private ChaseTheBoatDifficultySettings[] settings;

		private int foundFeathers;

		private Alarm timer;

		public override LevelSetting[] Levels
		{
			get
			{
				return settings;
			}
		}

		public ChaseTheBoatDifficultySettings CurrentSettings
		{
			get
			{
				return SelectedLevel<ChaseTheBoatDifficultySettings>();
			}
		}

		public override int GameId
		{
			get
			{
				return 6;
			}
		}

		public override Dictionary<string, int> ScoreCategories
		{
			get
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				dictionary.Add(StringFormatter.GetLocalizationFormatted("CTB_VICTORYWINDOW_SCORE_COLLECTABLES_FOUND", foundFeathers, 10), foundFeathers * 10);
				dictionary.Add(StringFormatter.GetLocalizationFormatted("CTB_VICTORYWINDOW_SCORE_HEARTS_LEFT", hearts, 100), hearts * 100);
				dictionary.Add(StringFormatter.GetLocalizationFormatted("CTB_VICTORYWINDOW_SCORE_TIME_LEFT", Mathf.CeilToInt(timer.TimeLeft), 10), Mathf.CeilToInt(timer.TimeLeft) * 10);
				return dictionary;
			}
		}

		public override string IntroNPCKey
		{
			get
			{
				return "NPC_NAME_DONALD";
			}
		}

		public override string IntroTextKey
		{
			get
			{
				return "CTB_CINEMATICINTRO_TEXT";
			}
		}

		public override Portrait DuckCharacter
		{
			get
			{
				return Portrait.DonaldDuck;
			}
		}

		public override void Play(LevelSetting selectedLevel)
		{
			base.Play(selectedLevel);
			for (int i = 0; i < settings.Length; i++)
			{
				settings[i].LevelRoot.SetActive(settings[i].LevelRoot == SelectedLevel<ChaseTheBoatDifficultySettings>().LevelRoot);
			}
			timer = Alarm.Create();
			timer.StartCountdown(CurrentSettings.CompletionTime, OnTimerFinished);
			OpenView<TimerView>().SetTimer(timer);
			GetView<ChaseTheBoatHUDView>().UpdateFeathers(foundFeathers);
		}

		public override void Finish(bool succes)
		{
			base.Finish(succes);
			timer.Pause();
			GetView<TimerView>().SetTimer(null);
		}

		protected void CollectFeather()
		{
			foundFeathers++;
			GetView<ChaseTheBoatHUDView>().UpdateFeathers(foundFeathers);
		}

		private IEnumerator DoFinishRoutine(P_Finish finish)
		{
			player.Pause();
			timer.Pause();
			Vector3 from = finish.entrance.transform.position;
			Vector3 to = finish.transform.position + Vector3.forward * 1f;
			player.GetComponent<Animator>().SetBool("Outro", true);
			player.transform.rotation = Quaternion.AngleAxis(0f, Vector3.up);
			float time = 0f;
			while (time < 1f)
			{
				time += Time.deltaTime;
				player.transform.position = Vector3.Lerp(from, to, time / 1f);
				yield return null;
			}
			bool faded = false;
			OpenView<FadeView>(((_003CDoFinishRoutine_003Ec__Iterator45)(object)this)._003C_003Em__3A);
			while (!faded)
			{
				player.transform.position = to;
				yield return null;
			}
			player.GetComponent<Animator>().SetBool("Walking", false);
			CloseView<FadeView>();
			Finish(true);
		}

		private void OnTimerFinished()
		{
			Finish(false);
		}

		protected override void onPickupCollectedEvent(PickupCollectedEvent evt)
		{
			base.onPickupCollectedEvent(evt);
			if (evt.pickup is CTB_Feather)
			{
				CollectFeather();
			}
		}

		protected override void onFinishReachedEvent(FinishReachedEvent evt)
		{
			StartCoroutine(DoFinishRoutine(evt.Finish));
		}
	}
}
