 using System;
 using UnityEngine;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem current;

    private void Awake()
    {
        current = this;
    }

    public event Action onAssembledPart;
    public void AssembledPart()
    {
        onAssembledPart?.Invoke();
    }
}
