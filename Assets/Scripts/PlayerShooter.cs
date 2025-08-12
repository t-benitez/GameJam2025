using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [Header("Disparo")]
    public Transform shootingOrbital; 
    public GameObject projectilePrefab;
    public float fireRate = 0.2f;          

    private float fireCooldown = 0f;

    private void Update()
    {
        if (shootingOrbital == null || projectilePrefab == null) return;

        fireCooldown -= Time.deltaTime;

        if (Keyboard.current.spaceKey.isPressed && fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = fireRate;
            AudioManager.Instance.PlaySound(AudioManager.Instance.jugadorTiro);
        }
    }

    private void Shoot()
    {
        Vector2 direction = (shootingOrbital.position - transform.position).normalized;

        GameObject bullet = Instantiate(projectilePrefab, shootingOrbital.position, Quaternion.identity);

        Projectile proj = bullet.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.SetDirection(direction);
        }
    }
}
