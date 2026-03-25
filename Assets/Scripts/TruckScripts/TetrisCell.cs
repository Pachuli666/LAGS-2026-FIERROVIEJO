using UnityEngine;

public class TetrisCell : MonoBehaviour
{
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Asset")) sr.enabled = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Asset")) sr.enabled = false;
    }
}