using System;
using System.Collections;
using SLAM.CameraSystem;
using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle
{
	public class MB_Flintheart : MonoBehaviour
	{
		[Serializable]
		public struct FlintheartTier
		{
			public float SafeJumpAngle;

			public float Radius;

			public Vector3 Center;
		}

		[SerializeField]
		private MB_PlayerController player;

		[SerializeField]
		private FlintheartTier[] tiers;

		[SerializeField]
		private MB_Cannon cannon;

		[SerializeField]
		private float movementSpeed = 3.5f;

		[SerializeField]
		private float cutsceneMovementSpeed = 7f;

		[SerializeField]
		private Animator animator;

		[SerializeField]
		private Transform gunObject;

		[SerializeField]
		private PrefabSpawner graphicsSpawner;

		[SerializeField]
		private MB_FlintheartIK flintheartIK;

		[SerializeField]
		private AnimationCurve tierSwitchCurveX;

		[SerializeField]
		private AnimationCurve tierSwitchCurveY;

		[SerializeField]
		private AnimationCurve tierSwitchCurveZ;

		[SerializeField]
		private AudioClip fireAudio;

		[SerializeField]
		private AudioClip shotgunAudio;

		private float initialDelay = 2f;

		private int currentTierIndex;

		private int shotPatternIndex;

		private bool invulnerable;

		private MonkeyBattleGame.MB_Settings settings;

		[SerializeField]
		private CameraBehaviour defaultBehaviour;

		[SerializeField]
		private CameraBehaviour finishCameraBehaviour;

		[SerializeField]
		private RotateAroundLookAtBehaviour tierJumpStartBehaviour;

		public float Health { get; protected set; }

		private void OnDrawGizmos()
		{
			int seed = UnityEngine.Random.seed;
			for (int i = 0; i < tiers.Length; i++)
			{
				UnityEngine.Random.seed = i;
				Gizmos.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
				drawCircleAt(tiers[i].Center, tiers[i].Radius);
			}
			Gizmos.color = Color.red;
			for (int j = 0; j < tiers.Length - 1; j++)
			{
				FlintheartTier tier = tiers[j];
				FlintheartTier tier2 = tiers[j + 1];
				Vector3 pointAtTier = getPointAtTier(tier, tier.SafeJumpAngle);
				Vector3 pointAtTier2 = getPointAtTier(tier2, tier.SafeJumpAngle);
				Vector3 from = pointAtTier;
				for (float num = 0f; num < 1f; num += 0.05f)
				{
					Vector3 vector = new Vector3(MathUtilities.LerpUnclamped(pointAtTier.x, pointAtTier2.x, tierSwitchCurveX.Evaluate(num)), MathUtilities.LerpUnclamped(pointAtTier.y, pointAtTier2.y, tierSwitchCurveY.Evaluate(num)), MathUtilities.LerpUnclamped(pointAtTier.z, pointAtTier2.z, tierSwitchCurveZ.Evaluate(num)));
					Gizmos.DrawLine(from, vector);
					from = vector;
				}
			}
			UnityEngine.Random.seed = seed;
		}

		private Vector3 getPointAtTier(FlintheartTier tier, float angle)
		{
			return tier.Center + new Vector3(Mathf.Sin(angle) * tier.Radius, 0f, Mathf.Cos(angle) * tier.Radius);
		}

		private void drawCircleAt(Vector3 center, float radius)
		{
			for (float num = 0f; num < (float)Math.PI * 2f; num += 0.1f)
			{
				Vector3 from = new Vector3(Mathf.Sin(num), 0f, Mathf.Cos(num)) * radius;
				Vector3 to = new Vector3(Mathf.Sin(num + 0.1f), 0f, Mathf.Cos(num + 0.1f)) * radius;
				from += center;
				to += center;
				Gizmos.DrawLine(from, to);
			}
		}

		private void Start()
		{
			GameObject gameObject = graphicsSpawner.Spawn();
			gameObject.SetLayerRecursively(base.gameObject.layer);
			gunObject.transform.parent = gameObject.transform.FindChildRecursively("Object_01_Loc");
			gunObject.transform.localPosition = Vector3.zero;
			gunObject.transform.localRotation = Quaternion.identity;
			animator.Rebind();
		}

		private void OnEnable()
		{
			GameEvents.Subscribe<MonkeyBattleGame.GameStartedEvent>(onGameStartedEvent);
		}

		private void OnDisable()
		{
			GameEvents.Unsubscribe<MonkeyBattleGame.GameStartedEvent>(onGameStartedEvent);
		}

		private void onGameStartedEvent(MonkeyBattleGame.GameStartedEvent evt)
		{
			settings = evt.settings;
			Health = 100f;
			StartCoroutine(behaviourLoop());
		}

		private void HitByBanana(MB_Banana other)
		{
			animator.SetTrigger("hit");
			GetComponent<PrefabSpawner>().SpawnOneAt<PrefabSpawner>(other.transform.position);
			UnityEngine.Object.Destroy(other.gameObject);
			if (!invulnerable)
			{
				Health -= settings.flintheartBananaDamage;
				if (Health <= 0f)
				{
					StopAllCoroutines();
					StartCoroutine(doFinishGame());
				}
				else if (Health <= 100f - ((float)currentTierIndex + 1f) * 100f / (float)tiers.Length)
				{
					StopAllCoroutines();
					StartCoroutine(doTierSwitchSequence());
				}
			}
		}

		private IEnumerator doFinishGame()
		{
			invulnerable = true;
			player.AreControlsLocked = true;
			yield return finishCameraBehaviour.CrossFadeIn(0.5f);
			animator.SetBool("died", true);
			yield return new WaitForSeconds(5f);
			UnityEngine.Object.FindObjectOfType<MonkeyBattleGame>().Finish(true);
		}

		private IEnumerator doTierSwitchSequence()
		{
			invulnerable = true;
			shotPatternIndex = 0;
			animator.SetBool("reloading", false);
			animator.SetBool("shooting", false);
			animator.SetBool("doingTierSwitch", true);
			if (settings.flintheartDoCinematicTierSwitches)
			{
				player.AreControlsLocked = true;
				tierJumpStartBehaviour.SetPositionOffset(new Vector3(-1f * (float)currentTierIndex, 4f - (float)currentTierIndex, 0f));
				tierJumpStartBehaviour.CrossFadeIn(0.5f);
			}
			FlintheartTier curTier = tiers[currentTierIndex];
			FlintheartTier nextTier = tiers[currentTierIndex + 1];
			Vector3 dir2 = player.transform.position - curTier.Center;
			yield return new WaitForSeconds(5f);
			float angle2 = Mathf.Atan2(dir2.x, dir2.z) * 57.29578f;
			float endAngle2 = curTier.SafeJumpAngle * 57.29578f;
			float delta2 = Mathf.Abs(angle2 - endAngle2);
			while (delta2 > 0f)
			{
				yield return null;
				float newAngle = Mathf.MoveTowardsAngle(angle2, endAngle2, Time.deltaTime * cutsceneMovementSpeed);
				delta2 = Mathf.Abs(angle2 - newAngle);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.Euler(0f, newAngle, 0f), Time.deltaTime * 5f);
				base.transform.position = getPointAtTier(curTier, newAngle * ((float)Math.PI / 180f));
				animator.SetFloat("horizontalDirection", Mathf.Sign(newAngle - angle2));
				animator.SetFloat("horizontalVelocity", Mathf.Abs(newAngle - angle2));
				graphicsSpawner.transform.localRotation = Quaternion.Slerp(graphicsSpawner.transform.localRotation, Quaternion.Euler(0f, 90f * Mathf.Sign(newAngle - angle2), 0f), Time.deltaTime * 5f);
				angle2 = newAngle;
			}
			graphicsSpawner.transform.localRotation = Quaternion.identity;
			yield return null;
			Stopwatch sw = new Stopwatch(1f);
			Vector3 startPos = getPointAtTier(curTier, curTier.SafeJumpAngle);
			Vector3 endPos = getPointAtTier(nextTier, curTier.SafeJumpAngle);
			AudioController.Play("MB_health_bar_lost");
			animator.SetBool("jumping", true);
			while ((bool)sw)
			{
				yield return null;
				Vector3 pos = new Vector3(MathUtilities.LerpUnclamped(startPos.x, endPos.x, tierSwitchCurveX.Evaluate(sw.Progress)), MathUtilities.LerpUnclamped(startPos.y, endPos.y, tierSwitchCurveY.Evaluate(sw.Progress)), MathUtilities.LerpUnclamped(startPos.z, endPos.z, tierSwitchCurveZ.Evaluate(sw.Progress)));
				base.transform.position = pos;
			}
			animator.SetBool("jumping", false);
			currentTierIndex++;
			dir2 = player.transform.position - nextTier.Center;
			angle2 = curTier.SafeJumpAngle * 57.29578f;
			endAngle2 = Mathf.Atan2(dir2.x, dir2.z) * 57.29578f;
			delta2 = Mathf.Abs(angle2 - endAngle2);
			while (delta2 > 0f)
			{
				yield return null;
				float newAngle2 = Mathf.MoveTowardsAngle(angle2, endAngle2, Time.deltaTime * cutsceneMovementSpeed);
				delta2 = Mathf.Abs(angle2 - newAngle2);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.Euler(0f, newAngle2, 0f), Time.deltaTime * 5f);
				base.transform.position = getPointAtTier(nextTier, newAngle2 * ((float)Math.PI / 180f));
				animator.SetFloat("horizontalDirection", Mathf.Sign(newAngle2 - angle2));
				animator.SetFloat("horizontalVelocity", Mathf.Abs(newAngle2 - angle2));
				graphicsSpawner.transform.localRotation = Quaternion.Slerp(graphicsSpawner.transform.localRotation, Quaternion.Euler(0f, 90f * Mathf.Sign(newAngle2 - angle2), 0f), Time.deltaTime * 5f);
				angle2 = newAngle2;
			}
			graphicsSpawner.transform.localRotation = Quaternion.identity;
			animator.SetBool("doingTierSwitch", false);
			if (settings.flintheartDoCinematicTierSwitches)
			{
				player.AreControlsLocked = false;
				yield return defaultBehaviour.CrossFadeIn(0.5f);
			}
			invulnerable = false;
			StartCoroutine(behaviourLoop());
		}

		private IEnumerator walkToLOSLoop()
		{
			while (true)
			{
				FlintheartTier tier = tiers[currentTierIndex];
				Vector3 playerDirFromCenter = player.transform.position - tier.Center;
				float playerAngle = Mathf.Atan2(playerDirFromCenter.x, playerDirFromCenter.z) * 57.29578f;
				Vector3 flintDirFromCenter = base.transform.position - tier.Center;
				float flintAngle = Mathf.Atan2(flintDirFromCenter.x, flintDirFromCenter.z) * 57.29578f;
				if (!Mathf.Approximately(playerAngle, flintAngle))
				{
					float newAngle = Mathf.MoveTowardsAngle(flintAngle, playerAngle, Time.deltaTime * movementSpeed);
					base.transform.rotation = Quaternion.Euler(0f, newAngle, 0f);
					base.transform.position = getPointAtTier(tier, newAngle * ((float)Math.PI / 180f));
					animator.SetFloat("horizontalDirection", Mathf.Sign(newAngle - flintAngle));
					animator.SetFloat("horizontalVelocity", Mathf.Abs(newAngle - flintAngle));
				}
				else
				{
					animator.SetFloat("horizontalVelocity", 0f);
				}
				yield return null;
			}
		}

		private IEnumerator behaviourLoop()
		{
			StartCoroutine(walkToLOSLoop());
			yield return new WaitForSeconds(1f);
			if (initialDelay > 0f)
			{
				yield return new WaitForSeconds(initialDelay);
				initialDelay = 0f;
			}
			while (base.enabled)
			{
				MonkeyBattleGame.ShootActions action = settings.shotPattern[shotPatternIndex];
				if (action == MonkeyBattleGame.ShootActions.Reloading)
				{
					GameEvents.Invoke(new MonkeyBattleGame.FlintheartStartedReloadingEvent());
					flintheartIK.SetIKWeight(AvatarIKGoal.LeftHand, 0f);
					animator.SetBool("reloading", true);
					yield return new WaitForSeconds(settings.flintheartReloadTime);
					animator.SetBool("reloading", false);
					flintheartIK.SetIKWeight(AvatarIKGoal.LeftHand, 1f);
					GameEvents.Invoke(new MonkeyBattleGame.FlintheartFinishedReloadingEvent());
				}
				else
				{
					yield return StartCoroutine(shootAtPlayer(action));
					yield return new WaitForSeconds(UnityEngine.Random.Range(settings.flintheartShootIntervalMin, settings.flintheartShootIntervalMax));
				}
				shotPatternIndex = ((shotPatternIndex + 1 < settings.shotPattern.Length) ? (shotPatternIndex + 1) : 0);
			}
		}

		private IEnumerator shootAtPlayer(MonkeyBattleGame.ShootActions shotType)
		{
			gunObject.transform.LookAt(player.GetAimingPosition());
			animator.SetBool("shooting", true);
			switch (shotType)
			{
			default:
				yield return StartCoroutine("doSingleShot");
				break;
			case MonkeyBattleGame.ShootActions.AngleShot:
				yield return StartCoroutine("doAngleShot");
				break;
			case MonkeyBattleGame.ShootActions.BuckShot:
				yield return StartCoroutine("doBuckShot");
				break;
			case MonkeyBattleGame.ShootActions.FollowShot:
				yield return StartCoroutine("doFollowShot");
				break;
			case MonkeyBattleGame.ShootActions.SprayShot:
				yield return StartCoroutine("doSprayShot");
				break;
			}
			animator.SetBool("shooting", false);
		}

		private IEnumerator doSprayShot()
		{
			float sign = Mathf.Sign(base.transform.InverseTransformDirection(player.transform.forward).x);
			Vector3 rot = gunObject.transform.rotation.eulerAngles;
			rot.y -= settings.sprayShotAngle * (settings.sprayShotCount - 1f) * 0.5f * sign;
			for (int i = 0; (float)i < settings.sprayShotCount; i++)
			{
				AudioController.Play(fireAudio.name, base.transform);
				gunObject.transform.eulerAngles = rot;
				cannon.Fire(cannon.transform.forward);
				rot.y += settings.sprayShotAngle * sign;
				yield return new WaitForSeconds(settings.sprayShotInterval);
			}
		}

		private IEnumerator doBuckShot()
		{
			Vector3 offset = Quaternion.Euler(0f, settings.buckShotCount * settings.buckShotAngle / 2f, 0f).eulerAngles;
			Vector3 rot = gunObject.transform.rotation.eulerAngles - offset;
			AudioController.Play(shotgunAudio.name, base.transform);
			for (int i = 0; (float)i < settings.buckShotCount; i++)
			{
				gunObject.transform.eulerAngles = rot;
				cannon.Fire(cannon.transform.forward);
				rot.y += settings.buckShotAngle;
			}
			yield return null;
		}

		private IEnumerator doFollowShot()
		{
			Stopwatch sw = new Stopwatch(settings.followShotDuration);
			while ((bool)sw)
			{
				yield return new WaitForSeconds(settings.followShotInterval);
				Vector3 pos = player.transform.position;
				float offset = Mathf.Sin((float)Math.PI * settings.followShotFrequency * sw.Progress) * settings.followShotAmplitude;
				pos += -player.transform.forward * offset;
				gunObject.transform.LookAt(pos + Vector3.up);
				cannon.Fire(cannon.transform.forward);
				AudioController.Play(fireAudio.name, base.transform);
			}
		}

		private IEnumerator doAngleShot()
		{
			Stopwatch sw = new Stopwatch(settings.shootInFrontDuration);
			float[] angles = new float[7] { 90f, -90f, 45f, -45f, 20f, -20f, 0f };
			float interval = settings.shootInFrontDuration / (float)angles.Length;
			int a = 0;
			while ((bool)sw)
			{
				yield return new WaitForSeconds(interval);
				Vector3 dir = Quaternion.Euler(0f, angles[a++], 0f) * (player.transform.position - base.transform.position).normalized;
				gunObject.transform.rotation = Quaternion.LookRotation(dir);
				cannon.Fire(cannon.transform.forward);
				AudioController.Play(fireAudio.name, base.transform);
			}
		}

		private IEnumerator doSingleShot()
		{
			Vector3 rot = gunObject.transform.rotation.eulerAngles;
			gunObject.transform.eulerAngles = rot;
			cannon.Fire(cannon.transform.forward);
			AudioController.Play(fireAudio.name, base.transform);
			yield return null;
		}

		private void OnFootstep()
		{
		}

		private void SpawnParticle(GameObject prefab)
		{
		}
	}
}
