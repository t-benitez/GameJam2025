using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public Transform dashTargetOrbital;
    public int dashDamage = 2;
    public float dashKnockbackForce = 5f;
    public float dashKnockbackDuration = 0.2f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private Vector2 dashDirection;

    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;

    [Header("Events")]

    //public UnityEvent<Vector2> onPositionChanged;

    private Vector2 lastPosition;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer SpriteRenderer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lastPosition = rb.position;
    }
    private void MoveToRight()
    {
        moveInput.x += 1f;
        SpriteRenderer.flipX = false;
    }
    private void MoveToLeft(){
        moveInput.x -= 1f;
        SpriteRenderer.flipX = true;
    }
    private void Update()
    {
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        if (!isDashing && !isKnockedBack)
        {
            moveInput = Vector2.zero;
            if (Keyboard.current.wKey.isPressed) moveInput.y += 1f;
            if (Keyboard.current.sKey.isPressed) moveInput.y -= 1f;
            if (Keyboard.current.dKey.isPressed) MoveToRight();
            if (Keyboard.current.aKey.isPressed) MoveToLeft();

            moveInput = moveInput.normalized;
        }

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame &&
            dashTargetOrbital != null &&
            !isDashing &&
            dashCooldownTimer <= 0f &&
            !isKnockedBack)
        {
            Dash();
        }
    }

    private void Dash()
    {
        dashDirection = (dashTargetOrbital.position - transform.position).normalized;
        SetDashPosition(dashDirection.x);
        isDashing = true;
        animator.SetBool("Dash", true);
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySound(AudioManager.Instance.jugadorDash);
    }

    private void SetDashPosition(float x)
    {

        if (x < 0)
            SpriteRenderer.flipX = true;
        else
            SpriteRenderer.flipX = false;
    
    
        
    }
    private void StopDashing()
    {
        isDashing = false;
        animator.SetBool("Dash", false);
    }
    private void FixedUpdate()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            if (knockbackTimer <= 0f)
                isKnockedBack = false;
            CheckPositionChange();
            return;
        }

        if (isDashing)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            dashTimer -= Time.fixedDeltaTime;

            if (dashTimer <= 0f)
            {
                StopDashing();
            }
        }
        else
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }
        CheckPositionChange();
    }

    private void CheckPositionChange()
    {
        if (rb.position != lastPosition)
        {
            lastPosition = rb.position;
            PlayerPositionNotifier.NotifyPositionChanged(lastPosition);
        }
    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        isKnockedBack = true;
        knockbackTimer = duration;
        rb.linearVelocity = direction * force;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing && collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(dashDamage);
            }

            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                Vector2 knockbackDir = (enemyController.transform.position - transform.position).normalized;
                enemyController.ApplyKnockback(knockbackDir, dashKnockbackForce, dashKnockbackDuration);
            }
        }
    }

    public bool IsInvulnerable()
    {
        return isDashing;
    }
}
