using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.ConnectThePipes
{
	public class CTPWaterFlowManager : MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class _003CdoDrainEffect_003Ec__Iterator54 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal CTPPipe[] _003CfilledPipes_003E__0;

			internal Stopwatch _003Csw_003E__1;

			internal int _003Ci_003E__2;

			internal int _0024PC;

			internal object _0024current;

			internal CTPWaterFlowManager _003C_003Ef__this;

			public static Func<CTPPipe, bool> _003C_003Ef__am_0024cache6;

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
				//Discarded unreachable code: IL_0147
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
				{
					CTPPipe[] componentsInChildren = _003C_003Ef__this.currentLevelInstance.GetComponentsInChildren<CTPPipe>();
					if (_003C_003Ef__am_0024cache6 == null)
					{
						_003C_003Ef__am_0024cache6 = _003C_003Em__3F;
					}
					_003CfilledPipes_003E__0 = componentsInChildren.Where(_003C_003Ef__am_0024cache6).ToArray();
					GameEvents.Invoke(new ConnectThePipesGame.WaterFlowStopped());
					_003Csw_003E__1 = new Stopwatch(_003C_003Ef__this.waterDrainTime);
					AudioController.Stop("CTP_water_rushing_through_pipes_loop");
					goto IL_00ba;
				}
				case 1u:
					Shader.SetGlobalFloat("_Drain", _003Csw_003E__1.Progress);
					goto IL_00ba;
				case 2u:
					{
						_003C_003Ef__this.IsWaterFlowing = false;
						_003C_003Ef__this.onWaterStoppedDraining();
						_0024PC = -1;
						break;
					}
					IL_00ba:
					if ((bool)_003Csw_003E__1)
					{
						_0024current = null;
						_0024PC = 1;
					}
					else
					{
						for (_003Ci_003E__2 = 0; _003Ci_003E__2 < _003CfilledPipes_003E__0.Length; _003Ci_003E__2++)
						{
							_003CfilledPipes_003E__0[_003Ci_003E__2].ResetWater();
						}
						_0024current = new WaitForSeconds(1f);
						_0024PC = 2;
					}
					return true;
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

			public static bool _003C_003Em__3F(CTPPipe pipe)
			{
				return pipe.HasWaterInTube();
			}
		}

		[SerializeField]
		private GameObject waterFillParticlePrefab;

		[SerializeField]
		private GameObject waterEndParticlePrefab;

		[SerializeField]
		private float waterDrainTime = 3f;

		[SerializeField]
		private GameObject[] waterPuddlePrefabs;

		[SerializeField]
		private AnimationCurve waterPuddleCurve;

		[SerializeField]
		private AnimationCurve waterJetCurve;

		private bool hasDeadEnd;

		private CTPPipe[,] pipeGrid;

		private GameObject currentLevelInstance;

		private float waterFlowStartTime;

		private float waterFlowDuration;

		[CompilerGenerated]
		private static Func<CTPEndPipe, bool> _003C_003Ef__am_0024cacheD;

		[CompilerGenerated]
		private static Func<CTPPipe, int> _003C_003Ef__am_0024cacheE;

		[CompilerGenerated]
		private static Func<CTPPipe, int> _003C_003Ef__am_0024cacheF;

		public bool IsWaterFlowing { get; protected set; }

		public int ActiveStreams { get; protected set; }

		public void StartWaterFlowFromBeginPipes(GameObject currentLevelInstance, float startWaterWaitTime)
		{
			StartCoroutine(doStartWaterFlowFromBeginPipes(currentLevelInstance, startWaterWaitTime));
		}

		private IEnumerator doStartWaterFlowFromBeginPipes(GameObject currentLevelInstance, float startWaterWaitTime)
		{
			yield return new WaitForSeconds(startWaterWaitTime);
			AudioController.Play("CTP_water_rushing_through_pipes_loop");
			Shader.SetGlobalFloat("_Drain", 0f);
			this.currentLevelInstance = currentLevelInstance;
			IsWaterFlowing = true;
			setupPipeGrid();
			hasDeadEnd = false;
			ActiveStreams = 0;
			waterFlowStartTime = Time.time;
			GameEvents.Invoke(new ConnectThePipesGame.WaterFlowStarted());
			CTPPipe[] componentsInChildren = currentLevelInstance.GetComponentsInChildren<CTPPipe>();
			foreach (CTPPipe pipe in componentsInChildren)
			{
				if (pipe is CTPBeginPipe)
				{
					StartCoroutine(doWaterFlow(pipe.transform.localPosition, Vector3.right));
				}
			}
		}

		public void StartWaterFlowFromPosition(Vector3 pos, Vector3 inDir, int startDelay = 0)
		{
			StartCoroutine(doWaterFlow(pos, inDir, startDelay));
		}

		public void StartWaterDrainEffect()
		{
			StartCoroutine(doDrainEffect());
		}

		private IEnumerator doDrainEffect()
		{
			CTPPipe[] componentsInChildren = currentLevelInstance.GetComponentsInChildren<CTPPipe>();
			if (_003CdoDrainEffect_003Ec__Iterator54._003C_003Ef__am_0024cache6 == null)
			{
				_003CdoDrainEffect_003Ec__Iterator54._003C_003Ef__am_0024cache6 = _003CdoDrainEffect_003Ec__Iterator54._003C_003Em__3F;
			}
			CTPPipe[] filledPipes = componentsInChildren.Where(_003CdoDrainEffect_003Ec__Iterator54._003C_003Ef__am_0024cache6).ToArray();
			GameEvents.Invoke(new ConnectThePipesGame.WaterFlowStopped());
			Stopwatch sw = new Stopwatch(waterDrainTime);
			AudioController.Stop("CTP_water_rushing_through_pipes_loop");
			while ((bool)sw)
			{
				yield return null;
				Shader.SetGlobalFloat("_Drain", sw.Progress);
			}
			for (int i = 0; i < filledPipes.Length; i++)
			{
				filledPipes[i].ResetWater();
			}
			yield return new WaitForSeconds(1f);
			IsWaterFlowing = false;
			onWaterStoppedDraining();
		}

		private IEnumerator doWaterFlow(Vector3 pos, Vector3 inDir, int startDelay = 0)
		{
			GameObject waterFlowParticles = null;
			if (getPipeAt(pos) != null)
			{
				waterFlowParticles = UnityEngine.Object.Instantiate(waterFillParticlePrefab, getPipeAt(pos).transform.position, getPipeAt(pos).transform.rotation) as GameObject;
				StartCoroutine(setActiveAfter(waterFlowParticles, (float)startDelay * 0.5f));
			}
			ActiveStreams++;
			Vector3 outDir = Vector3.zero;
			int count = 0;
			while (getPipeAt(pos) != null && getPipeAt(pos).CanFlowWater(inDir, out outDir))
			{
				getPipeAt(pos).StartDoFillWaterEffect(inDir, waterFlowParticles, startDelay + count++);
				yield return null;
				pos += outDir;
				inDir = outDir;
			}
			float waitTime = ((float)startDelay + (float)count) * 0.5f;
			if (waterFlowDuration < waitTime)
			{
				waterFlowDuration = waitTime;
			}
			yield return new WaitForSeconds(waitTime);
			if (waterFlowParticles != null && waterFlowParticles.transform.HasComponent<ParticleSystem>())
			{
				waterFlowParticles.GetComponent<ParticleSystem>().Stop(true);
			}
			if (getPipeAt(pos) == null || (!getPipeAt(pos).HasComponent<CTPEndPipe>() && outDir == Vector3.zero))
			{
				StartCoroutine(makeWaterPuddleAt(getPipeAt(pos - inDir), inDir));
				StartCoroutine(spawnWaterJetAt(getPipeAt(pos - inDir), inDir));
				AudioController.Play("CTP_water_spillage", getPipeAt(pos - inDir).transform);
				hasDeadEnd = true;
			}
			if (--ActiveStreams <= 0)
			{
				onWaterStoppedFlowing();
			}
		}

		private IEnumerator setActiveAfter(GameObject waterFlowParticles, float startDelay)
		{
			waterFlowParticles.SetActive(false);
			yield return new WaitForSeconds(startDelay);
			waterFlowParticles.SetActive(true);
		}

		private void onWaterStoppedFlowing()
		{
			if (!hasDeadEnd)
			{
				CTPEndPipe[] componentsInChildren = currentLevelInstance.GetComponentsInChildren<CTPEndPipe>();
				if (_003C_003Ef__am_0024cacheD == null)
				{
					_003C_003Ef__am_0024cacheD = _003ConWaterStoppedFlowing_003Em__3C;
				}
				if (componentsInChildren.All(_003C_003Ef__am_0024cacheD))
				{
					IsWaterFlowing = false;
					GameEvents.Invoke(new ConnectThePipesGame.LevelCompletedEvent());
					return;
				}
			}
			StartWaterDrainEffect();
		}

		private IEnumerator spawnWaterJetAt(CTPPipe pipe, Vector3 lookDir)
		{
			float duration = waterFlowStartTime - Time.time + waterFlowDuration + waterDrainTime;
			Stopwatch sw = new Stopwatch(duration);
			ParticleSystem particles = (UnityEngine.Object.Instantiate(waterEndParticlePrefab, pipe.transform.position + lookDir / 2f, Quaternion.LookRotation(lookDir)) as GameObject).GetComponentInChildren<ParticleSystem>();
			float startSpeed = particles.startSpeed;
			float startSize = particles.startSize;
			while ((bool)sw)
			{
				yield return null;
				float t = 1f - waterJetCurve.Evaluate(sw.Progress);
				particles.startSpeed = startSpeed * t;
				particles.startSize = startSize * t;
			}
			particles.Stop(true);
		}

		private IEnumerator makeWaterPuddleAt(CTPPipe pipe, Vector3 lookDir)
		{
			GameObject puddle = UnityEngine.Object.Instantiate(waterPuddlePrefabs.GetRandom());
			puddle.transform.position = pipe.transform.position + lookDir + new Vector3(0f, -0.25f);
			Material mat = puddle.GetComponentInChildren<Renderer>().material;
			float duration = waterFlowStartTime - Time.time + waterFlowDuration + waterDrainTime;
			Stopwatch sw = new Stopwatch(duration);
			float cur = 1f - waterPuddleCurve.Evaluate(0f);
			while ((bool)sw)
			{
				yield return null;
				float next = 1f - waterPuddleCurve.Evaluate(sw.Progress);
				if (next > cur)
				{
					mat.SetInt("_UseAlpha", 1);
				}
				else
				{
					mat.SetInt("_UseAlpha", 0);
				}
				mat.SetFloat("_Progress", next);
				cur = next;
			}
			UnityEngine.Object.Destroy(puddle);
		}

		private void onWaterStoppedDraining()
		{
			GameEvents.Invoke(new ConnectThePipesGame.LevelFailedEvent());
		}

		private CTPPipe getPipeAt(Vector3 pos)
		{
			int num = Mathf.RoundToInt(pos.x);
			int num2 = Mathf.RoundToInt(0f - pos.z);
			if (num >= 0 && num2 >= 0 && num < pipeGrid.GetLength(0) && num2 < pipeGrid.GetLength(1))
			{
				return pipeGrid[num, num2];
			}
			return null;
		}

		private void setupPipeGrid()
		{
			CTPPipe[] componentsInChildren = currentLevelInstance.GetComponentsInChildren<CTPPipe>();
			if (_003C_003Ef__am_0024cacheE == null)
			{
				_003C_003Ef__am_0024cacheE = _003CsetupPipeGrid_003Em__3D;
			}
			int num = componentsInChildren.Max(_003C_003Ef__am_0024cacheE);
			if (_003C_003Ef__am_0024cacheF == null)
			{
				_003C_003Ef__am_0024cacheF = _003CsetupPipeGrid_003Em__3E;
			}
			int num2 = componentsInChildren.Max(_003C_003Ef__am_0024cacheF);
			pipeGrid = new CTPPipe[num + 1, num2 + 1];
			CTPPipe[] array = componentsInChildren;
			foreach (CTPPipe cTPPipe in array)
			{
				pipeGrid[Mathf.RoundToInt(cTPPipe.transform.localPosition.x), Mathf.RoundToInt(0f - cTPPipe.transform.localPosition.z)] = cTPPipe;
			}
		}

		[CompilerGenerated]
		private static bool _003ConWaterStoppedFlowing_003Em__3C(CTPEndPipe p)
		{
			return p.HasWaterInTube();
		}

		[CompilerGenerated]
		private static int _003CsetupPipeGrid_003Em__3D(CTPPipe pipe)
		{
			return (int)Mathf.Abs(pipe.transform.localPosition.x);
		}

		[CompilerGenerated]
		private static int _003CsetupPipeGrid_003Em__3E(CTPPipe pipe)
		{
			return (int)Mathf.Abs(pipe.transform.localPosition.z);
		}
	}
}
