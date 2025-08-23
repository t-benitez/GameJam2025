
using UnityEngine;

public static class WaveObserver
{
    public static event System.Action<int> OnNewWave;
    
    public static void NotifyNewWave(int wave)
    {
        OnNewWave?.Invoke(wave);
    }
}