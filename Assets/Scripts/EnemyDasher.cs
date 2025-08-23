using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyDasher : MonoBehaviour
{
    [Header("Referencias")]
    public Vector3 playerPosition;
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
    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

     //subscribe events
    private void OnEnable()
    {
        PlayerPositionNotifier.OnPlayerPositionChanged += UpdatePlayerPosition;
    }

    private void OnDisable()
    {
        PlayerPositionNotifier.OnPlayerPositionChanged -= UpdatePlayerPosition;
    }

    private void UpdatePlayerPosition(Vector3 newPosition)
    {
        playerPosition = newPosition;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        damageOnTouch = GetComponent<EnemyDamageOnTouch>();
        if (damageOnTouch != null)
            damageOnTouch.enabled = false; 
    }

    private void Update()
    {
        if (playerPosition == null) return;

        float distance = Vector2.Distance(transform.position, playerPosition);

        if (!isCharging && !isDashing && !isOnCooldown)
        {
            // si está en la distancia de carga -> empieza a cargar
            if (distance <= stopDistance)
            {
                Vector2 direction = (playerPosition - transform.position).normalized;
                flipX(direction.x);
                animator.SetBool("Charge", true);

                isCharging = true;
                chargeTimer = chargeTime;
                rb.linearVelocity = Vector2.zero;

                // guardamos la última posición del jugador
                lastSeenPlayerPos = playerPosition;

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

            if(chargeTimer <= 0.3f && chargeTimer>=0.2f )
                lastSeenPlayerPos = playerPosition;
            if (chargeTimer <= 0f)
            {
                dash();
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

    private void dash()
    {
        /*
        Vector2 actualPosition = new Vector2(playerPosition.x, playerPosition.y);
        dashDirection = ( actualPosition - rb.position).normalized;*/
        dashDirection = (lastSeenPlayerPos - rb.position).normalized;
        animator.SetBool("Charge", false);
        animator.SetBool("Dash", true);

        isCharging = false;
        isDashing = true;
        dashTraveled = 0f;

        if (damageOnTouch != null)
            damageOnTouch.enabled = true; 
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
        else if (!isCharging && !isOnCooldown && playerPosition != null)
        {
            Vector2 direction = (playerPosition - transform.position).normalized;
            flipX(direction.x);
            rb.linearVelocity = direction * 3f;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    
    private void flipX(float x)
    {
        if (x < 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
    }

    private void EndDash()
    {
        isDashing = false;
        isOnCooldown = true;
        cooldownTimer = cooldownTime;
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("Dash", false);

        if (damageOnTouch != null)
            damageOnTouch.enabled = false;
    }
}
