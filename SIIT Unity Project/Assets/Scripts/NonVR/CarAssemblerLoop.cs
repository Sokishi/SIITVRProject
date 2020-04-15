using System;
using UnityEngine;

namespace NonVR
{
    public class CarAssemblerLoop : MonoBehaviour
    {
        [SerializeField] private int numberOfLoops;
        private int currentLoop = 0;

        private void OnEnable()
        {
            GameEventSystem.Instance.onAssemblyComplete += AssemblyCompleted;
        }

        private void OnDisable()
        {
            GameEventSystem.Instance.onAssemblyComplete -= AssemblyCompleted;
        }

        private void AssemblyCompleted()
        {
            // If more loops 
            //   record time, increment currentLoop, reset loop + objects
            // else
            //   print time logs of previous loops (send event?)
            throw new NotImplementedException();
        }
    }
}
