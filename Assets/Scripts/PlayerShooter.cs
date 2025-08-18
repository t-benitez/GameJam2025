using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    public enum AttackMode { Normal, Boomerang, Spin }

    private OrbitalController orbitalController;

    [Header("Disparo")]
    public Transform shootingOrbital; 
    public GameObject projectilePrefab;
    public float fireRate = 0.2f;

    [Header("Boomerang")]
    public float boomerangDistance = 8f;
    public float boomerangSpeed = 10f;
    public float boomerangReturnDelay = 0.2f;

    [Header("Spin")]
    public float spinSpeed = 720f; // degrees per second
    public float spinDuration = 1f;

    private float fireCooldown = 0f;
    private AttackMode currentAttack = AttackMode.Normal;
    private bool isBoomerangActive = false;
    private Vector3 orbitalStartLocalPos;
    private bool isSpinning = false;
    private float spinTimer = 0f;

    private void Awake()
    {
        if (shootingOrbital != null)
            orbitalController = shootingOrbital.GetComponent<OrbitalController>();
    }

    private void Update()
    {
        // Switch attack modes
        if (Keyboard.current.digit1Key.wasPressedThisFrame) currentAttack = AttackMode.Normal;
        if (Keyboard.current.digit2Key.wasPressedThisFrame) currentAttack = AttackMode.Boomerang;
        if (Keyboard.current.digit3Key.wasPressedThisFrame) currentAttack = AttackMode.Spin;

        fireCooldown -= Time.deltaTime;

        switch (currentAttack)
        {
            case AttackMode.Normal:
                if (shootingOrbital == null || projectilePrefab == null) return;
                if (Keyboard.current.spaceKey.isPressed && fireCooldown <= 0f)
                {
                    Shoot();
                }
                // Ensure orbital is in normal state
                if (orbitalController != null)
                {
                    orbitalController.SetAttackActive(false);
                    orbitalController.SetDamageActive(false);
                }
                break;

            case AttackMode.Boomerang:
                if (!isBoomerangActive && Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    if (orbitalController != null)
                    {
                        orbitalController.SetAttackActive(true);
                        orbitalController.SetDamageActive(true);
                    }
                    StartCoroutine(BoomerangRoutine());
                }
                break;

            case AttackMode.Spin:
                if (!isSpinning && Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    isSpinning = true;
                    spinTimer = spinDuration;
                    if (orbitalController != null)
                    {
                        orbitalController.SetAttackActive(true);
                        orbitalController.SetDamageActive(true);
                    }
                }
                if (isSpinning)
                {
                    SpinAttack();
                    spinTimer -= Time.deltaTime;
                    if (spinTimer <= 0f)
                    {
                        isSpinning = false;
                        if (orbitalController != null)
                        {
                            orbitalController.SetAttackActive(false);
                            orbitalController.SetDamageActive(false);
                        }
                    }
                }
                break;
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
        fireCooldown = fireRate;
        if(AudioManager.Instance != null)
            AudioManager.Instance.PlaySound(AudioManager.Instance.jugadorTiro);

    }

    private System.Collections.IEnumerator BoomerangRoutine()
    {
        isBoomerangActive = true;
        orbitalStartLocalPos = shootingOrbital.localPosition;
        Vector3 startWorld = shootingOrbital.position;
        Vector3 dir = (shootingOrbital.position - transform.position).normalized;
        Vector3 targetWorld = transform.position + dir * boomerangDistance;

        // Move out
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * boomerangSpeed;
            shootingOrbital.position = Vector3.Lerp(startWorld, targetWorld, t);
            yield return null;
        }

        yield return new WaitForSeconds(boomerangReturnDelay);

        // Move back
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * boomerangSpeed;
            shootingOrbital.position = Vector3.Lerp(targetWorld, startWorld, t);
            yield return null;
        }

        shootingOrbital.localPosition = orbitalStartLocalPos;
        isBoomerangActive = false;

        // End attack state
        if (orbitalController != null)
        {
            orbitalController.SetAttackActive(false);
            orbitalController.SetDamageActive(false);
        }
    }

    private void SpinAttack()
    {
        // Spin the orbital rapidly around the player
        shootingOrbital.RotateAround(transform.position, Vector3.forward, spinSpeed * Time.deltaTime);
        // Collider is enabled during spin for damage (handled by OrbitalController)
    }
}