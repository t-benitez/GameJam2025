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

        PlayRandomDamageSound();

        Debug.Log($"Jugador recibió daño. Vida actual: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void PlayRandomDamageSound()
    {
        AudioClip[] clips = { sonidoDano1, sonidoDano2, sonidoDano3 };
        AudioClip clipToPlay = clips[Random.Range(0, clips.Length)];
        if (clipToPlay != null)
        {
            audioSource.PlayOneShot(clipToPlay);
        }
    }
    private void Die()
    {
        Debug.Log("Jugador muerto");
    }
}
