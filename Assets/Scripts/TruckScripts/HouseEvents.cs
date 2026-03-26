using UnityEngine;

public class HouseEvents : MonoBehaviour
{
    [Range(0f, 1f)]
    public float availabilityChance = 0.6f;

    [HideInInspector] public HouseEvents nextHouse;

    private SpriteRenderer admiration;
    private SpriteRenderer interrogation;
    private SpriteRenderer parkingZone;

    void Awake()
    {
        admiration = transform.Find("ADMIRATION_PNG_0").GetComponent<SpriteRenderer>();
        interrogation = transform.Find("INTERRROGATION_PNG_0").GetComponent<SpriteRenderer>();
        parkingZone = transform.Find("ParkingZone").GetComponent<SpriteRenderer>();

        SetActive(false);
    }

    public void SetActive(bool on)
    {
        bool showAdmiration = Random.value <= availabilityChance;
        admiration.enabled = on && showAdmiration;
        interrogation.enabled = on && !showAdmiration;
        parkingZone.enabled = on;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (nextHouse != null) nextHouse.SetActive(true);
        SetActive(false);
    }
}