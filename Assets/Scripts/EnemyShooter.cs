using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Stats de ataque")]
    public float fireCooldown = 5f;
    private float fireTimer = 0f;

    private void Update()
    {
        if (player == null) return;

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireCooldown;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab != null && firePoint != null && player != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Vector2 dir = (player.position - firePoint.position).normalized;

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = dir * 8f; // velocidad de bala
            }
        }
    }
}
