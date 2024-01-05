using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using Reptile.Phone;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CommonAPI.Phone {
    /// <summary>
    /// Base class for custom phone apps.
    /// </summary>
    public abstract class CustomApp : App {
        public virtual Sprite Icon { get; } = null;
        public abstract string Name { get; }

        /// <summary>
        /// Creates a title bar for this app with the specified title and icon.
        /// </summary>
        protected void CreateTitleBar(string title, Sprite icon = null, float fontSize = 80f) {
            var sourceApp = MyPhone.GetAppInstance<AppGraffiti>();
            var overlay = sourceApp.transform.Find("Overlay");
            var newOverlay = GameObject.Instantiate(overlay);
            var icons = newOverlay.transform.Find("Icons");
            icons.Find("GraffitiIcon").GetComponent<Image>().sprite = icon;
            var header = icons.Find("HeaderLabel");
            Component.Destroy(header.GetComponent<TMProLocalizationAddOn>());
            var tmpro = header.GetComponent<TextMeshProUGUI>();
            tmpro.text = title;
            tmpro.fontSize = fontSize;
            tmpro.fontSizeMax = fontSize;
            tmpro.fontSizeMin = fontSize;
            newOverlay.SetParent(transform, false);
        }
    }
}
