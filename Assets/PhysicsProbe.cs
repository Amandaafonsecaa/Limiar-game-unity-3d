using UnityEngine;

public class PhysicsProbe : MonoBehaviour
{
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log("Physics.gravity = " + Physics.gravity);
        Debug.Log("rb.useGravity = " + (rb != null && rb.useGravity));
    }

    void FixedUpdate()
    {
        if (rb == null) return;
        Debug.Log($"Y={transform.position.y:F3}  velY={rb.linearVelocity.y:F3}  gravity={Physics.gravity}");
    }
}