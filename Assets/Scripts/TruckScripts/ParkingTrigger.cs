using UnityEngine;

public class ParkingTrigger : MonoBehaviour
{
    private HouseSpawner spawner;

    public void Init(HouseSpawner s) { spawner = s; }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        spawner.OnTruckEntered();

        // Activar UI de venta al entrar al parking
        UISwitch.Instance.ShowSaleUI();

        Destroy(this);
    }
}