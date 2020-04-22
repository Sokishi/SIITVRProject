using System;
using System.Collections.Generic;
using System.Linq;
using echo17.Signaler.Core;
using Signals;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace NonVR
{
    public class CarAssembler : MonoBehaviour, IBroadcaster, ISubscriber
    {
        [SerializeField] private List<CarAssemblyPart.PartType> requirements = new List<CarAssemblyPart.PartType>();
        [SerializeField] private AutoMachine autoMachine;

        public bool AreAllPartsAssembled => IsEveryPartAssembled();
        
        private int currentRequirementIndex;
        private CarAssemblyPart[] parts;
        // Wow don't do this
        private GameObject copy;
        private readonly AssemblySignals.AssemblyCompleteSignal assemblyCompleteSignal =
            new AssemblySignals.AssemblyCompleteSignal();
        private MessageSubscription<AssemblySignals.AssembledPartSignal> assembledPartSignalSubscription;
        private MessageSubscription<AssemblySignals.AutoMachineCompleted> autoMachineSignalSubscription;
        private CarAssemblyPart.PartType currentPartRequirement => requirements[currentRequirementIndex];

        enum AssemblyState
        {
            Assembling,
            AutoMachining,
            Completed
        }

        private AssemblyState currentState;
        
        private void Awake()
        {
            currentState = AssemblyState.Assembling;
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
            autoMachineSignalSubscription =
                Signaler.Instance.Subscribe<AssemblySignals.AutoMachineCompleted>(this, AutoMachineCompleted);
        }

        private bool AutoMachineCompleted(AssemblySignals.AutoMachineCompleted signal)
        {
            autoMachine.enabled = false;
            SetState(AssemblyState.Completed);
            return true;
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
            autoMachineSignalSubscription.UnSubscribe();
        }

        private bool AssembledPart(AssemblySignals.AssembledPartSignal signal)
        {
            if (!enabled)
                return false;

            // Check if all parts of currentRequirement type have been assembled
            // if not, do nothing
            // if they have, move to next requirement
            ProgressRequirementIfCompleted();

            if (AreAllPartsAssembled)
            {
                // Move to next stage of assembly
                SetState(AssemblyState.AutoMachining);
            }

            return true;
        }

        private void SetState(AssemblyState state)
        {
            currentState = state;

            switch (currentState)
            {
                case AssemblyState.Assembling:
                    break;
                case AssemblyState.AutoMachining:
                    // Enable automachine component
                    if (autoMachine) autoMachine.enabled = true;
                    var collider = GetComponent<Collider>();
                    if (collider) collider.enabled = true;
                    var rigidbody = GetComponent<Rigidbody>();
                    if (rigidbody) rigidbody.isKinematic = false;
                    var throwable = GetComponent<Throwable>();
                    if (throwable) throwable.enabled = true;
                    var childColliders = GetComponentsInChildren<Collider>();
                    foreach (var childCollider in childColliders)
                    {
                        if (childCollider == collider) continue;
                        childCollider.enabled = false;
                    }
                    break;
                case AssemblyState.Completed:
                    AssemblyComplete();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

        private bool IsEveryPartAssembled()
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