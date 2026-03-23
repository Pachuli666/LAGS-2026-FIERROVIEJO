using UnityEngine;
using UnityEngine.InputSystem;

public class TruckController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 10f;        // Velocidad hacia adelante y atras
    public float sideSpeed = 8f;     // Velocidad de movimiento lateral

    [Header("Road Reference")]
    public Transform roadPlane;      // Referencia al plano de la carretera
    public float sideMargin = 0.5f;  // Margen de seguridad para no tocar el borde

    private float leftLimit;         // Limite izquierdo del plano
    private float rightLimit;        // Limite derecho del plano

    void Start()
    {
        // Al iniciar el juego, calculamos los limites del plano
        CalculateLimits();
    }

    void CalculateLimits()
    {
        // Verificamos que el plano este asignado en el Inspector
        if (roadPlane != null)
        {
            // Intentamos obtener el Renderer del plano para usar sus bounds reales
            Renderer r = roadPlane.GetComponent<Renderer>();

            if (r != null)
            {
                // Usamos los bounds del Renderer para obtener los bordes exactos del plano
                // y sumamos/restamos el margen de seguridad
                leftLimit = r.bounds.min.z + sideMargin;
                rightLimit = r.bounds.max.z - sideMargin;
            }
            else
            {
                // Si no hay Renderer, calculamos el ancho usando la escala del transform
                // Un plano por defecto de Unity mide 10 unidades, por eso multiplicamos por 10
                float w = roadPlane.localScale.z * 10f;
                leftLimit = roadPlane.position.z - w / 2f + sideMargin;
                rightLimit = roadPlane.position.z + w / 2f - sideMargin;
            }
        }
        else
        {
            // Si no hay plano asignado, usamos valores por defecto
            leftLimit = -3f;
            rightLimit = 3f;
        }
    }

    void Update()
    {
        // Inicializamos los ejes en 0 cada frame
        float vertical = 0f;
        float horizontal = 0f;

        // Verificamos que haya un teclado conectado
        if (Keyboard.current != null)
        {
            // A o flecha arriba: mover hacia adelante (vertical positivo)
            if (Keyboard.current.aKey.isPressed || Keyboard.current.upArrowKey.isPressed) vertical = 1f;

            // D o flecha abajo: mover hacia atras (vertical negativo)
            if (Keyboard.current.dKey.isPressed || Keyboard.current.downArrowKey.isPressed) vertical = -1f;

            // S o flecha izquierda: mover a la izquierda (horizontal negativo)
            if (Keyboard.current.sKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal = -1f;

            // W o flecha derecha: mover a la derecha (horizontal positivo)
            if (Keyboard.current.wKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal = 1f;
        }

        // Obtenemos la posicion actual del camion
        Vector3 pos = transform.position;

        // Movemos el camion en el eje X segun el eje vertical (adelante/atras)
        pos.x += vertical * speed * Time.deltaTime;

        // Movemos el camion en el eje Z segun el eje horizontal (izquierda/derecha)
        pos.z -= horizontal * sideSpeed * Time.deltaTime;

        // Limitamos la posicion Z para que no se salga de los bordes del plano
        pos.z = Mathf.Clamp(pos.z, leftLimit, rightLimit);

        // Aplicamos la nueva posicion al camion
        transform.position = pos;
    }
}