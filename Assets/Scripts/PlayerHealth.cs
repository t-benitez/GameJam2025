using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 10;
    public int currentHealth;

    [Header("Invulnerabilidad")]
    public float invulnerableDuration = 0.5f;
    private bool isInvulnerable = false;

    [Header("Parpadeo")]
    public float blinkInterval = 0.1f; // tiempo entre parpadeos
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [Header("Sonidos de daño")]
    public AudioClip sonidoDano1;
    public AudioClip sonidoDano2;
    public AudioClip sonidoDano3;
    int defaultLayer;

    private AudioSource audioSource;

    private void Awake()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable) return; // ignorar daño si está invulnerable

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        PlayDamageSound();

        Debug.Log($"Jugador recibió daño. Vida actual: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvulnerabilityRoutine());
        }
    }

private IEnumerator InvulnerabilityRoutine()
{
    isInvulnerable = true;

    // guardar capa actual y cambiar
    defaultLayer = gameObject.layer;
    gameObject.layer = LayerMask.NameToLayer("InvulnerablePlayer");

    float timer = 0f;
    bool faded = false;

    while (timer < invulnerableDuration)
    {
        if (spriteRenderer != null)
        {
            Color c = originalColor;
            c.a = faded ? 1f : 0.3f;
            spriteRenderer.color = c;
            faded = !faded;
        }

        yield return new WaitForSeconds(blinkInterval);
        timer += blinkInterval;
    }

    // restaurar
    gameObject.layer = defaultLayer;
    if (spriteRenderer != null) spriteRenderer.color = originalColor;
    isInvulnerable = false;
}


    private void Die()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySound(AudioManager.Instance.jugadorMuere);

        PlayerDeathNotifier.Die(true);
        Destroy(gameObject);
    }
    private void PlayDamageSound(){
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayRandomDamageSound();
    }

    public bool IsInvulnerable(){
        return isInvulnerable;
    }
}
