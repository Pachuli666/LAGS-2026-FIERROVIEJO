using UnityEngine;
using UnityEngine.InputSystem;

public class UISwitch : MonoBehaviour
{
    public static UISwitch Instance;

    [Header("UIs")]
    public GameObject driveUI;
    public GameObject saleUI;

    void Awake() { Instance = this; }

    void Start()
    {
        driveUI.SetActive(true);
        saleUI.SetActive(false);
    }

    void Update()
    {
        // Nuevo Input System
        if (Keyboard.current.spaceKey.wasPressedThisFrame && saleUI.activeSelf)
        {
            ShowDriveUI();
        }
    }

    public void ShowSaleUI()
    {
        driveUI.SetActive(false);
        saleUI.SetActive(true);
    }

    public void ShowDriveUI()
    {
        saleUI.SetActive(false);
        driveUI.SetActive(true);
    }
}