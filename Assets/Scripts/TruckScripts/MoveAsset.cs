using UnityEngine;
using UnityEngine.InputSystem;

public class MoveAsset : MonoBehaviour
{
    public float speed = 5f;
    private static int selectedIndex = 0;
    private static MoveAsset[] all;
    private Rigidbody rb;
    private bool wasInInventoryMode = false;

    void Start()
    {
        all = FindObjectsByType<MoveAsset>(FindObjectsInactive.Exclude);
        rb = GetComponent<Rigidbody>();
    }

    bool IsSelected() => all != null && all.Length > 0 && all[selectedIndex] == this;
    bool InInventoryMode() => UISwitch.Instance != null && UISwitch.Instance.inventoryUI.activeSelf;

    void Update()
    {
        bool inInventory = InInventoryMode();

        if (rb != null && inInventory != wasInInventoryMode)
        {
            rb.isKinematic = inInventory;
            wasInInventoryMode = inInventory;
        }

        if (!inInventory) return;
        if (Keyboard.current == null) return;

        if (all != null && all[0] == this)
            if (Keyboard.current.oKey.wasPressedThisFrame)
                selectedIndex = (selectedIndex + 1) % all.Length;

        if (!IsSelected()) return;

        float x = 0f, z = 0f;
        if (Keyboard.current.wKey.isPressed) z = -1f;
        if (Keyboard.current.sKey.isPressed) z = 1f;
        if (Keyboard.current.aKey.isPressed) x = 1f;
        if (Keyboard.current.dKey.isPressed) x = -1f;

        transform.position += new Vector3(x, 0f, z) * speed * Time.deltaTime;
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying || !IsSelected()) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, transform.localScale * 1.2f);
    }
}