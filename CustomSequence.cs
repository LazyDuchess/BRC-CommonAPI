using System;
using System.Collections.Generic;
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
        public double time = 0.0;
        public virtual void Play(Player player)
        {
            this.dialogueUI = Core.Instance.UIManager.dialogueUI;
            this.player = player;
        }

        public virtual void Stop()
        {
            player.sequenceState = SequenceState.NONE;
            if (CurrentCamera != null)
                CurrentCamera.SetActive(false);
        }

        protected void StartDialogue(CustomDialogue dialogue)
        {
            CustomSequenceHandler.instance.StartDialogue(dialogue);
        }

        protected void ExitSequence()
        {
            CustomSequenceHandler.instance.ExitCurrentSequence();
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
