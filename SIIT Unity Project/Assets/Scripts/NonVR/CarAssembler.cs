using UnityEngine;

namespace NonVR
{
    public class CarAssembler : MonoBehaviour
    {
        private CarAssemblyPart[] parts;
        private bool isComplete => IsAssemblyComplete();
        
        private void Awake()
        {
            parts = gameObject.GetComponentsInChildren<CarAssemblyPart>();
        }

        private void OnEnable()
        {
            GameEventSystem.current.onAssembledPart += AssembledPart;
        }

        private void AssembledPart()
        {
            if (isComplete)
            {
                AssemblyComplete();
            }
        }

        private void OnDisable()
        {
            GameEventSystem.current.onAssembledPart -= AssembledPart;
        }

        private void AssemblyComplete()
        {
            // Start timer here
            print("Assembly is complete");
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
