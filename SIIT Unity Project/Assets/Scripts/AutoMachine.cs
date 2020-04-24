using System;
using echo17.Signaler.Core;
using NonVR;
using Signals;
using UnityEngine;
using Utilities;

public class AutoMachine : MonoBehaviour, IBroadcaster
{
    [SerializeField] private FloatReference totalRequiredTime;
    [SerializeField] private FloatVariable currentTime;
    
    
    private Transform partOnAutoMachine = null;
    // private float currentTime = 0f;
    
    private void Awake()
    {
        currentTime.SetValue(totalRequiredTime);
        enabled = false;
    }

    private void Update()
    {
        if (partOnAutoMachine)
        {
            currentTime.ApplyChange(-Time.deltaTime);
            
            if (currentTime.Value < 0) 
            {
                // AutoMachine has completed
                partOnAutoMachine = null;
                var signal = new AssemblySignals.AutoMachineCompleted();
                Signaler.Instance.Broadcast(this, signal);
                ResetTime();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StartAutoMachineIfProperPart(other.transform);
    }

    private void OnTriggerStay(Collider other)
    {
        StartAutoMachineIfProperPart(other.transform);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.transform != partOnAutoMachine) return;
        partOnAutoMachine = null;
        ResetTime();
    }

    private void StartAutoMachineIfProperPart(Transform other)
    {
        if (!IsOtherPartAutoMachinable(other)) return;
        partOnAutoMachine = other;
        ResetTime();
    }

    private bool IsOtherPartAutoMachinable(Component other)
    {
        if (partOnAutoMachine != null) return false; // Only 1 assembler should be on the automachine
        var otherAssembler = other.GetComponentInParent<CarAssembler>();
        if (otherAssembler == null) return false;
        if (!otherAssembler.AreAllPartsAssembled) return false; // Only allow fully assembled assemblers
        var otherRigidbody = other.GetComponentInParent<Rigidbody>();
        if (otherRigidbody == null) return false;
        if (otherRigidbody.isKinematic) return false; // Physics should be enabled on the object, otherwise ignore this object
        if (otherRigidbody.velocity != Vector3.zero) return false; // Other rigidbody should not be moving
        return true;
    }

    private void ResetTime()
    {
        currentTime.SetValue(totalRequiredTime);
        // currentTime = secondsToComplete;
    }
}