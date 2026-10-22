using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MenuBuilderUI;

// Generates the settings / pause menu prefab with placeholder UI wired to SettingsMenu.
// Run from Tools > Reap or Repent > Build Settings Menu Prefab, or in batch mode with
// -executeMethod SettingsMenuBuilder.BuildFromCommandLine [-overwrite]
public static class SettingsMenuBuilder
{
    public const string PrefabPath = "Assets/_Prefabs/UI/SettingsMenu.prefab";
    private const string MixerPath = "Assets/_Sound/MainMixer.mixer";

    [MenuItem("Tools/Reap or Repent/Build Settings Menu Prefab")]
    private static void BuildFromMenu()
    {
        if (!HasTmpResources())
        {
            TMPro.TMP_PackageResourceImporter.ImportResources(true, false, false);
            EditorUtility.DisplayDialog("Settings Menu Builder",
                "Importing TextMeshPro resources. Run Build Settings Menu Prefab again once the import finishes.", "OK");
            return;
        }

        if (File.Exists(PrefabPath) && !EditorUtility.DisplayDialog("Settings Menu Builder",
                PrefabPath + " already exists. Replace it? Scenes using it keep their overrides.", "Replace", "Cancel"))
        {
            return;
        }

        BuildPrefab();
    }

    public static void BuildFromCommandLine()
    {
        if (!HasTmpResources())
        {
            Debug.LogError("TextMeshPro Essential Resources are missing. Import them before building the settings menu.");
            EditorApplication.Exit(1);
            return;
        }

        if (File.Exists(PrefabPath) && !System.Environment.GetCommandLineArgs().Contains("-overwrite"))
        {
            Debug.LogError(PrefabPath + " already exists. Pass -overwrite to replace it.");
            EditorApplication.Exit(1);
            return;
        }

        BuildPrefab();
        EditorApplication.Exit(0);
    }

    public static GameObject BuildPrefab()
    {
        var mixer = AssetDatabase.LoadAssetAtPath<AudioMixer>(MixerPath);
        if (mixer == null)
        {
            Debug.LogWarning(MixerPath + " not found. The volume sliders won't change any audio until a mixer is assigned.");
        }

        // Built in a preview scene so the open scene isn't touched.
        var previewScene = EditorSceneManager.NewPreviewScene();
        var root = CreateCanvas("SettingsMenu", 10);
        SceneManager.MoveGameObjectToScene(root.gameObject, previewScene);

        var panel = CreateRect("Panel", root);
        Stretch(panel);
        // Opaque enough to block clicks on whatever is behind it.
        panel.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.85f);

        var title = CreateText("Title", panel, "Settings", 72, TextAlignmentOptions.Center);
        AnchorCenter(title.rectTransform, new Vector2(0, 320), new Vector2(800, 120));

        var musicLabel = CreateText("MusicLabel", panel, "Music Volume", 36, TextAlignmentOptions.Center);
        AnchorCenter(musicLabel.rectTransform, new Vector2(0, 170), new Vector2(600, 60));
        var musicSlider = CreateSlider("MusicSlider", panel);
        AnchorCenter((RectTransform)musicSlider.transform, new Vector2(0, 110), new Vector2(600, 40));

        var sfxLabel = CreateText("SFXLabel", panel, "Sound Effects Volume", 36, TextAlignmentOptions.Center);
        AnchorCenter(sfxLabel.rectTransform, new Vector2(0, 20), new Vector2(600, 60));
        var sfxSlider = CreateSlider("SFXSlider", panel);
        AnchorCenter((RectTransform)sfxSlider.transform, new Vector2(0, -40), new Vector2(600, 40));

        var resumeButton = CreateButton("ResumeButton", panel, "Resume");
        AnchorCenter((RectTransform)resumeButton.transform, new Vector2(0, -190), new Vector2(360, 80));
        var quitButton = CreateButton("QuitButton", panel, "Quit Game");
        AnchorCenter((RectTransform)quitButton.transform, new Vector2(0, -300), new Vector2(360, 80));

        var menu = root.gameObject.AddComponent<SettingsMenu>();
        UnityEventTools.AddPersistentListener(musicSlider.onValueChanged, menu.SetMusicVolume);
        UnityEventTools.AddPersistentListener(sfxSlider.onValueChanged, menu.SetSfxVolume);
        UnityEventTools.AddPersistentListener(resumeButton.onClick, menu.Close);
        UnityEventTools.AddPersistentListener(quitButton.onClick, menu.QuitPressed);

        var serializedMenu = new SerializedObject(menu);
        serializedMenu.FindProperty("panel").objectReferenceValue = panel.gameObject;
        serializedMenu.FindProperty("mixer").objectReferenceValue = mixer;
        serializedMenu.FindProperty("musicSlider").objectReferenceValue = musicSlider;
        serializedMenu.FindProperty("sfxSlider").objectReferenceValue = sfxSlider;
        serializedMenu.FindProperty("firstSelected").objectReferenceValue = resumeButton;
        serializedMenu.ApplyModifiedPropertiesWithoutUndo();

        // Hidden until opened, so it doesn't cover scenes in the Editor.
        panel.gameObject.SetActive(false);

        var folder = Path.GetDirectoryName(PrefabPath).Replace('\\', '/');
        if (!AssetDatabase.IsValidFolder(folder))
        {
            AssetDatabase.CreateFolder(Path.GetDirectoryName(folder).Replace('\\', '/'), Path.GetFileName(folder));
        }

        var prefab = PrefabUtility.SaveAsPrefabAsset(root.gameObject, PrefabPath);
        EditorSceneManager.ClosePreviewScene(previewScene);

        Debug.Log("Built " + PrefabPath);
        return prefab;
    }
}
