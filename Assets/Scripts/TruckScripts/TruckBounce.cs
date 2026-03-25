using UnityEngine;

public class TruckBounce : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float amplitude = 0.02f;  // que tan alto sube y baja
    public float frequency = 8f;     // velocidad del rebote (RPM del motor)

    private Vector3 startLocalPos;

    void Start()
    {
        startLocalPos = transform.localPosition;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startLocalPos + new Vector3(0f, offsetY, 0f);
    }
}