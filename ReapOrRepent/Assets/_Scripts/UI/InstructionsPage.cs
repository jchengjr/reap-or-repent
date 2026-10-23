using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InstructionsPage : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup page;

    [SerializeField]
    private Button closeButton;

    [SerializeField]
    private TextMeshProUGUI closeLabel;

    [SerializeField]
    private float fadeDuration = 0.5f;

    private Action onClosed;
    private GameObject selectionBeforeOpen;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        page.gameObject.SetActive(false);
    }

    public void Open(string closeText, Action closed = null)
    {
        if (IsOpen)
        {
            return;
        }

        IsOpen = true;
        onClosed = closed;
        closeLabel.text = closeText;
        page.gameObject.SetActive(true);

        var eventSystem = EventSystem.current;
        selectionBeforeOpen = eventSystem != null ? eventSystem.currentSelectedGameObject : null;

        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public void Close()
    {
        if (!IsOpen)
        {
            return;
        }

        IsOpen = false;
        StopAllCoroutines();
        page.gameObject.SetActive(false);

        var eventSystem = EventSystem.current;
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(selectionBeforeOpen);
        }

        var closed = onClosed;
        onClosed = null;
        closed?.Invoke();
    }

    private IEnumerator FadeIn()
    {
        page.interactable = false;
        page.alpha = 0f;

        for (var elapsed = 0f; elapsed < fadeDuration; elapsed += Time.unscaledDeltaTime)
        {
            page.alpha = elapsed / fadeDuration;
            yield return null;
        }

        page.alpha = 1f;
        page.interactable = true;

        var eventSystem = EventSystem.current;
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(closeButton.gameObject);
        }
    }
}
