using UnityEngine;

public class TetrisCell : MonoBehaviour
{
    private SpriteRenderer sr;
    private int assetsInside = 0; // ← contador

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Asset")) return;
        assetsInside++;
        sr.enabled = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Asset")) return;
        assetsInside--;
        if (assetsInside <= 0)
        {
            assetsInside = 0; // evita negativos
            sr.enabled = false;
        }
    }
}