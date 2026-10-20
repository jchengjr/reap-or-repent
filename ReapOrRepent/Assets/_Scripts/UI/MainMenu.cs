using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Scene loaded by Play. Must be added to the Build Profiles scene list.
    [SerializeField]
    private string gameSceneName = "NoahWorkScene";

    // Shared settings panel (also used by the in-game Esc menu).
    [SerializeField]
    private GameObject settingsPanel;

    private void Start()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

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
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
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
