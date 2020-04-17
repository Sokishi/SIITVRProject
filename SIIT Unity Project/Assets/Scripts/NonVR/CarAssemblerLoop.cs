using echo17.Signaler.Core;
using Signals;
using UnityEngine;

namespace NonVR
{
    public class CarAssemblerLoop : MonoBehaviour, IBroadcaster, ISubscriber
    {
        [SerializeField] private int numberOfLoops = 1;
        [SerializeField] private GameObject copyPrefab;

        private int currentLoop = 0;
        private GameObject copiedObject;

        private AssemblySignals.UpdateLoopsSignal updateLoopsSignal;

        private readonly AssemblySignals.StartAssemblyLoopSignal startAssemblyLoopSignal =
            new AssemblySignals.StartAssemblyLoopSignal();

        private readonly AssemblySignals.StopAssemblyLoopSignal stopAssemblyLoopSignal =
            new AssemblySignals.StopAssemblyLoopSignal();

        private void Awake()
        {
            CopyAndHideRootObject(copyPrefab);
            Signaler.Instance.Subscribe<AssemblySignals.AssemblyCompleteSignal>(this, AssemblyCompleted);
        }

        private void Start()
        {
            var startLoopSignal = new AssemblySignals.StartAssemblyLoopSignal();
            Signaler.Instance.Broadcast(this, startLoopSignal);
            BroadcastUpdateLoops();
        }

        private void CopyAndHideRootObject(GameObject objToCopy)
        {
            copiedObject = Instantiate(objToCopy, transform);
            copiedObject.SetActive(false);
        }

        private bool AssemblyCompleted(AssemblySignals.AssemblyCompleteSignal signal)
        {
            if (currentLoop < numberOfLoops - 1)
            {
                currentLoop++;
                copiedObject.SetActive(true);
                Signaler.Instance.Broadcast(this, startAssemblyLoopSignal);
                CopyAndHideRootObject(copiedObject);
                BroadcastUpdateLoops();
            }
            else
            {
                currentLoop++;
                BroadcastUpdateLoops();
                Signaler.Instance.Broadcast(this, stopAssemblyLoopSignal);
            }

            return true;
        }

        private void BroadcastUpdateLoops()
        {
            updateLoopsSignal.currentLoop = currentLoop;
            updateLoopsSignal.totalLoops = numberOfLoops;
            Signaler.Instance.Broadcast(this, updateLoopsSignal);
        }
    }
}