using System;
using System.Collections.Generic;
using UnityEngine;

namespace NonVR
{
    public class AssemblerTimer : MonoBehaviour
    {
        private bool isTimerOn;
        private float timeValue;
        private List<float> timeHistoryList = new List<float>();

        private void Start()
        {
            GameEventSystem.Instance.onStartAssemblyLoop += StartTimer;
            GameEventSystem.Instance.onStopAssemblyLoop += StopTimer;
        }

        private void OnDestroy()
        {
            GameEventSystem.Instance.onStartAssemblyLoop -= StartTimer;
            GameEventSystem.Instance.onStopAssemblyLoop -= StopTimer;

        }

        private void Update()
        {
            if (!isTimerOn) return;
            timeValue += Time.deltaTime;
            GameEventSystem.Instance.UpdateAssemblyUi(null, null, timeValue);
        }

        private void StartTimer()
        {
            // If history is empty, don't record time.
            // If history is not empty, add history time log
            if (Math.Abs(timeValue) > 0.000000001)
            {
                RecordTime();
            }

            timeValue = 0;
            isTimerOn = true;
        }

        private void StopTimer()
        {
            isTimerOn = false;
            RecordTime();
        }

        private void RecordTime()
        {
            timeHistoryList.Add(timeValue);
            GameEventSystem.Instance.UpdateAssemblyLogsUi(timeValue);
        }
    }
}