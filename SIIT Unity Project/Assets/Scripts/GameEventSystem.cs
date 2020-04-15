 using System;
 using UnityEngine;

public class GameEventSystem : MonoBehaviour
{
    private static GameEventSystem _instance;
    public static GameEventSystem Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    public event Action onAssembledPart;
    public void AssembledPart()
    {
        onAssembledPart?.Invoke();
    }

    public event Action onAssemblyComplete;
    public void AssemblyComplete()
    {
        onAssemblyComplete?.Invoke();
    }

    public event Action onStartAssemblyLoop;
    public void StartAssemblyLoop()
    {
        onStartAssemblyLoop?.Invoke();
    }

    public event Action<int?, int?, float?> onUpdateAssemblyUi;
    public void UpdateAssemblyUi(int? currentLoop, int? totalLoops, float? time)
    {
        onUpdateAssemblyUi?.Invoke(currentLoop, totalLoops, time);
    }

    public event Action<float> onUpdateAssemblyLogsUi;
    public void UpdateAssemblyLogsUi(float timeValue)
    {
        onUpdateAssemblyLogsUi?.Invoke(timeValue);
    }

    public event Action onStopAssemblyLoop;
    public void StopAssemblyLoop()
    {
        onStopAssemblyLoop?.Invoke();
    }
}
