using UnityEngine;

public class ParkingHandler : MonoBehaviour
{

    public bool inParking;

    public float parkingTolerance = 0.5f;

    private BoxCollider col;

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    void CalculateFullParked(Collider parking)
    {
        Bounds parkingBounds = parking.bounds;
        Bounds carBounds = col.bounds;

        float distanceX = Mathf.Abs(parkingBounds.center.x - carBounds.center.x);
        float distanceZ = Mathf.Abs(parkingBounds.center.z - carBounds.center.z);

        Vector2 diff = new Vector2(distanceX, distanceZ);

        inParking = diff.magnitude <= parkingTolerance;

        if (inParking)
        {
            TradeEvents.TriggerOnParking();
            Destroy(this);
        }

        Debug.Log(inParking);
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CalculateFullParked(other);

    }
}
