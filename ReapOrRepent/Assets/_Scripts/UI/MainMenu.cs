using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Scene loaded by Play. Must be added to the Build Profiles scene list.
    [SerializeField]
    private string gameSceneName = "NoahWorkScene";

    // Shared settings menu (the same prefab is the in-game pause menu).
    [SerializeField]
    private SettingsMenu settingsMenu;

    public void PlayPressed()
    {
        if (!Application.CanStreamedLevelBeLoaded(gameSceneName))
        {
            Debug.LogError($"Scene '{gameSceneName}' is not in the Build Profiles scene list.");
            return;
        }

        SceneManager.LoadScene(gameSceneName);
    }

    public void SettingsPressed()
    {
        if (settingsMenu != null)
        {
            settingsMenu.Open();
        }
    }

    public void QuitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
