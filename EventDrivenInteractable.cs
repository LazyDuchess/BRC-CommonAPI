using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using UnityEngine;

namespace CommonAPI {
    public class EventDrivenInteractable : CustomInteractable {
        public Func<Player, bool> OnTest;
        public Func<Vector3> OnGetLookAtPos;
        public Action<Player> OnInteract;

        public override Vector3 GetLookAtPos() {
            if (OnGetLookAtPos == null)
                return base.GetLookAtPos();
            return OnGetLookAtPos.Invoke();
        }

        public override void Interact(Player player) {
            if (OnInteract == null)
                base.Interact(player);
            else
                OnInteract.Invoke(player);
        }

        public override bool Test(Player player) {
            if (OnTest == null)
                return base.Test(player);
            return OnTest.Invoke(player);
        }
    }
}
