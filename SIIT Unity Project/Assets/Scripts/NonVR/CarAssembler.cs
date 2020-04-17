using echo17.Signaler.Core;
using Signals;
using UnityEngine;

namespace NonVR
{
    public class CarAssembler : MonoBehaviour, IBroadcaster, ISubscriber
    {
        private CarAssemblyPart[] parts;
        private bool isComplete => IsAssemblyComplete();

        // Wow don't do this
        private GameObject copy;
        private readonly AssemblySignals.AssemblyCompleteSignal assemblyCompleteSignal = new AssemblySignals.AssemblyCompleteSignal();
        private MessageSubscription<AssemblySignals.AssembledPartSignal> assembledPartSignalSubscription;

        private void Awake()
        {
            parts = GetComponentsInChildren<CarAssemblyPart>();
        }

        private void OnEnable()
        {
            assembledPartSignalSubscription = Signaler.Instance.Subscribe<AssemblySignals.AssembledPartSignal>(this, AssembledPart);
        }

        private void OnDisable()
        {
            UnSubscribeSignal();
        }

        private void OnDestroy()
        {
            UnSubscribeSignal();
        }

        private void UnSubscribeSignal()
        {
            assembledPartSignalSubscription.UnSubscribe();
        }

        private bool AssembledPart(AssemblySignals.AssembledPartSignal signal)
        {
            if (!enabled)
                return false;

            if (isComplete)
                AssemblyComplete();

            return true;
        }
        
        private void AssemblyComplete()
        {
            Signaler.Instance.Broadcast(this, assemblyCompleteSignal);
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