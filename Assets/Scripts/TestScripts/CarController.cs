using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CarController : MonoBehaviour
{
    public float speed;

    public float maxSpeed;

    public float smooth;

    public bool locked;

    public float weight;

    public Vector2 zDir;

    public bool inParking;

    public float parkingTolerance = 0.5f;

    [SerializeField]
    [HideInInspector]
    private Rigidbody rb;

    private InputAction moveAction;

    private BoxCollider col;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        zDir = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (!locked && !inParking) {
            rb.linearVelocity = (new Vector3(1,0,zDir.x) * speed) * Time.fixedDeltaTime;
        }
    }

    void CalculateFullParked(Collider parking) {
        Bounds parkingBounds = parking.bounds;
        Bounds carBounds = col.bounds;

        float distanceX = Mathf.Abs(parkingBounds.center.x - carBounds.center.x);
        float distanceZ = Mathf.Abs(parkingBounds.center.z - carBounds.center.z);

        Vector2 diff = new Vector2(distanceX, distanceZ);

        inParking = diff.magnitude <= parkingTolerance;

        Debug.Log(inParking);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Parking") {
            CalculateFullParked(other);
        }
    }
}
