using UnityEngine;

public class CargoReaction : MonoBehaviour
{
    [Header("Physics")]
    public float bounceForce = 2f;
    public float frequency = 8f;
    public float lateralForce = 3f;
    public float dragForce = 3f;

    [Header("Cajuela Limits (espacio local del truck)")]
    public Vector3 minLocalPos = new Vector3(-0.3f, 0f, -0.3f);
    public Vector3 maxLocalPos = new Vector3(0.3f, 0.5f, 0.3f);

    private Rigidbody rb;
    private TruckMovement truck;
    private Vector3 lastTruckPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        truck = GetComponentInParent<TruckMovement>();

        if (truck != null)
        {
            lastTruckPos = truck.transform.position;
            transform.SetParent(null);
        }
    }

    void FixedUpdate()
    {
        if (truck == null) return;

        Vector3 truckPos = truck.transform.position;
        Vector3 delta = truckPos - lastTruckPos;
        lastTruckPos = truckPos;

        // Fuerzas de inercia
        float bounce = Mathf.Sin(Time.time * frequency) * bounceForce;
        rb.AddForce(Vector3.up * bounce, ForceMode.Acceleration);
        rb.AddForce(new Vector3(-delta.x, 0f, -delta.z) * dragForce / Time.fixedDeltaTime, ForceMode.Acceleration);

        // Convertimos la posicion del objeto a espacio local del truck
        Vector3 localPos = truck.transform.InverseTransformPoint(transform.position);

        // Clamp dentro de los limites de la cajuela
        localPos.x = Mathf.Clamp(localPos.x, minLocalPos.x, maxLocalPos.x);
        localPos.y = Mathf.Clamp(localPos.y, minLocalPos.y, maxLocalPos.y);
        localPos.z = Mathf.Clamp(localPos.z, minLocalPos.z, maxLocalPos.z);

        // Volvemos a espacio mundo y teleportamos si se salio
        Vector3 worldClamped = truck.transform.TransformPoint(localPos);
        if (worldClamped != transform.position)
        {
            rb.MovePosition(worldClamped);
            // Cancelamos velocidad en la direccion que choco
            Vector3 localVel = truck.transform.InverseTransformDirection(rb.linearVelocity);
            if (localPos.x == minLocalPos.x || localPos.x == maxLocalPos.x) localVel.x = 0f;
            if (localPos.y == minLocalPos.y || localPos.y == maxLocalPos.y) localVel.y = 0f;
            if (localPos.z == minLocalPos.z || localPos.z == maxLocalPos.z) localVel.z = 0f;
            rb.linearVelocity = truck.transform.TransformDirection(localVel);
        }
    }
}