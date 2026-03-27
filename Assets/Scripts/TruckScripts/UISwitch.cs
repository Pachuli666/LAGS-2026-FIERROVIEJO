using UnityEngine;
using UnityEngine.InputSystem;

public class UISwitch : MonoBehaviour
{
    public static UISwitch Instance;

    [Header("UIs")]
    public GameObject driveUI;
    public GameObject saleUI;
    public GameObject inventoryUI; // ← nueva

    [Header("Camera")]
    public Transform mainCamera;

    // Posiciones locales (hijo del camión)
    private readonly Vector3 driveCamPos = new Vector3(0f, 392.14f, 616.89f);
    private readonly Vector3 driveCamRot = new Vector3(16.017f, 180f, 0f);

    private readonly Vector3 inventoryCamPos = new Vector3(0f, 143.27f, 136.4f); // ← reemplaza con tus coords de inventario
    private readonly Vector3 inventoryCamRot = new Vector3(70f, 180f, 0f);          // ← reemplaza con tus coords de inventario

    void Awake() { Instance = this; }

    void Start()
    {
        driveUI.SetActive(true);
        saleUI.SetActive(false);
        inventoryUI.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (saleUI.activeSelf)
                ShowInventoryUI();        // Sale UI → Inventario
            else if (inventoryUI.activeSelf)
                ShowDriveUI();            // Inventario → Drive
        }
    }

    public void ShowSaleUI()
    {
        driveUI.SetActive(false);
        saleUI.SetActive(true);
    }

    void ShowInventoryUI()
    {
        saleUI.SetActive(false);
        inventoryUI.SetActive(true);
        SetCamera(inventoryCamPos, inventoryCamRot);
    }

    public void ShowDriveUI()
    {
        inventoryUI.SetActive(false);
        saleUI.SetActive(false);
        driveUI.SetActive(true);
        SetCamera(driveCamPos, driveCamRot);
    }

    void SetCamera(Vector3 pos, Vector3 rot)
    {
        mainCamera.localPosition = pos;
        mainCamera.localEulerAngles = rot;
    }
}