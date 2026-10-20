using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

// Generates the main menu scene with placeholder UI wired to MainMenu.
// Run from Tools > Reap or Repent > Build Main Menu Scene, or in batch mode with
// -executeMethod MainMenuSceneBuilder.BuildFromCommandLine
public static class MainMenuSceneBuilder
{
    private const string ScenePath = "Assets/_Scenes/MainMenu.unity";
    private const string GameScenePath = "Assets/_Scenes/NoahWorkScene.unity";
    private const string DefaultUIActionsPath = "Packages/com.unity.inputsystem/InputSystem/Plugins/PlayerInput/DefaultInputActions.inputactions";

    private static readonly Color BackgroundColor = new Color(0.05f, 0.05f, 0.08f);
    private static readonly Color TextColor = new Color(0.9f, 0.88f, 0.84f);

    [MenuItem("Tools/Reap or Repent/Build Main Menu Scene")]
    private static void BuildFromMenu()
    {
        if (!HasTmpResources())
        {
            TMP_PackageResourceImporter.ImportResources(true, false, false);
            EditorUtility.DisplayDialog("Main Menu Builder",
                "Importing TextMeshPro resources. Run Build Main Menu Scene again once the import finishes.", "OK");
            return;
        }

        if (File.Exists(ScenePath) && !EditorUtility.DisplayDialog("Main Menu Builder",
                ScenePath + " already exists. Replace it?", "Replace", "Cancel"))
        {
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            return;
        }

        Build();
    }

    public static void BuildFromCommandLine()
    {
        if (!HasTmpResources())
        {
            Debug.LogError("TextMeshPro Essential Resources are missing. Import them before building the main menu.");
            EditorApplication.Exit(1);
            return;
        }

        if (File.Exists(ScenePath))
        {
            Debug.LogError(ScenePath + " already exists. Not overwriting it from the command line.");
            EditorApplication.Exit(1);
            return;
        }

        Build();
        EditorApplication.Exit(0);
    }

    private static bool HasTmpResources()
    {
        return Resources.Load<TMP_Settings>("TMP Settings") != null;
    }

    private static void Build()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        CreateCamera();
        var eventSystem = CreateEventSystem();
        var canvas = CreateCanvas();

        var background = CreateRect("Background", canvas);
        Stretch(background);
        background.gameObject.AddComponent<Image>().color = BackgroundColor;

        var title = CreateText("Title", canvas, "Reap or Repent", 96, TextAlignmentOptions.Left);
        AnchorTopLeft(title.rectTransform, new Vector2(120, -120), new Vector2(1000, 140));

        var buttonColumn = CreateRect("Buttons", canvas);
        AnchorTopLeft(buttonColumn, new Vector2(120, -320), new Vector2(360, 288));
        var layout = buttonColumn.gameObject.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 24;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        var playButton = CreateButton("PlayButton", buttonColumn, "Play");
        var settingsButton = CreateButton("SettingsButton", buttonColumn, "Settings");
        var quitButton = CreateButton("QuitButton", buttonColumn, "Quit");

        var settingsPanel = CreateSettingsPanel(canvas, out var backButton);

        var mainMenu = new GameObject("MainMenu").AddComponent<MainMenu>();
        UnityEventTools.AddPersistentListener(playButton.onClick, mainMenu.PlayPressed);
        UnityEventTools.AddPersistentListener(settingsButton.onClick, mainMenu.SettingsPressed);
        UnityEventTools.AddPersistentListener(quitButton.onClick, mainMenu.QuitPressed);
        UnityEventTools.AddPersistentListener(backButton.onClick, mainMenu.CloseSettings);

        var serializedMenu = new SerializedObject(mainMenu);
        serializedMenu.FindProperty("gameSceneName").stringValue = Path.GetFileNameWithoutExtension(GameScenePath);
        serializedMenu.FindProperty("settingsPanel").objectReferenceValue = settingsPanel;
        serializedMenu.ApplyModifiedPropertiesWithoutUndo();

        // Lets keyboard and gamepad navigation start on Play.
        eventSystem.firstSelectedGameObject = playButton.gameObject;
        settingsPanel.SetActive(false);

        EditorSceneManager.SaveScene(scene, ScenePath);
        AddToBuildScenes();
        Debug.Log("Built " + ScenePath);
    }

    private static void CreateCamera()
    {
        var cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
        cameraObject.tag = "MainCamera";

        var camera = cameraObject.GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = BackgroundColor;
    }

    private static EventSystem CreateEventSystem()
    {
        // The project only has the new Input System enabled, so the old StandaloneInputModule won't work.
        var eventSystemObject = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
        AssignDefaultUIActions(eventSystemObject.GetComponent<InputSystemUIInputModule>());
        return eventSystemObject.GetComponent<EventSystem>();
    }

    // Same assignment the Inspector makes when the module is added by hand: references to the
    // Input System package's default UI actions, rather than a copy embedded in the scene.
    private static void AssignDefaultUIActions(InputSystemUIInputModule module)
    {
        var references = AssetDatabase.LoadAllAssetsAtPath(DefaultUIActionsPath).OfType<InputActionReference>().ToArray();

        InputActionReference Find(string actionName)
        {
            return references.FirstOrDefault(r => r.action != null && r.action.name == actionName);
        }

        module.actionsAsset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(DefaultUIActionsPath);
        module.point = Find("Point");
        module.leftClick = Find("Click");
        module.rightClick = Find("RightClick");
        module.middleClick = Find("MiddleClick");
        module.scrollWheel = Find("ScrollWheel");
        module.move = Find("Navigate");
        module.submit = Find("Submit");
        module.cancel = Find("Cancel");
        module.trackedDevicePosition = Find("TrackedDevicePosition");
        module.trackedDeviceOrientation = Find("TrackedDeviceOrientation");
    }

    private static RectTransform CreateCanvas()
    {
        var canvasObject = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasObject.layer = LayerMask.NameToLayer("UI");
        canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        var scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        return canvasObject.GetComponent<RectTransform>();
    }

    private static GameObject CreateSettingsPanel(Transform parent, out Button backButton)
    {
        var panel = CreateRect("SettingsPanel", parent);
        Stretch(panel);
        // Opaque enough to block clicks on the menu behind it.
        panel.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.85f);

        var title = CreateText("Title", panel, "Settings", 72, TextAlignmentOptions.Center);
        AnchorCenter(title.rectTransform, new Vector2(0, 300), new Vector2(800, 120));

        var placeholder = CreateText("Placeholder", panel, "Settings options go here.", 32, TextAlignmentOptions.Center);
        AnchorCenter(placeholder.rectTransform, Vector2.zero, new Vector2(1000, 80));

        backButton = CreateButton("BackButton", panel, "Back");
        AnchorCenter((RectTransform)backButton.transform, new Vector2(0, -300), new Vector2(360, 80));

        return panel.gameObject;
    }

    private static Button CreateButton(string name, Transform parent, string label)
    {
        var rect = CreateRect(name, parent);

        var image = rect.gameObject.AddComponent<Image>();
        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        image.type = Image.Type.Sliced;

        var button = rect.gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        var colors = button.colors;
        colors.normalColor = new Color(0.16f, 0.16f, 0.2f);
        colors.highlightedColor = new Color(0.3f, 0.28f, 0.34f);
        colors.selectedColor = colors.highlightedColor;
        colors.pressedColor = new Color(0.1f, 0.1f, 0.12f);
        button.colors = colors;

        rect.gameObject.AddComponent<LayoutElement>().preferredHeight = 80;

        var text = CreateText("Text (TMP)", rect, label, 40, TextAlignmentOptions.Center);
        Stretch(text.rectTransform);

        return button;
    }

    private static TextMeshProUGUI CreateText(string name, Transform parent, string content, float fontSize, TextAlignmentOptions alignment)
    {
        var text = CreateRect(name, parent).gameObject.AddComponent<TextMeshProUGUI>();
        text.text = content;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = TextColor;
        text.raycastTarget = false;
        return text;
    }

    private static RectTransform CreateRect(string name, Transform parent)
    {
        var rectObject = new GameObject(name, typeof(RectTransform));
        rectObject.layer = LayerMask.NameToLayer("UI");

        var rect = rectObject.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        return rect;
    }

    private static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static void AnchorTopLeft(RectTransform rect, Vector2 position, Vector2 size)
    {
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
    }

    private static void AnchorCenter(RectTransform rect, Vector2 position, Vector2 size)
    {
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
    }

    private static void AddToBuildScenes()
    {
        var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        scenes.RemoveAll(s => s.path == ScenePath || s.path == GameScenePath);

        scenes.Insert(0, new EditorBuildSettingsScene(ScenePath, true));
        if (File.Exists(GameScenePath))
        {
            scenes.Insert(1, new EditorBuildSettingsScene(GameScenePath, true));
        }

        EditorBuildSettings.scenes = scenes.ToArray();
    }
}
