using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [Header("Movimiento")]
    public Transform player;
    public float moveSpeed = 3f;
    public float stopDistance = 0f; // 0 = siempre intenta tocar al jugador (scout)

    private Rigidbody2D rb;
    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            if (knockbackTimer <= 0f)
                isKnockedBack = false;
            return;
        }

        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (stopDistance <= 0f)
        {
            // 👾 Caso SCOUT → siempre persigue al jugador hasta tocarlo
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
        else
        {
            // 👾 Caso TIRADOR / CAÑONERO / EMBESTIDOR
            if (distance > stopDistance)
            {
                // Está más lejos de la distancia deseada → se acerca
                Vector2 direction = (player.position - transform.position).normalized;
                rb.linearVelocity = direction * moveSpeed;
            }
            else if (distance < stopDistance * 0.8f)
            {
                // Está demasiado cerca → se queda quieto (sin retroceder)
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                // Está en la zona correcta → se queda quieto
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        isKnockedBack = true;
        knockbackTimer = duration;
        rb.linearVelocity = direction * force;
    }
}
