using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    private OrbitalController orbitalController;

    [Header("Boomerang")]
    public Transform shootingOrbital; 
    public float boomerangDistance = 8f;
    public float boomerangSpeed = 10f;
    public float boomerangReturnDelay = 0.2f;

    private bool isBoomerangActive = false;

    private void Awake()
    {
        if (shootingOrbital != null)
            orbitalController = shootingOrbital.GetComponent<OrbitalController>();
    }

    private void Update()
    {
        // Disparo del boomerang al presionar espacio
        if (!isBoomerangActive && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (orbitalController != null)
            {
                orbitalController.SetAttackActive(true);
                orbitalController.SetDamageActive(true);
            }
            StartCoroutine(BoomerangRoutine());
        }
    }

    private System.Collections.IEnumerator BoomerangRoutine()
    {
        isBoomerangActive = true;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySound(AudioManager.Instance.jugadorTiro);
    
        // Guardar la posición local inicial relativa al jugador
        Vector3 initialLocalPosition = shootingOrbital.localPosition;
    
        // Calcular dirección y destino
        Vector3 localDirection = shootingOrbital.localPosition.normalized;
        Vector3 targetLocalPosition = localDirection * boomerangDistance;

        // Iniciar rotación
        Coroutine rotationCoroutine = StartCoroutine(RotateOrbitalRoutine());

        // Mover hacia adelante
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * boomerangSpeed;
            shootingOrbital.localPosition = Vector3.Lerp(initialLocalPosition, targetLocalPosition, t);
            yield return null;
        }

        yield return new WaitForSeconds(boomerangReturnDelay);

        // Mover de regreso
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * boomerangSpeed;
            shootingOrbital.localPosition = Vector3.Lerp(targetLocalPosition, initialLocalPosition, t);
            yield return null;
        }

        // Detener la rotación
        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        // Volver a posición inicial exacta
        shootingOrbital.localPosition = initialLocalPosition;
        isBoomerangActive = false;

        // Terminar ataque
        if (orbitalController != null)
        {
            orbitalController.SetAttackActive(false);
            orbitalController.SetDamageActive(false);
        }
    }

    private System.Collections.IEnumerator RotateOrbitalRoutine()
    {
        float rotationSpeed = 720f; // grados por segundo
    
        while (true)
        {
            // Rotar alrededor de su propio eje
            shootingOrbital.Rotate(0f, 0f, rotationSpeed * Time.deltaTime, Space.Self);
            yield return null;
        }
    }
}
