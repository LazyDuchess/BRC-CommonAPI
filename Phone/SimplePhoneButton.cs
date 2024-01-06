using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CommonAPI.Phone {
    public class SimplePhoneButton : PhoneButton {
        private const float ButtonImageWidth = 530f * 2f;
        private const float ButtonImageHeight = 75f * 2f;
        private static bool LoadedResources = false;
        private static Sprite ButtonSprite_Unselected = null;
        public override float Width => ButtonImage.rectTransform.sizeDelta.x;
        public override float Height => ButtonImage.rectTransform.sizeDelta.y;

        public Image ButtonImage = null;

        private static void CacheResources() {
            if (LoadedResources) return;
            LoadedResources = true;
            var resourceLocation = Path.GetDirectoryName(CommonAPIPlugin.Instance.Info.Location);
            var buttonUnselectedSpriteLocation = Path.Combine(resourceLocation, "Phone-SimpleButton.png");
            ButtonSprite_Unselected = TextureUtility.LoadSprite(buttonUnselectedSpriteLocation);
        }

        public static SimplePhoneButton Create() {
            CacheResources();
            var buttonGameObject = new GameObject("Simple Button");
            var buttonAnimationParent = new GameObject("Animation Parent");
            var buttonImageGO = new GameObject("Button Image");
            buttonImageGO.transform.SetParent(buttonAnimationParent.transform, false);
            buttonAnimationParent.transform.SetParent(buttonGameObject.transform, false);
            var buttonImage = buttonImageGO.AddComponent<Image>();
            buttonImage.sprite = ButtonSprite_Unselected;
            buttonImageGO.RectTransform().sizeDelta = new Vector2(ButtonImageWidth, ButtonImageHeight);
            var rect = buttonGameObject.AddComponent<RectTransform>();
            rect.anchorMax = new Vector2(1f, 1f);
            rect.anchorMin = new Vector2(1f, 1f);
            rect.pivot = new Vector2(1f, 1f);
            var button = buttonGameObject.AddComponent<SimplePhoneButton>();
            button.ButtonImage = buttonImage;
            return button;
        }
    }
}
