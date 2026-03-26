using UnityEngine;

public class IconMove : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float amplitude = 0.02f;  // qué tan alto sube y baja
    public float frequency = 8f;     // velocidad del rebote

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
