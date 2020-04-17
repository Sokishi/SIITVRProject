using echo17.Signaler.Core;
using Signals;
using UnityEngine;

namespace NonVR
{
    public class CarAssemblyPart : MonoBehaviour, IBroadcaster
    {
        public enum PartType
        {
            Roof,
            Hood,
            StepUp,
            Wheel,
            FrameSide
        }
        [SerializeField] private PartType partType;
        // non-movable part attached to the car for detection purposes
        [SerializeField] private bool isStaticPart = false;
        public PartType GetPartType => partType;

        [HideInInspector] public bool isAssembled = false;

        private GameObject part;

        private readonly AssemblySignals.AssembledPartSignal assembledPartSignal =
            new AssemblySignals.AssembledPartSignal();

        private CarAssembler assembler;

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
            if (otherPart == null) return;
            if (otherPart.partType != partType) return;

            if (!assembler.CanAssemblePart(partType))
                return;

            // TODO: LeanTween animation to proper part position + rotation
            DisableColliders();
            otherPart.gameObject.SetActive(false);
            var meshRenderer = otherPart.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer) meshRenderer.enabled = false;
            
            part.gameObject.SetActive(true);
            isAssembled = true;
            Signaler.Instance.Broadcast(this, assembledPartSignal);
        }

        private void DisableColliders()
        {
            var colliders = GetComponentsInChildren<Collider>();
            foreach (var col in colliders)
            {
                if (col.isTrigger)
                {
                    col.enabled = false;
                }
            }
        }

        public void SetAssembler(CarAssembler carAssembler)
        {
            assembler = carAssembler;
        }
    }
}