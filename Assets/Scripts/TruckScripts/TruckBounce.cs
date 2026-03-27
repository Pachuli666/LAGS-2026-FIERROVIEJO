using UnityEngine;

public class TruckBounce : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float amplitude = 0.02f;
    public float frequency = 8f;

    private Vector3 startLocalPos;

    void Start()
    {
        startLocalPos = transform.localPosition;
    }

    void Update()
    {
        // Pausar bounce cuando UIs estén activas
        if (UISwitch.Instance != null &&
           (UISwitch.Instance.saleUI.activeSelf || UISwitch.Instance.inventoryUI.activeSelf))
        {
            transform.localPosition = startLocalPos; // regresa a posición base
            return;
        }

        float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startLocalPos + new Vector3(0f, offsetY, 0f);
    }
}