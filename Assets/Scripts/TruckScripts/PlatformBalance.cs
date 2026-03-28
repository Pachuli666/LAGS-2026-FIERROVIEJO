using UnityEngine;

public class PlatformBalance : MonoBehaviour
{
    [Range(0f, 1f)]
    public float returnStrength = 0.2f;

    [Range(0f, 1f)]
    public float damping = 0.3f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Elevar al cubo mapea 0→0, 0.2→0.008, 0.5→0.125, 1→1
        // Así tienes precisión en valores pequeños Y llegas hasta 1
        float realStrength = returnStrength * returnStrength * returnStrength;
        float realDamping = damping * damping * damping;

        Quaternion deltaRotation = Quaternion.FromToRotation(transform.up, Vector3.up);
        deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);

        if (angle > 0.1f)
            rb.AddTorque(axis * angle * realStrength, ForceMode.Acceleration);

        rb.AddTorque(-rb.angularVelocity * realDamping, ForceMode.Acceleration);
    }
}