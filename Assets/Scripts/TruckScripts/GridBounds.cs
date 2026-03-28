using UnityEngine;

public class GridBoundsProvider : MonoBehaviour
{
    public static GridBoundsProvider Instance { get; private set; }
    private BoxCollider _col;

    void Awake()
    {
        Instance = this;
        _col = GetComponent<BoxCollider>();
    }

    public Bounds GetBounds() => _col.bounds;
}