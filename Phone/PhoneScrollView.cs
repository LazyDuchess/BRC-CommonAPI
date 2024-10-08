using Reptile;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Reptile.Phone;
using UnityEngine.UIElements;

namespace CommonAPI.Phone {
    /// <summary>
    /// UI Element that scrolls through buttons in a phone app.
    /// </summary>
    public class PhoneScrollView : MonoBehaviour {
        public const float DefaultLength = 1600f;
        public float AnimationSpeed = 0.1f;
        public List<PhoneButton> Buttons = [];
        public float Separation = 50f;
        public float Height = CustomApp.TitleBarHeight;
        public float Length = DefaultLength;
        public int SelectedIndex = 0;
        public CustomApp App = null;
        public float CurrentScroll = 0f;
        private float continuousScrollTimer;

        /// <summary>
        /// Create an empty scroll view.
        /// </summary>
        /// <param name="app">App this scroll view belongs to.</param>
        /// <param name="height">Height at which this scroll view begins, under the title bar by default.</param>
        /// <param name="length">Length of this scroll view.</param>
        public static PhoneScrollView Create(CustomApp app, float height = CustomApp.TitleBarHeight, float length = DefaultLength) {
            var scrollview = new GameObject("Scroll View");
            scrollview.transform.SetParent(app.Content, false);
            var scrollViewComponent = scrollview.AddComponent<PhoneScrollView>();
            scrollViewComponent.App = app;
            var rect = scrollview.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            scrollViewComponent.Height = height;
            scrollViewComponent.Length = length;
            rect.anchoredPosition = new Vector2(0f, -height);
            scrollview.layer = Layers.Phone;
            return scrollViewComponent;
        }

        public bool ValidateSelectedIndex() {
            if (Buttons.Count <= 0)
                return false;

            if (SelectedIndex < 0)
                SelectedIndex = 0;

            if (SelectedIndex >= Buttons.Count)
                SelectedIndex = Buttons.Count - 1;
            return true;
        }

        /// <summary>
        /// Adds a button and updates the scroll view.
        /// </summary>
        public void AddButton(PhoneButton button) {
            button.transform.SetParent(transform, false);
            Buttons.Add(button);
            UpdateButtons();
        }

        /// <summary>
        /// Inserts a button at the specified index.
        /// </summary>
        public void InsertButton(int index, PhoneButton button) {
            button.transform.SetParent(transform, false);
            Buttons.Insert(index, button);
            UpdateButtons();
        }

        /// <summary>
        /// Destroys a button from this scrollview by its index.
        /// </summary>
        public void RemoveButton(int index) {
            var button = Buttons[index];
            Buttons.RemoveAt(index);
            Destroy(button.gameObject);
            UpdateButtons();
        }

        /// <summary>
        /// Destroys a button from this scrollview.
        /// </summary>
        public void RemoveButton(PhoneButton button) {
            Buttons.Remove(button);
            Destroy(button.gameObject);
            UpdateButtons();
        }

        /// <summary>
        /// Destroys all buttons from this scrollview.
        /// </summary>
        public void RemoveAllButtons() {
            foreach(var button in Buttons)
                Destroy(button.gameObject);
            Buttons.Clear();
            UpdateButtons();
            // this might be hacky?
            ResetScroll();
            CancelAnimation();
        }

        public bool HasButton(PhoneButton button) {
            return Buttons.Contains(button);
        }

        public void StartScrollAnimation() {
            StartCoroutine(AnimateScrollCoroutine());
        }

        public void CancelAnimation() {
            StopAllCoroutines();
            var rect = this.RectTransform();
            rect.anchoredPosition = new Vector2(0f, -Height + CurrentScroll);
        }

        private IEnumerator AnimateScrollCoroutine() {
            var rect = this.RectTransform();
            var targetY = -Height + CurrentScroll;
            var currentY = rect.anchoredPosition.y;
            var distanceToTarget = targetY - currentY;
            var speed = distanceToTarget / AnimationSpeed;
            while (rect.anchoredPosition.y != targetY) {
                currentY = rect.anchoredPosition.y;
                if (targetY > currentY) {
                    currentY += Core.dt * speed;
                    if (currentY > targetY)
                        currentY = targetY;
                }
                else if (targetY < currentY) {
                    currentY += Core.dt * speed;
                    if (currentY < targetY)
                        currentY = targetY;
                }
                rect.anchoredPosition = new Vector2(0f, currentY);
                yield return null;
            }
        }

        public void OnReleaseRight() {
            if (!ValidateSelectedIndex()) return;
            var currentButton = Buttons[SelectedIndex];
            if (currentButton.BeingPressed)
            {
                currentButton.PlayHighlightAnimation();
                App.PlayConfirmSFX();
                currentButton.Confirm();
            }
            foreach (var button in Buttons)
                button.BeingPressed = false;
        }

        public void OnPressRight() {
            if (!ValidateSelectedIndex()) return;
            var currentButton = Buttons[SelectedIndex];
            currentButton.BeingPressed = true;
            currentButton.PlayHoldAnimation();
        }

        public void OnPressUp() {
            ScrollUp();
            continuousScrollTimer -= 0.4f;
        }

        public void OnPressDown() {
            ScrollDown();
            continuousScrollTimer -= 0.4f;
        }

        public void OnReleaseUp() {
            continuousScrollTimer = 0f;
        }

        public void OnReleaseDown() {
            continuousScrollTimer = 0f;
        }

        public void OnHoldUp() {
            continuousScrollTimer += Core.dt;
            if (continuousScrollTimer >= 0.1f) {
                continuousScrollTimer = 0f;
                ScrollUp();
            }
        }
        
        public void OnHoldDown() {
            continuousScrollTimer += Core.dt;
            if (continuousScrollTimer >= 0.1f) {
                continuousScrollTimer = 0f;
                ScrollDown();
            }
        }

        public void ScrollUp() {
            App.PlaySelectSFX();
            if (!ValidateSelectedIndex()) return;
            var previousButton = Buttons[SelectedIndex];
            SelectedIndex--;
            if (SelectedIndex < 0)
                SelectedIndex = 0;
            var currentButton = Buttons[SelectedIndex];
            if (currentButton != previousButton) {
                previousButton.PlayDeselectAnimation();
                currentButton.PlayHighlightAnimation();
            }
            var rect = currentButton.RectTransform();
            var buttonHeight = rect.anchoredPosition.y;
            var buttonHeightPlusSize = -buttonHeight + (currentButton.Height / 2f) + Separation - CurrentScroll;
            if (buttonHeightPlusSize < (currentButton.Height / 2f) + Separation) {
                CancelAnimation();
                CurrentScroll = -buttonHeight - (currentButton.Height / 2f) - Separation;
                StartScrollAnimation();
            }
        }

        /// <summary>
        /// Resets the scroll and selected item back to zero and updates the buttons.
        /// </summary>
        public void ResetScroll() {
            SelectedIndex = 0;
            CurrentScroll = 0f;
            UpdateButtons();
        }

        public void ScrollDown() {
            App.PlaySelectSFX();
            if (!ValidateSelectedIndex()) return;
            var previousButton = Buttons[SelectedIndex];
            SelectedIndex++;
            if (SelectedIndex >= Buttons.Count)
                SelectedIndex = Buttons.Count - 1;
            var currentButton = Buttons[SelectedIndex];
            if (currentButton != previousButton) {
                previousButton.PlayDeselectAnimation();
                currentButton.PlayHighlightAnimation();
            }
            var rect = currentButton.RectTransform();
            var buttonHeight = rect.anchoredPosition.y;
            var buttonHeightPlusSize = -buttonHeight + (currentButton.Height / 2f) + Separation - CurrentScroll;
            if (buttonHeightPlusSize > Length) {
                CancelAnimation();
                var buttonsThatFit = Mathf.Ceil(Length / (currentButton.Height + Separation));
                buttonsThatFit -= 2;
                CurrentScroll = -buttonHeight - (currentButton.Height / 2f) - Separation;
                CurrentScroll -= buttonsThatFit * (currentButton.Height + Separation);
                StartScrollAnimation();
            }
        }

        /// <summary>
        /// Updates the animations and positioning of all buttons.
        /// </summary>
        public void UpdateButtons() {
            if (!ValidateSelectedIndex()) return;
            var currentY = Separation;
            for (var i = 0; i < Buttons.Count; i++) {
                var button = Buttons[i];
                button.PlayDeselectAnimation(true);
                var rect = button.RectTransform();
                rect.anchoredPosition = new Vector2(-button.Width / 2f, (-button.Height / 2f) - currentY);
                currentY += button.Height + Separation;
            }
            var currentButton = Buttons[SelectedIndex];
            currentButton.PlayHighlightAnimation();
        }
    }
}
