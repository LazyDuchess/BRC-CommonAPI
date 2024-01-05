using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommonAPI.Phone {
    /// <summary>
    /// Defines the metadata for a phone app to be automatically added to the phone by CommonAPI.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PhoneAppAttribute : Attribute {
        public string Title;
        public Sprite Icon;
        public PhoneAppAttribute(string title, Sprite icon) {
            Title = title;
            Icon = icon;
        }
    }
}
