using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
    [Header("Configuración de Gravedad")]
    public bool useCustomGravity = true;
    public Vector3 customGravity = new Vector3(0f, -9.81f, 0f);
    
    [Tooltip("Multiplicador de caída (Solo funciona en el aire)")]
    [Range(1f, 10f)] 
    public float gravityMultiplier = 4f;

    private Rigidbody rb;
    private bool isTouchingPlatform = false; // El interruptor

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Importante: Desactiva la gravedad base de Unity
    }

    void FixedUpdate()
    {
        Vector3 baseGravity = useCustomGravity ? customGravity : Physics.gravity;

        if (!isTouchingPlatform)
        {
            // EN EL AIRE: Aplicamos el multiplicador para que caiga rápido
            rb.AddForce(baseGravity * gravityMultiplier, ForceMode.Acceleration);
        }
        else
        {
            // EN LA PLATAFORMA: Gravedad normal (1x) para que no se hunda y pueda patinar
            rb.AddForce(baseGravity, ForceMode.Acceleration);
        }
    }

    // Se activa al tocar la plataforma
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            isTouchingPlatform = true;
        }
    }

    // Se activa al salir de la plataforma (o caerse)
    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            isTouchingPlatform = false;
        }
    }
}