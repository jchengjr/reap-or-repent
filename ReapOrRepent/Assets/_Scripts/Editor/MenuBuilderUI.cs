using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// Placeholder-styled UI helpers shared by the menu builders.
internal static class MenuBuilderUI
{
    public static readonly Color BackgroundColor = new Color(0.05f, 0.05f, 0.08f);
    public static readonly Color TextColor = new Color(0.9f, 0.88f, 0.84f);
    private static readonly Color ControlColor = new Color(0.16f, 0.16f, 0.2f);
    private static readonly Color HighlightColor = new Color(0.3f, 0.28f, 0.34f);
    private static readonly Color PressedColor = new Color(0.1f, 0.1f, 0.12f);
    private static readonly Color FillColor = new Color(0.62f, 0.58f, 0.66f);

    public static bool HasTmpResources()
    {
        return Resources.Load<TMP_Settings>("TMP Settings") != null;
    }

    public static RectTransform CreateCanvas(string name, int sortingOrder)
    {
        var canvasObject = new GameObject(name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasObject.layer = LayerMask.NameToLayer("UI");

        var canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = sortingOrder;

        var scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        return canvasObject.GetComponent<RectTransform>();
    }

    public static Button CreateButton(string name, Transform parent, string label)
    {
        var rect = CreateRect(name, parent);

        var image = rect.gameObject.AddComponent<Image>();
        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        image.type = Image.Type.Sliced;

        var button = rect.gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        var colors = button.colors;
        colors.normalColor = ControlColor;
        colors.highlightedColor = HighlightColor;
        colors.selectedColor = HighlightColor;
        colors.pressedColor = PressedColor;
        button.colors = colors;

        rect.gameObject.AddComponent<LayoutElement>().preferredHeight = 80;

        var text = CreateText("Text (TMP)", rect, label, 40, TextAlignmentOptions.Center);
        Stretch(text.rectTransform);

        return button;
    }

    public static Slider CreateSlider(string name, Transform parent)
    {
        var resources = new DefaultControls.Resources
        {
            standard = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd"),
            background = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd"),
            knob = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd"),
        };

        var sliderObject = DefaultControls.CreateSlider(resources);
        sliderObject.name = name;
        sliderObject.transform.SetParent(parent, false);
        foreach (var child in sliderObject.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = LayerMask.NameToLayer("UI");
        }

        var slider = sliderObject.GetComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 1f;

        sliderObject.transform.Find("Background").GetComponent<Image>().color = ControlColor;
        slider.fillRect.GetComponent<Image>().color = FillColor;
        slider.handleRect.GetComponent<Image>().color = TextColor;
        slider.handleRect.sizeDelta = new Vector2(32f, 0f);

        var colors = slider.colors;
        colors.highlightedColor = new Color(1f, 0.85f, 0.65f);
        colors.selectedColor = colors.highlightedColor;
        slider.colors = colors;

        return slider;
    }

    public static TextMeshProUGUI CreateText(string name, Transform parent, string content, float fontSize, TextAlignmentOptions alignment)
    {
        var text = CreateRect(name, parent).gameObject.AddComponent<TextMeshProUGUI>();
        text.text = content;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = TextColor;
        text.raycastTarget = false;
        return text;
    }

    public static RectTransform CreateRect(string name, Transform parent)
    {
        var rectObject = new GameObject(name, typeof(RectTransform));
        rectObject.layer = LayerMask.NameToLayer("UI");

        var rect = rectObject.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        return rect;
    }

    public static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    public static void AnchorTopLeft(RectTransform rect, Vector2 position, Vector2 size)
    {
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
    }

    public static void AnchorCenter(RectTransform rect, Vector2 position, Vector2 size)
    {
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
    }
}
