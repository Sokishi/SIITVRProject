using System.Collections.Generic;
using System.Linq;
using echo17.Signaler.Core;
using Signals;
using UnityEngine;

namespace NonVR
{
    public class CarAssembler : MonoBehaviour, IBroadcaster, ISubscriber
    {
        [SerializeField] private List<CarAssemblyPart.PartType> requirements = new List<CarAssemblyPart.PartType>();
        private int currentRequirementIndex;
        private CarAssemblyPart[] parts;
        private bool isComplete => IsAssemblyComplete();
        // Wow don't do this
        private GameObject copy;
        private readonly AssemblySignals.AssemblyCompleteSignal assemblyCompleteSignal =
            new AssemblySignals.AssemblyCompleteSignal();
        private MessageSubscription<AssemblySignals.AssembledPartSignal> assembledPartSignalSubscription;
        private CarAssemblyPart.PartType currentPartRequirement => requirements[currentRequirementIndex];
        
        private void Awake()
        {
            parts = GetComponentsInChildren<CarAssemblyPart>();
            ValidateRequirements();

            if (requirements.Count > 0)
                currentRequirementIndex = 0;

            foreach (var part in parts)
            {
                part.SetAssembler(this);
            }
        }

        private void ValidateRequirements()
        {
            var partTypesInAssembler = new List<CarAssemblyPart.PartType>();
            foreach (var part in parts)
            {
                if (!partTypesInAssembler.Contains(part.GetPartType))
                {
                    partTypesInAssembler.Add(part.GetPartType);
                }
            }

            foreach (var requirement in requirements)
            {
                if (!partTypesInAssembler.Contains(requirement))
                {
                    Debug.LogError("Assembler does not contain part type: " + requirement +
                                   ". Check the setup for CarAssembler.");
                }
            }
        }

        private void OnEnable()
        {
            assembledPartSignalSubscription =
                Signaler.Instance.Subscribe<AssemblySignals.AssembledPartSignal>(this, AssembledPart);
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

            // Check if all parts of currentRequirement type have been assembled
            // if not, do nothing
            // if they have, move to next requirement
            ProgressRequirementIfCompleted();

            if (isComplete)
                AssemblyComplete();

            return true;
        }

        private void ProgressRequirementIfCompleted()
        {
            var currentRequirementParts = parts.Where(part => part.GetPartType == currentPartRequirement);
            var hasCompletedRequirement = currentRequirementParts.All(part => part.isAssembled);

            if (hasCompletedRequirement)
            {
                if (currentRequirementIndex < requirements.Count)
                {
                    currentRequirementIndex++;
                }
            }
        }

        private void AssemblyComplete()
        {
            Signaler.Instance.Broadcast(this, assemblyCompleteSignal);
            // Destroy(gameObject);
            gameObject.SetActive(false);
            enabled = false;
        }

        private bool IsAssemblyComplete()
        {
            foreach (var part in parts)
            {
                if (!part.isAssembled) return false;
            }

            return true;
        }

        public bool CanAssemblePart(CarAssemblyPart.PartType partType)
        {
            return currentPartRequirement == partType;
        }
    }
}