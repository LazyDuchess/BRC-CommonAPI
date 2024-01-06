using Reptile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using CommonAPI.UI;

namespace CommonAPI.Phone {
    public class SimplePhoneButton : PhoneButton {
        private const float ButtonImageWidth = 530f * 2f;
        private const float ButtonImageHeight = 75f * 2f;
        private static bool LoadedResources = false;
        private static Sprite ButtonSprite_Unselected = null;
        private static Sprite ButtonSprite_Selected = null;
        public override float Width => ButtonImage.rectTransform.sizeDelta.x;
        public override float Height => ButtonImage.rectTransform.sizeDelta.y;

        public Image ButtonImage = null;
        private Transform animationParent = null;

        public override void PlayHoldAnimation() {
            animationParent.DOLocalMoveX(50f, 0.05f, false).SetEase(Ease.Linear);
            ButtonImage.sprite = ButtonSprite_Selected;
        }

        public override void PlayHighlightAnimation() {
            animationParent.DOLocalMoveX(0f, 0.1f, false).SetEase(Ease.Linear);
            ButtonImage.sprite = ButtonSprite_Selected;
        }

        public override void PlayDeselectAnimation(bool skip = false) {
            animationParent.DOLocalMoveX(70f, 0.1f * (float) Convert.ToInt32(!skip), false).SetEase(Ease.Linear);
            ButtonImage.sprite = ButtonSprite_Unselected;
        }

        private static void CacheResources() {
            if (LoadedResources) return;
            LoadedResources = true;
            var resourceLocation = Path.GetDirectoryName(CommonAPIPlugin.Instance.Info.Location);
            var buttonUnselectedSpriteLocation = Path.Combine(resourceLocation, "Phone-SimpleButton.png");
            var buttonSelectedSpriteLocation = Path.Combine(resourceLocation, "Phone-SimpleButton-Selected.png");
            ButtonSprite_Unselected = TextureUtility.LoadSprite(buttonUnselectedSpriteLocation);
            ButtonSprite_Selected = TextureUtility.LoadSprite(buttonSelectedSpriteLocation);
        }

        public static SimplePhoneButton Create(string label = "") {
            CacheResources();
            var buttonGameObject = new GameObject("Simple Button");
            buttonGameObject.layer = Layers.Phone;
            var buttonAnimationParent = new GameObject("Animation Parent");
            buttonAnimationParent.layer = Layers.Phone;
            var buttonImageGO = new GameObject("Button Image");
            buttonImageGO.layer = Layers.Phone;
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
            button.animationParent = buttonAnimationParent.transform;
            button.ButtonImage = buttonImage;

            var labelGO = new GameObject("Label");
            var tmp = labelGO.AddComponent<TextMeshProUGUI>();
            tmp.text = label;
            tmp.alignment = TextAlignmentOptions.Left;
            tmp.font = AppUtility.GetAppFont();
            tmp.fontSize = 60f;
            tmp.fontSizeMax = 60f;
            labelGO.transform.SetParent(buttonAnimationParent.transform, false);
            labelGO.transform.localPosition = new Vector3(-475f, 0f, 0f);
            var labelRect = labelGO.RectTransform();
            labelRect.SetAnchorAndPivot(0f, 0.5f);
            labelGO.RectTransform().sizeDelta = new Vector2(850f, 1f);
            return button;
        }
    }
}
