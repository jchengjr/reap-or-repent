using UnityEngine;

public class AmbientDrift : MonoBehaviour
{
    [SerializeField]
    private Vector2 distance = new Vector2(0.5f, 0f);

    [SerializeField]
    private float period = 16f;

    private Vector3 restPosition;

    private void Awake()
    {
        restPosition = transform.localPosition;
    }

    private void Update()
    {
        var sway = Mathf.Sin(Time.time * 2f * Mathf.PI / period);
        transform.localPosition = restPosition + (Vector3)(distance * sway);
    }
}
