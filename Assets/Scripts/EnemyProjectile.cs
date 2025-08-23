using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage = 1f;
    public float lifetime = 5f;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    private void Awake()
    {
        Destroy(gameObject, lifetime);
        StartCoroutine(projectile());

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage((int)damage);
            animator.SetTrigger("Hit");
            Destroy(gameObject, 0.2f);
        }
    }

    private System.Collections.IEnumerator projectile()
    {
        yield return new WaitForSeconds(lifetime-0.2f);
        animator.SetTrigger("Hit");
    }
    
}
