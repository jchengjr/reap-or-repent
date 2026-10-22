using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

// Moves the target that the room's CinemachineCamera follows, so the player can look around a room
// that's bigger than the screen. Arrow keys / WASD or the on-screen arrows pan; the camera's
// damping smooths the motion, and the target is kept where the view stays inside the room.
public class RoomCameraPan : MonoBehaviour
{
    // Room units per second (1920x1080 art at 100 pixels per unit is 19.2 x 10.8 units).
    [SerializeField]
    private float panSpeed = 5f;

    [SerializeField]
    private CinemachineCamera roomCamera;

    // Area the camera may show, normally the size of the room art.
    [SerializeField]
    private Collider2D roomBounds;

    [SerializeField]
    private InputAction panAction = CreateDefaultPanAction();

    private Vector2 arrowDirection;

    private void Awake()
    {
        EnsureCinemachineBrain();
    }

    private void OnEnable()
    {
        panAction.Enable();
    }

    private void OnDisable()
    {
        panAction.Disable();
        arrowDirection = Vector2.zero;
    }

    private void Update()
    {
        var direction = panAction.ReadValue<Vector2>() + arrowDirection;
        if (direction.sqrMagnitude > 1f)
        {
            direction.Normalize();
        }

        // Time.deltaTime is 0 while the pause menu has the game paused, so panning stops too.
        var position = (Vector2)transform.position + direction * (panSpeed * Time.deltaTime);
        var limits = GetLimits();
        position.x = Mathf.Clamp(position.x, limits.xMin, limits.xMax);
        position.y = Mathf.Clamp(position.y, limits.yMin, limits.yMax);
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }

    // Set by the on-screen arrows while one is held down.
    public void SetArrowDirection(Vector2 direction)
    {
        arrowDirection = direction;
    }

    // True when the view can still move further in this direction.
    public bool CanPan(Vector2 direction)
    {
        const float margin = 0.01f;
        var limits = GetLimits();
        var position = transform.position;

        return (direction.x < 0f && position.x > limits.xMin + margin)
            || (direction.x > 0f && position.x < limits.xMax - margin)
            || (direction.y < 0f && position.y > limits.yMin + margin)
            || (direction.y > 0f && position.y < limits.yMax - margin);
    }

    // Where the target can go while the whole view stays inside the room.
    private Rect GetLimits()
    {
        var bounds = roomBounds.bounds;
        var halfHeight = roomCamera.Lens.OrthographicSize;
        var halfWidth = halfHeight * OutputAspect();

        var min = new Vector2(bounds.min.x + halfWidth, bounds.min.y + halfHeight);
        var max = new Vector2(bounds.max.x - halfWidth, bounds.max.y - halfHeight);

        // If the view is bigger than the room on an axis, stay centered on that axis.
        if (min.x > max.x)
        {
            min.x = max.x = bounds.center.x;
        }

        if (min.y > max.y)
        {
            min.y = max.y = bounds.center.y;
        }

        return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
    }

    private static float OutputAspect()
    {
        var camera = Camera.main;
        return camera != null ? camera.aspect : 16f / 9f;
    }

    private static void EnsureCinemachineBrain()
    {
        // Cinemachine cameras only move the scene's camera through a CinemachineBrain on it.
        var camera = Camera.main;
        if (camera != null && camera.GetComponent<CinemachineBrain>() == null)
        {
            camera.gameObject.AddComponent<CinemachineBrain>();
        }
    }

    private static InputAction CreateDefaultPanAction()
    {
        var action = new InputAction("Pan Camera", InputActionType.Value, expectedControlType: "Vector2");
        action.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        action.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");
        return action;
    }
}
