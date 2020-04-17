using TMPro;
using UnityEngine;

public class AssemblyUi : MonoBehaviour
{
    [SerializeField] private TMP_Text loopText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text timeLogText;

    private void OnEnable()
    {
        GameEventSystem.Instance.onUpdateAssemblyUi += UpdateUi;
        GameEventSystem.Instance.onUpdateAssemblyLogsUi += UpdateTimeLogsUi;
        GameEventSystem.Instance.onUpdateLoopUi += UpdateLoopUi;
    }

    private void UpdateLoopUi(int currentLoop, int totalLoops)
    {
        loopText.text = "Loop: " + currentLoop + "/" + totalLoops;
    }
    
    private void OnDisable()
    {
        GameEventSystem.Instance.onUpdateAssemblyUi -= UpdateUi;
        GameEventSystem.Instance.onUpdateAssemblyLogsUi -= UpdateTimeLogsUi;
        GameEventSystem.Instance.onUpdateLoopUi -= UpdateLoopUi;
    }

    private void UpdateUi(int? currentLoop, int? totalLoops, float? time)
    {
        if (currentLoop != null && totalLoops != null)
        {
            loopText.text = "Loops: " + currentLoop + "/" + totalLoops;
        }

        if (time != null)
        {
            timeText.text = "Time: " + time?.ToString("F2");
            ;
        }
    }

    private void UpdateTimeLogsUi(float time)
    {
        timeLogText.text += "\n" + time;
    }
}