using UnityEngine;

public static class PlayerDeathNotifier
{
    public static event System.Action<bool> OnPlayerDeath;
    
    public static void Die(bool death)
    {
        OnPlayerDeath?.Invoke(death);
    }
}