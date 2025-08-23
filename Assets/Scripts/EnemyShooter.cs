using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Referencias")]
    public Vector3 playerPosition;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Stats de ataque")]
    public float fireCooldown = 5f;
    private float fireTimer = 0f;



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

    private void Update()
    {
        if (playerPosition == null) return;

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireCooldown;
        }
    }


    private void Shoot()
    {
        if (bulletPrefab != null && firePoint != null && playerPosition != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Vector2 dir = (playerPosition - firePoint.position).normalized;

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = dir * 8f; // velocidad de bala
            }
        }
    }
}
