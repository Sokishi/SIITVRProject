using System;
using System.Collections.Generic;
using echo17.Signaler.Core;
using Signals;
using UnityEngine;

namespace NonVR
{
    public class AssemblerTimer : MonoBehaviour, IBroadcaster, ISubscriber
    {
        private bool isTimerOn;
        private float timeValue;
        private readonly List<float> timeHistoryList = new List<float>();

        private AssemblySignals.UpdateTimeSignal updateTimeSignal;
        private AssemblySignals.UpdateRecordsSignal updateRecordsSignal;

        private void Awake()
        {
            Signaler.Instance.Subscribe<AssemblySignals.StartAssemblyLoopSignal>(this, StartTimer);
            Signaler.Instance.Subscribe<AssemblySignals.StopAssemblyLoopSignal>(this, StopTimer);
        }

        private void Update()
        {
            if (!isTimerOn) return;
            timeValue += Time.deltaTime;
            updateTimeSignal.time = timeValue;
            Signaler.Instance.Broadcast(this, updateTimeSignal);
        }

        private bool StartTimer(AssemblySignals.StartAssemblyLoopSignal signal)
        {
            // If history is empty, don't record time.
            // If history is not empty, add history time log
            if (Math.Abs(timeValue) > 0.000000001)
            {
                RecordTime();
            }

            timeValue = 0;
            isTimerOn = true;
            return true;
        }

        private bool StopTimer(AssemblySignals.StopAssemblyLoopSignal signal)
        {
            isTimerOn = false;
            RecordTime();
            return true;
        }

        private void RecordTime()
        {
            timeHistoryList.Add(timeValue);
            updateRecordsSignal.timeRecords = timeHistoryList;
            Signaler.Instance.Broadcast(this, updateRecordsSignal);
        }
    }
}
