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
        public const float TitleBarHeight = 300f;
        /// <summary>
        /// For unread indicator in the homescreen.
        /// </summary>
        public virtual int Unread { get; } = 0;
        /// <summary>
        /// Whether the app should show up on the phone.
        /// </summary>
        public virtual bool Available { get; } = true;

        public override void OnAppInit() {
            this.m_Unlockables = Array.Empty<AUnlockable>();
        }

        /// <summary>
        /// Creates a title bar for this app with the specified title and no icon.
        /// </summary>
        protected void CreateIconlessTitleBar(string title, float fontSize = 80f) {
            var sourceApp = MyPhone.GetAppInstance<AppGraffiti>();
            var overlay = sourceApp.transform.Find("Overlay");
            var newOverlay = GameObject.Instantiate(overlay);
            var icons = newOverlay.transform.Find("Icons");
            Destroy(icons.Find("GraffitiIcon").gameObject);
            var header = icons.Find("HeaderLabel");
            header.localPosition = new Vector3(140f, header.localPosition.y, header.localPosition.z);
            Component.Destroy(header.GetComponent<TMProLocalizationAddOn>());
            var tmpro = header.GetComponent<TextMeshProUGUI>();
            tmpro.text = title;
            tmpro.fontSize = fontSize;
            tmpro.fontSizeMax = fontSize;
            tmpro.fontSizeMin = fontSize;
            newOverlay.SetParent(transform, false);
        }

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
