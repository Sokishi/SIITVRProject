using System;
using System.Collections.Generic;
using System.Linq;
using echo17.Signaler.Core;
using Signals;
using UnityEngine;
using UnityEngine.Events;

namespace NonVR
{
    public class AssemblerTimer : MonoBehaviour, IBroadcaster, ISubscriber
    {
        [SerializeField] private GradeCalculator gradeCalculator;
        
        private bool isTimerOn;
        private float timeValue;
        private readonly List<float> timeHistoryList = new List<float>();

        private AssemblySignals.UpdateTimeSignal updateTimeSignal;
        private AssemblySignals.UpdateRecordsSignal updateRecordsSignal;

        private void Awake()
        {
            Signaler.Instance.Subscribe<AssemblySignals.StartAssemblyLoopSignal>(this, StartTimer);
            Signaler.Instance.Subscribe<AssemblySignals.StopAssemblyLoopSignal>(this, StopTimer);
            Signaler.Instance.Subscribe<AssemblySignals.AllLoopsCompleted>(this, CalculateGrade);
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
        

        private bool CalculateGrade(AssemblySignals.AllLoopsCompleted signal)
        {
            if (gradeCalculator == null) return false;
            var averageTime = (int) timeHistoryList.Average();
            print("Average time: " + averageTime);
            var grade = gradeCalculator.CalculateGrade(averageTime);
            Signaler.Instance.Broadcast(this, new AssemblySignals.AchievedGradeSignal {Grade = grade});
            return true;
        }
    }
}
