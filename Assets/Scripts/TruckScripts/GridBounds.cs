using UnityEngine;

public class GridBoundsProvider : MonoBehaviour
{
    public static GridBoundsProvider Instance { get; private set; }

    [SerializeField] private string assetTag = "Asset";

    private BoxCollider _col;

    void Awake()
    {
        Instance = this;
        _col = GetComponent<BoxCollider>();
    }

    public Bounds GetBounds() => _col.bounds;

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(assetTag)) return;

        other.transform.SetParent(null);
    }
}