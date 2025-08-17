using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage = 1f;
    public float lifetime = 5f;

    private void Awake()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage((int)damage);
            Destroy(gameObject);
        }
    }
}
