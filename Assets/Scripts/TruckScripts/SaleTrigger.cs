using UnityEngine;

public class SaleTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Activar UI de venta al entrar al parking
        UISwitch.Instance.ShowSaleUI();

        Destroy(this);
    }
}