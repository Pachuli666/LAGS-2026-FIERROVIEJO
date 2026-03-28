using UnityEngine;
using System.Collections.Generic;

public class TetrisCell : MonoBehaviour
{
    private SpriteRenderer sr;
    private HashSet<Collider> assetsInside = new HashSet<Collider>();

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Asset")) return;
        assetsInside.Add(other);
        UpdateVisual();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Asset")) return;
        assetsInside.Remove(other);
        UpdateVisual();
    }

    // Llámalo periódicamente o desde el GameManager
    public void CleanDestroyedAssets()
    {
        assetsInside.RemoveWhere(c => c == null);
        UpdateVisual();
    }

    void UpdateVisual()
    {
        sr.enabled = assetsInside.Count > 0;
    }
}