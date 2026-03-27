using UnityEngine;
using UnityEngine.InputSystem;

public class TruckMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 10f;
    public float sideSpeed = 8f;

    [Header("Road Reference")]
    public Transform roadPlane;
    public float sideMargin = 0.5f;

    private float leftLimit;
    private float rightLimit;
    private Rigidbody rb;
    private bool wasUIOpen = false;

    void Start()
    {
        CalculateLimits();
        rb = GetComponent<Rigidbody>();
    }

    bool IsUIOpen() => UISwitch.Instance != null &&
                   (UISwitch.Instance.inventoryUI.activeSelf ||
                    UISwitch.Instance.tradeUI.activeSelf ||
                    UISwitch.Instance.saleUI.activeSelf);

    void Update()
    {
        bool uiOpen = IsUIOpen();

        if (rb != null && uiOpen != wasUIOpen)
        {
            rb.isKinematic = uiOpen;
            wasUIOpen = uiOpen;
        }

        if (uiOpen || Keyboard.current == null) return;

        float vertical = 0f;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) vertical = 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) vertical = -1f;

        float horizontal = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal = 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal = -1f;

        Vector3 pos = transform.position;
        pos.z -= vertical * speed * Time.deltaTime;
        pos.x += horizontal * sideSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);

        transform.position = pos;
    }

    void CalculateLimits()
    {
        if (roadPlane == null) { leftLimit = -3f; rightLimit = 3f; return; }

        MeshFilter mf = roadPlane.GetComponent<MeshFilter>();
        if (mf != null && mf.sharedMesh != null)
        {
            Bounds b = mf.sharedMesh.bounds;
            Vector3 worldMin = roadPlane.TransformPoint(b.min);
            Vector3 worldMax = roadPlane.TransformPoint(b.max);
            leftLimit = Mathf.Min(worldMin.x, worldMax.x) + sideMargin;
            rightLimit = Mathf.Max(worldMin.x, worldMax.x) - sideMargin;
        }
        else
        {
            float w = roadPlane.localScale.x * 10f;
            leftLimit = roadPlane.position.x - w / 2f + sideMargin;
            rightLimit = roadPlane.position.x + w / 2f - sideMargin;
        }
    }
}