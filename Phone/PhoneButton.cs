using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommonAPI.Phone {
    public abstract class PhoneButton : MonoBehaviour {
        public bool BeingPressed = false;
        public abstract float Width { get; }
        public abstract float Height { get; }
        /// <summary>
        /// Action to take when the user presses this button.
        /// </summary>
        public Action OnConfirm;

        public virtual void PlayHighlightAnimation() { }
        public virtual void PlayDeselectAnimation(bool skip = false) { }
        public virtual void PlayHoldAnimation() { }
        public virtual void Confirm() {
            OnConfirm?.Invoke();
        }
    }
}
