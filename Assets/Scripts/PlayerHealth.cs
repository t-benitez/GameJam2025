using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;
    [Header("Sonidos de daño")]
    public AudioClip sonidoDano1;
    public AudioClip sonidoDano2;
    public AudioClip sonidoDano3;

    private AudioSource audioSource;
    private void Awake()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayRandomDamageSound();

        Debug.Log($"Jugador recibió daño. Vida actual: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.jugadorMuere);
        Destroy(gameObject);
    }
}
