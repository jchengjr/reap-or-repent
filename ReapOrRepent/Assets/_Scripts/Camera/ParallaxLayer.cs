using Unity.Cinemachine;
using UnityEngine;

// Shifts a layer of room objects slightly against the camera as it pans, so the layer reads as
// closer than the background (the two-layer parallax). Place the layer's root at the room's center,
// the point it rests at when the camera is centered on the room.
public class ParallaxLayer : MonoBehaviour
{
    // 0 moves with the background; higher values shift the layer more as the camera pans.
    [SerializeField, Range(0f, 0.5f)]
    private float strength = 0.15f;

    private Vector3 restPosition;

    private void Awake()
    {
        restPosition = transform.position;
    }

    private void OnEnable()
    {
        CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdated);
    }

    private void OnDisable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
    }

    // Runs after Cinemachine moves the camera each frame, so the layer never lags a frame behind.
    private void OnCameraUpdated(CinemachineBrain brain)
    {
        var cameraOffset = (Vector2)brain.OutputCamera.transform.position - (Vector2)restPosition;
        transform.position = restPosition - (Vector3)(cameraOffset * strength);
    }
}
