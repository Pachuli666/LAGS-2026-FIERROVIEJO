using UnityEngine;

public class TetrisGrid : MonoBehaviour
{
    public bool[] active;
    private SpriteRenderer[] cells;

    void Awake()
    {
        cells = GetComponentsInChildren<SpriteRenderer>();
        if (active == null || active.Length != cells.Length)
            active = new bool[cells.Length];
        Apply();
    }

    void Update()
    {
        Apply();
    }

    void Apply()
    {
        for (int i = 0; i < cells.Length; i++)
            if (cells[i]) cells[i].enabled = active[i];
    }
}