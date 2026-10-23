using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class IntroSequence : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector director;

    [SerializeField]
    private CanvasGroup menuButtons;

    [SerializeField]
    private GameObject skipHint;

    [SerializeField]
    private InstructionsPage instructionsPage;

    [SerializeField]
    private float menuFadeDuration = 0.4f;

    [SerializeField]
    private InputAction skipAction = CreateDefaultSkipAction();

    private Action onFinished;
    private bool isPlaying;

    private void Awake()
    {
        skipHint.SetActive(false);
    }

    private void OnEnable()
    {
        skipAction.performed += OnSkipPerformed;
    }

    private void OnDisable()
    {
        skipAction.performed -= OnSkipPerformed;
        skipAction.Disable();
    }

    private void Update()
    {
        if (isPlaying && director.time >= director.duration)
        {
            ShowInstructions();
        }
    }

    public void Play(Action finished)
    {
        if (isPlaying)
        {
            return;
        }

        isPlaying = true;
        onFinished = finished;

        menuButtons.interactable = false;
        menuButtons.blocksRaycasts = false;
        StartCoroutine(FadeOut(menuButtons));

        skipHint.SetActive(true);
        skipAction.Enable();
        director.Play();
    }

    public void Skip()
    {
        if (!isPlaying)
        {
            return;
        }

        director.time = director.duration;
        director.Evaluate();
        ShowInstructions();
    }

    private void ShowInstructions()
    {
        isPlaying = false;
        skipAction.Disable();
        skipHint.SetActive(false);
        instructionsPage.Open("Begin", onFinished);
    }

    private void OnSkipPerformed(InputAction.CallbackContext context)
    {
        Skip();
    }

    private IEnumerator FadeOut(CanvasGroup group)
    {
        for (var elapsed = 0f; elapsed < menuFadeDuration; elapsed += Time.deltaTime)
        {
            group.alpha = 1f - elapsed / menuFadeDuration;
            yield return null;
        }

        group.alpha = 0f;
    }

    private static InputAction CreateDefaultSkipAction()
    {
        var action = new InputAction("Skip Intro", InputActionType.Button);
        action.AddBinding("<Keyboard>/space");
        action.AddBinding("<Keyboard>/enter");
        action.AddBinding("<Gamepad>/start");
        return action;
    }
}
