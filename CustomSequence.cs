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
    public class CustomSequence
    {
        public GameObject CurrentCamera = null;
        public Player player;
        public DialogueUI dialogueUI;
        public EffectsUI effectsUI;
        public double time = 0.0;
        public virtual void Play(Player player)
        {
            this.dialogueUI = Core.Instance.UIManager.dialogueUI;
            this.player = player;
            this.effectsUI = Core.instance.UIManager.effects;
            this.effectsUI.ShowBars(0.25f, 150f, UpdateType.Manual);
        }

        public virtual void Stop()
        {
            this.effectsUI.HideBars(0.25f, UpdateType.Manual);
            player.sequenceState = SequenceState.NONE;
            if (CurrentCamera != null)
                CurrentCamera.SetActive(false);
        }

        protected void StartDialogue(CustomDialogue dialogue, float delay = CustomSequenceHandler.DefaultDialogueDelay)
        {
            CustomSequenceHandler.instance.StartDialogueDelayed(dialogue, delay);
        }

        protected void ExitSequence(float delay = CustomSequenceHandler.DefaultExitDelay)
        {
            CustomSequenceHandler.instance.ExitCurrentSequenceDelayed(delay);
        }

        protected void SetCamera(GameObject camera)
        {
            if (CurrentCamera != null)
                CurrentCamera.SetActive(false);
            CurrentCamera = camera;
            if (CurrentCamera != null)
                CurrentCamera.SetActive(true);
        }

        protected void RequestYesNoPrompt()
        {
            dialogueUI.RequestYesNoPrompt(DialogueBehaviour.DialogueType.YES_NO_GENERIC);
        }
    }
}
