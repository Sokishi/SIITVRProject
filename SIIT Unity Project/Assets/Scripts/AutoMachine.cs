using System;
using echo17.Signaler.Core;
using NonVR;
using Signals;
using UnityEngine;

public class AutoMachine : MonoBehaviour, IBroadcaster
{
    [SerializeField] private int secondsToComplete = 10;
    
    private Transform partOnAutoMachine = null;
    private float currentTime = 0f;
    private AssemblySignals.UpdateAutoMachineProgressTime updateTimeSignal;
    
    private void Awake()
    {
        enabled = false;
    }

    private void Update()
    {
        if (partOnAutoMachine)
        {
            currentTime -= Time.deltaTime; // tick down time
            updateTimeSignal.time = currentTime;
            Signaler.Instance.Broadcast(this, updateTimeSignal);
            
            if (currentTime < 0) 
            {
                // Automachine has completed
                partOnAutoMachine = null;
                var signal = new AssemblySignals.AutoMachineCompleted();
                Signaler.Instance.Broadcast(this, signal);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherAssembler = other.GetComponentInParent<CarAssembler>();
        if (otherAssembler == null) return;
        if (!otherAssembler.AreAllPartsAssembled) return; // Only allow fully assembled assemblers
        var otherRigidbody = other.GetComponentInParent<Rigidbody>();
        if (otherRigidbody == null) return;
        if (otherRigidbody.isKinematic) return; // Physics should be enabled on the object, otherwise ignore this object
        if (otherRigidbody.velocity != Vector3.zero) return; // Other rigidbody should not be moving
        if (partOnAutoMachine != null) return; // Only 1 assembler should be on the automachine
        partOnAutoMachine = other.transform;
        currentTime = secondsToComplete;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform != partOnAutoMachine) return;
        partOnAutoMachine = null;
        ResetTime();
    }

    private void ResetTime()
    {
        currentTime = secondsToComplete;
        Signaler.Instance.Broadcast(this, updateTimeSignal);
    }
}