using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommonAPI.Phone {
    public class PhoneScrollView : MonoBehaviour {
        public static PhoneScrollView Create(CustomApp app) {
            var scrollview = new GameObject("Scroll View");
            scrollview.transform.SetParent(app.Content, false);
            var scrollViewComponent = scrollview.AddComponent<PhoneScrollView>();
            var rect = scrollview.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.anchoredPosition = new Vector2(0f, -CustomApp.TitleBarHeight);
            return scrollViewComponent;
        }

        public void AddButton(PhoneButton button) {
            button.transform.SetParent(transform, false);
            var rect = button.RectTransform();
            rect.anchoredPosition = new Vector2(-button.Width / 2f, -button.Height / 2f);
        }
    }
}
