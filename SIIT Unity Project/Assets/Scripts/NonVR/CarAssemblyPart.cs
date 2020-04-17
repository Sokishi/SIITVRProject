using echo17.Signaler.Core;
using Signals;
using UnityEngine;

namespace NonVR
{
    public class CarAssemblyPart : MonoBehaviour, IBroadcaster
    {
        private enum PartType
        {
            Roof,
            Hood,
            StepUp,
            Wheel,
            FrameSide
        }
        
        [SerializeField] private PartType partType;
        [SerializeField] private bool isStaticPart = false; // non-movable part attached to the car for detection purposes
      
        public bool isComplete = false;

        private GameObject part;
        private AssemblySignals.AssembledPartSignal assembledPartSignal = new AssemblySignals.AssembledPartSignal();
        
        private void Awake()
        {
            // TODO: Don't do this. This is a terrible way
            part = transform.GetChild(0).gameObject;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isStaticPart) return; // Don't check for moving parts
            
            // TODO: Check if part matches this.carType;
            if (other.isTrigger) return; // Another semi-redundant check for moving part

            // TODO: Don't do this, should be GetComponent only
            var otherPart = other.GetComponentInParent<CarAssemblyPart>();
            if (otherPart == null)
            {
                print("Could not find CarAssemblyPart");
                return;
            }
            
            if (otherPart.partType != partType) return;
            
            // TODO: LeanTween animation to proper part position + rotation
            print(otherPart.name + " entered area of: " + part.name);
            Destroy(otherPart.gameObject);
            part.gameObject.SetActive(true);
            isComplete = true;
            Signaler.Instance.Broadcast(this, assembledPartSignal);
        }
    }
}
