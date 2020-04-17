using System;
using UnityEngine;

namespace NonVR
{
    public class CarAssembler : MonoBehaviour
    {
        
        private CarAssemblyPart[] parts;
        private bool isComplete => IsAssemblyComplete();
        
        // Wow don't do this
        private GameObject copy;
        
        private void Awake()
        {
            parts = GetComponentsInChildren<CarAssemblyPart>();
        }

        private void OnEnable()
        {
            GameEventSystem.Instance.onAssembledPart += AssembledPart;
        }

        private void OnDisable()
        {
            GameEventSystem.Instance.onAssembledPart -= AssembledPart;
        }

        private void AssembledPart()
        {
            if (isComplete)
            {
                AssemblyComplete();
            }
        }

        private void AssemblyComplete()
        {
            // Start timer here
            print("Assembly is complete");
            GameEventSystem.Instance.AssemblyComplete();
            Destroy(transform.parent.gameObject);
        }

        private bool IsAssemblyComplete()
        {
            foreach (var part in parts)
            {
                if (!part.isComplete) return false;
            }

            return true;
        }
    }
}
