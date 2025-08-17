using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyDasher : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    private EnemyDamageOnTouch damageOnTouch;

    [Header("Stats de dash")]
    public float stopDistance = 5f;    
    public float chargeTime = 2f;      
    public float dashSpeed = 12f;      
    public float dashDistance = 8f;    
    public float cooldownTime = 3f;    

    [Header("Efecto de carga")]
    public float chargeShakeAmount = 0.1f; 
    public float chargeShakeSpeed = 20f;   

    private Rigidbody2D rb;
    private Vector2 dashDirection;
    private Vector2 lastSeenPlayerPos; // <- última posición vista del jugador

    private bool isCharging = false;
    private bool isDashing = false;
    private bool isOnCooldown = false;

    private float chargeTimer = 0f;
    private float dashTraveled = 0f;
    private float cooldownTimer = 0f;

    private Vector2 originalPosition; // para temblor

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        damageOnTouch = GetComponent<EnemyDamageOnTouch>();
        if (damageOnTouch != null)
            damageOnTouch.enabled = false; 
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (!isCharging && !isDashing && !isOnCooldown)
        {
            // si está en la distancia de carga -> empieza a cargar
            if (distance <= stopDistance)
            {
                isCharging = true;
                chargeTimer = chargeTime;
                rb.linearVelocity = Vector2.zero;

                // guardamos la última posición del jugador
                lastSeenPlayerPos = player.position;

                // guardamos la posición base para el temblor
                originalPosition = rb.position;
            }
        }

        if (isCharging)
        {
            chargeTimer -= Time.deltaTime;

            // aplicar temblor mientras carga
            float offset = Mathf.Sin(Time.time * chargeShakeSpeed) * chargeShakeAmount;
            Vector2 perpendicular = new Vector2(-1, 1).normalized; 
            rb.MovePosition(originalPosition + perpendicular * offset);

            if (chargeTimer <= 0f)
            {
                // calcula la dirección hacia la última posición vista
                dashDirection = (lastSeenPlayerPos - rb.position).normalized;

                // empieza el dash
                isCharging = false;
                isDashing = true;
                dashTraveled = 0f;

                if (damageOnTouch != null)
                    damageOnTouch.enabled = true; 
            }
        }

        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            Vector2 movement = dashDirection * dashSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
            dashTraveled += movement.magnitude;

            if (dashTraveled >= dashDistance)
            {
                EndDash();
            }
        }
        else if (!isCharging && !isOnCooldown && player != null)
        {
            // persecución normal
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * 3f; 
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void EndDash()
    {
        isDashing = false;
        isOnCooldown = true;
        cooldownTimer = cooldownTime;
        rb.linearVelocity = Vector2.zero;

        if (damageOnTouch != null)
            damageOnTouch.enabled = false; 
    }
}
