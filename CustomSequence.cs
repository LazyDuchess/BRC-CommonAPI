using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;
using DG.Tweening;

namespace CommonAPI
{
    /// <summary>
    /// Custom cutscene sequence class.
    /// </summary>
    public class CustomSequence
    {
        public GameObject CurrentCamera = null;
        public Player player;
        public DialogueUI dialogueUI;
        public EffectsUI effectsUI;
        public double time = 0.0;

        public void Init()
        {
            dialogueUI = Core.Instance.UIManager.dialogueUI;
            player = WorldHandler.instance.GetCurrentPlayer();
            effectsUI = Core.instance.UIManager.effects;
        }

        /// <summary>
        /// Called when the sequence starts playing.
        /// </summary>
        public virtual void Play()
        {
            effectsUI.ShowBars(0.25f, 150f, UpdateType.Manual);
        }

        /// <summary>
        /// Called when the sequence ends or is skipped.
        /// </summary>
        public virtual void Stop()
        {
            this.effectsUI.HideBars(0.25f, UpdateType.Manual);
            player.sequenceState = SequenceState.NONE;
            if (CurrentCamera != null)
                CurrentCamera.SetActive(false);
        }

        /// <summary>
        /// Show a custom dialogue.
        /// </summary>
        protected void StartDialogue(CustomDialogue dialogue, float delay = CustomSequenceHandler.DefaultDialogueDelay)
        {
            CustomSequenceHandler.instance.StartDialogueDelayed(dialogue, delay);
        }

        /// <summary>
        /// End this sequence. Call this when you're finished.
        /// </summary>
        protected void ExitSequence(float delay = CustomSequenceHandler.DefaultExitDelay)
        {
            CustomSequenceHandler.instance.ExitCurrentSequenceDelayed(delay);
        }

        /// <summary>
        /// Sets the cutscene camera. Camera GameObject must have a CameraRegisterer component.
        /// </summary>
        protected void SetCamera(GameObject camera)
        {
            if (CurrentCamera != null)
                CurrentCamera.SetActive(false);
            CurrentCamera = camera;
            if (CurrentCamera != null)
                CurrentCamera.SetActive(true);
        }

        /// <summary>
        /// Shows a Yes/No prompt on the current dialogue. The answer will be stored in the AnsweredYes field of the custom dialogue.
        /// </summary>
        protected void RequestYesNoPrompt()
        {
            dialogueUI.RequestYesNoPrompt(DialogueBehaviour.DialogueType.YES_NO_GENERIC);
        }
    }
}
