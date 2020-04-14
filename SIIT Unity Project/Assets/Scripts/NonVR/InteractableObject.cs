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
            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
            }

            transform.SetParent(holdingTransform, false);
            transform.localPosition = Vector3.zero;
        }

        public void Drop()
        {
            transform.SetParent(originalParent);
            if (rigidbody != null)
            {
                rigidbody.isKinematic = false;
            }
        }

        public void Drop(Vector3 dropPosition)
        {
            Transform objTransform;
            (objTransform = transform).SetParent(originalParent);
            objTransform.position = dropPosition;
            if (rigidbody != null)
            {
                rigidbody.isKinematic = false;
            }
        }
    }

    public interface IInteractable
    {
        void PickUp(Transform holdingTransform);
        void Drop();
        void Drop(Vector3 dropPosition);
    }
}