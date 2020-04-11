using System;
using UnityEngine;

namespace NonVR
{
    public class Grabber : MonoBehaviour
    {
        [SerializeField] private Transform grabbedTargetTransform;
        
        // WARNING Hardcoded for simplicity's sake
        private string grabbingObjectTag = "Car";
        
        private void OnTriggerEnter(Collider other)
        {
            // Checking if the other tag is car, it could be another object we don't want to grab
            if (other.CompareTag(grabbingObjectTag))
            {
                print("caught target: " + other.name);
                // Grab animation
                // Set kinematic to true so it doesn't get affected by physics anymore
                other.GetComponent<Rigidbody>().isKinematic = true;
                const float tweenTime = 0.75f;
                LeanTween.move(other.gameObject, grabbedTargetTransform, tweenTime).setEaseOutQuad();
                LeanTween.rotate(other.gameObject, grabbedTargetTransform.rotation.eulerAngles, tweenTime).setEaseOutQuad();
                
                // The following line will grab it instantly without the animation
                // other.transform.position = grabbedTargetTransform.position; 
            }
        }
    }
}
