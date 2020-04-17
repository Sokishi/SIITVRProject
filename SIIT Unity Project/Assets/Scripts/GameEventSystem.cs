 using System;
 using UnityEngine;

public class GameEventSystem : MonoBehaviour
{
    private static GameEventSystem _instance;

    public static GameEventSystem Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = FindObjectOfType(typeof(GameEventSystem)) as GameEventSystem;

            if (_instance == null)
            {
                Debug.LogError("There needs to be one active GameEventSystem script on a GameObject in your scene.");
            }
            else
            {
                _instance.Initialize();
            }

            return _instance;
        }
    }

    private void Initialize()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Awake()
    {
        Initialize();
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

    public event Action<int, int> onUpdateLoopUi;
    public void UpdateLoopUi(int currentLoop, int numberOfLoops)
    {
        onUpdateLoopUi?.Invoke(currentLoop, numberOfLoops);
    }
}
