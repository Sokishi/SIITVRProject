using echo17.Signaler.Core;
using Signals;
using UnityEngine;

namespace NonVR
{
    public class CarAssemblerLoop : MonoBehaviour, IBroadcaster, ISubscriber
    {
        [SerializeField] private int numberOfLoops = 1;
        [SerializeField] private GameObject prefabToLoop;
        [SerializeField] private Transform spawnPoint;
        
        private int currentLoop = 0;

        private AssemblySignals.UpdateLoopsSignal updateLoopsSignal;
        private readonly AssemblySignals.StartAssemblyLoopSignal startAssemblyLoopSignal =
            new AssemblySignals.StartAssemblyLoopSignal();
        private readonly AssemblySignals.StopAssemblyLoopSignal stopAssemblyLoopSignal =
            new AssemblySignals.StopAssemblyLoopSignal();

        private void Awake()
        {
            Signaler.Instance.Subscribe<AssemblySignals.AssemblyCompleteSignal>(this, AssemblyCompleted);
        }

        private void Start()
        {
            var startLoopSignal = new AssemblySignals.StartAssemblyLoopSignal();
            Signaler.Instance.Broadcast(this, startLoopSignal);
            BroadcastUpdateLoops();
        }
        
        private bool AssemblyCompleted(AssemblySignals.AssemblyCompleteSignal signal)
        {
            if (currentLoop < numberOfLoops - 1)
            {
                currentLoop++;
                // Don't do this
                foreach (Transform child in transform)
                {
                    if (child.name.Contains("Assembly Root"))
                    {
                        Destroy(child.gameObject);
                    }
                }
                var newAssemblyRoot = Instantiate(prefabToLoop, transform);
                newAssemblyRoot.transform.position = spawnPoint.position;
                newAssemblyRoot.SetActive(true);
                Signaler.Instance.Broadcast(this, startAssemblyLoopSignal);
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