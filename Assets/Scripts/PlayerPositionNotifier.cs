
using UnityEngine;

public static class PlayerPositionNotifier
{
    public static event System.Action<Vector3> OnPlayerPositionChanged;
    
    public static void NotifyPositionChanged(Vector3 position)
    {
        OnPlayerPositionChanged?.Invoke(position);
    }
}