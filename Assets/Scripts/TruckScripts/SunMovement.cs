using UnityEngine;

public class SunMovement : MonoBehaviour
{
    public float speed = 1f;

    void Update()
    {
        transform.Rotate(Vector3.right * speed * Time.deltaTime);
    }
}