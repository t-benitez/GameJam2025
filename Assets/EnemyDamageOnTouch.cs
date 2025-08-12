using UnityEngine;

public class EnemyDamageOnTouch : MonoBehaviour
{
    public int damage = 1;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        if (playerHealth != null && playerController != null)
        {
            if (playerController.IsInvulnerable())
                return;
            playerHealth.TakeDamage(damage);

            Vector2 contactPoint = collision.GetContact(0).point;
            Vector2 knockbackDirection = ((Vector2)playerController.transform.position - contactPoint).normalized;
            playerController.ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
        }
    }
}
