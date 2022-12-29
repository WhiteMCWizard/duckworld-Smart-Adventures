using System;
using System.Runtime.CompilerServices;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.ConnectThePipes
{
	public class CTPTutorialView01 : TutorialView
	{
		[CompilerGenerated]
		private sealed class _003ConPipeClicked_003Ec__AnonStorey15E
		{
			internal ConnectThePipesGame.PipeClickedEvent evt;

			internal bool _003C_003Em__45(PipesToTurn p)
			{
				return p.pipe == evt.pipe;
			}
		}

		[SerializeField]
		private ToolTip pipeLineToolTip;

		[SerializeField]
		private ToolTip pipeToolTip;

		[SerializeField]
		private ToolTip hydrantLineToolTip;

		[SerializeField]
		private ToolTip hydrantToolTip;

		[SerializeField]
		private PipesToTurn[] pipesToTurn;

		private CTPBeginPipe beginPipe;

		[CompilerGenerated]
		private static Func<PipesToTurn, bool> _003C_003Ef__am_0024cache6;

		private void OnEnable()
		{
			GameEvents.Subscribe<ConnectThePipesGame.PipeClickedEvent>(onPipeClicked);
			GameEvents.Subscribe<ConnectThePipesGame.WaterFlowStarted>(onWaterFlows);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<ConnectThePipesGame.PipeClickedEvent>(onPipeClicked);
			GameEvents.Unsubscribe<ConnectThePipesGame.WaterFlowStarted>(onWaterFlows);
		}

		protected override void Start()
		{
			base.Start();
			beginPipe = Controller<ConnectThePipesGame>().CurrentSettings.LevelRoot.GetComponentInChildren<CTPBeginPipe>();
			pipeLineToolTip.Show(pipesToTurn.First().pipe.transform);
			pipeToolTip.Show(pipeLineToolTip.GO.transform);
		}

		private void onPipeClicked(ConnectThePipesGame.PipeClickedEvent evt)
		{
			_003ConPipeClicked_003Ec__AnonStorey15E _003ConPipeClicked_003Ec__AnonStorey15E = new _003ConPipeClicked_003Ec__AnonStorey15E();
			_003ConPipeClicked_003Ec__AnonStorey15E.evt = evt;
			PipesToTurn pipesToTurn = this.pipesToTurn.FirstOrDefault(_003ConPipeClicked_003Ec__AnonStorey15E._003C_003Em__45);
			if (pipesToTurn != null)
			{
				pipesToTurn.Turns++;
			}
			PipesToTurn[] collection = this.pipesToTurn;
			if (_003C_003Ef__am_0024cache6 == null)
			{
				_003C_003Ef__am_0024cache6 = _003ConPipeClicked_003Em__46;
			}
			PipesToTurn pipesToTurn2 = collection.FirstOrDefault(_003C_003Ef__am_0024cache6);
			if (pipesToTurn2 != null)
			{
				hideAllToolTips();
				pipeLineToolTip.Show(pipesToTurn2.pipe.transform);
				pipeToolTip.Show(pipeLineToolTip.GO.transform);
			}
			else
			{
				hideAllToolTips();
				hydrantLineToolTip.Show(beginPipe.transform);
				hydrantToolTip.Show(hydrantLineToolTip.GO.transform);
			}
		}

		private void onWaterFlows(ConnectThePipesGame.WaterFlowStarted evt)
		{
			hideAllToolTips();
		}

		private void hideAllToolTips()
		{
			hydrantLineToolTip.Hide();
			pipeLineToolTip.Hide();
			pipeToolTip.Hide();
			hydrantToolTip.Hide();
		}

		[CompilerGenerated]
		private static bool _003ConPipeClicked_003Em__46(PipesToTurn p)
		{
			return !p.TurnedCorrectly;
		}
	}
}
