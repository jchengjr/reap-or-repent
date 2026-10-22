using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

// Settings panel shared by the main menu and gameplay scenes. As the pause menu,
// Esc opens it and pauses the game; Resume or Esc closes it.
public class SettingsMenu : MonoBehaviour
{
    // Exposed parameter names in MainMixer, also used as the PlayerPrefs keys.
    private const string MusicVolumeParameter = "MusicVolume";
    private const string SfxVolumeParameter = "SFXVolume";

    // Slider value that maps to the mixer's -80 dB floor.
    private const float MinVolume = 0.0001f;

    // True while the pause menu is open, for systems that should ignore input then.
    public static bool IsPaused { get; private set; }

    // Off in the main menu: there Esc only closes the panel, and opening it doesn't pause.
    [SerializeField]
    private bool isPauseMenu = true;

    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider sfxSlider;

    // Selected when the panel opens, so keyboard and gamepad navigation work.
    [SerializeField]
    private Selectable firstSelected;

    [SerializeField]
    private InputAction toggleAction = new InputAction("Toggle Settings", InputActionType.Button, "<Keyboard>/escape");

    private bool isOpen;
    private float timeScaleBeforePause = 1f;
    private GameObject selectionBeforeOpen;

    private void Awake()
    {
        EnsureEventSystem();
        panel.SetActive(false);

        if (musicSlider != null)
        {
            musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(MusicVolumeParameter, 1f));
        }

        if (sfxSlider != null)
        {
            sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(SfxVolumeParameter, 1f));
        }
    }

    private void Start()
    {
        // AudioMixer ignores SetFloat calls made during Awake, so saved volumes are applied here.
        ApplyVolume(MusicVolumeParameter, PlayerPrefs.GetFloat(MusicVolumeParameter, 1f));
        ApplyVolume(SfxVolumeParameter, PlayerPrefs.GetFloat(SfxVolumeParameter, 1f));
    }

    private void OnEnable()
    {
        toggleAction.performed += OnTogglePerformed;
        toggleAction.Enable();
    }

    private void OnDisable()
    {
        toggleAction.performed -= OnTogglePerformed;
        toggleAction.Disable();

        // Don't leave the game frozen if this menu goes away while it's open.
        Close();
    }

    public void Open()
    {
        if (isOpen)
        {
            return;
        }

        isOpen = true;
        panel.SetActive(true);

        if (isPauseMenu)
        {
            timeScaleBeforePause = Time.timeScale;
            Time.timeScale = 0f;
            IsPaused = true;
        }

        var eventSystem = EventSystem.current;
        if (eventSystem != null)
        {
            selectionBeforeOpen = eventSystem.currentSelectedGameObject;
            if (firstSelected != null)
            {
                eventSystem.SetSelectedGameObject(firstSelected.gameObject);
            }
        }
    }

    public void Close()
    {
        if (!isOpen)
        {
            return;
        }

        isOpen = false;
        panel.SetActive(false);
        PlayerPrefs.Save();

        if (isPauseMenu)
        {
            Time.timeScale = timeScaleBeforePause;
            IsPaused = false;
        }

        var eventSystem = EventSystem.current;
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(selectionBeforeOpen != null ? selectionBeforeOpen : null);
        }
    }

    public void SetMusicVolume(float volume)
    {
        SetVolume(MusicVolumeParameter, volume);
    }

    public void SetSfxVolume(float volume)
    {
        SetVolume(SfxVolumeParameter, volume);
    }

    public void QuitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnTogglePerformed(InputAction.CallbackContext context)
    {
        if (isOpen)
        {
            Close();
        }
        else if (isPauseMenu)
        {
            Open();
        }
    }

    private void SetVolume(string parameter, float volume)
    {
        PlayerPrefs.SetFloat(parameter, volume);
        ApplyVolume(parameter, volume);
    }

    private void ApplyVolume(string parameter, float volume)
    {
        if (mixer == null)
        {
            return;
        }

        // Sliders are linear from 0 to 1; the mixer works in decibels.
        mixer.SetFloat(parameter, Mathf.Log10(Mathf.Max(volume, MinVolume)) * 20f);
    }

    private static void EnsureEventSystem()
    {
        // Gameplay scenes may not have an EventSystem yet, and the menu's controls need one.
        if (FindAnyObjectByType<EventSystem>() == null)
        {
            new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
        }
    }
}
