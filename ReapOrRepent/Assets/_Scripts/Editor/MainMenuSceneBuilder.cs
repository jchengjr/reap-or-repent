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
using static MenuBuilderUI;

// Generates the main menu scene with placeholder UI wired to MainMenu, using the settings menu prefab.
// Run from Tools > Reap or Repent > Build Main Menu Scene, or in batch mode with
// -executeMethod MainMenuSceneBuilder.BuildFromCommandLine [-overwrite]
public static class MainMenuSceneBuilder
{
    private const string ScenePath = "Assets/_Scenes/MainMenu.unity";
    private const string GameScenePath = "Assets/_Scenes/NoahWorkScene.unity";
    private const string DefaultUIActionsPath = "Packages/com.unity.inputsystem/InputSystem/Plugins/PlayerInput/DefaultInputActions.inputactions";

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

        if (File.Exists(ScenePath) && !System.Environment.GetCommandLineArgs().Contains("-overwrite"))
        {
            Debug.LogError(ScenePath + " already exists. Pass -overwrite to replace it.");
            EditorApplication.Exit(1);
            return;
        }

        Build();
        EditorApplication.Exit(0);
    }

    private static void Build()
    {
        var settingsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(SettingsMenuBuilder.PrefabPath);
        if (settingsPrefab == null)
        {
            settingsPrefab = SettingsMenuBuilder.BuildPrefab();
        }

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        CreateCamera();
        var eventSystem = CreateEventSystem();
        var canvas = CreateCanvas("Canvas", 0);

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

        var settingsMenu = CreateSettingsMenu(settingsPrefab, scene);

        var mainMenu = new GameObject("MainMenu").AddComponent<MainMenu>();
        UnityEventTools.AddPersistentListener(playButton.onClick, mainMenu.PlayPressed);
        UnityEventTools.AddPersistentListener(settingsButton.onClick, mainMenu.SettingsPressed);
        UnityEventTools.AddPersistentListener(quitButton.onClick, mainMenu.QuitPressed);

        var serializedMenu = new SerializedObject(mainMenu);
        serializedMenu.FindProperty("gameSceneName").stringValue = Path.GetFileNameWithoutExtension(GameScenePath);
        serializedMenu.FindProperty("settingsMenu").objectReferenceValue = settingsMenu;
        serializedMenu.ApplyModifiedPropertiesWithoutUndo();

        // Lets keyboard and gamepad navigation start on Play.
        eventSystem.firstSelectedGameObject = playButton.gameObject;

        EditorSceneManager.SaveScene(scene, ScenePath);
        AddToBuildScenes();
        Debug.Log("Built " + ScenePath);
    }

    // In the main menu the settings menu doesn't pause, its Resume button reads "Back",
    // and Quit Game is hidden because the menu has its own Quit.
    private static SettingsMenu CreateSettingsMenu(GameObject prefab, UnityEngine.SceneManagement.Scene scene)
    {
        var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);
        var settingsMenu = instance.GetComponent<SettingsMenu>();

        var serializedSettings = new SerializedObject(settingsMenu);
        serializedSettings.FindProperty("isPauseMenu").boolValue = false;
        serializedSettings.ApplyModifiedPropertiesWithoutUndo();

        var resumeLabel = instance.transform.Find("Panel/ResumeButton/Text (TMP)").GetComponent<TextMeshProUGUI>();
        resumeLabel.text = "Back";
        PrefabUtility.RecordPrefabInstancePropertyModifications(resumeLabel);

        var quitButton = instance.transform.Find("Panel/QuitButton").gameObject;
        quitButton.SetActive(false);
        PrefabUtility.RecordPrefabInstancePropertyModifications(quitButton);

        return settingsMenu;
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
