using UnityEngine;

public class GridBoundsProvider : MonoBehaviour
{
    public static GridBoundsProvider Instance { get; private set; }
    private BoxCollider _col;
    private Transform truckParent;

    void Awake()
    {
        Instance = this;
        _col = GetComponent<BoxCollider>();
        _col.isTrigger = true; // ← activa Is Trigger por código
        truckParent = GameObject.FindWithTag("Player")?.transform;
    }

    public Bounds GetBounds() => _col.bounds;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Asset")) return;
        other.transform.SetParent(truckParent);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Asset")) return;
        other.transform.SetParent(null);
    }
}