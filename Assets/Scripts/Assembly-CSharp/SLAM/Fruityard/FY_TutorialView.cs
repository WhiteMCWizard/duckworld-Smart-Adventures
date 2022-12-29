using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.Fruityard
{
	public class FY_TutorialView : TutorialView
	{
		[CompilerGenerated]
		private sealed class _003Cstep1_003Ec__Iterator9C : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal int _0024PC;

			internal object _0024current;

			internal FY_TutorialView _003C_003Ef__this;

			public static Func<FYHelper, bool> _003C_003Ef__am_0024cache3;

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
				//Discarded unreachable code: IL_01af
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
				case 1u:
					if (_003C_003Ef__this.spot.RequiredActionIndex < _003C_003Ef__this.spot.RequiredActions.Length)
					{
						FYHelper[] helpers = _003C_003Ef__this.helpers;
						if (_003C_003Ef__am_0024cache3 == null)
						{
							_003C_003Ef__am_0024cache3 = _003C_003Em__75;
						}
						if (helpers.Count(_003C_003Ef__am_0024cache3) > 0)
						{
							_003C_003Ef__this.helperToolTip.Hide();
							_003C_003Ef__this.spotToolTip.Hide();
						}
						else if (_003C_003Ef__this.selectedHelper == _003C_003Ef__this.correctHelper)
						{
							if (!_003C_003Ef__this.spotToolTip.IsVisible)
							{
								_003C_003Ef__this.helperToolTip.Hide();
								_003C_003Ef__this.spotToolTip.Show(_003C_003Ef__this.spot.transform, _003C_003Ef__this.spotOffset);
							}
						}
						else if (!_003C_003Ef__this.helperToolTip.IsVisible)
						{
							_003C_003Ef__this.spotToolTip.Hide();
							_003C_003Ef__this.helperToolTip.Show(_003C_003Ef__this.correctHelper.transform, _003C_003Ef__this.helperOffset);
						}
						_003C_003Ef__this.correctHelper = _003C_003Ef__this.helpers.FirstOrDefault(_003C_003Em__76);
						_0024current = null;
						_0024PC = 1;
						return true;
					}
					_0024PC = -1;
					break;
				}
				return false;
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

			public static bool _003C_003Em__75(FYHelper h)
			{
				return h.IsPerformingAction;
			}

			internal bool _003C_003Em__76(FYHelper h)
			{
				return h.GetAppropiateAction(_003C_003Ef__this.spot) != FruityardGame.FYTreeAction.None;
			}
		}

		[SerializeField]
		private ToolTip helperToolTip;

		[SerializeField]
		private ToolTip spotToolTip;

		[SerializeField]
		private ToolTip seedToolTip;

		[SerializeField]
		private Vector3 spotOffset;

		[SerializeField]
		private Vector3 helperOffset;

		[SerializeField]
		private Transform cherryTarget;

		private FYSpot spot;

		private FYHelper[] helpers;

		private FYHelper selectedHelper;

		private FYHelper correctHelper;

		private void OnEnable()
		{
			GameEvents.Subscribe<FruityardGame.TreeTaskFailedEvent>(onTaskFailed);
			GameEvents.Subscribe<FruityardGame.HelperSelectedEvent>(onHelperSelected);
			GameEvents.Subscribe<FruityardGame.TreeTaskSucceededEvent>(onTaskSucceeded);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<FruityardGame.TreeTaskFailedEvent>(onTaskFailed);
			GameEvents.Unsubscribe<FruityardGame.HelperSelectedEvent>(onHelperSelected);
			GameEvents.Unsubscribe<FruityardGame.TreeTaskSucceededEvent>(onTaskSucceeded);
		}

		protected override void Start()
		{
			spot = UnityEngine.Object.FindObjectOfType<FYSpot>();
			helpers = UnityEngine.Object.FindObjectsOfType<FYHelper>();
			StartCoroutine(step1());
			StartCoroutine(step2());
		}

		private IEnumerator step1()
		{
			while (spot.RequiredActionIndex < spot.RequiredActions.Length)
			{
				FYHelper[] collection = helpers;
				if (_003Cstep1_003Ec__Iterator9C._003C_003Ef__am_0024cache3 == null)
				{
					_003Cstep1_003Ec__Iterator9C._003C_003Ef__am_0024cache3 = _003Cstep1_003Ec__Iterator9C._003C_003Em__75;
				}
				if (collection.Count(_003Cstep1_003Ec__Iterator9C._003C_003Ef__am_0024cache3) > 0)
				{
					helperToolTip.Hide();
					spotToolTip.Hide();
				}
				else if (selectedHelper == correctHelper)
				{
					if (!spotToolTip.IsVisible)
					{
						helperToolTip.Hide();
						spotToolTip.Show(spot.transform, spotOffset);
					}
				}
				else if (!helperToolTip.IsVisible)
				{
					spotToolTip.Hide();
					helperToolTip.Show(correctHelper.transform, helperOffset);
				}
				correctHelper = helpers.FirstOrDefault(((_003Cstep1_003Ec__Iterator9C)(object)this)._003C_003Em__76);
				yield return null;
			}
		}

		private IEnumerator step2()
		{
			yield return CoroutineUtils.WaitForGameEvent<FruityardGame.ShowSeedViewEvent>();
			seedToolTip.Show(cherryTarget);
			yield return CoroutineUtils.WaitForGameEvent<FruityardGame.SeedTreeEvent>();
			seedToolTip.Hide();
			StartCoroutine(step2());
		}

		private void onHelperSelected(FruityardGame.HelperSelectedEvent evt)
		{
			selectedHelper = evt.Helper;
		}

		private void onTaskSucceeded(FruityardGame.TreeTaskSucceededEvent evt)
		{
			helperToolTip.Hide();
			spotToolTip.Hide();
		}

		private void onTaskFailed(FruityardGame.TreeTaskFailedEvent evt)
		{
			helperToolTip.Hide();
			spotToolTip.Hide();
		}
	}
}
