using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [Header("Disparo")]
    public Transform shootingOrbital;     // Orbital desde donde sale el disparo
    public GameObject projectilePrefab;   // Prefab del proyectil
    public float fireRate = 0.2f;          // Tiempo entre disparos (segundos)

    private float fireCooldown = 0f;

    private void Update()
    {
        if (shootingOrbital == null || projectilePrefab == null) return;

        fireCooldown -= Time.deltaTime;

        // Si se mantiene espacio, disparar
        if (Keyboard.current.spaceKey.isPressed && fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = fireRate;
        }
    }

    private void Shoot()
    {
        // Dirección contraria al jugador → "hacia afuera"
        Vector2 direction = (shootingOrbital.position - transform.position).normalized;

        // Instanciar proyectil
        GameObject bullet = Instantiate(projectilePrefab, shootingOrbital.position, Quaternion.identity);

        // Asignar dirección
        Projectile proj = bullet.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.SetDirection(direction);
        }
    }
}
