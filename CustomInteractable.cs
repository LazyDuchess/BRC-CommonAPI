using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace CommonAPI
{
    /// <summary>
    /// Interactable object. Shows a button prompt when the player touches its trigger.
    /// </summary>
    public class CustomInteractable : MonoBehaviour
    {
        public bool LookAt = true;
        public bool ShowRep = false;
        public InteractableIcon Icon = InteractableIcon.Talk;
        public Sprite CustomIcon;

        void OnTriggerStay(Collider other)
        {
            var player = other.GetComponentInChildren<Player>();
            if (!player)
                player = other.GetComponentInParent<Player>();
            if (!player)
                return;
            CheckForInteraction(player);
        }

        public bool CheckForInteraction(Player player)
        {
            if (player.isAI)
                return false;
            if (player.IsDead())
                return false;
            if (!Player.NoMenuOpen())
                return false;
            if (!player.IsNotInCutsceneOrGettingCalled())
                return false;
            if (player.breakChainContextAvailable > 0)
                return false;
            if (player.graffitiContextAvailable > 0)
                return false;
            if (player.talkContextAvailable > 0)
                return false;
            var playerComponent = CustomPlayerComponent.Get(player);
            if (!playerComponent)
                return false;
            if (!Test(player))
                return false;
            playerComponent.CurrentCustomInteractable = this;
            playerComponent.CustomInteractableContextAvailable = 10;
            if (ShowRep)
                player.showRepLingerTimer = 2f;
            if (LookAt)
                player.characterVisual.LookAtSubject(gameObject, GetLookAtPos());
            if (player.sprayButtonNew)
            {
                Interact(player);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Point the player's head will look at when touching the trigger.
        /// </summary>
        public virtual Vector3 GetLookAtPos()
        {
            var lookTarget = transform.Find("LookTarget");
            if (lookTarget)
                return lookTarget.position;
            return transform.position;
        }

        /// <summary>
        /// Test function for interaction, if this returns false then the prompt won't be shown and the player won't be able to interact with the object.
        /// Called every tick the player is inside the trigger, and not busy or dead.
        /// </summary>
        public virtual bool Test(Player player)
        {
            return true;
        }

        /// <summary>
        /// Called when the player interacts with the object.
        /// </summary>
        public virtual void Interact(Player player)
        {

        }
    }
}
