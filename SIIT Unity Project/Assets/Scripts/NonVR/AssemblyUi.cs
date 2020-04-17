using System;
using System.Collections.Generic;
using echo17.Signaler.Core;
using Signals;
using TMPro;
using UnityEngine;

public class AssemblyUi : MonoBehaviour, ISubscriber
{
    [SerializeField] private TMP_Text loopText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text timeLogText;

    private void Awake()
    {
        Signaler.Instance.Subscribe<AssemblySignals.UpdateTimeSignal>(this, UpdateTimeUi);
        Signaler.Instance.Subscribe<AssemblySignals.UpdateLoopsSignal>(this, UpdateLoopUi);
        Signaler.Instance.Subscribe<AssemblySignals.UpdateRecordsSignal>(this, UpdateTimeRecordsUi);
    }
    
    private bool UpdateTimeUi(AssemblySignals.UpdateTimeSignal signal)
    {
        timeText.text = "Time: " + signal.time.ToString("F2");
        return true;
    }

    private bool UpdateLoopUi(AssemblySignals.UpdateLoopsSignal signal)
    {
        loopText.text = "Loop: " + signal.currentLoop + "/" + signal.totalLoops;
        return true;
    }

    private bool UpdateTimeRecordsUi(AssemblySignals.UpdateRecordsSignal signal)
    {
        timeLogText.text = "Records: " + "\n" + string.Join("\n", signal.timeRecords);
        return true;
    }
}