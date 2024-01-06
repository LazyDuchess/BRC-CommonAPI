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
        public const float ButtonImageWidth = 530f * 2f;
        public const float ButtonImageHeight = 75f * 2f;
        private static bool LoadedResources = false;
        private static Sprite ButtonSprite_Unselected = null;
        private static Sprite ButtonSprite_Selected = null;
        public override float Width => ButtonImage.rectTransform.sizeDelta.x;
        public override float Height => ButtonImage.rectTransform.sizeDelta.y;

        public Image ButtonImage = null;
        public Sprite SelectedButtonSprite = null;
        public Sprite UnselectedButtonSprite = null;
        public Transform AnimationParent = null;
        public TextMeshProUGUI Label = null;
        public Color LabelSelectedColor = new Color32(49, 90, 165, 255);
        public Color LabelUnselectedColor = Color.white;

        public override void PlayHoldAnimation() {
            AnimationParent.DOLocalMoveX(50f, 0.05f, false).SetEase(Ease.Linear);
            ButtonImage.sprite = ButtonSprite_Selected;
            Label.faceColor = LabelSelectedColor;
        }

        public override void PlayHighlightAnimation() {
            AnimationParent.DOLocalMoveX(0f, 0.1f, false).SetEase(Ease.Linear);
            ButtonImage.sprite = ButtonSprite_Selected;
            Label.faceColor = LabelSelectedColor;
        }

        public override void PlayDeselectAnimation(bool skip = false) {
            AnimationParent.DOLocalMoveX(70f, 0.1f * (float) Convert.ToInt32(!skip), false).SetEase(Ease.Linear);
            ButtonImage.sprite = ButtonSprite_Unselected;
            Label.faceColor = LabelUnselectedColor;
        }

        public static void CacheResources() {
            if (LoadedResources) return;
            LoadedResources = true;
            var resourceLocation = Path.GetDirectoryName(CommonAPIPlugin.Instance.Info.Location);
            var buttonUnselectedSpriteLocation = Path.Combine(resourceLocation, "Phone-SimpleButton.png");
            var buttonSelectedSpriteLocation = Path.Combine(resourceLocation, "Phone-SimpleButton-Selected.png");
            ButtonSprite_Unselected = TextureUtility.LoadSprite(buttonUnselectedSpriteLocation);
            ButtonSprite_Selected = TextureUtility.LoadSprite(buttonSelectedSpriteLocation);
        }

        public void SetGraphics() {
            CacheResources();
            SelectedButtonSprite = ButtonSprite_Selected;
            UnselectedButtonSprite = ButtonSprite_Unselected;
        }
    }
}
