using UnityEngine;

public class FreezeOnLand : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "PLATFORM" || col.gameObject.CompareTag("Platform"))
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
    }
}
