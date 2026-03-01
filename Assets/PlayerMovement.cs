using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float runSpeed = 10f; // Nova variável para a velocidade de corrida [cite: 2026-01-27]
    public float jumpForce = 5.5f;
    public float groundRayDistance = 1.35f;

    private Rigidbody rb;
    private Animator anim;
    private bool isGrounded;

    private Coroutine jumpRoutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Detecta chão
        Vector3 origin = (rb != null) ? rb.worldCenterOfMass : transform.position;
        isGrounded = Physics.Raycast(origin, Vector3.down, groundRayDistance);

        // Detecta tecla de pulo
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (jumpRoutine != null)
                StopCoroutine(jumpRoutine);

            jumpRoutine = StartCoroutine(JumpAfterDelay());
        }

        // Lógica de animação integrada com a corrida [cite: 2026-01-17, 2026-02-27]
        if (anim != null)
        {
            float hInput = Input.GetAxisRaw("Horizontal");
            bool estaCorrendo = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            
            float valorAnimacao = 0f;

            if (hInput != 0)
            {
                // Se apertar CTRL vira 2.0 (Greater 1.5), senão vira 1.0 (Less 1.5) [cite: 2026-01-27]
                valorAnimacao = estaCorrendo ? 2f : 1f;
            }

            anim.SetFloat("Speed", valorAnimacao);
            anim.SetBool("IsGrounded", isGrounded);
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        float h = Input.GetAxisRaw("Horizontal");
        
        // Verifica se o jogador quer correr [cite: 2026-02-27]
        bool estaCorrendo = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        float velocidadeAtual = estaCorrendo ? runSpeed : speed;

        // Aplica movimento físico [cite: 2026-01-27]
        Vector3 v = rb.linearVelocity;
        v.x = h * velocidadeAtual;
        rb.linearVelocity = v;

        // Rotação do personagem
        if (h > 0) transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (h < 0) transform.rotation = Quaternion.Euler(0, 270, 0);
    }

    private IEnumerator JumpAfterDelay()
    {
        if (anim != null)
            anim.SetTrigger("Jump");

        yield return new WaitForSeconds(0.7f);

        Vector3 origin = (rb != null) ? rb.worldCenterOfMass : transform.position;
        bool groundedNow = Physics.Raycast(origin, Vector3.down, groundRayDistance);

        if (!groundedNow)
            yield break;

        Vector3 v = rb.linearVelocity;
        v.y = 0f;
        rb.linearVelocity = v;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundRayDistance);
    }
}