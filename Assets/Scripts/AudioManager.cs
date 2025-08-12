using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sonidos Jugador")]
    public AudioClip jugadorMuere;
    public AudioClip jugadorDash;
    public AudioClip jugadorTiro;

    [Header("Sonidos Enemigo")]
    public AudioClip enemigoMuere;

    [Header("Sonidos de daño jugador")]
    public AudioClip sonidoDano1;
    public AudioClip sonidoDano2;
    public AudioClip sonidoDano3;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
    
        public void PlayRandomDamageSound()
    {
        AudioClip[] clips = { sonidoDano1, sonidoDano2, sonidoDano3 };
        AudioClip clipToPlay = clips[Random.Range(0, clips.Length)];
        PlaySound(clipToPlay);
    }
}
