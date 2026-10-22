using UnityEngine;
using UnityEngine.EventSystems;

// On-screen arrow that pans the room camera while it's held, and hides when the view can't go further that way.
[RequireComponent(typeof(CanvasGroup))]
public class PanArrowButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField]
    private RoomCameraPan pan;

    [SerializeField]
    private Vector2 direction;

    private CanvasGroup canvasGroup;
    private bool held;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        var visible = pan.CanPan(direction);
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.blocksRaycasts = visible;

        if (!visible)
        {
            Release();
        }
    }

    private void OnDisable()
    {
        Release();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        held = true;
        pan.SetArrowDirection(direction);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Release();
    }

    // Dragging off the arrow stops panning too.
    public void OnPointerExit(PointerEventData eventData)
    {
        Release();
    }

    private void Release()
    {
        if (!held)
        {
            return;
        }

        held = false;
        pan.SetArrowDirection(Vector2.zero);
    }
}
