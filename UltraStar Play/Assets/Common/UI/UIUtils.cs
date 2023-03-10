using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class UIUtils
{
    public static void ForEachElementWithClass(VisualElement root, Action<VisualElement> callback, params string[] classNames)
    {
        foreach (string className in classNames)
        {
            root.Query(null, className).ForEach(callback);
        }
    }

    private static readonly Dictionary<VisualElement, BackgroundColorConfig> elementsBgColorsDict = new();

    public static void SetBackgroundStyleWithHoverAndFocus(VisualElement root, Color backgroundColor, Color hoverBackgroundColor, Color focusBackgroundColor, Color fontColor)
    {
        SetBackgroundStyleWithHoverAndFocus(root, root, backgroundColor, hoverBackgroundColor, focusBackgroundColor, fontColor);
    }

    public static void SetBackgroundStyleWithHoverAndFocus(VisualElement root, VisualElement hoverRoot, Color backgroundColor, Color hoverBackgroundColor, Color focusBackgroundColor, Color fontColor)
    {
        if (root == null)
        {
            return;
        }

        root.style.color = fontColor;
        root.style.backgroundColor = backgroundColor;

        // We can't access pseudo states through the API (e.g. :hover), so we have to manually mimic them
        if (!elementsBgColorsDict.ContainsKey(hoverRoot))
        {
            elementsBgColorsDict.Add(hoverRoot, new BackgroundColorConfig(backgroundColor, hoverBackgroundColor, focusBackgroundColor));

            hoverRoot.RegisterCallback<PointerEnterEvent>(evt =>
            {
                Color color = elementsBgColorsDict[hoverRoot].hoverBackgroundColor;
                color.a = root.resolvedStyle.backgroundColor.a;
                root.style.backgroundColor = color;
            });
            hoverRoot.RegisterCallback<PointerLeaveEvent>(evt =>
            {
                Color color = elementsBgColorsDict[hoverRoot].backgroundColor;
                color.a = root.resolvedStyle.backgroundColor.a;
                root.style.backgroundColor = color;
            });

            hoverRoot.RegisterCallback<FocusEvent>(evt =>
            {
                Color color = elementsBgColorsDict[hoverRoot].focusBackgroundColor;
                color.a = root.resolvedStyle.backgroundColor.a;
                root.style.backgroundColor = color;
            });
            hoverRoot.RegisterCallback<BlurEvent>(evt =>
            {
                Color color = elementsBgColorsDict[hoverRoot].backgroundColor;
                color.a = root.resolvedStyle.backgroundColor.a;
                root.style.backgroundColor = color;
            });
        }
        else
        {
            elementsBgColorsDict[hoverRoot] = new BackgroundColorConfig(backgroundColor, hoverBackgroundColor, focusBackgroundColor);
        }
    }

    public static void ApplyFontColorForElements(VisualElement root, string[] names, string[] classes, Color fontColor)
    {
        if (names == null)
        {
            root.Query(null, classes).ForEach(element => element.style.color = fontColor);
            return;
        }

        foreach (string name in names)
        {
            root.Query(name, classes).ForEach(element => element.style.color = fontColor);
        }
    }

    public static Color ColorHSVOffset(Color inputColor, float hueOffset, float saturationOffset, float valueOffset)
    {
        float h, s, v;
        Color.RGBToHSV(inputColor, out h, out s, out v);
        h += hueOffset;
        s += saturationOffset;
        v += valueOffset;
        return Color.HSVToRGB(h, s, v);
    }

    public class BackgroundColorConfig
    {
        public Color backgroundColor;
        public Color hoverBackgroundColor;
        public Color focusBackgroundColor;

        public BackgroundColorConfig(Color backgroundColor, Color hoverBackgroundColor, Color focusBackgroundColor)
        {
            this.backgroundColor = backgroundColor;
            this.hoverBackgroundColor = hoverBackgroundColor;
            this.focusBackgroundColor = focusBackgroundColor;
        }
    }
}
