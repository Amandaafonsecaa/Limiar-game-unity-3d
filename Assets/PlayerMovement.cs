using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    public float speed = 5f;

    [Header("Jump")]
    public float jumpForce = 5.5f;
    public float groundRayDistance = 1.3f;

    private Rigidbody rb;
    private Animator anim;

    private bool isGrounded; // <- AGORA EXISTE

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 1) Detecta chão (raycast pra baixo)
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundRayDistance);

        // 2) Pulo
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // zera o Y pra pulo consistente
            Vector3 v = rb.linearVelocity;
            v.y = 0f;
            rb.linearVelocity = v;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (anim != null) anim.SetTrigger("Jump");
        }

        // 3) Animator (sem YVelocity)
        float hAnim = Mathf.Abs(Input.GetAxisRaw("Horizontal"));
        if (anim != null)
        {
            anim.SetFloat("Speed", hAnim);
            anim.SetBool("IsGrounded", isGrounded);
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");

        // movimento físico (colisão funciona)
        Vector3 v = rb.linearVelocity;
        v.x = h * speed;
        rb.linearVelocity = v;

        // virar
        if (h > 0) transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (h < 0) transform.rotation = Quaternion.Euler(0, 270, 0);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundRayDistance);
    }
}