using UnityEngine;
using UnityEngine.InputSystem;

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
    
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer= 0f;
    private Vector2 dashDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        if (!isDashing)
        {
            moveInput = Vector2.zero;
            if (Keyboard.current.wKey.isPressed) moveInput.y += 1f;
            if (Keyboard.current.sKey.isPressed) moveInput.y -= 1f;
            if (Keyboard.current.dKey.isPressed) moveInput.x += 1f;
            if (Keyboard.current.aKey.isPressed) moveInput.x -= 1f;

            moveInput = moveInput.normalized;
        }

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame && 
            dashTargetOrbital != null && 
            !isDashing && 
            dashCooldownTimer <= 0f)
        {
            dashDirection = (dashTargetOrbital.position - transform.position).normalized;
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
        }
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            dashTimer -= Time.fixedDeltaTime;

            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }
        else
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }
    
    }
}
