using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlaceManager : MonoBehaviour
{
    [Header("Objetos a instanciar en orden")]
    public GameObject[] prefabs;

    [Header("Movimiento")]
    public float speed = 10f;
    public float sideSpeed = 8f;

    [Header("Plataforma (calcula límites automáticamente)")]
    public Transform platform;
    public float edgeMargin = 0.5f;

    private float leftLimit;
    private float rightLimit;
    private float frontLimit;
    private float backLimit;

    private int currentIndex = 0;
    private GameObject currentObject;

    void Start()
    {
        CalculateLimits();
        SpawnNext();
    }

    void CalculateLimits()
    {
        if (platform == null) { leftLimit = -3f; rightLimit = 3f; frontLimit = 3f; backLimit = -3f; return; }

        // Usar BoxCollider si existe, si no usar Renderer bounds
        BoxCollider bc = platform.GetComponent<BoxCollider>();
        if (bc != null)
        {
            // Bounds en espacio mundo del BoxCollider
            Vector3 center = platform.TransformPoint(bc.center);
            Vector3 extents = new Vector3(
                bc.size.x * platform.localScale.x * 0.5f,
                bc.size.y * platform.localScale.y * 0.5f,
                bc.size.z * platform.localScale.z * 0.5f
            );

            leftLimit = center.x - extents.x + edgeMargin;
            rightLimit = center.x + extents.x - edgeMargin;
            backLimit = center.z - extents.z + edgeMargin;
            frontLimit = center.z + extents.z - edgeMargin;
        }
        else
        {
            // Fallback: Renderer bounds
            Renderer r = platform.GetComponent<Renderer>();
            if (r != null)
            {
                Bounds b = r.bounds;
                leftLimit = b.min.x + edgeMargin;
                rightLimit = b.max.x - edgeMargin;
                backLimit = b.min.z + edgeMargin;
                frontLimit = b.max.z - edgeMargin;
            }
        }
    }

    void SpawnNext()
    {
        if (currentIndex >= prefabs.Length)
        {
            Debug.Log("¡Todos los objetos colocados!");
            return;
        }

        currentObject = Instantiate(prefabs[currentIndex], transform.position, Quaternion.identity);

        Rigidbody rb = currentObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        CustomGravity cg = currentObject.GetComponent<CustomGravity>();
        if (cg != null) cg.enabled = false;

        Debug.Log($"Objeto {currentIndex + 1}/{prefabs.Length}: {prefabs[currentIndex].name} — ESPACIO para soltar");
    }

    void Update()
    {
        if (currentObject == null) return;

#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current == null) return;

        float vertical = 0f;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) vertical = 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) vertical = -1f;

        float horizontal = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal = 1f;

        bool space = Keyboard.current.spaceKey.wasPressedThisFrame;
#else
        float vertical   = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))    vertical =  1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))  vertical = -1f;

        float horizontal = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow)) horizontal =  1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow))  horizontal = -1f;

        bool space = Input.GetKeyDown(KeyCode.Space);
#endif

        Vector3 pos = currentObject.transform.position;
        pos.z += vertical * speed * Time.deltaTime;
        pos.x += horizontal * sideSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
        pos.z = Mathf.Clamp(pos.z, backLimit, frontLimit);

        currentObject.transform.position = pos;

        if (!space) return;

        Rigidbody rb = currentObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        CustomGravity cg = currentObject.GetComponent<CustomGravity>();
        if (cg != null) cg.enabled = true;

        currentIndex++;
        currentObject = null;
        SpawnNext();
    }
}