using CommonAPI.UI;
using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CommonAPI.Phone {
    /// <summary>
    /// Utility class for creating Phone UI elements.
    /// </summary>
    public static class PhoneUIUtility {
        /// <summary>
        /// Creates a small button with a label.
        /// </summary>
        public static SimplePhoneButton CreateSimpleButton(string label) {
            var buttonGameObject = new GameObject("Simple Button");
            buttonGameObject.layer = Layers.Phone;
            var button = buttonGameObject.AddComponent<SimplePhoneButton>();
            var buttonAnimationParent = new GameObject("Animation Parent");
            buttonAnimationParent.layer = Layers.Phone;
            var buttonImageGO = new GameObject("Button Image");
            buttonImageGO.layer = Layers.Phone;
            buttonImageGO.transform.SetParent(buttonAnimationParent.transform, false);
            buttonAnimationParent.transform.SetParent(buttonGameObject.transform, false);
            var buttonImage = buttonImageGO.AddComponent<Image>();
            button.ButtonImage = buttonImage;
            buttonImageGO.RectTransform().sizeDelta = new Vector2(SimplePhoneButton.ButtonImageWidth, SimplePhoneButton.ButtonImageHeight);
            button.SetGraphics();
            var rect = buttonGameObject.AddComponent<RectTransform>();
            rect.anchorMax = new Vector2(1f, 1f);
            rect.anchorMin = new Vector2(1f, 1f);
            rect.pivot = new Vector2(1f, 1f);
            button.AnimationParent = buttonAnimationParent.transform;
            var labelGO = new GameObject("Label");
            var tmp = labelGO.AddComponent<TextMeshProUGUI>();
            tmp.text = label;
            tmp.alignment = TextAlignmentOptions.Left;
            tmp.font = AppUtility.GetAppFont();
            tmp.fontSize = 60f;
            tmp.fontSizeMax = 60f;
            tmp.fontSizeMin = 50f;
            tmp.enableAutoSizing = true;
            button.Label = tmp;
            labelGO.transform.SetParent(buttonAnimationParent.transform, false);
            labelGO.transform.localPosition = new Vector3(-475f, 0f, 0f);
            var labelRect = labelGO.RectTransform();
            labelRect.SetAnchorAndPivot(0f, 0.5f);
            labelGO.RectTransform().sizeDelta = new Vector2(850f, 100f);
            button.ConfirmArrow = new GameObject("Confirm Arrow");
            button.ConfirmArrow.layer = Layers.Phone;
            button.ConfirmArrow.transform.SetParent(buttonAnimationParent.transform, false);
            var confirmArrowImage = button.ConfirmArrow.AddComponent<Image>();
            confirmArrowImage.sprite = SimplePhoneButton.ConfirmArrowSprite;
            var arrowRect = button.ConfirmArrow.RectTransform();
            arrowRect.sizeDelta = new Vector2(SimplePhoneButton.ConfirmArrowWidth, SimplePhoneButton.ConfirmArrowHeight);
            arrowRect.localPosition = new Vector3(425f, 0f, 0f);
            return button;
        }
    }
}
