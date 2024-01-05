// Thanks Giygas and Nite!
// https://github.com/SlopCrew/SlopCrew/blob/f356897d0e480673fd6a0715c6f3a6bec6338255/SlopCrew.Plugin/UI/RectTransformExtensions.cs

using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

namespace CommonAPI.UI;

public static class RectTransformExtensions {
    public static void SetBounds(this RectTransform rect, float left, float top, float right, float bottom) {
        rect.offsetMin = new Vector2(-left, -top);
        rect.offsetMax = new Vector2(right, bottom);
    }

    public static void SetAnchor(this RectTransform rect, float x, float y) {
        Vector2 anchor = new Vector2(x, y);
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
    }

    public static void SetPivot(this RectTransform rect, float x, float y) {
        rect.pivot = new Vector2(x, y);
    }

    public static void SetAnchorAndPivot(this RectTransform rect, float x, float y) {
        Vector2 point = new Vector2(x, y);
        rect.anchorMin = point;
        rect.anchorMax = point;
        rect.pivot = point;
    }

    public static void StretchToFillParent(this RectTransform rect) {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.SetBounds(0, 0, 0, 0);
    }
}
