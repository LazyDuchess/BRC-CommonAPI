using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommonAPI.Phone {
    public abstract class PhoneButton : MonoBehaviour {
        public abstract float Width { get; }
        public abstract float Height { get; }
        public Action OnPressed;
    }
}
