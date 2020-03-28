using UnityEngine;

namespace NonVR
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        private new Rigidbody rigidbody;
        private Transform originalParent;
        
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            originalParent = transform.parent;
        }

        public void PickUp(Transform holdingTransform)
        {
            if (rigidbody == null) return;
            rigidbody.isKinematic = true;
            transform.SetParent(holdingTransform, false);
            transform.localPosition = Vector3.zero;
        }

        public void Drop()
        {
            if (rigidbody == null) return;
            rigidbody.isKinematic = false;
            transform.SetParent(originalParent);
        }
    }

    public interface IInteractable
    {
        void PickUp(Transform holdingTransform);
        void Drop();
    }
}