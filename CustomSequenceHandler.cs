using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using UnityEngine;
using DG.Tweening;

namespace CommonAPI
{
	/// <summary>
	/// Like the SequenceHandler, but for custom sequences.
	/// </summary>
    public class CustomSequenceHandler : AMenuController
	{
		public const float DefaultExitDelay = 0.9f;
		public const float DefaultDialogueDelay = 0.9f;
		public static CustomSequenceHandler instance;
		public CustomSequence sequence;
		public bool disabledExit;
		private GameInput gameInput;
		private BaseModule baseModule;
		private Player player;
		private bool sequenceOverwritten;
		private float skipTimer;
		private SequenceHandler.SkipState skipTextActiveState = SequenceHandler.SkipState.IDLE;
		private float skipTextTimer;
		public bool isBusy;
		private bool invokeExitSequenceEvent = true;
		private bool restartCurrentEncounter;
		private bool hidePlayer;
		private bool pausePlayer;
		private bool interruptPlayer;
		private bool lowerVolume;
		private float fadeDuration = 0.4f;
		internal float shutDuration = 0.2f;
		private float skipFadeDuration = 0.3f;
		private float skipStartTimer;
		private float skipThreshold = 0.4f;
		private Coroutine exitSequenceRoutine;
		private List<Coroutine> queuedSequenceActions = new List<Coroutine>();
		internal bool allowPhoneOnAfterSequence;

		internal CustomDialogue CurrentDialogue = null;

		public static void Initialize()
        {
			StageManager.OnStagePostInitialization += StageManager_OnStagePostInitialization;
		}

		private IEnumerator StartDialogueCoroutine(CustomDialogue dialogue, float delay = DefaultDialogueDelay)
        {
			yield return new WaitForSeconds(DefaultDialogueDelay);
			StartDialogue(dialogue);
        }

		public void StartDialogueDelayed(CustomDialogue dialogue, float delay = DefaultDialogueDelay)
        {
			queuedSequenceActions.Add(StartCoroutine(StartDialogueCoroutine(dialogue, delay)));
        }

		public void StartDialogue(CustomDialogue dialogue)
		{
			instance.CurrentDialogue = dialogue;
			var dialogueUI = Core.Instance.UIManager.dialogueUI;
			dialogueUI.effectsUI.ShowBars(0.25f, 150f, UpdateType.Manual);
			dialogueUI.CanBeSkipped = true;
			dialogueUI.ToggleDialogueUI(true);
			dialogueUI.SetLine(dialogue.Dialogue);
			dialogueUI.characterNameText.enabled = true;
			dialogue.OnDialogueBegin?.Invoke();
		}

		static void StageManager_OnStagePostInitialization()
        {
			var gameObject = new GameObject("Custom Sequence Handler");
			gameObject.AddComponent<CustomSequenceHandler>().Init();
        }

		public void Init()
		{
			instance = this;
			allowMouseInput = false;
			gameInput = Core.Instance.GameInput;
			uIManager = Core.Instance.UIManager;
			audioManager = Core.Instance.AudioManager;
			baseModule = Core.Instance.BaseModule;
			player = WorldHandler.instance.GetCurrentPlayer();
			Core.OnUpdate += UpdateSequenceHandler;
			Core.OnCoreUpdatePaused += OnCoreUpdatePaused;
			Core.OnCoreUpdateUnPaused += OnCoreUpdateUnPaused;
		}

		public void ExitCurrentSequenceDelayed(float delay = DefaultExitDelay)
        {
			queuedSequenceActions.Add(StartCoroutine(ExitCurrentSequenceDelayedCoroutine(delay)));
        }

		private IEnumerator ExitCurrentSequenceDelayedCoroutine(float delay = DefaultExitDelay)
        {
			yield return new WaitForSeconds(delay);
			yield return ExitSequenceRoutine();
        }

		public void ExitCurrentSequence()
		{
			exitSequenceRoutine = StartCoroutine(ExitSequenceRoutine());
		}

		public override void OnDestroy()
        {
			base.OnDestroy();
			if (Core.Instance != null && uIManager != null)
			{
				uIManager.AbsolutePopIfPresent(this);
			}
			audioManager = null;
			uIManager = null;
			gameInput = null;
			player = null;
			instance = null;
			Core.OnUpdate -= UpdateSequenceHandler;
			Core.OnCoreUpdatePaused -= OnCoreUpdatePaused;
			Core.OnCoreUpdateUnPaused -= OnCoreUpdateUnPaused;
		}

		public void UpdateSequenceHandler()
		{
			if (!isBusy)
				return;
			if (player.sequenceState == SequenceState.IN_SEQUENCE)
			{
				DialogueUI dialogueUI = uIManager.dialogueUI;
				bool flag = IsEnabled && !disabledExit && gameInput.GetButtonNew(2, 0);
				if (flag && dialogueUI.CanBeSkipped && !dialogueUI.isYesNoPromptEnabled)
				{
					if (dialogueUI.ReadyToResume)
					{
						//ResumeSequence();
						if (dialogueUI.IsShowingDialogue())
						{
							Core.Instance.AudioManager.PlaySfxUI(SfxCollectionID.MenuSfx, AudioClipID.dialogueconfirm, 0f);
						}
						dialogueUI.EndDialogue();
					}
					else
					{
						FastForwardTypewriter();
					}
				}
				if (skipTextActiveState != SequenceHandler.SkipState.NOT_SKIPPABLE && !dialogueUI.isYesNoPromptEnabled)
				{
					EffectsUI effects = uIManager.effects;
					float dt = Core.dt;
					bool flag2 = IsEnabled && gameInput.GetButtonHeld(64, 0);
					if (skipStartTimer >= 0.5f)
					{
						if (skipTextActiveState == SequenceHandler.SkipState.TEXT_ACTIVE)
						{
							skipTextTimer += dt;
							if (skipTextTimer > 1.5f && skipTimer == 0f)
							{
								Tween tween = effects.FadeSkipOut(skipFadeDuration, UpdateType.Manual);
								tween.onComplete = (TweenCallback)Delegate.Combine(tween.onComplete, new TweenCallback(delegate
								{
									skipTextActiveState = SequenceHandler.SkipState.IDLE;
								}));
								skipTextActiveState = SequenceHandler.SkipState.WAIT_FOR_FADE;
								skipTextTimer = 0f;
							}
						}
						else if (skipTextActiveState == SequenceHandler.SkipState.IDLE && !flag && (gameInput.GetButtonHeld(64, 0) || gameInput.GetButtonHeld(4, 0)))
						{
							Tween tween2 = effects.FadeSkipIn(skipFadeDuration, UpdateType.Manual);
							tween2.onComplete = (TweenCallback)Delegate.Combine(tween2.onComplete, new TweenCallback(delegate
							{
								skipTextActiveState = SequenceHandler.SkipState.TEXT_ACTIVE;
							}));
							skipTextActiveState = SequenceHandler.SkipState.WAIT_FOR_FADE;
						}
					}
					skipStartTimer += dt;
					if (flag2 && skipStartTimer >= 0.5f)
					{
						skipTimer += dt;
						skipTextTimer = 0f;
					}
					else
					{
						skipTimer = 0f;
					}
				}
				if (!disabledExit && (skipTimer >= skipThreshold))
				{
					exitSequenceRoutine = StartCoroutine(ExitSequenceRoutine());
				}
			}
		}

		private IEnumerator ExitSequenceRoutine()
		{
			if (player.sequenceState == SequenceState.EXITING)
				yield break;
			player.sequenceState = SequenceState.EXITING;
			Core coreInstance = Core.Instance;
			EffectsUI effectsUI = this.uIManager.effects;
			skipTextActiveState = SequenceHandler.SkipState.EXITING_SEQUENCE;
			effectsUI.FadeSkipOut(this.skipFadeDuration, UpdateType.Manual);
			double num = (double)Time.deltaTime;
			float stayBlackSeconds = this.fadeDuration;
			Tween tween = effectsUI.FadeToBlack(fadeDuration);
			yield return tween.WaitForCompletion();
			yield return new WaitForSeconds(shutDuration);
			this.SetExitSequence();
			yield return new WaitForSeconds(shutDuration);
			yield return new WaitWhile(() => coreInstance.IsCorePaused);
			if (!this.IsInSequence())
			{
				effectsUI.FadeOpen(this.fadeDuration);
				this.isBusy = false;
				effectsUI.HideVerticalBars();
			}
			this.exitSequenceRoutine = null;
			yield break;
		}

		public override void Activate()
		{
		}

		public override void Deactivate()
		{
		}

		private void StopAllSequenceActions()
        {
			foreach(var routine in queuedSequenceActions)
            {
				if (routine == null)
					continue;
				StopCoroutine(routine);
            }
			queuedSequenceActions.Clear();
        }

		private void SetExitSequence()
		{
			StopAllSequenceActions();
			if (this.hidePlayer)
			{
				this.player.HideForSequence(false);
			}
			if (this.pausePlayer)
			{
				this.player.PauseForSequence(false);
			}
			if (this.lowerVolume || (this.audioManager.IsVolumeLowered && !this.IsInSequence()))
			{
				this.lowerVolume = false;
				this.audioManager.StopTemporarilyLowerVolume(this);
			}
			this.player.sequenceState = SequenceState.NONE;
			this.player.RecheckLoopingSounds();
			this.sequence.time = 0.0;
		    sequence.Stop();
			WorldHandler worldHandler = WorldHandler.instance;
			if (worldHandler)
			{
				Player currentPlayer = worldHandler.GetCurrentPlayer();
				if (currentPlayer != null)
				{
					GameplayCamera cam = currentPlayer.cam;
					if (cam != null && worldHandler.CurrentCamera != cam.cam)
					{
						worldHandler.CurrentCamera.enabled = false;
					}
				}
			}
			this.uIManager.AbsolutePopIfPresent(this);
			if (this.uIManager.dialogueUI.IsShowingDialogue())
			{
				this.uIManager.dialogueUI.AbortDialogue();
			}
			this.LetPlayerExitSequence();
		}

		public void LetPlayerExitSequence()
		{
			this.player.userInputEnabled = true;
			this.player.ui.gameObject.SetActive(true);
			this.player.phone.SetAllCamerasForSequenceExit();
			this.player.phone.AllowPhone(true, false, this.allowPhoneOnAfterSequence);
			this.player.EnablePlayer(false);
			if (!this.uIManager.IsShowingAnyMenu && this.baseModule.IsPlayingInStage)
			{
				this.baseModule.StageManager.RestoreCurrentPlayerInput();
			}
		}

		private void FastForwardTypewriter()
		{
			this.uIManager.dialogueUI.FastForwardTypewriter();
		}

		public void StartEnteringSequence(CustomSequence setSequence, bool setHidePlayer = true, bool setInterruptPlayer = true, bool instantly = false, bool setPausePlayer = false, bool setAllowPhoneOnAfterSequence = true, bool skippable = true, bool lowerVolumeDuringSequence = false, bool disabledExitOnInput = false)
        {
			sequenceOverwritten = false;
			if (IsInSequence() && player.sequenceState != SequenceState.PRE_ENTERING)
			{
				/*
				FixedFramerateSequence component = this.sequence.gameObject.GetComponent<FixedFramerateSequence>();
				if (component != null)
				{
					component.Stop();
				}
				else
				{
					this.sequence.Stop();
				}*/
				sequenceOverwritten = true;
			}
			if (player.ability == player.characterSelectAbility)
			{
				instantly = false;
			}
			sequence = setSequence;
			skipTimer = 0f;
			skipTextActiveState = (skippable ? SequenceHandler.SkipState.IDLE : SequenceHandler.SkipState.NOT_SKIPPABLE);
			skipTextTimer = 0f;
			isBusy = true;
			invokeExitSequenceEvent = true;
			restartCurrentEncounter = false;
			hidePlayer = setHidePlayer;
			allowPhoneOnAfterSequence = setAllowPhoneOnAfterSequence;
			interruptPlayer = setInterruptPlayer;
			pausePlayer = setPausePlayer;
			lowerVolume = lowerVolume != lowerVolumeDuringSequence && lowerVolumeDuringSequence;
			disabledExit = disabledExitOnInput;
			audioManager.PauseAllGameplayLoopingSfx();
			audioManager.ClearAllGameplayLoopingSfx();
			if (uIManager.IsShowingAnyMenu)
			{
				uIManager.PopAllMenusInstant();
			}
			if (instantly)
			{
				SetInSequenceImmediate();
				return;
			}
			StartCoroutine(EnterSequenceRoutine());
		}

		private IEnumerator EnterSequenceRoutine()
		{
			player.sequenceState = SequenceState.ENTERING;
			if (player.inGraffitiGame)
			{
				yield return new WaitUntil(() => !player.inGraffitiGame);
			}
			if (player.ability == player.characterSelectAbility)
			{
				yield return new WaitUntil(() => player.ability != player.characterSelectAbility);
			}
			else if (player.ability == player.danceAbility)
			{
				player.StopCurrentAbility();
			}
			player.userInputEnabled = false;
			player.phone.AllowPhone(false, false, false);
			SetPartiallyInSequence();
			yield return this.uIManager.effects.FadeToBlack(fadeDuration).WaitForCompletion();
			SetFullyInSequence();
			yield return new WaitForSeconds(shutDuration);
			uIManager.effects.FadeOpen(fadeDuration);
			uIManager.effects.ShowVerticalBars();
			yield break;
		}

		private void SetPartiallyInSequence()
		{
			player.FlushInput();
			if (interruptPlayer)
			{
				player.CompletelyStop();
			}
			if (pausePlayer)
			{
				player.PauseForSequence(true);
			}
			player.StopHoldProps();
			player.DestroyPickupVisuals();
			player.phone.AllowPhone(false, false, false);
			if (!sequenceOverwritten)
			{
				player.DisablePlayer();
			}
			if (lowerVolume)
			{
				audioManager.StartTemporarilyLowerVolume(this);
			}
		}

		private void SetFullyInSequence()
		{
			player.sequenceState = SequenceState.IN_SEQUENCE;
			if (hidePlayer)
			{
				player.HideForSequence(true);
			}
			gameInput.DisableAllControllerMaps(0);
			gameInput.EnableControllerMap(1, 0);
			if (!IsEnabled)
			{
				uIManager.PushNewMenuInstant(this);
			}
			player.ui.gameObject.SetActive(false);
			sequence.Init();
			sequence.Play();
			skipStartTimer = 0f;
		}

		private void SetInSequenceImmediate()
		{
			EffectsUI effects = uIManager.effects;
			if (!effects.IsFadedToColor(EffectsUI.niceClear))
			{
				effects.FadeOpen(0f);
				effects.ShowVerticalBars();
			}
			SetPartiallyInSequence();
			SetFullyInSequence();
		}

		public bool IsInSequence()
		{
			return player.sequenceState != SequenceState.NONE;
		}

		public void SetPreEnteringSequence()
		{
			player.sequenceState = SequenceState.PRE_ENTERING;
		}

		private void OnCoreUpdatePaused()
		{
			if (sequence != null && !uIManager.dialogueUI.isYesNoPromptEnabled && !disabledExit)
			{
				/*
				FixedFramerateSequence component = this.sequence.GetComponent<FixedFramerateSequence>();
				if (component != null)
				{
					component.Pause();
					return;
				}
				if (this.sequence.playableGraph.IsValid())
				{
					this.sequence.playableGraph.GetRootPlayable(0).SetSpeed(0.0);
				}*/
			}
		}

		private void OnCoreUpdateUnPaused()
		{
			if (sequence != null && !uIManager.dialogueUI.isYesNoPromptEnabled && !disabledExit)
			{
				/*
				FixedFramerateSequence component = this.sequence.GetComponent<FixedFramerateSequence>();
				if (component != null)
				{
					component.Resume();
					return;
				}
				if (this.sequence.playableGraph.IsValid())
				{
					DialogueUI dialogueUI = this.uIManager.dialogueUI;
					if (dialogueUI != null)
					{
						bool flag = dialogueUI.IsDialogueUIActive();
						if (!flag || (flag && !dialogueUI.CanBeSkipped))
						{
							this.sequence.playableGraph.GetRootPlayable(0).SetSpeed(1.0);
						}
					}
				}*/
			}
		}
	}
}
