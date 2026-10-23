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

    // Plays before the game scene loads. Without it, Play loads the game straight away.
    [SerializeField]
    private IntroSequence intro;

    public void PlayPressed()
    {
        if (!Application.CanStreamedLevelBeLoaded(gameSceneName))
        {
            Debug.LogError($"Scene '{gameSceneName}' is not in the Build Profiles scene list.");
            return;
        }

        if (intro != null)
        {
            intro.Play(StartGame);
        }
        else
        {
            StartGame();
        }
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

    private void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
